//using SimpleJSON;
//using System;
//using UnityEngine;

//internal class Test_CSharp : MonoBehaviour
//{
//    private string m_InGameLog = string.Empty;
//    private Vector2 m_Position = Vector2.zero;

//    private void OnGUI()
//    {
//        this.m_Position = GUILayout.BeginScrollView(this.m_Position, new GUILayoutOption[0]);
//        GUILayout.Label(this.m_InGameLog, new GUILayoutOption[0]);
//        GUILayout.EndScrollView();
//    }

//    private void P(string aText)
//    {
//        this.m_InGameLog = this.m_InGameLog + aText + "\n";
//    }

//    private void Start()
//    {
//        this.Test();
//        Debug.Log("Test results:\n" + this.m_InGameLog);
//    }

//    private void Test()
//    {
//        JSONNode node = JSONNode.Parse("{\"name\":\"test\", \"array\":[1,{\"data\":\"value\"}]}");
//        node["array"][1]["Foo"] = "Bar";
//        this.P("'nice formatted' string representation of the JSON tree:");
//        this.P(node.ToString(string.Empty));
//        this.P(string.Empty);
//        this.P("'normal' string representation of the JSON tree:");
//        this.P(node.ToString());
//        this.P(string.Empty);
//        this.P("content of member 'name':");
//        this.P((string) node["name"]);
//        this.P(string.Empty);
//        this.P("content of member 'array':");
//        this.P(node["array"].ToString(string.Empty));
//        this.P(string.Empty);
//        this.P("first element of member 'array': " + ((string) node["array"][0]));
//        this.P(string.Empty);
//        node["array"][0].AsInt = 10;
//        this.P("value of the first element set to: " + ((string) node["array"][0]));
//        this.P("The value of the first element as integer: " + node["array"][0].AsInt);
//        this.P(string.Empty);
//        this.P("N[\"array\"][1][\"data\"] == " + ((string) node["array"][1]["data"]));
//        this.P(string.Empty);
//        string aText = node.SaveToBase64();
//        string str2 = node.SaveToCompressedBase64();
//        node = null;
//        this.P("Serialized to Base64 string:");
//        this.P(aText);
//        this.P("Serialized to Base64 string (compressed):");
//        this.P(str2);
//        this.P(string.Empty);
//        node = JSONNode.LoadFromBase64(aText);
//        this.P("Deserialized from Base64 string:");
//        this.P(node.ToString());
//        this.P(string.Empty);
//        JSONClass class2 = new JSONClass();
//        class2["version"].AsInt = 5;
//        class2["author"]["name"] = "Bunny83";
//        class2["author"]["phone"] = "0123456789";
//        class2["data"][-1] = "First item\twith tab";
//        class2["data"][-1] = "Second item";
//        class2["data"][-1]["value"] = "class item";
//        class2["data"].Add("Forth item");
//        class2["data"][1] = ((string) class2["data"][1]) + " 'addition to the second item'";
//        class2.Add("version", "1.0");
//        this.P("Second example:");
//        this.P(class2.ToString());
//        this.P(string.Empty);
//        this.P("I[\"data\"][0]            : " + ((string) class2["data"][0]));
//        this.P("I[\"data\"][0].ToString() : " + class2["data"][0].ToString());
//        this.P("I[\"data\"][0].Value      : " + class2["data"][0].Value);
//        this.P(class2.ToString());
//    }
//}

