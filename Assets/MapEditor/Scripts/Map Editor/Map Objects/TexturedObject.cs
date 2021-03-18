using System;
using System.Text;
using UnityEngine;

namespace MapEditor
{
    public class TexturedObject : MapObject
    {
        #region Data Members

        private string[] defaultMatNames;
        private string materialValue;
        private Vector2 tilingValue;
        private Color colorValue;
        private bool transparant = false;
        private float colorAlphaValue = 1f;

        #endregion

        #region Properties

        public bool ColorEnabled { get; set; }

        //The name of the material applied to the object
        public string Material
        {
            get { return materialValue; }
            set
            {
                materialValue = value;
                SetMaterial(value);
            }
        }

        //How many times the texture will repeat in the x and y directions
        public Vector2 Tiling
        {
            get { return tilingValue; }
            set
            {
                tilingValue = value;
                SetTiling(value);
            }
        }

        //The color of the object. Does not include the opacity.
        public Color Color
        {
            get { return colorValue; }
            set
            {
                colorValue = value;
                SetColor(value);
            }
        }

        //The opacity of the object
        public float ColorAlpha
        {
            get { return colorAlphaValue; }
            set
            {
                colorAlphaValue = value;
                SetColor(colorValue);
            }
        }

        #endregion

        #region Initialization

        //Copy the values from the given object
        public override void CopyValues(MapObject originalObject)
        {
            //Copy the generic map object values
            base.CopyValues(originalObject);

            //Try to cast the MapObject to a Textured Object
            TexturedObject texturedObject = originalObject as TexturedObject;

            //If the cast succeeded, copy the texture related values
            if (texturedObject != null)
            {
                materialValue = texturedObject.Material;
                tilingValue = texturedObject.Tiling;
                ColorEnabled = texturedObject.ColorEnabled;
                colorValue = texturedObject.Color;
            }
        }

        #endregion

        #region Setters

        //Apply the given material as the new material of the object
        private void SetMaterial(string materialName)
        {
            //Check if the new material is the default materials applied to the prefab
            if (materialName == "default")
            {
                //If the default materials haven't been set yet, the currently applied materials are the defaults
                if (defaultMatNames == null)
                {
                    //Get a list of the default renderers attached to the game object
                    Renderer[] defaultRenderers = GetComponentsInChildren<Renderer>();
                    //Create a string array for the material names
                    defaultMatNames = new string[defaultRenderers.Length];

                    //Iterate through the renderers and save all of the material names
                    for (int matIndex = 0; matIndex < defaultRenderers.Length; matIndex++)
                        defaultMatNames[matIndex] = defaultRenderers[matIndex].material.name;
                }
                //If the default materials weren't set, apply the saved default materials
                else
                {
                    //Get a list of the renderers currently attached to the game object
                    Renderer[] renderers = GetComponentsInChildren<Renderer>();

                    //Instantiate all of the default materials and assign them to the renderers
                    for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
                        renderers[rendererIndex].material = AssetManager.LoadRcMaterial(defaultMatNames[rendererIndex]);
                }
            }
            //Otherwise Apply the material to all of the children of the object
            else
            {
                //Set the transparent flag based on the material name
                transparant = (materialName == "transparent");

                foreach (Renderer render in gameObject.GetComponentsInChildren<Renderer>())
                {
                    //Don't apply the default material and don't apply the material to the particle system of supply stations
                    if (!(render.name.Contains("Particle System") && ObjectName.StartsWith("aot_supply")))
                        render.material = AssetManager.LoadRcMaterial(materialName);
                }
            }
        }

        //Resize the texture on the object
        private void SetTiling(Vector2 newTiling)
        {
            //If the material is the default on the game object, don't scale the texture
            if (materialValue == "default")
                return;

            //Apply the texture resizing to all of the children of the object
            foreach (Renderer render in gameObject.GetComponentsInChildren<Renderer>())
                render.material.mainTextureScale = new Vector2(render.material.mainTextureScale.x * newTiling.x,
                    render.material.mainTextureScale.y * newTiling.y);
        }

        //Change the color of the material on the object
        private void SetColor(Color newColor)
        {
            //If the object is transparent, set the color of the material
            if (transparant)
            {
                newColor.a = colorAlphaValue;

                foreach (Renderer render in gameObject.GetComponentsInChildren<Renderer>())
                    render.material.color = newColor;
            }
            //If the object is not transparent, set the color of the mesh vertices
            else
            {
                //Iterate through all of the filters in the object
                foreach (MeshFilter filter in gameObject.GetComponentsInChildren<MeshFilter>())
                {
                    Mesh mesh = filter.mesh;

                    //Create an array filled with the new color to apply to the mesh
                    Color[] colorArray = new Color[mesh.vertexCount];

                    for (int colorIndex = 0; colorIndex < colorArray.Length; colorIndex++)
                        colorArray[colorIndex] = newColor;

                    //Apply the colors
                    mesh.colors = colorArray;
                }
            }
        }

