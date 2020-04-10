using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractionWheel : MonoBehaviour
{
	public Text Label;
	public WheelButton ButtonPrefab;
	public Interactable Selected;
	private List<Interactable> interactables;
	private GameObject player;

	public void OnEnable ()
	{
		if (!Label)
			Label = GetComponentInChildren<Text>();
		Label.text = "";
		base.StartCoroutine(SpawnButtons());
	}

	private IEnumerator SpawnButtons()
	{
		player = GameObject.FindGameObjectsWithTag("Player").Single(x => x.GetComponent<PhotonView>().isMine);
		interactables = player.GetComponent<PlayerInteractable>().Collisions;
		for (int i = 0; i < interactables.Count; i++)
		{
			WheelButton newButton = Instantiate(ButtonPrefab) as WheelButton;
			newButton.transform.SetParent(transform, false);
			newButton.InteractionWheel = this;
			newButton.MyAction = interactables[i];
			newButton.Icon.sprite = interactables[i].Icon;
			float theta = (2 * Mathf.PI / interactables.Count) * i;
			float xPos = Mathf.Sin(theta);
			float yPos = Mathf.Cos(theta);
			newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 200f;
			newButton.InteractionWheel = this;
			newButton.Animate();
			yield return new WaitForSeconds(.05f);

		}
	}

	private void OnDisable()
	{
		//remove all buttons when menu is closed.
		foreach (Transform t in transform)
		{
			if(t.name != "Context")
			{
				Destroy(t.gameObject);
			}
		}
	}

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (Selected && player)
			{
				Selected.Action(player);
			}
		}
	}
}
