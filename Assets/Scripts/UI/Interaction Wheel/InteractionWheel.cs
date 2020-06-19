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

    private void OnAvailableInteractablesChanged(IEnumerable<Interactable> availableInteractables)
    {
        if (gameObject.activeInHierarchy)
        {
            RemoveAllButtons();
            StartCoroutine(SpawnButtons(availableInteractables.ToList()));
        }
    }

    private void OnDisable()
    {
        InteractionManager.AvailableInteractablesChanged -= OnAvailableInteractablesChanged;

        // Remove all buttons when menu is closed.
        RemoveAllButtons();
    }

    private void OnEnable()
    {
        if (!Label)
            Label = GetComponentInChildren<Text>();
        Label.text = string.Empty;

        StartCoroutine(SpawnButtons(InteractionManager.AvailableInteractables.ToArray()));
    }

    private void RemoveAllButtons()
    {
        foreach (Transform t in transform)
        {
            if (t.name != "Context")
                Destroy(t.gameObject);
        }
    }

    private IEnumerator SpawnButtons(IEnumerable<Interactable> interactables)
    {
        var count = interactables.Count();
        using (var enumerator = interactables.GetEnumerator())
        {
            for (var i = 0; enumerator.MoveNext(); i++)
            {
                var interactable = enumerator.Current;
                var newButton = Instantiate(ButtonPrefab);
                newButton.transform.SetParent(transform, false);
                newButton.InteractionWheel = this;
                newButton.MyAction = interactable;
                newButton.Icon.sprite = interactable.Icon;
                var theta = (2 * Mathf.PI / count) * i;
                var xPos = Mathf.Sin(theta);
                var yPos = Mathf.Cos(theta);
                newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 200f;
                newButton.InteractionWheel = this;
                newButton.Animate();
                yield return new WaitForSeconds(.05f);
            }
        }

        InteractionManager.AvailableInteractablesChanged += OnAvailableInteractablesChanged;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Selected && InteractionManager.Player)
        {
            Selected.Interact(InteractionManager.Player);
            gameObject.SetActive(false);
        }
    }
}