using System.Text;

namespace MapEditor
{
    public class RacingObject : MapObject
    {
        //Sets all of the object properties except for the type based on the parsed object script
        public override void LoadProperties(string[] properties)
        {
            base.LoadProperties(properties);

            Scale = ParseVector3(properties[2], properties[3], properties[4]);
            Position = ParseVector3(properties[5], properties[6], properties[7]);
            Rotation = ParseQuaternion(properties[8], properties[9], properties[10], properties[11]);
        }

        //Convert the map object into a script
        public override string ToString()
        {
            //Create a string builder to efficiently construct the script
            //Initialize with a starting buffer with enough room to fit a long object script
            StringBuilder scriptBuilder = new StringBuilder(100);

            //Append the object type and name to the script
            scriptBuilder.Append(FullTypeName + "," + ObjectName);
            //Append the transform values
            scriptBuilder.Append("," + Vector3ToString(Scale) + "," + Vector3ToString(Position) + "," + QuaternionToString(Rotation) + ";");

            //Get the script string and return it
            return scriptBuilder.ToString();
        }
    }
}