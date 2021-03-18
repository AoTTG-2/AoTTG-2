using System.Text;
using UnityEngine;

namespace MapEditor
{
    public class RegionObject : MapObject
    {
        #region Fields
        //The underlying value for the RegionName property
        private string regionNameValue;
        //The text component on the billboard script
        private TextMesh billboardContent;

        //The two children game objects attached to the region object
        private GameObject regionModel;
        private GameObject billboardObject;
        #endregion

        #region Properties
        //The name of the region if the object is a region
        public string RegionName
        {
            get { return regionNameValue; }

            set
            {
                regionNameValue = value;
                billboardContent.text = value;
            }
        }

        //Prevent the region from being rotated
        public override Quaternion Rotation
        {
            get { return Quaternion.identity; }
            set { }
        }

        //Scale the region model without scaling the billboard
        public override Vector3 Scale
        {
            get { return regionModel.transform.localScale; }
            set { regionModel.transform.localScale = value; }
        }
        #endregion

        #region Initialization
        private void Awake()
        {
            //Store references to the prefab's child objects
            foreach (Transform childTransform in gameObject.transform)
            {
                GameObject childObject = childTransform.gameObject;

                if (childObject.name == "Region Editor Model")
                    regionModel = childObject;
                else if (childObject.name == "Billboard")
                    billboardObject = childObject;
            }

            //Save a reference to the text mesh component
            billboardContent = billboardObject.GetComponent<TextMesh>();
        }

        //Copy the values from the given object
        public void CopyValues(RegionObject originalObject)
        {
            base.CopyValues(originalObject);

            RegionName = originalObject.RegionName;
        }

        //Sets all of the object properties except for the type based on the parsed object script
        public override void LoadProperties(string[] properties)
        {
            base.LoadProperties(properties);

            RegionName = properties[2];
            Scale = ParseVector3(properties[3], properties[4], properties[5]);
            Position = ParseVector3(properties[6], properties[7], properties[8]);
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
            scriptBuilder.Append(FullTypeName + "," + ObjectName + "," + RegionName);
            //Append the transform values
            scriptBuilder.Append("," + Vector3ToString(Scale) + "," + Vector3ToString(Position) + "," + QuaternionToString(Rotation) + ";");

            //Get the script string and return it
            return scriptBuilder.ToString();
        }
        #endregion
    }
}