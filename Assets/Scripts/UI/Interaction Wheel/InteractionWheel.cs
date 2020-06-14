using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class InteractionWheel : MonoBehaviour
{
    public WheelButton ButtonPrefab;
    public Text Label;
    public Interactable Selected;

    private void OnDisable()
    {
        // Remove all buttons when menu is closed.
        foreach (Transform t in transform)
        {
            if (t.name != "Context")
                Destroy(t.gameObject);
        }
    }

    private void OnEnable()
    {
        if (!Label)
            Label = GetComponentInChildren<Text>();
        Label.text = string.Empty;

        StartCoroutine(SpawnButtons());
    }

    private IEnumerator SpawnButtons()
    {
        var interactables = InteractionManager.Interactables;
        var enumerator = interactables.GetEnumerator();
        for (var i = 0; enumerator.MoveNext(); i++)
        {
            var interactable = enumerator.Current;
            var newButton = Instantiate(ButtonPrefab);
            newButton.transform.SetParent(transform, false);
            newButton.InteractionWheel = this;
            newButton.MyAction = interactable;
            newButton.Icon.sprite = interactable.Icon;
            var theta = (2 * Mathf.PI / interactables.Count) * i++;
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
        if (Input.GetButtonDown("Fire1") && Selected && InteractionManager.Player)
            Selected.Interact(InteractionManager.Player);
    }
}