using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WheelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Image circle;
	public Image icon;
	public InteractionWheel InteractionWheel;
	public float speed = 8f;
	public Interactable myAction;

	public Color baseColor = Color.white;
	public Color hoverColor = new Color(0.9f, 1.0f, 0.1f);

	public void Animate()
	{
		icon.sprite = myAction.icon;
		StartCoroutine(GrowButton());
	}

	IEnumerator GrowButton()
	{
		transform.localScale = Vector3.zero;
		float timer = 0f;
		while (timer < (1 / speed)){
			timer += Time.deltaTime;
			transform.localScale = Vector3.one * timer * speed;
			yield return null;
		}
		transform.localScale = Vector3.one;
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		InteractionWheel.Selected = myAction;
		InteractionWheel.Label.text = myAction.context;
		circle.color = hoverColor;
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		InteractionWheel.Selected = null;
		InteractionWheel.Label.text = "";
		circle.color = baseColor;
	}
}
