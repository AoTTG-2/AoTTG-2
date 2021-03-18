using System;
using System.Text;
using UnityEngine;

namespace MapEditor
{
    public class SpawnerObject : MapObject
    {
        #region Properties
        //The amount of time until the titan spawns
        public float SpawnTimer { get; set; }
        //Determines if the spawner will continue to spawn titans
        public bool EndlessSpawn { get; set; }

        //Prevent the spawner from being scaled
        public override Vector3 Scale
        {
            get { return DefaultScale; }
            set { }
        }

        //Prevent the spawner from being rotated
        public override Quaternion Rotation
        {
            get { return Quaternion.identity; }
            set { }
        }
        #endregion

        #region Initialization
        //Copy the values from the given object
        public void CopyValues(SpawnerObject originalObject)
        {
            SpawnTimer = originalObject.SpawnTimer;
            EndlessSpawn = originalObject.EndlessSpawn;
        }

        //Sets all of the object properties except for the type based on the parsed object script
        public override void LoadProperties(string[] properties)
        {
            base.LoadProperties(properties);

            SpawnTimer = Convert.ToSingle(properties[2]);
            EndlessSpawn = (Convert.ToInt32(properties[3]) != 0);
            Position = ParseVector3(properties[4], properties[5], properties[6]);
            Rotation = Quaternion.identity;
        }
        #endregion

        #region Update
        //If the object was rotated, set it back to the default rotation
        private void LateUpdate()
        {
            if (transform.hasChanged && SelectionHandle.Instance.Tool == Tool.Rotate)
            {
                transform.rotation = Quaternion.identity;
                transform.hasChanged = false;
            }
        }
        #endregion

        #region Methods
        //Convert the map object into a script
        public override string ToString()
        {
            //Create a string builder to efficiently construct the script
            //Initialize with a starting buffer with enough room to fit a long object script
            StringBuilder scriptBuilder = new StringBuilder(100);

            //Append the object type and name to the script
            scriptBuilder.Append(FullTypeName + "," + ObjectName);
            //Append the titan spawning settings
            scriptBuilder.Append("," + SpawnTimer + "," + BoolToString(EndlessSpawn));
            //Append the transform values
            scriptBuilder.Append("," + Vector3ToString(Position) + "," + QuaternionToString(Rotation) + ";");

            //Get the script string and return it
            return scriptBuilder.ToString();
        }
        #endregion
    }
}