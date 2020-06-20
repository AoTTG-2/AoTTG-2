using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class InteractionWheel : MonoBehaviour
{
    public WheelButton ButtonPrefab;
    public Text Label;
    public WheelButton Selected;

    private Vector2 accumulatedDelta;

    private WheelButton IntantiateButton(int count, IEnumerator<Interactable> enumerator, int i)
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
        return newButton;
    }

    private void OnAvailableInteractablesChanged(IEnumerable<Interactable> availableInteractables)
    {
        if (gameObject && gameObject.activeInHierarchy)
        {
            Selected?.Deselect();
            RemoveAllButtons();
            StartCoroutine(SpawnButtons(availableInteractables.ToList()));
        }
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");

        InteractionManager.AvailableInteractablesChanged -= OnAvailableInteractablesChanged;

        if (InteractionManager.Player && Selected)
            Selected.MyAction.Interact(InteractionManager.Player);

        // Remove all buttons when menu is closed.
        RemoveAllButtons();

        GameCursor.ApplyCameraMode();
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");

        GameCursor.CursorMode = CursorMode.InteractionWheel;

        if (!Label)
            Label = GetComponentInChildren<Text>();
        Label.text = string.Empty;

        StartCoroutine(SpawnButtons(InteractionManager.AvailableInteractables.ToArray()));

        InteractionManager.AvailableInteractablesChanged += OnAvailableInteractablesChanged;
    }

    private void RemoveAllButtons()
    {
        foreach (Transform t in transform)
            if (t.name != "Context")
                Destroy(t.gameObject);
    }

    private IEnumerator SpawnButtons(IEnumerable<Interactable> interactables)
    {
        Debug.Log("SpawnButtons");

        var count = interactables.Count();
        using (var enumerator = interactables.GetEnumerator())
        {
            if (enumerator.MoveNext())
                IntantiateButton(count, enumerator, 0).Select();

            for (var i = 1; enumerator.MoveNext(); i++)
            {
                yield return new WaitForSeconds(.05f);
                IntantiateButton(count, enumerator, i);
            }
        }
    }

    private void Update()
    {
        // TODO: Better selection.
        var mouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));
        var shortest = float.MaxValue;
        var amplifier = 100f;
        accumulatedDelta = Vector2.ClampMagnitude(accumulatedDelta + mouseDelta * amplifier, 100f);
        var accumulatedPosition = (Vector2) Input.mousePosition + accumulatedDelta;
        foreach (Transform t in transform)
            if (t.name != "Context")
            {
                var current = ((Vector2) t.position - accumulatedPosition).sqrMagnitude;
                if (current < shortest)
                {
                    shortest = current;
                    Selected.Deselect();
                    t.GetComponent<WheelButton>().Select();
                }
            }
    }
}