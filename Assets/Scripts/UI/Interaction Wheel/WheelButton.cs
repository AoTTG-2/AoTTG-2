using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WheelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color BaseColor = Color.white;
    public Image Circle;
    public Color HoverColor = new Color(0.9f, 1.0f, 0.1f);
    public Image Icon;
    public InteractionWheel InteractionWheel;
    public Interactable MyAction;
    public float Speed = 8f;

    public void Animate()
    {
        StartCoroutine(GrowButton());
    }

    public void Deselect()
    {
        InteractionWheel.Selected = null;
        InteractionWheel.Label.text = string.Empty;
        Circle.color = BaseColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deselect();
    }

    public void Select()
    {
        InteractionWheel.Selected = this;
        InteractionWheel.Label.text = MyAction.Context;
        Circle.color = HoverColor;
    }

    private IEnumerator GrowButton()
    {
        transform.localScale = Vector3.zero;
        float timer = 0f;
        while (timer < (1 / Speed))
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * timer * Speed;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}