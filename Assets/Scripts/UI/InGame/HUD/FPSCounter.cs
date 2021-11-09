using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI;

public class FPSCounter : UiElement {

	private float time;
    private float avgUpdateTime;
	[SerializeField] public Text counterText;
	public Text CounterText
	{
		get { return counterText; }
		set { counterText = value; }
	}

	private void Update() {
		Counter();
	}

	public void Counter()
	{
        //the single update time gives info only about the current
        //frame, using the previous value makes it an average value
        //which is more significative than the single update evaluation
        avgUpdateTime = avgUpdateTime * .7f + Time.deltaTime*.3f;
		time += Time.deltaTime;

        //increased update rate to 4 times for second which seems to be the sweet spot for me
		if(time >= 0.25f)
		{
			var fps = 1.0f/this.avgUpdateTime;
			CounterText.text = fps.ToString("N0");
			time = 0f;
		}
		
	}
}
