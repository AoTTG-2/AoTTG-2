using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Newtonsoft.Json;
public class FPSCounter : MonoBehaviour {

	private float time;
	[SerializeField] public Text counterText;
	public Text CounterText
	{
		get { return counterText; }
		set { counterText = value; }
	}

	private void Start() {
		time = 0f;
	}

	private void Update() {
		Counter();
	}

	public void Counter()
	{
		time += Time.deltaTime;
		if(time >= 1.0f)
		{
			var fps = 1.0f/Time.deltaTime;
			CounterText.text = Convert.ToInt64(fps).ToString();
			time = 0f;
		}
		
	}
}