        #endregion

        #region Methods

        //Takes an array containing a parsed object script and set all of the variables except for the type
        public override void LoadProperties(string[] properties)
        {
            base.LoadProperties(properties);

            //If the script is too short, only parse the position and rotation
            if (properties.Length < 19)
                LoadPropertiesPartial(properties);
            //Otherwise parse all of the object info from the script
            else
                LoadPropertiesFull(properties);
        }

        //Load the position and rotation from the object script, and set the texture information to a default value
        private void LoadPropertiesPartial(string[] properties)
        {
            Scale = DefaultScale;
            Position = ParseVector3(properties[2], properties[3], properties[4]);
            Rotation = ParseQuaternion(properties[5], properties[6], properties[7], properties[8]);

            //Disable the color by default
            ColorEnabled = false;
            Color = Color.white;

            //Get a list of the renderers in the object hierarchy
            Renderer[] childRenderers = gameObject.GetComponentsInChildren<Renderer>();

            //Loop through the materials and check if they are the same
            for (int rendererIndex = 0; rendererIndex < childRenderers.Length; rendererIndex++)
            {
                //Skip the renderer if it is the particle system of the resupply station
                if (childRenderers[rendererIndex].name.Contains("Particle System") &&
                    ObjectName.StartsWith("aot_supply"))
                    continue;

                //Use the material and tiling of the first renderer for the object values
                if (rendererIndex == 0)
                {
                    materialValue = childRenderers[rendererIndex].material.name;
                    tilingValue = childRenderers[rendererIndex].material.mainTextureScale;
                }
                //If a subsequent renderer has different settings than the first, set the material and texture to null
                else if (childRenderers[rendererIndex].material.name != Material ||
                         childRenderers[rendererIndex].material.mainTextureScale != Tiling)
                {
                    materialValue = null;
                    tilingValue = Vector2.zero;
                    break;
                }
            }
        }

        //Load all of the object properties from the object script
        private void LoadPropertiesFull(string[] properties)
        {
            //If the material is the transparent material, parse the alpha value
            if (properties[2].StartsWith("transparent"))
            {
                transparant = true;
                Material = "transparent";

                //Try to parse the substring after 'transparent' as a float and use it as the alpha
                if (float.TryParse(properties[2].Substring(11), out float parsedFloat))
                    colorAlphaValue = parsedFloat;
                //If the parse fails, make the object opaque
                else
                    colorAlphaValue = 1f;
            }
            //Otherwise use the material name as is
            else
                Material = properties[2];

            //Parse the length, width, and hight
            Scale = ParseVector3(properties[3], properties[4], properties[5]);
            //The 'color enabled' field must be the exact string '0' for the color to be disabled
            //Any other value, numeric or otherwise, will be interpreted as the color is enabled
            ColorEnabled = (properties[6] != "0");

            //If the color is enabled, parse the color and set it
            if (ColorEnabled)
                Color = ParseColorRGB(properties[7], properties[8], properties[9]);
            //Otherwise, use white as a default color
            else
                Color = Color.white;

            Tiling = ParseVector2(properties[10], properties[11]);
            Position = ParseVector3(properties[12], properties[13], properties[14]);
            Rotation = ParseQuaternion(properties[15], properties[16], properties[17], properties[18]);
        }

        //Convert the map object into a script
        public override string ToString()
        {
            //Create a string builder to efficiently construct the script
            //Initialize with a starting buffer with enough room to fit a long object script
            StringBuilder scriptBuilder = new StringBuilder(100);

            //The extended name of the material
            string fullMaterialName = Material;

            //If this object has the transparent material, add the opacity value to the end
            if (transparant)
                fullMaterialName += colorAlphaValue;

            //Append the object type and name to the script
            scriptBuilder.Append(FullTypeName + "," + ObjectName);
            //Append the material and scale values
            scriptBuilder.Append("," + fullMaterialName + "," + Vector3ToString(Scale) + "," +
                                 BoolToString(ColorEnabled) + "," + ColorToString(Color) + "," +
                                 Vector2ToString(Tiling));
            //Append the transform values
            scriptBuilder.Append("," + Vector3ToString(Position) + "," + QuaternionToString(Rotation) + ";");

            //Get the script string and return it
            return scriptBuilder.ToString();
        }

        #endregion
    }
}