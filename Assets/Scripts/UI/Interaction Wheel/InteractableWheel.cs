using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractableWheel : MonoBehaviour {

	public Text label;
	public WheelButton buttonPrefab;
	public Interactable selected;
    private List<Interactable> interactables;
    private GameObject player;

    public void OnEnable ()
    {
        label.text = "";
        base.StartCoroutine(SpawnButtons());
    }

    IEnumerator SpawnButtons()
    {
        player = GameObject.FindGameObjectsWithTag("Player").Single(x => x.GetComponent<PhotonView>().isMine);
        interactables = player.GetComponent<PlayerInteractable>().Collisions;
        for (int i = 0; i < interactables.Count; i++)
        {
            WheelButton newButton = Instantiate(buttonPrefab) as WheelButton;
            newButton.transform.SetParent(transform, false);
            newButton.myMenu = this;
            newButton.myAction = interactables[i];
            //borrowed math.
            float theta = (2 * Mathf.PI / interactables.Count) * i;
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);
            newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 200f;
            newButton.myMenu = this;
            newButton.Anim();
            //add delay for cool effect.
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
            if (selected && player) //check if something is selected and that the player still exists.
            {
                selected.Action(player);
                Debug.Log($"{selected.name} selected!");
            }
        }
    }
}
