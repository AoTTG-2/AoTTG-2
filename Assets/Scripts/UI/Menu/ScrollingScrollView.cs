using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingScrollView : MonoBehaviour 
{
    public float ScrollSpeed = 0.5f;
	private float timer = 0;
	private float waitFor = 2f;
    public RectTransform RectTransform;
	
	void OnEnable()
	{
		timer = 0;
	}
	
	void Update()
    {
		if (Input.GetMouseButton(0) || Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			timer = 0;
		}
		if(timer < waitFor) 
		{
			timer += Time.deltaTime;
		}
		else
		{
			RectTransform.localPosition += new Vector3(0f, ScrollSpeed, 0f);
		}
    }
	
}