using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

namespace MapEditor
{
    //A singleton class for creating and deleting map objects
    public class MapManager : MonoBehaviour
    {
        #region Fields

        //A reference to the root for all copied objects
        [SerializeField] private GameObject copiedObjectsRoot;

        //Determines if there are copied objects saved
        private bool selectionCopied = false;

        //A reference to the root for all deleted objects
        [SerializeField] private GameObject deletedObjectsRoot;

        //A reference to the empty map to add objects to
        [SerializeField] private GameObject mapRoot;

        //References to the large and small map boundaries
        [SerializeField] private GameObject smallMapBounds;
        [SerializeField] private GameObject largeMapBounds;

        //The maximum in-game frame rate while loading or exporting
        [SerializeField] private float maxLoadingFPS = 60f;

        //The maximum amount of time a coroutine can run before returning. Calculated from minimum loading frame rate
        private float loadingDelay;

        //A dictionary mapping game objects to MapObject scripts
        public Dictionary<GameObject, MapObject> objectScriptTable;

        //Determines if the small map bounds have been disabled or not
        private bool boundsDisabled;

        #endregion

        #region Properties

        //A self-reference to the singleton instance of this script
        public static MapManager Instance { get; private set; }

        //Static properties to access private instance data members
        public Dictionary<GameObject, MapObject> ObjectScriptTable
        {
            get { return Instance.objectScriptTable; }
            private set { Instance.objectScriptTable = value; }
        }

        private bool BoundsDisabled
        {
            get { return Instance.boundsDisabled; }
            set { Instance.boundsDisabled = value; }
        }

        #endregion

        #region Delegates

        public delegate void OnImportEvent(HashSet<GameObject> imported);

        public event OnImportEvent OnImport;

        public delegate void OnPasteEvent(HashSet<GameObject> pastedObjects);

        public event OnPasteEvent OnPaste;

        public delegate void OnDeleteEvent(HashSet<GameObject> deletedObjects);

        public event OnDeleteEvent OnDelete;

        public delegate void OnUndeleteEvent(HashSet<GameObject> undeletedObjects);

        public event OnUndeleteEvent OnUndelete;

        #endregion

        #region Initialization

        //Set this script as the only instance of the MapManager script
        void Awake()
        {
            if (Instance == null)
                Instance = this;

            //Instantiate the script table
            ObjectScriptTable = new Dictionary<GameObject, MapObject>();
            //Calculate the delay between each frame while loading in milliseconds
            loadingDelay = 1f / maxLoadingFPS * 1000;
        }

        #endregion

        #region Edit Commands

        //Delete and undelete the pasted objects
        private class PasteSelectionCommand : EditCommand
        {
            private GameObject[] previousSelection;
            private GameObject[] pastedObjects;

            //Store the previously selected and pasted objects (must be called after the objects are pasted)
            public PasteSelectionCommand(GameObject[] previousSelection)
            {
                this.previousSelection = previousSelection;
                this.pastedObjects = ObjectSelection.Instance.GetSelection().ToArray();
            }

            //Undelete the pasted objects (must have been deleted first)
            public override void ExecuteEdit()
            {
                Instance.UndeleteObjects(pastedObjects);
            }

            //Remove the pasted objects by deleting them
            public override void RevertEdit()
            {
                Instance.DeleteSelection();

                //Reselect the objects that were previously selected
                foreach (GameObject mapObject in previousSelection)
                    ObjectSelection.Instance.SelectObject(mapObject);
            }
        }

        //Delete the selected objects
        private class DeleteSelectionCommand : EditCommand
        {
            private GameObject[] deletedObjects;

            public DeleteSelectionCommand()
            {
                //Save the objects to be deleted
                deletedObjects = ObjectSelection.Instance.GetSelection().ToArray();
            }

            //Delete the current selection
            public override void ExecuteEdit()
            {
                Instance.DeleteSelection();
            }

            //Undelete the objects that were deleted
            public override void RevertEdit()
            {
                Instance.UndeleteObjects(deletedObjects);
            }
        }

        #endregion

        #region Update

        void Update()
        {
            //Stores the command that needs to be executed
            EditCommand editCommand = null;

            //If the game is in edit mode, check for keyboard shortcut inputs
            if (EditorManager.Instance.CurrentMode == EditorMode.Edit)
            {
                //Check the delete keys
                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
                {
                    //Only create a delete command if there are any selected objects
                    if (ObjectSelection.Instance.GetSelectionCount() > 0)
                    {
                        editCommand = new DeleteSelectionCommand();
                        editCommand.ExecuteEdit();
                    }
                }
                //Check for copy & paste shortcuts
                else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand))
                {
                    if (Input.GetKeyDown(KeyCode.C))
                        CopySelection();
                    else if (Input.GetKeyDown(KeyCode.V))
                    {
                        //Only paste if there are any copied objects
                        if (selectionCopied)
                        {
                            GameObject[] previousSelection = ObjectSelection.Instance.GetSelection().ToArray();
                            PasteSelection();
                            editCommand = new PasteSelectionCommand(previousSelection);
                        }
                    }
                }

