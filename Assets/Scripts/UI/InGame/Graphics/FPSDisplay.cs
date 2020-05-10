using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour {

	public Text fps;
	// Update is called once per frame
	void Update () {
		Debug.Log(FrameTimingManager.GetVSyncsPerSecond());
	}
}
