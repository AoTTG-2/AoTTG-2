using OutlineEffect;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapEditor
{
    //A singleton class for managing the currently selected objects
    public class ObjectSelection : MonoBehaviour
    {
        #region Data Members
        //A self-reference to the singleton instance of this script
        public static ObjectSelection Instance { get; private set; }
        private Camera mainCamera;

        //Shortcut classes to set the shortcut combination and check when its pressed
        [SerializeField] private Shortcut selectAllShortcut;
        [SerializeField] private Shortcut invertSelectionShortcut;

        //A hash set containing the objects that can be selected
        private HashSet<GameObject> selectableObjects = new HashSet<GameObject>();
        //A hash set containing the objects currently selected
        private HashSet<GameObject> selectedObjects = new HashSet<GameObject>();

        //The average point of all the selected objects
        private Vector3 selectionAverage = Vector3.zero;
        //The sum of the points of all the selected objects for calculating the average
        private Vector3 positionSum = Vector3.zero;

        //The bounding box encapsulating all of the selected objects
        Bounds selectionBounds;
        #endregion

        #region Properties
        public Bounds SelectionBounds
        {
            get { return selectionBounds; }
        }
        #endregion

        #region Instantiation
        void Awake()
        {
            //Set this script as the only instance of the ObjectSelection script
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            //Find and store the main camera in the scene
            mainCamera = Camera.main;

            //Add listeners to events in the SelectionHandle and DragSelect classes
            SelectionHandle.Instance.OnHandleFinish += EndSelection;
            SelectionHandle.Instance.OnHandleMove += EditSelection;
            DragSelect.Instance.OnDragEnd += EndDrag;
        }
        #endregion

        #region Selection Edit Commands
        //Add an object to the current selection
        private class SelectAdditiveCommand : EditCommand
        {
            private GameObject selectedObject;

            public SelectAdditiveCommand(GameObject selectedObject)
            {
                this.selectedObject = selectedObject;
            }

            public override void ExecuteEdit()
            {
                Instance.SelectObject(selectedObject);
                Instance.EncapsulateSelectionBounds(selectedObject);
            }

            public override void RevertEdit()
            {
                Instance.DeselectObject(selectedObject);
                Instance.UpdateSelectionBounds();
            }
        }

        //Deselect the current selection and select a single object
        private class SelectReplaceCommand : EditCommand
        {
            private GameObject selectedObject;
            private GameObject[] previousSelection;

            public SelectReplaceCommand(GameObject selectedObject)
            {
                this.selectedObject = selectedObject;
                previousSelection = Instance.selectedObjects.ToArray();
            }

            //Deselect all map objects and select the new object
            public override void ExecuteEdit()
            {
                Instance.DeselectAll();
                Instance.SelectObject(selectedObject);
                Instance.UpdateSelectionBounds();
            }

            //Deselect the new object and re-select the objects that were previously selected
            public override void RevertEdit()
            {
                Instance.DeselectObject(selectedObject);

                foreach (GameObject mapObject in previousSelection)
                    Instance.SelectObject(mapObject);

                Instance.UpdateSelectionBounds();
            }
        }

        private class SelectAllCommand : EditCommand
        {
            //The objects that were previously unselected
            private GameObject[] unselectedObjects;

            public SelectAllCommand()
            {
                //Find the unselected objects by excluding the selected objects from the set of all objects
                unselectedObjects = Instance.selectableObjects.ExcludeToArray(Instance.selectedObjects);
            }

            //Select the objects that aren't selected
            public override void ExecuteEdit()
            {
                foreach (GameObject mapObject in unselectedObjects)
                    Instance.SelectObject(mapObject);

                Instance.UpdateSelectionBounds();
            }

            //Deselect the objects that weren't previously selected
            public override void RevertEdit()
            {
                foreach (GameObject mapObject in unselectedObjects)
                    Instance.DeselectObject(mapObject);

                Instance.UpdateSelectionBounds();
            }
        }

        private class DeselectObjectCommand : EditCommand
        {
            private GameObject deselectedObject;

            public DeselectObjectCommand(GameObject deselectedObject)
            {
                this.deselectedObject = deselectedObject;
            }

            public override void ExecuteEdit()
            {
                Instance.DeselectObject(deselectedObject);
                Instance.UpdateSelectionBounds();
            }

            public override void RevertEdit()
            {
                Instance.SelectObject(deselectedObject);
                Instance.EncapsulateSelectionBounds(deselectedObject);
            }
        }

        private class DeselectAllCommand : EditCommand
        {
            private GameObject[] previousSelection;

            public DeselectAllCommand()
            {
                previousSelection = new GameObject[Instance.selectedObjects.Count];
                Instance.selectedObjects.CopyTo(previousSelection);
            }

            //Deselect all objects
            public override void ExecuteEdit()
            {
                Instance.DeselectAll();
                Instance.ResetSelecitonBounds();
            }

            //Select all of the previously selected objects
            public override void RevertEdit()
            {
                foreach (GameObject mapObject in previousSelection)
                    Instance.SelectObject(mapObject);

                Instance.UpdateSelectionBounds();
            }
        }

        private class InvertSelectionCommand : EditCommand
        {
            public override void ExecuteEdit()
            {
                Instance.InvertSelection();
                Instance.UpdateSelectionBounds();
            }

            public override void RevertEdit()
            {
                Instance.InvertSelection();
                Instance.UpdateSelectionBounds();
            }
        }
        #endregion

        #region Transform Edit Commands
        private class TranslateSelectionCommand : EditCommand
        {
            private Vector3 displacement;
            private Vector3 negativeDisplacement;

            public TranslateSelectionCommand(Vector3 posDisplacement)
            {
                //Save the displacement
                displacement = posDisplacement;

                //Negate the displacement and store it
                negativeDisplacement = new Vector3();

                for (int axis = 0; axis < 3; axis++)
                    negativeDisplacement[axis] = -displacement[axis];
            }

            public override void ExecuteEdit()
            {
                TransformTools.TranslateSelection(Instance.selectedObjects, displacement);

                //Update the selection average
                Instance.TranslateSelectionAverage(displacement);
                SelectionHandle.Instance.Position = Instance.selectionAverage;
            }

            public override void RevertEdit()
            {
                TransformTools.TranslateSelection(Instance.selectedObjects, negativeDisplacement);

                //Update the selection average
                Instance.TranslateSelectionAverage(negativeDisplacement);
                SelectionHandle.Instance.Position = Instance.selectionAverage;
            }
        }

        private class RotateSelectionCommand : EditCommand
        {
            private Quaternion startRotation;
            private Quaternion endRotation;

            private float angleDisplacement;
            private Vector3 rotationAxis;

            public RotateSelectionCommand(Quaternion originalRotation, Quaternion currentRotation)
            {
                startRotation = originalRotation;
                endRotation = currentRotation;

                //Calculate the displacement
                Quaternion rotDisplacement = Quaternion.Inverse(originalRotation) * currentRotation;
                //Store the angle and axis of rotation
                rotDisplacement.ToAngleAxis(out angleDisplacement, out rotationAxis);

                //Multiply the rotation axis by the original rotation to get the axis relative to the handle
                rotationAxis = originalRotation * rotationAxis;
            }

            public override void ExecuteEdit()
            {
                TransformTools.RotateSelection(Instance.selectedObjects, Instance.selectionAverage, rotationAxis, angleDisplacement);
                SelectionHandle.Instance.Rotation = endRotation;
            }

            public override void RevertEdit()
            {
                TransformTools.RotateSelection(Instance.selectedObjects, Instance.selectionAverage, rotationAxis, -angleDisplacement);
                SelectionHandle.Instance.Rotation = startRotation;
            }
        }

        private class ScaleSelectionCommand : EditCommand
        {
            private Vector3 scaleUpFactor;
            private Vector3 scaleDownFactor;

            public ScaleSelectionCommand(Vector3 scaleUpFactor)
            {
                this.scaleUpFactor = scaleUpFactor;

                //Calculate the scale down factor
                scaleDownFactor = new Vector3();

                for (int axis = 0; axis < 3; axis++)
                    scaleDownFactor[axis] = 1f / scaleUpFactor[axis];
            }

            public override void ExecuteEdit()
            {
                TransformTools.ScaleSelection(Instance.selectedObjects, Instance.selectionAverage, scaleUpFactor, false);

                //Recalculate the bounding sphere
                Instance.UpdateSelectionBounds();
            }

            public override void RevertEdit()
            {
                TransformTools.ScaleSelection(Instance.selectedObjects, Instance.selectionAverage, scaleDownFactor, false);

                //Recalculate the bounding sphere
                Instance.UpdateSelectionBounds();
            }
        }
        #endregion

        #region Update Selection Methods
        private void Update()
        {
            //Check for an object selection if in edit mode and nothing is being dragged
            if (EditorManager.Instance.CurrentMode == EditorMode.Edit &&
                EditorManager.Instance.ShortcutsEnabled &&
                EditorManager.Instance.CursorAvailable)
                checkSelect();
        }

        //Test if any objects were clicked
        private void checkSelect()
        {
            //Stores the command that needs to be executed
            EditCommand selectionCommand = null;

            //If escape is pressed, deselect all
            if (Input.GetKeyDown(KeyCode.Escape))
                selectionCommand = new DeselectAllCommand();
            //If escape was not pressed, check for shortcut commands
            else
            {
                if (selectAllShortcut.Pressed())
                {
                    if (selectedObjects.Count > 0)
                        selectionCommand = new DeselectAllCommand();
                    else
                        selectionCommand = new SelectAllCommand();
                }

                if (invertSelectionShortcut.Pressed())
                    selectionCommand = new InvertSelectionCommand();
            }

            //If the mouse was clicked and the cursor is not over the UI, check if any objects were selected
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject(-1))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                bool rayHit = Physics.Raycast(ray, out hit, Mathf.Infinity);

                //Check if nothing was hit or the hit object isn't selectable
                if(!rayHit || hit.transform.gameObject.tag != "Selectable")
                {
                    //If not in additive mode and there is a selection, deselect all
                    if(!Input.GetKey(KeyCode.LeftControl) && Instance.selectedObjects.Count > 0)
                        selectionCommand = new DeselectAllCommand();
                }
                //If an object was clicked, select it
                else
                {
                    //Select the parent of the object
                    GameObject parentObject = GetParent(hit.transform.gameObject);

                    //If left control is not held, deselect all objects and select the clicked object
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        //If the clicked object is already the only selected object, skip it
                        if (Instance.selectedObjects.Count == 1 &&
                            Instance.selectedObjects.Contains(parentObject))
                            return;

                        selectionCommand = new SelectReplaceCommand(parentObject);
                    }
                    //If left control is held, select or deselect the object based on if its currently selected
                    else
                    {
                        if (!selectedObjects.Contains(parentObject))
                            selectionCommand = new SelectAdditiveCommand(parentObject);
                        else
                            selectionCommand = new DeselectObjectCommand(parentObject);
                    }
                }
            }

            //If a selection was made, execute its associated command and add it to the history
            if (selectionCommand != null)
            {
                selectionCommand.ExecuteEdit();
                EditHistory.Instance.AddCommand(selectionCommand);
            }
        }
        #endregion

        #region Event Handler Methods
        //When the handle is released, create a command 
        private void EndSelection()
        {
            //The command to be executed
            EditCommand transformCommand = null;

            switch (SelectionHandle.Instance.Tool)
            {
                case Tool.Translate:
                    //Get the translation displacement from the selection handle
                    Vector3 posDisplacement = SelectionHandle.Instance.Position -
                                              SelectionHandle.Instance.GetStartPosition();
                    
                    //If the handle wasn't moved, don't create a command
                    if(posDisplacement.magnitude == 0)
                        return;

                    //Otherwise, create a translate command
                    transformCommand = new TranslateSelectionCommand(posDisplacement);
                    break;

                case Tool.Rotate:
                    //Get the previous and current rotation of the selection handle
                    Quaternion originalRotation = SelectionHandle.Instance.GetStartRotation();
                    Quaternion currentRotation = SelectionHandle.Instance.Rotation;

                    //If the handle wasn't rotated, don't create a command
                    if (originalRotation == currentRotation)
                        return;

                    //Otherwise, create a rotation command
                    transformCommand = new RotateSelectionCommand(originalRotation, currentRotation);
                    break;

                default:
                    //Get the current scale of the tool handle
                    Vector3 scaleFactor = SelectionHandle.Instance.GetEndScale();

                    //If the selection wasn't scaled, don't create a command
                    if (scaleFactor == Vector3.one)
                        return;

                    //Otherwise, create a scale command
                    transformCommand = new ScaleSelectionCommand(scaleFactor);
                    break;
            }

            if (transformCommand != null)
                EditHistory.Instance.AddCommand(transformCommand);

            Instance.UpdateSelectionBounds();
        }

        //Update the position, rotation, or scale of the object selections based on the tool handle
        private void EditSelection()
        {
            //Determine which tool was used and call the respective transform
            switch (SelectionHandle.Instance.Tool)
            {
                case Tool.Translate:
                    //Get the position displacement and translate the selected objects
                    Vector3 posDisplacement = SelectionHandle.Instance.GetPosDisplacement();
                    TransformTools.TranslateSelection(Instance.selectedObjects, posDisplacement);

                    //Update the selection average
                    TranslateSelectionAverage(posDisplacement);
                    break;

                case Tool.Rotate:
                    //Get the angle and axis and to rotate around
                    Vector3 rotationAxis;
                    float angle = SelectionHandle.Instance.GetRotDisplacement(out rotationAxis);

                    //Rotate the selected objects around the selection average
                    TransformTools.RotateSelection(Instance.selectedObjects, selectionAverage, rotationAxis, angle);
                    break;

                case Tool.Scale:
                    //Get the scale displacement and scale the selected objects
                    Vector3 scaleDisplacement = SelectionHandle.Instance.GetScaleDisplacement();
                    TransformTools.ScaleSelection(Instance.selectedObjects, selectionAverage, scaleDisplacement, false);
                    break;
            }
        }

        //Update the bounding sphere when the drag select is released
        private void EndDrag()
        {
            UpdateSelectionBounds();
        }
        #endregion

        #region Selection Average Methods
        //Add a point to the total average
        private void AddAveragePoint(Vector3 point)
        {
            //Add the point to the total and update the average
            positionSum += point;
            selectionAverage = positionSum / selectedObjects.Count;
            SelectionHandle.Instance.Position = selectionAverage;

            //If the tool handle is not active, activate it
            SelectionHandle.Instance.Show();
        }

        //Add all selected objects to the total average
        private void AddAverageAll()
        {
            //Reset the total
            positionSum = Vector3.zero;

            //Count up the total of all the objects
            foreach (GameObject mapObject in selectedObjects)
                positionSum += mapObject.transform.position;

            //Average the points
            selectionAverage = positionSum / selectableObjects.Count;
            SelectionHandle.Instance.Position = selectionAverage;

            //If the tool handle is not active, activate it
            SelectionHandle.Instance.Show();
        }

        //Remove a point from the total average
        private void RemoveAveragePoint(Vector3 point)
        {
            //Subtract the point to the total and update the average
            positionSum -= point;

            //If there are any objects selected, update the handle position
            if (selectedObjects.Count != 0)
            {
                selectionAverage = positionSum / selectedObjects.Count;
                SelectionHandle.Instance.Position = selectionAverage;
            }
            //Otherwise, disable the tool handle
            else
                SelectionHandle.Instance.Hide();
        }

        //Remove all selected objects from the total average
        private void RemoveAverageAll()
        {
            //Reset the total and average
            positionSum = Vector3.zero;
            selectionAverage = Vector3.zero;

            //Hide the tool handle
            SelectionHandle.Instance.Hide();
        }

        //Updates the average when the whole selection is translated by the same amount
        private void TranslateSelectionAverage(Vector3 displacement)
        {
            positionSum += displacement * Instance.selectedObjects.Count;
            selectionAverage += displacement;
        }
        #endregion

        #region Bounding Sphere Methods
        //Encapsulate the bounds of the given object into the selection bounds
        private void EncapsulateSelectionBounds(GameObject mapObject)
        {
            //Attempt to get the renderer of the target object
            Renderer renderer = mapObject.GetComponent<Renderer>();

            //If the object has a renderer, update the selection bounds
            if (renderer != null)
            {
                if (selectionBounds.extents == Vector3.zero)
                    selectionBounds = renderer.bounds;
                else
                    selectionBounds.Encapsulate(renderer.bounds);
            }

            //Go through the children of the object and check their bounding radii
            foreach (Transform child in mapObject.transform)
                EncapsulateSelectionBounds(child.gameObject);
        }

        //Calculate the bounding sphere of all selected objects
        private void UpdateSelectionBounds()
        {
            //Reset the selection bounding radius
            ResetSelecitonBounds();

            //Check the bounding spheres of each object in the selection
            foreach (GameObject mapObject in selectedObjects)
                EncapsulateSelectionBounds(mapObject);
        }

        //Reset the selection bounds
        private void ResetSelecitonBounds()
        {
            selectionBounds.SetMinMax(Vector3.zero, Vector3.zero);
        }
        #endregion

        #region Select Objects Methods
        //Return the parent of the given object. If there is no parent, return the given object
        private GameObject GetParent(GameObject childObject)
        {
            //The tag of the parent object
            string parentTag = childObject.transform.parent.gameObject.tag;

            //If the parent isn't a map object, return the child
            if (parentTag == "Map" || parentTag == "Group")
                return childObject;

            //Otherwise return the parent of the child
            return childObject.transform.parent.gameObject;
        }

        //Add the given object to the selectable objects list
        public void AddSelectable(GameObject objectToAdd)
        {
            selectableObjects.Add(GetParent(objectToAdd));
        }

        //Remove the given object from both the selectable and selected objects lists
        public void RemoveSelectable(GameObject objectToRemove)
        {
            //Deselect the object
            DeselectObject(objectToRemove);
            //Remove the object from the selectable objects list
            selectableObjects.Remove(GetParent(objectToRemove));
        }

        //Remove any selected objects from both the selected and selectable objects lists
        public HashSet<GameObject> RemoveSelected()
        {
            //Remove the selected objects from the selectable set
            selectableObjects.ExceptWith(Instance.selectedObjects);

            //Clone the selected objects set
            HashSet<GameObject> originalSelection = new HashSet<GameObject>(Instance.selectedObjects);

            //Deselect each object
            foreach (GameObject mapObject in originalSelection)
                DeselectObject(mapObject);

            //Reset the selection average
            RemoveAverageAll();
            //Reset the selection bounding sphere
            ResetSelecitonBounds();

            //Return a reference to the selected objects list
            return originalSelection;
        }

        public void SelectObject(GameObject objectToSelect)
        {
            //Get the parent of the object
            GameObject parentObject = GetParent(objectToSelect);

            //If the object is already selected, skip it
            if (selectedObjects.Contains(parentObject))
                return;

            //Select the object
            selectedObjects.Add(parentObject);
            AddOutline(parentObject);

            //Update the position of the tool handle
            AddAveragePoint(parentObject.transform.position);
            //Reset the rotation on the tool handle
            ResetToolHandleRotation();
        }

        public void SelectAll()
        {
            //Select all objects by copying the selectedObjects list
            selectedObjects = new HashSet<GameObject>(selectableObjects);

            //Add the outline to all of the objects
            foreach (GameObject selectedObject in selectedObjects)
                AddOutline(selectedObject);

            //Update the tool handle position
            AddAverageAll();
            //Reset the rotation on the tool handle
            ResetToolHandleRotation();
        }

        public void DeselectObject(GameObject objectToDeselect)
        {
            //Get the parent of the object
            GameObject parentObject = GetParent(objectToDeselect);

            //If the object isn't selected, skip it
            if (!selectedObjects.Contains(parentObject))
                return;

            //Deselect the object
            selectedObjects.Remove(parentObject);
            RemoveOutline(parentObject);

            //Update the position of the tool handle
            RemoveAveragePoint(parentObject.transform.position);
            //Reset the rotation on the tool handle
            ResetToolHandleRotation();
        }

        public void DeselectAll()
        {
            //If there are no selected objects, return from the function
            if (selectedObjects.Count == 0)
                return;

            //Remove the outline on all selected objects
            foreach (GameObject selectedObject in selectedObjects)
                RemoveOutline(selectedObject);

            //Deselect all objects by deleting the selected objects list
            selectedObjects.Clear();

            //Reset the position of the tool handle
            RemoveAverageAll();
            //Reset the rotation on the tool handle
            ResetToolHandleRotation();
        }

        //Deselect the current selection and select all other objects
        public void InvertSelection()
        {
            //Iterate over all selectable map objects
            foreach (GameObject mapObject in selectableObjects)
                InvertObject(mapObject);
        }

        //Invert the selection on the given object
        public void InvertObject(GameObject mapObject)
        {
            //If the map object is already selected, deselect it
            if (selectedObjects.Contains(mapObject))
                DeselectObject(mapObject);
            //Otherwise, select it
            else
                SelectObject(mapObject);
        }

        //Resets both the selected and selectable object lists
        public void ResetSelection()
        {
            selectedObjects.Clear();
            selectableObjects.Clear();
            RemoveAverageAll();
        }
        #endregion

        #region Selection Getters
        public HashSet<GameObject> GetSelection()
        {
            return Instance.selectedObjects;
        }

        public int GetSelectionCount()
        {
            return Instance.selectedObjects.Count;
        }

        public HashSet<GameObject> GetSelectable()
        {
            return Instance.selectableObjects;
        }

        public int GetSelectableCount()
        {
            return Instance.selectableObjects.Count;
        }
        #endregion

        #region Tool Methods
        //Set the type of the tool handle
        public void SetTool(Tool toolType)
        {
            SelectionHandle.Instance.Tool = toolType;
            ResetToolHandleRotation();
        }

        //Set the rotation of the tool handle based on how many objects are selected
        public void ResetToolHandleRotation()
        {
            //If the tool handle is in rotate mode and only one object is selected, use the rotation of that object
            if ((SelectionHandle.Instance.Tool == Tool.Rotate || SelectionHandle.Instance.Tool == Tool.Scale) && selectedObjects.Count == 1)
            {
                GameObject[] selectedArray = new GameObject[1];
                selectedObjects.CopyTo(selectedArray, 0);
                SelectionHandle.Instance.Rotation = selectedArray[0].transform.rotation;
            }
            //Otherwise reset the rotation
            else
                SelectionHandle.Instance.Rotation = Quaternion.identity;
        }
        #endregion

        #region Outline Methods
        //Add a green outline around a GameObject
        private void AddOutline(GameObject objectToAddOutline)
        {
            //If object has an outline script, enable it
            if (objectToAddOutline.tag == "Selectable")
                objectToAddOutline.GetComponent<Outline>().enabled = true;

            //Go through the children of the object and enable the outline if it is a selectable object
            foreach (Transform child in objectToAddOutline.transform)
                AddOutline(child.gameObject);
        }

        //Remove the green outline shader
        private void RemoveOutline(GameObject objectToRemoveOutline)
        {
            //If object has an outline script, disable it
            if (objectToRemoveOutline.tag == "Selectable")
                objectToRemoveOutline.GetComponent<Outline>().enabled = false;

            //Go through the children of the object and disable the outline if it is a selectable object
            foreach (Transform child in objectToRemoveOutline.transform)
                RemoveOutline(child.gameObject);
        }
        #endregion
    }
}