using System;
using UnityEngine;

public class PVPchkPtPreset : MonoBehaviour
{
    public float humanPt;
    public int humanPtMax = 15;
    public int id;
    public float interval = 20f;
    public int[] nextChkPtId;
    public int[] prevChkPtId;
    public float size = 30f;
    public float titanPt;
    public int titanPtMax = 15;

    private void START()
    {
    }
}

