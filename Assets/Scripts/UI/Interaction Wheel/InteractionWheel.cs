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

    private readonly WaitForSeconds buttonSpawnInterval = new WaitForSeconds(.05f);

    private Vector2 accumulatedDelta;
    private Coroutine spawnButtonsCoroutine;

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
                    Selected?.Deselect();
                    t.GetComponent<WheelButton>().Select();
                }
            }
    }

    private void OnEnable()
    {
        GameCursor.CursorMode = CursorMode.InteractionWheel;

        if (!Label)
            Label = GetComponentInChildren<Text>();
        Label.text = string.Empty;

        SpawnButtons();

        InteractionManager.AvailableInteractablesChanged += OnAvailableInteractablesChanged;
    }

    private void OnDisable()
    {
        InteractionManager.AvailableInteractablesChanged -= OnAvailableInteractablesChanged;

        if (InteractionManager.Player && Selected)
            Selected.MyAction.Interact(InteractionManager.Player);

        // Remove all buttons when menu is closed.
        RemoveAllButtons();

        GameCursor.ApplyCameraMode();
    }

    private WheelButton InstantiateButton(int count, Interactable interactable, int i)
    {
        var newButton = Instantiate(ButtonPrefab, transform, false);
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
            spawnButtonsCoroutine = StartCoroutine(SpawnButtonsCoroutine(availableInteractables.ToList()));
        }
    }

    private void SpawnButtons()
    {
        StartCoroutine(SpawnButtonsCoroutine(InteractionManager.AvailableInteractables.ToList()));
    }

    private void RemoveAllButtons()
    {
        if (spawnButtonsCoroutine != null)
        {
            StopCoroutine(spawnButtonsCoroutine);
            spawnButtonsCoroutine = null;
        }

        foreach (Transform t in transform)
            if (t.name != "Context")
                Destroy(t.gameObject);
    }

    private IEnumerator SpawnButtonsCoroutine(List<Interactable> interactables)
    {
        var count = interactables.Count;
        using (var enumerator = interactables.GetEnumerator())
        {
            if (enumerator.MoveNext())
                InstantiateButton(count, enumerator.Current, 0).Select();
            
            for (var i = 1; enumerator.MoveNext(); i++)
            {
                yield return buttonSpawnInterval;
                InstantiateButton(count, enumerator.Current, i);
            }
        }

        spawnButtonsCoroutine = null;
    }
}