                //If a selection was made, add it to the history
                if (editCommand != null)
                    EditHistory.Instance.AddCommand(editCommand);
            }
        }

        #endregion

        #region Shortcut Methods

        //Copy a selection by cloning all of the selected objects and storing them
        private void CopySelection()
        {
            //Get a reference to the list of selected objects
            HashSet<GameObject> selectedObjects = ObjectSelection.Instance.GetSelection();

            //If there aren't any objects to copy, return
            if (selectedObjects.Count == 0)
                return;

            //Destroy any previously copied objects
            foreach (Transform copiedObject in Instance.copiedObjectsRoot.transform)
                Destroy(copiedObject.gameObject);

            //Temporary GameObject to disable cloned objects before storing them
            GameObject objectClone;

            //Clone each selected object and save it in the copied objects list
            foreach (GameObject mapObject in selectedObjects)
            {
                //Instantiate and disable the copied objects
                objectClone = Instantiate(mapObject);
                objectClone.SetActive(false);
                //Get a reference to the cloned object's MapObject script
                MapObject mapObjectScript = objectClone.GetComponent<MapObject>();
                //Copy the values of the original map object script
                mapObjectScript.CopyValues(mapObject.GetComponent<MapObject>());
                //Set the object as the child of the copied objects root
                objectClone.transform.parent = Instance.copiedObjectsRoot.transform;
            }

            selectionCopied = true;
        }

        //Paste the copied objects by instantiating them
        private void PasteSelection()
        {
            //Temporary GameObject to enable cloned objects before storing them
            GameObject objectClone;
            //Reset the current selection
            ObjectSelection.Instance.DeselectAll();

            //Loop through all of the copied objects
            foreach (Transform copiedObject in Instance.copiedObjectsRoot.transform)
            {
                //Instantiate and enable the cloned object
                objectClone = Instantiate(copiedObject.gameObject);
                objectClone.SetActive(true);
                //Get a reference to the cloned object's MapObject script
                MapObject mapObjectScript = objectClone.GetComponent<MapObject>();
                //Copy the values of the original map object script
                mapObjectScript.CopyValues(copiedObject.GetComponent<MapObject>());
                //Add the object to the map and make it selectable
                AddObjectToMap(objectClone, mapObjectScript);
                ObjectSelection.Instance.SelectObject(objectClone);
            }

            //Once the selection is pasted, change the tool type to translate
            ToolButtonManager.SetTool(Tool.Translate);

            //Notify listeners that the copied objects were pasted at the end of the frame
            StartCoroutine(InvokeOnPaste());
        }

        //Delete the selected objects
        private void DeleteSelection()
        {
            //Deselect the selection and get a reference
            HashSet<GameObject> selectedObjects = ObjectSelection.Instance.RemoveSelected();

            //Move the objects under the deleted objects root and hide them
            foreach (GameObject objectToDelete in selectedObjects)
            {
                objectToDelete.transform.parent = Instance.deletedObjectsRoot.transform;
                objectToDelete.SetActive(false);
            }

            //Notify listeners that the selected objects were deleted
            OnDelete?.Invoke(selectedObjects);
        }

        //Add the given objects back into the game
        private void UndeleteObjects(GameObject[] deletedObjects)
        {
            //Activate the object, move it back into the level, and select it
            foreach (GameObject mapObject in deletedObjects)
            {
                mapObject.SetActive(true);
                mapObject.transform.parent = Instance.mapRoot.transform;
                ObjectSelection.Instance.AddSelectable(mapObject);
                ObjectSelection.Instance.SelectObject(mapObject);
            }

            StartCoroutine(InvokeOnUndelete());
        }

        #endregion

        #region Event Invocation

        private IEnumerator InvokeOnImport()
        {
            //Wait until the pasted objects are rendered
            yield return new WaitForEndOfFrame();

            //Notify listeners that the copied objects were pasted
            OnImport?.Invoke(ObjectSelection.Instance.GetSelectable());
        }

        private IEnumerator InvokeOnPaste()
        {
            //Wait until the pasted objects are rendered
            yield return new WaitForEndOfFrame();

            //Notify listeners that the copied objects were pasted
            OnPaste?.Invoke(ObjectSelection.Instance.GetSelection());
        }

        private IEnumerator InvokeOnUndelete()
        {
            //Wait until the pasted objects are rendered
            yield return new WaitForEndOfFrame();

            //Notify listeners that the selected objects were undeleted
            OnUndelete?.Invoke(ObjectSelection.Instance.GetSelection());
        }

        #endregion

        #region Map Methods

        //Delete all of the map objects
        public void clearMap()
        {
            //Remove all deleted objects from the selection lists
            ObjectSelection.Instance.ResetSelection();
            //Reset the hash table for MapObject scripts
            ObjectScriptTable = new Dictionary<GameObject, MapObject>();
            //Reset the boundaries disabled flag and activate the small bounds
            BoundsDisabled = false;
            EnableLargeMapBounds(false);

            //Iterate over all children objects and destroy them
            foreach (Transform child in Instance.mapRoot.GetComponentInChildren<Transform>())
                Destroy(child.gameObject);

            //Iterate over all of the temporarily deleted objects and destroy them
            foreach (Transform child in Instance.deletedObjectsRoot.GetComponentInChildren<Transform>())
                Destroy(child.gameObject);
        }

        //Parse the given map script and load the map
        //Accepts additional parameters for outputting loading progress, total amount of map objects, and a callback function
        public IEnumerator LoadMap(string mapScript, TextMeshProUGUI progressText = null,
            TextMeshProUGUI totalText = null, Action callback = null)
        {
            //Used to keep track of how much time has elapsed between frames
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            //Remove all of the new lines and spaces in the script
            mapScript = mapScript.Replace("\n", "");
            mapScript = mapScript.Replace("\r", "");
            mapScript = mapScript.Replace("\t", "");
            mapScript = mapScript.Replace(" ", "");

            //Separate the map by semicolon
            string[] parsedMap = mapScript.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            //Create each object and add it to the map
            for (int scriptIndex = 0; scriptIndex < parsedMap.Length; scriptIndex++)
            {
                try
                {
                    //Display how many objects have been loaded
                    progressText.text = scriptIndex.ToString();

                    //If the object script starts with '//' ignore it
                    if (parsedMap[scriptIndex].StartsWith("//"))
                        continue;

                    //Parse the object script and create a new map object
                    LoadMapObject(parsedMap[scriptIndex]);
                }
                catch (Exception e)
                {
                    //If there was an issue parsing the object, log the error and skip it
                    Debug.Log("Skipping object on line " + (scriptIndex + 1) + "\t(" + parsedMap[scriptIndex] + ")");
                    Debug.Log(e + ":\t" + e.Message);
                }

                //Get the time elapsed since last frame
                stopWatch.Stop();

                //Check if enough time has passed to start a new frame
                if (stopWatch.ElapsedMilliseconds > loadingDelay)
                {
                    //Update the total number of objects in the script
                    totalText.text = parsedMap.Length.ToString();
                    //Return from the coroutine and render a frame
                    yield return null;
                    //Start counting the elapsed time from zero
                    stopWatch.Restart();
                }
                //Otherwise, resume measuring the elapsed time
                else
                    stopWatch.Start();
            }

            //Notify listeners that a map was loaded at the end of the frame
            StartCoroutine(InvokeOnImport());
            //Run the callback method
            callback();
        }

        //Add the given object to the map hierarchy and make it selectable
        private void AddObjectToMap(GameObject objectToAdd, MapObject objectScript)
        {
            //Make the new object a child of the map root.
            objectToAdd.transform.parent = Instance.mapRoot.transform;
            //Make the new object selectable
            ObjectSelection.Instance.AddSelectable(objectToAdd);
            //Add the object and its MapObject script to the dictionary
            ObjectScriptTable.Add(objectToAdd, objectScript);
        }

        //Remove the given object to the map hierarchy and make object selection script
        private void RemoveObjectFromMap(GameObject objectToRemove)
        {
            //Remove the object from the object selection script
            ObjectSelection.Instance.RemoveSelectable(objectToRemove);
            //Remove the object from the script dictionary
            ObjectScriptTable.Remove(objectToRemove);
            //Delete the object itself
            Destroy(objectToRemove);
        }

        //Parse the given object script and instantiate a new GameObject with the data
        private void LoadMapObject(string objectScript)
        {
            //Separate the object script by comma
            string[] parsedScript = objectScript.Split(',');

            try
            {
                //If the script is "map,disableBounds" then set a flag to disable the map boundaries and skip the object
                if (parsedScript[0].StartsWith("map") && parsedScript[1].StartsWith("disablebounds"))
                {
                    BoundsDisabled = true;
                    EnableLargeMapBounds(true);
                    return;
                }

                //If the length of the string is too short, raise an error
                if (parsedScript.Length < 9)
                    throw new Exception("Too few elements in object script");

                //Use the object script to create the map object
                CreateMapObject(parsedScript);
            }
            //If there are any other errors, destroy the created object
            catch (Exception e)
            {
                throw e;
            }
        }

        //Convert the map into a script
        public override string ToString()
        {
            //Create a string builder to efficiently construct the script
            //Initialize with a starting buffer with enough room to fit a large map script
            StringBuilder scriptBuilder = new StringBuilder(100000);

            //If bounds are disabled, append the bounds disable script
            if (BoundsDisabled)
                scriptBuilder.AppendLine("map,disablebounds;");

            //Append the script for each active object to the map script
            foreach (MapObject objectScript in ObjectScriptTable.Values)
            {
                if (objectScript.gameObject.activeSelf)
                    scriptBuilder.AppendLine(objectScript.ToString());
            }

            //Get the script string and return it
            return scriptBuilder.ToString();
        }

        #endregion

        #region Parser Helpers

        //If the object exists, disable and destroy it
        private void DestroyObject(GameObject objectToDestroy)
        {
            if (objectToDestroy)
            {
                objectToDestroy.SetActive(false);
                Destroy(objectToDestroy);
            }
        }

        //Toggle between the small and large map bounds being active
        private void EnableLargeMapBounds(bool enabled)
        {
            Instance.smallMapBounds.SetActive(!enabled);
            Instance.largeMapBounds.SetActive(enabled);
        }

        //Load the GameObject from RCAssets with the corresponding object name and attach a MapObject script to it
        private GameObject CreateMapObject(string[] parsedScript)
        {
            //The GameObject loaded from RCAssets corresponding to the object name
            GameObject newObject;

            //Store the object type and name from the parsed script
            ObjectType type = MapObject.ParseType(parsedScript[0]);
            string objectName = parsedScript[1];

            //If the object is a vanilla object, instantiate it from the vanilla assets
            if (type == ObjectType.@base)
                newObject = AssetManager.InstantiateVanillaObject(objectName);
            //If the object is a barrier or region, instantiate the editor version
            else if (objectName == "barrier" || objectName == "region")
                newObject = AssetManager.InstantiateRcObject(objectName + "Editor");
            //Otherwise, instantiate the object from RC assets
            else
                newObject = AssetManager.InstantiateRcObject(objectName);

            //If the object name wasn't valid, raise an error
            if (!newObject)
                throw new Exception("The object '" + objectName + "' does not exist");

            try
            {
                //Attach the MapObject script to the new object
                MapObject mapObjectScript;

                //Attach the appropriate Map Object script based on the object type and name
                if (type == ObjectType.spawnpoint)
                    mapObjectScript = newObject.AddComponent<SpawnPointObject>();
                else if (type == ObjectType.photon && objectName.StartsWith("spawn"))
                    mapObjectScript = newObject.AddComponent<SpawnerObject>();
                else if (type == ObjectType.racing)
                    mapObjectScript = newObject.AddComponent<RacingObject>();
                else if (objectName.StartsWith("region"))
                    mapObjectScript = newObject.AddComponent<RegionObject>();
                else if (objectName.StartsWith("barrier"))
                    mapObjectScript = newObject.AddComponent<BarrierObject>();
                else
                    mapObjectScript = newObject.AddComponent<TexturedObject>();

                //Set the type of the mapObject
                mapObjectScript.Type = type;

                //Use the parsedObject array to set the rest of the properties of the object
                mapObjectScript.LoadProperties(parsedScript);
                //Add the object to the hierarchy and store its script
                AddObjectToMap(newObject, mapObjectScript);

                //Attempt to cast the map object to a textured object
                TexturedObject texturedObject = mapObjectScript as TexturedObject;

                //If the object is a textured object with the default texture, outline only the texture instead of the entire mesh
                if (texturedObject != null && texturedObject.Material == "default")
                    foreach (OutlineEffect.Outline outlineScript in newObject
                        .GetComponentsInChildren<OutlineEffect.Outline>())
                        outlineScript.alphaIsTransparency = true;

                //Return the new object 
                return newObject;
            }
            //If there was an error converting an element to a float, destroy the object and pass a new exception to the caller
            catch (FormatException)
            {
                DestroyObject(newObject);
                throw new Exception("Error converting data");
            }
            //If there are any other errors, destroy the object and pass them back up to the caller
            catch (Exception e)
            {
                DestroyObject(newObject);
                throw e;
            }
        }

        #endregion
    }
}