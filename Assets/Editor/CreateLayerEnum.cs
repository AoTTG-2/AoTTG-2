using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{

    /// <summary>
    /// This Class automatically creates the Layers.Enum.cs at <see cref="GENERATED_FILE_PATH"/>
    /// </summary>
    public class CreateLayerEnum
    {
        private const string GENERATED_FILE_PATH = "Assets/Scripts/Constants/Layers.Enum.cs";


        [InitializeOnLoadMethod]
        private static void SerializeLayersToClass()
        {
            var layers = new List<(string name, int value)>();

            for(var i = 0; i < 32; i++)
            {
                var name = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(name))
                {
                    layers.Add((name, i));
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine("// This script is automatically updated by CreateLayerEnum.cs");
            sb.AppendLine("// Any changes made to this script WILL BE LOST!");
            sb.AppendLine();

            sb.AppendLine("namespace Assets.Scripts.Constants");
            sb.AppendLine("{");
            
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// Use with <see cref=\"Layer.ToName(Layers)\"/>");
            sb.AppendLine("\t/// </summary>");

            sb.AppendLine("\tpublic enum Layers");
            sb.AppendLine("\t{");

            foreach(var l in layers)
            {
                sb.Append("\t\t").Append(l.name.Replace(" ", "_")).Append(" = ").Append(l.value).AppendLine(",");
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            File.WriteAllText(GENERATED_FILE_PATH, sb.ToString());
        }
    }

}