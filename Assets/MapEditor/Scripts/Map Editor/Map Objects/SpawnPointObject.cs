using System.Text;
using UnityEngine;

namespace MapEditor
{
    public class SpawnPointObject : MapObject
    {
        #region Properties
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
        //Sets all of the object properties except for the type based on the parsed object script
        public override void LoadProperties(string[] properties)
        {
            base.LoadProperties(properties);

            Position = ParseVector3(properties[2], properties[3], properties[4]);
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
            //Append the transform values
            scriptBuilder.Append("," + Vector3ToString(Position) + "," + QuaternionToString(Rotation) + ";");

            //Get the script string and return it
            return scriptBuilder.ToString();
        }
        #endregion
    }
}