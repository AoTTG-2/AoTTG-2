using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class WheelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Image circle;
    [SerializeField] private Color hoverColor = new Color(0.9f, 1.0f, 0.1f);
    [SerializeField] private Image icon;
    [SerializeField] private float speed = 8f;

    public InteractionWheel InteractionWheel { get; set; }
    public Interactable MyAction { get; set; }
    public Image Icon => icon;

    private void Reset()
    {
        circle = GetComponent<Image>();
        icon = GetComponentsInChildren<Image>().FirstOrDefault(i => !ReferenceEquals(i, circle));
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Deselect();
    }

    public void Animate()
    {
        StartCoroutine(GrowButtonCoroutine());
    }

    public void Deselect()
    {
        InteractionWheel.Selected = null;
        InteractionWheel.Label = string.Empty;
        circle.color = baseColor;
    }

    public void Select()
    {
        InteractionWheel.Selected = this;
        InteractionWheel.Label = MyAction.Context;
        circle.color = hoverColor;
    }

    private IEnumerator GrowButtonCoroutine()
    {
        transform.localScale = Vector3.zero;
        
        var timer = 0f;
        while (timer < 1 / speed)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * (timer * speed);
            yield return null;
        }
        
        transform.localScale = Vector3.one;
    }
}