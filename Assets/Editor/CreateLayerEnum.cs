using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

namespace AOTEditor.Tools
{

    /// <summary>
    /// This Class automatically creates the Layers.Enum.cs at <see cref="GENERATED_FILE_PATH"/>
    /// </summary>
    public class CreateLayerEnum
    {
        const string GENERATED_FILE_PATH = "Assets/Scripts/Constants/Layers.Enum.cs";


        [InitializeOnLoadMethod]
        static void SerializeLayersToClass()
        {
            List<(string name ,int value)> layers = new List<(string name, int value)>();

            for(int i = 0; i < 32; i++)
            {
                var name = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(name))
                {
                    layers.Add((name, i));
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// This script is automatically updated by CreateLayerEnum.cs");
            sb.AppendLine("// Any changes made to this script WILL BE LOST!");
            sb.AppendLine();
            
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// Use with <see cref=\"Layer.ToName(Layers)\"/>");
            sb.AppendLine("/// </summary>");

            sb.AppendLine("public enum Layers");
            sb.AppendLine("{");

            foreach(var l in layers)
            {
                sb.Append("\t").Append(l.name.Replace(" ", "_")).Append(" = ").Append(l.value).AppendLine(",");
            }



            sb.AppendLine("}");

            File.WriteAllText(GENERATED_FILE_PATH, sb.ToString());


        }
    }

}