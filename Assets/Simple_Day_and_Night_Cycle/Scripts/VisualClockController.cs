using UnityEngine;
using System.Collections;

public class VisualClockController : MonoBehaviour {

	public Transform hourHand;
	public Transform minuteHand;

	float hoursToDegrees = 360f / 60f; //the hour hand moves 6 degrees per second, but only according to the SecondsInAFullDay var
	float minsToDegrees = 360f / 12f; //the minute hand moves 30 degrees per second
	DayAndNightControl controller;

	// Use this for initialization
	void Awake () {
		controller = GameObject.Find ("Day and Night Controller").GetComponent<DayAndNightControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		float currHour = 24 * controller.currentTime;
		float currMin = 60 * (currHour - Mathf.Floor (currHour));

		hourHand.localRotation = Quaternion.Euler (0, 0, currHour * hoursToDegrees);
		minuteHand.localRotation = Quaternion.Euler (0, 0, currMin * minsToDegrees);
	}
}
