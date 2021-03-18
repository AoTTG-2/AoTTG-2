using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapEditor
{
    //A singleton class for displaying the drag selection box and selecting objects
    public class DragSelect : MonoBehaviour
    {
        #region Data Members

        //A self-reference to the singleton instance of this script
        public static DragSelect Instance { get; private set; }

        //The unscaled canvas game object
        [SerializeField] private GameObject unscaledCanvas;

        //The RectTransform component of the unscaled canvas
        private RectTransform canvasRect;

        //The selection box image object under the unscaled canvas
        [SerializeField] private GameObject dragSelectBox;

        //The RectTransform component of the drag selection box game object
        private RectTransform dragBoxRect;

        //How many pixels the cursor has to move after the drag starts before the drag select starts
        [SerializeField] private float deadzone = 5f;

        //A reference to the main camera in the scene
        private Camera mainCamera;

        //Converts world space coordinates to viewport space
        Matrix4x4 worldToViewportMatrix;

        //Variables for managing the drag selection box
        private bool mouseDown = false;
        private bool dragging = false;
        private Vector2 mousePosition = Vector2.zero;

        private Vector2 dragStartPosition;

        //The mode that the drag selection box is currently in
        DragSelectMode selectMode;

        //A dictionary that maps visible game objects to their screen space bounding box (top left and bottom right vertices)
        private Dictionary<GameObject, Tuple<Vector2, Vector2>> boundingBoxTable;

        //The selected objects before the drag was started
        private HashSet<GameObject> originalSeleciton = null;

        #endregion

        #region Delegates

        public delegate void OnDragStartEvent();

        public event OnDragStartEvent OnDragStart;

        public delegate void OnDragEndEvent();

        public event OnDragEndEvent OnDragEnd;

        #endregion

        #region Enums

        //Stores the three selection types the drag selection box can use
        private enum DragSelectMode
        {
            Replace,
            Additive,
            Subtractive
        }

        #endregion

        #region Instantiation

        private void Awake()
        {
            //Set this script as the only instance of the ObjectSelection script
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            //Find and store the main camera in the scene
            mainCamera = Camera.main;
            //Get the rect representing the screen space of the unscaled canvas
            canvasRect = unscaledCanvas.GetComponent<RectTransform>();
            //Get references to components on other game objects
            dragBoxRect = dragSelectBox.GetComponent<RectTransform>();
            //Instantiate the dictionary containing the bounding boxes for on-screen objects
            boundingBoxTable = new Dictionary<GameObject, Tuple<Vector2, Vector2>>();

            //Add listeners to save map object vertices when needed
            MapManager.Instance.OnImport += OnImport;
            MapManager.Instance.OnPaste += SaveBoundingBoxes;
            MapManager.Instance.OnDelete += RemoveBoundingBoxes;
            MapManager.Instance.OnUndelete += SaveBoundingBoxes;
            EditorManager.Instance.OnChangeMode += OnModeChange;
            EditorManager.Instance.OnResize += OnResize;
            SelectionHandle.Instance.OnHandleFinish += SaveSelectedBBs;
            CameraController.Instance.OnFocus += SaveAllBBs;

            //Save the bounding box of all on-screen selectable objects
            SaveBoundingBoxes(ObjectSelection.Instance.GetSelectable());
        }

        #endregion

        #region Event Handlers

        //If the game loses focus, end the selection
        private void OnApplicationFocus(bool focus)
        {
            if (!focus && dragging)
            {
                EndDrag();
                StartCoroutine(InvokeOnDragEndEvent());
            }
        }

        //Clear the current bounding boxes and create new ones for the imported objects
        private void OnImport(HashSet<GameObject> mapObjects)
        {
            ClearBoundingBoxes();
            SaveBoundingBoxes(mapObjects);
        }

        //Save or clear the bounding boxes based on the new mode
        private void OnModeChange(EditorMode prevMode, EditorMode newMode)
        {
            //When the mode changes from fly to edit mode, save the bounding boxes of on-screen objects
            if (prevMode == EditorMode.Fly && newMode == EditorMode.Edit)
                SaveAllBBs();
            //If the new mode is fly mode, then clear the bounding boxes
            else if (newMode == EditorMode.Fly)
                ClearBoundingBoxes();
        }

        //When the screen is resized, calculate the new screen-to-canvas ratio and bounding boxes
        private void OnResize(Vector2 prevResolution)
        {
            ScaleBoundingBoxes(prevResolution);
        }

        private void SaveAllBBs()
        {
            //Store the matrix that transforms world space coordinates to viewport space
            CalculateWorldToViewportMatrix();
            //Save the bounding box of all on-screen selectable objects
            SaveBoundingBoxes(ObjectSelection.Instance.GetSelectable());
        }

        //Saves the bounding boxes of the currently updated objects
        private void SaveSelectedBBs()
        {
            SaveBoundingBoxes(ObjectSelection.Instance.GetSelection());
        }

        private IEnumerator InvokeOnDragEndEvent()
        {
            //Wait until the end of the frame so that the OnHandleFinish event doesn't overlap with the OnMouseUp event
            yield return new WaitForEndOfFrame();

            //After dragging the handle, release the cursor
            EditorManager.Instance.ReleaseCursor();
            //Notify all listeners that the selection drag box was disabled
            OnDragEnd?.Invoke();
        }

        #endregion

        #region Edit Commands

        //Replace the previous selection with what was in the drag select box
        private class ReplaceSelectionCommand : EditCommand
        {
            private GameObject[] previousSelection;
            private GameObject[] selectedObjects;

            public ReplaceSelectionCommand()
            {
                previousSelection = Instance.originalSeleciton.ToArray();
                selectedObjects = ObjectSelection.Instance.GetSelection().ToArray();
            }

            //Select only the new selection
            public override void ExecuteEdit()
            {
                ObjectSelection.Instance.DeselectAll();

                foreach (GameObject mapObject in selectedObjects)
                    ObjectSelection.Instance.SelectObject(mapObject);
            }

            //Select only the original selection
            public override void RevertEdit()
            {
                ObjectSelection.Instance.DeselectAll();

                foreach (GameObject mapObject in previousSelection)
                    ObjectSelection.Instance.SelectObject(mapObject);
            }
        }

        //Add the objects in the drag select box to the current selection
        private class AddSelectionCommand : EditCommand
        {
            private GameObject[] selectedObjects;

            //Store the objects that were newly selected
            public AddSelectionCommand()
            {
                selectedObjects = ObjectSelection.Instance.GetSelection()
                    .ExcludeToArray(Instance.originalSeleciton);
            }

            public override void ExecuteEdit()
            {
                foreach (GameObject mapObject in selectedObjects)
                    ObjectSelection.Instance.SelectObject(mapObject);
            }

            public override void RevertEdit()
            {
                foreach (GameObject mapObject in selectedObjects)
                    ObjectSelection.Instance.DeselectObject(mapObject);
            }
        }

        //Remove the objects in the drag select box from the selection
        private class RemoveSelectionCommand : EditCommand
        {
            private GameObject[] removedObjects;

            //Store the objects that were removed
            public RemoveSelectionCommand()
            {
                removedObjects = Instance.originalSeleciton
                    .ExcludeToArray(ObjectSelection.Instance.GetSelection());
            }

            public override void ExecuteEdit()
            {
                foreach (GameObject mapObject in removedObjects)
                    ObjectSelection.Instance.DeselectObject(mapObject);
            }

            public override void RevertEdit()
            {
                foreach (GameObject mapObject in removedObjects)
                    ObjectSelection.Instance.SelectObject(mapObject);
            }
        }

        #endregion

        #region Update

        private void Update()
        {
            //If in edit mode and the selection handle is not being dragged, update the drag selection box
            if (EditorManager.Instance.CurrentMode == EditorMode.Edit && !SelectionHandle.Instance.GetDragging())
            {
                //When the mouse is clicked and the cursor is not over the UI, save the position where the mouse was pressed down
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(-1))
                {
                    dragStartPosition = Input.mousePosition;
                    mousePosition = Input.mousePosition;
                    mouseDown = true;
                }
                //Disable the drag selection box when the mouse is released
                else if (Input.GetMouseButtonUp(0))
                {
                    if (dragging)
                    {
                        EndDrag();
                        StartCoroutine(InvokeOnDragEndEvent());
                    }

                    mouseDown = false;
                }
                //Update the drag selection box while the mouse is held down
                else if (Input.GetMouseButton(0))
                {
                    //If the escapes key was pressed, cancel the selection
                    if (Input.GetKeyDown(KeyCode.Escape))
                        CancelSelection();

                    mousePosition = Input.mousePosition;

                    //If the drag selection box wasn't enabled yet and the cursor is outside of the dead zone, enable the drag selection box
                    if (mouseDown && !dragging && (mousePosition - dragStartPosition).magnitude > deadzone)
                        StartDrag();

                    //If the drag selection box is enabled, update the box and check for selected objects
                    if (dragging)
                    {
                        UpdateDragRect();
                        UpdateSelectMode();
                        UpdateSelection();
                    }
                }
            }
        }

        //Update the position and size of the drag selection box
        private void UpdateDragRect()
        {
            //Calculate the dimensions of the drag selection box
            float width = Mathf.Abs(mousePosition.x - dragStartPosition.x);
            float height = Mathf.Abs(mousePosition.y - dragStartPosition.y);

            //Divide the dimensions of the box by the scale factor of the canvas to correct the width and height
            dragBoxRect.sizeDelta = new Vector2(width, height);
            dragBoxRect.position = (mousePosition + dragStartPosition) / 2f;
        }

        //Change how the drag box selects objects based on what keys are held
        private void UpdateSelectMode()
        {
            //Get which selection modifier keys are held down``
            bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
            bool controlHeld = Input.GetKey(KeyCode.LeftControl);

            //Set the select mode
            if (!shiftHeld && !controlHeld)
                selectMode = DragSelectMode.Replace;
            else if (shiftHeld)
                selectMode = DragSelectMode.Additive;
            else if (controlHeld)
                selectMode = DragSelectMode.Subtractive;
        }

        //Check for any objects that were deselected or selected
        private void UpdateSelection()
        {
            //The bounds of the drag selection box
            float minX, maxX, minY, maxY;

            //Find horizontal bounds
            if (dragStartPosition.x < mousePosition.x)
            {
                minX = dragStartPosition.x;
                maxX = mousePosition.x;
            }
            else
            {
                minX = mousePosition.x;
                maxX = dragStartPosition.x;
            }

            //Find vertical bounds
            if (dragStartPosition.y < mousePosition.y)
            {
                minY = dragStartPosition.y;
                maxY = mousePosition.y;
            }
            else
            {
                minY = mousePosition.y;
                maxY = dragStartPosition.y;
            }

            //If the drag select mode is additive or subtractive, select all of the originally selected objects
            if (selectMode == DragSelectMode.Additive || selectMode == DragSelectMode.Subtractive)
            {
                foreach (GameObject selectedObject in originalSeleciton)
                    ObjectSelection.Instance.SelectObject(selectedObject);
            }
            //If the drag select mode is replace, deselect all map objects
            else
                ObjectSelection.Instance.DeselectAll();

            //Iterate through all on-screen objects
            foreach (KeyValuePair<GameObject, Tuple<Vector2, Vector2>> objectEntry in boundingBoxTable)
            {
                //Determines if the map object is inside the drag selection box
                bool inDragBox = false;

                //Get the top left and bottom right of the object's bounding box
                Vector2 topLeft = objectEntry.Value.Item1;
                Vector2 bottomRight = objectEntry.Value.Item2;
                //Get the map object
                GameObject mapObject = objectEntry.Key;

                //If the bounding box of the object is inside the drag selection box, set the inDragBox flag
                if (topLeft.x > minX && topLeft.y < maxY && bottomRight.x < maxX && bottomRight.y > minY)
                    inDragBox = true;

                //Select the object based on the select mode
                switch (selectMode)
                {
                    case DragSelectMode.Replace:
                        //Select the object if it is in the selection box and deselect it if not
                        if (inDragBox)
                            ObjectSelection.Instance.SelectObject(mapObject);
                        else
                            ObjectSelection.Instance.DeselectObject(mapObject);

                        break;

                    case DragSelectMode.Additive:
                        //If the object was in the original selection, ignore it
                        if (originalSeleciton.Contains(mapObject))
                            continue;

                        //Select the object if it is in the selection box
                        if (inDragBox)
                            ObjectSelection.Instance.SelectObject(mapObject);
                        //Otherwise, deselect it
                        else
                            ObjectSelection.Instance.DeselectObject(mapObject);

                        break;

                    case DragSelectMode.Subtractive:
                        //Deselect the object if it's in the selection box
                        if (inDragBox)
                            ObjectSelection.Instance.DeselectObject(mapObject);

                        break;
                }
            }
        }

        #endregion

        #region Drag Select Methods

        //Enable the drag selection box
        private void StartDrag()
        {
            //Save the currently selected objects
            originalSeleciton = new HashSet<GameObject>(ObjectSelection.Instance.GetSelection());
            //Enable the drag selection box
            dragSelectBox.SetActive(true);
            dragging = true;

            //Capture the cursor while dragging
            EditorManager.Instance.CaptureCursor();
            //Notify all listeners that the tool handle was activated
            OnDragStart?.Invoke();
        }

        //Disable the drag select box
        private void EndDrag()
        {
            //If the drag select box isn't active, skip the drag ending process
            if (!dragging)
                return;

            //Get the current selection after the drag select
            HashSet<GameObject> currentSelection = ObjectSelection.Instance.GetSelection();

            //Don't save a command if the selection is identical before and after the drag
            if (!currentSelection.SetEquals(originalSeleciton))
            {
                //Create a command for the selection and add it to the history
                switch (selectMode)
                {
                    case DragSelectMode.Replace:
                        EditHistory.Instance.AddCommand(new ReplaceSelectionCommand());
                        break;

                    case DragSelectMode.Additive:
                        EditHistory.Instance.AddCommand(new AddSelectionCommand());
                        break;

                    case DragSelectMode.Subtractive:
                        EditHistory.Instance.AddCommand(new RemoveSelectionCommand());
                        break;
                }
            }

            //Release the old selected object set
            originalSeleciton = null;
            //Disable the drag section box
            dragSelectBox.SetActive(false);
            mouseDown = false;
            dragging = false;
        }

        //Revert the selection to how it was before the drag selection
        private void CancelSelection()
        {
            //Deselect all of the currently selected objects
            ObjectSelection.Instance.DeselectAll();

            //Select the previously selected objects
            foreach (GameObject mapObject in originalSeleciton)
                ObjectSelection.Instance.SelectObject(mapObject);

            //Disable the drag selection box
            EndDrag();
        }

        #endregion

        #region Bounding Box Methods

        //Store the screen space bounding box of the given map objects
        private void SaveBoundingBoxes(HashSet<GameObject> mapObjects)
        {
            //Iterate through the selectable objects
            foreach (GameObject mapObject in mapObjects)
                SaveBoundingBox(mapObject);
        }

        //Save the screen space bounding of the given map object
        private void SaveBoundingBox(GameObject mapObject)
        {
            //Loop through all renderers attached to the object and check if they are visible
            foreach (Renderer render in mapObject.GetComponentsInChildren<Renderer>())
                if (!render.isVisible)
                    return;

            //Get the vertices of the object in screen space
            Tuple<Vector2, Vector2> boundingBox = Get2DBoundingBox(mapObject);

            //If the bounding box is off-screen, skip the object
            if (boundingBox == null)
                return;

            //If the table contains this item, update the value
            if (boundingBoxTable.ContainsKey(mapObject))
                boundingBoxTable[mapObject] = boundingBox;
            //Otherwise create a new entry
            else
                boundingBoxTable.Add(mapObject, boundingBox);
        }

        //Removes the bounding boxes from the given list of map objects 
        private void RemoveBoundingBoxes(HashSet<GameObject> deletedObjects)
        {
            //Remove all of the bounding boxes for the objects that were deleted
            foreach (GameObject mapObject in deletedObjects)
                if (boundingBoxTable.ContainsKey(mapObject))
                    boundingBoxTable.Remove(mapObject);
        }

        //Scale the bounding boxes to match the new resolution
        private void ScaleBoundingBoxes(Vector2 prevResolution)
        {
            //Calculate the factor by which the screen was scaled
            //The screen only scales when the height is changed
            float scaleFactor = Screen.height / prevResolution.y;

            //Update all bounding boxes in the table
            foreach (GameObject mapObject in boundingBoxTable.Keys.ToList())
            {
                //Get the unscaled bounding box
                Tuple<Vector2, Vector2> boundingBox = boundingBoxTable[mapObject];

                //Calculate the x position of the center of the screen before and after the resize
                float prevScreenCenterX = prevResolution.x / 2;
                float newScreenCenterX = Screen.width / 2f;

                //Translate the x positions so that the x axis aligns with the center of the screen,
                //scale the positions, and translate the positions back
                float newTopLeftXPos = newScreenCenterX + ((boundingBox.Item1.x - prevScreenCenterX) * scaleFactor);
                float newBottomRightXPos = newScreenCenterX + ((boundingBox.Item2.x - prevScreenCenterX) * scaleFactor);

                //Calculate the scaled bounding box
                Vector2 topLeftScaled = new Vector2(newTopLeftXPos, boundingBox.Item1.y * scaleFactor);
                Vector2 bottomRightScaled = new Vector2(newBottomRightXPos, boundingBox.Item2.y * scaleFactor);

                //Replace the unscaled bounding box with the scaled one
                boundingBoxTable[mapObject] = new Tuple<Vector2, Vector2>(topLeftScaled, bottomRightScaled);
            }
        }

        //Return a list of screen space vertices for the meshes of the given game object
        private Tuple<Vector2, Vector2> Get2DBoundingBox(GameObject mapObject)
        {
            //Used to find the bounding box of the object
            bool firstVertex = true;
            float minX = 0;
            float maxX = 0;
            float minY = 0;
            float maxY = 0;

            //Iterate through all meshes in the map object and its children
            foreach (MeshFilter meshFilter in mapObject.GetComponentsInChildren<MeshFilter>())
            {
                //Calculate the matrix that converts points in the game object's local space to world space
                Matrix4x4 localToScreenMatrix = CalculateLocalToScreenMatrix(meshFilter.gameObject);

                //Convert the vertices of the mesh to screen space and store the position in a list
                foreach (Vector3 localVertex in meshFilter.mesh.vertices)
                {
                    //Get the vertex in screen space
                    Vector2 screenVertex = localToScreenMatrix.MultiplyPoint(localVertex);

                    //If any of the vertices is off-screen, return null
                    if (screenVertex.x < 0 || screenVertex.x > Screen.width ||
                        screenVertex.y < 0 || screenVertex.y > Screen.height)
                        return null;

                    //If this is the first vertex, use it to initialize the bounds
                    if (firstVertex)
                    {
                        firstVertex = false;
                        minX = screenVertex.x;
                        maxX = screenVertex.x;
                        minY = screenVertex.y;
                        maxY = screenVertex.y;
                    }
                    //Check if this vertex is a new minimum or maximum
                    else
                    {
                        if (screenVertex.x < minX)
                            minX = screenVertex.x;
                        else if (screenVertex.x > maxX)
                            maxX = screenVertex.x;

                        if (screenVertex.y < minY)
                            minY = screenVertex.y;
                        else if (screenVertex.y > maxY)
                            maxY = screenVertex.y;
                    }
                }
            }

            //Return the list of screen space vertices
            return new Tuple<Vector2, Vector2>(new Vector2(minX, maxY), new Vector2(maxX, minY));
        }

        //Calculate and store the matrix that transforms world space coordinates to viewport space
        private void CalculateWorldToViewportMatrix()
        {
            worldToViewportMatrix = mainCamera.projectionMatrix * mainCamera.worldToCameraMatrix;
        }

        //Create a transformation from the local space of the given game object to screen space
        private Matrix4x4 CalculateLocalToScreenMatrix(GameObject mapObject)
        {
            //Get the local space to world space matrix
            Matrix4x4 localToWorldMatrix = mapObject.transform.localToWorldMatrix;

            //Used to remap points from the view port to the screen
            float screenWidth = Screen.width * 0.5f;
            float screenHeight = Screen.height * 0.5f;

            //Calculate the matrix that converts points in the view port to screen space
            Matrix4x4 viewToScreenMatrix = Matrix4x4.TRS(new Vector3(screenWidth, screenHeight, 0f),
                Quaternion.identity, new Vector3(screenWidth, screenHeight, 1f));

            //Calculate and return the matrix that converts total points to screen space
            return viewToScreenMatrix * worldToViewportMatrix * localToWorldMatrix;
        }

        #endregion

        #region Public Methods

        private void ClearBoundingBoxes()
        {
            boundingBoxTable = new Dictionary<GameObject, Tuple<Vector2, Vector2>>();
        }

        public bool GetDragging()
        {
            return dragging;
        }

        #endregion
    }
}