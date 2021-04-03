using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WheelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Image Circle;
	public Image Icon;
	public InteractionWheel InteractionWheel;
	public float Speed = 8f;
	public Interactable MyAction;

	public Color BaseColor = Color.white;
	public Color HoverColor = new Color(0.9f, 1.0f, 0.1f);

	public void Animate()
	{
		StartCoroutine(GrowButton());
	}

	private IEnumerator GrowButton()
	{
		transform.localScale = Vector3.zero;
		float timer = 0f;
		while (timer < (1 / Speed)){
			timer += Time.deltaTime;
			transform.localScale = Vector3.one * timer * Speed;
			yield return null;
		}
		transform.localScale = Vector3.one;
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		InteractionWheel.Selected = MyAction;
		InteractionWheel.Label.text = MyAction.Context;
		Circle.color = HoverColor;
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		InteractionWheel.Selected = null;
		InteractionWheel.Label.text = "";
		Circle.color = BaseColor;
	}
}
