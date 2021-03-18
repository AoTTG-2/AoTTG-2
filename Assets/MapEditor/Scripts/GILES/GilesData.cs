using UnityEngine;

namespace MapEditor
{
    //The three modes the tool handle can be in
    public enum Tool
    {
        Translate,
        Rotate,
        Scale
    }

    //Used in the ToolHandle class for which axis to move the object along
    public enum Axis
    {
        None = 0x0,
        X = 0x1,
        Y = 0x2,
        Z = 0x4
    }

    //Used in the GILES classes to describe different culling options
    public enum Culling
    {
        Back = 0x1,
        Front = 0x2,
        FrontBack = 0x4
    }

    //Used in HandleUntility class to pass raycast data in a nullable container
    public class RaycastHitData
    {
        public Vector3 point;
        public float distance;
        public Vector3 normal;
        public int[] triangle;
    }
}