using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GeneralGraphics : MonoBehaviour {

	public int counter = 3;

	public void SwitchGraphics()
	{
		Debug.Log("Switching Graphic!");
		var button = gameObject.GetComponentInChildren<Button>();
		button.GetComponentInChildren<Text>().text = IncrementCounter();
	}

	public string IncrementCounter()
	{
		Debug.Log(Graphics.activeTier.ToString());
		if(counter == 2)
		{
			counter = 0;
		}
		else
		{
			counter++;
		}
		
		switch (counter)
		{
			case 0:
				Graphics.activeTier = GraphicsTier.Tier1;
				return "Low";
			case 1:
				Graphics.activeTier = GraphicsTier.Tier2;
				return "Medium";
			case 2:
				Graphics.activeTier = GraphicsTier.Tier3;
				return "High";	
			default:
				Graphics.activeTier = GraphicsTier.Tier2;
				return "Medium";	
		}
	}
}
