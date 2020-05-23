using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingScrollView : MonoBehaviour {

    public float scrollSpeed = 0.5f;
	private bool autoScroll = true;
	private float timer = 0;
	private float time = 0;	
	private float waitFor = 2f;
	public ScrollRect scrollRect;

    public RectTransform rectTransform;

	
	void OnEnable()
	{
		scrollRect.verticalNormalizedPosition = 1;
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
		} else if(autoScroll) 
		{
			rectTransform.localPosition += new Vector3(0f, scrollSpeed, 0f);
		}
    }
	
}