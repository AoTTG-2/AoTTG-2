using UnityEngine;
using OutlineEffect;

namespace MapEditor
{
    public static class AssetManager
    {
        //The locations of the vanilla resources
        private const string VanillaPrefabFolder = "Vanilla Resources/Vanilla Prefabs/";

        private const string VanillaMaterialFolder = "Vanilla Resources/Vanilla Materials/";

        //The locations of the RC resources
        private const string RcPrefabFolder = "RC Resources/RC Prefabs/";
        private const string RcMaterialFolder = "RC Resources/RC Materials/";

        //Instantiate the vanilla object with the given name
        public static GameObject InstantiateVanillaObject(string objectName)
        {
            GameObject newObject = Object.Instantiate(Resources.Load<GameObject>(VanillaPrefabFolder + objectName));
            AddObjectToMap(newObject);

            return newObject;
        }

        //Instantiate the RC object with the given name
        public static GameObject InstantiateRcObject(string objectName)
        {
            GameObject newObject = Object.Instantiate(Resources.Load<GameObject>(RcPrefabFolder + objectName));
            AddObjectToMap(newObject);

            return newObject;
        }

        //Make the given object selectable and tag it as a map object
        private static void AddObjectToMap(GameObject newObject)
        {
            //If the game object has a mesh, add the outline script
            if (newObject.GetComponent<Renderer>() != null)
            {
                newObject.AddComponent<Outline>();
                newObject.gameObject.tag = "Selectable";
            }

            //Go through the children of the object and add the outline script if it has a mesh
            foreach (Transform child in newObject.transform)
                if (child.GetComponent<ParticleSystem>() == null)
                    AddObjectToMap(child.gameObject);
        }

        //Load the given vanilla material
        public static Material LoadVanillaMaterial(string materialName)
        {
            return Resources.Load<Material>(VanillaMaterialFolder + materialName + "/" + materialName);
        }

        //Load the given RC material
        public static Material LoadRcMaterial(string materialName)
        {
            return Resources.Load<Material>(RcMaterialFolder + materialName + "/" + materialName);
        }
    }
}