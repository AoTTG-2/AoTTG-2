using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class InteractionWheel : MonoBehaviour
{
    [SerializeField] private WheelButton buttonPrefab;
    [SerializeField] private Text label;

    private readonly List<WheelButton> buttons = new List<WheelButton>();
    private readonly WaitForSeconds buttonSpawnInterval = new WaitForSeconds(.05f);

    private Vector2 accumulatedDelta;
    private Coroutine spawnButtonsCoroutine;

    public WheelButton Selected { get; set; }

    public string Label
    {
        set { label.text = value; }
    }

    private void Reset()
    {
        label = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        // TODO: Better selection.
        // Consider keeping old code by using strategy pattern.
        // This could allow for hot-switching of prototypes.
        SelectFromMouseDelta();
    }

    private void OnEnable()
    {
        GameCursor.CursorMode = CursorMode.InteractionWheel;

        Label = string.Empty;
        SpawnButtons();

        InteractionManager.AvailableInteractablesChanged += OnAvailableInteractablesChanged;
    }

    private void OnDisable()
    {
        InteractionManager.AvailableInteractablesChanged -= OnAvailableInteractablesChanged;

        if (InteractionManager.Player && Selected)
            Selected.MyAction.Interact(InteractionManager.Player);

        Label = string.Empty;
        RemoveAllButtons();

        GameCursor.ApplyCameraMode();
    }

    private void SelectFromMouseDelta()
    {
        if (buttons.Count == 0)
            return;
        
        var mouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));
        var shortest = float.MaxValue;
        var amplifier = 100f;
        accumulatedDelta = Vector2.ClampMagnitude(accumulatedDelta + mouseDelta * amplifier, 100f);
        var accumulatedPosition = (Vector2) Input.mousePosition + accumulatedDelta;
        WheelButton newSelected = null;
        foreach (var button in buttons)
        {
            var current = ((Vector2) button.transform.position - accumulatedPosition).sqrMagnitude;
            if (current >= shortest) continue;
            shortest = current;
            newSelected = button;
        }

        if (newSelected == Selected) return;
        Selected.Deselect();
        Selected = newSelected;
        Selected.Select();
    }

    private WheelButton InstantiateButton(int count, Interactable interactable, int i)
    {
        var newButton = Instantiate(buttonPrefab, transform, false);
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
        Label = string.Empty;
        RemoveAllButtons();
        if (!availableInteractables.Any()) return;
        SpawnButtons();
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

        foreach (var button in buttons)
            Destroy(button.gameObject);

        buttons.Clear();
    }

    private IEnumerator SpawnButtonsCoroutine(List<Interactable> interactables)
    {
        var count = interactables.Count;
        using (var enumerator = interactables.GetEnumerator())
        {
            if (enumerator.MoveNext())
            {
                var button = InstantiateButton(count, enumerator.Current, 0);
                buttons.Add(button);
                button.Select();
            }
            
            for (var i = 1; enumerator.MoveNext(); i++)
            {
                yield return buttonSpawnInterval;
                buttons.Add(InstantiateButton(count, enumerator.Current, i));
            }
        }

        spawnButtonsCoroutine = null;
    }
}