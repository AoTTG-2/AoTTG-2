using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class InteractionWheel : MonoBehaviour
{
    public WheelButton ButtonPrefab;
    public Text Label;
    public Interactable Selected;
    private List<Interactable> interactables;
    private GameObject player;

    public void OnEnable()
    {
        if (!Label)
            Label = GetComponentInChildren<Text>();
        Label.text = string.Empty;

        // There may be 0 players, in the case of the player spawning as a titan.
        player = GameObject.FindGameObjectsWithTag("Player").SingleOrDefault(x => x.GetComponent<PhotonView>().isMine);
        if (player)
        {
            interactables = player.GetComponent<PlayerInteractable>().Collisions;
            StartCoroutine(SpawnButtons());
        }
    }

    private void OnDisable()
    {
        // Remove all buttons when menu is closed.
        foreach (Transform t in transform)
        {
            if (t.name != "Context")
                Destroy(t.gameObject);
        }
    }

    private IEnumerator SpawnButtons()
    {
        for (var i = 0; i < interactables.Count; i++)
        {
            var newButton = Instantiate(ButtonPrefab);
            newButton.transform.SetParent(transform, false);
            newButton.InteractionWheel = this;
            newButton.MyAction = interactables[i];
            newButton.Icon.sprite = interactables[i].Icon;
            var theta = (2 * Mathf.PI / interactables.Count) * i;
            var xPos = Mathf.Sin(theta);
            var yPos = Mathf.Cos(theta);
            newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 200f;
            newButton.InteractionWheel = this;
            newButton.Animate();
            yield return new WaitForSeconds(.05f);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Selected && player)
            Selected.Action(player);
    }
}