using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class WheelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Image circle;
	public Image icon;
	public InteractableWheel myMenu;
	public float speed = 8f;
    public Interactable myAction;

    public Color baseColor = Color.white;
    public Color hoverColor = new Color(0.9f, 1.0f, 0.1f);

	public void Anim(){
        icon.sprite = myAction.icon;
		StartCoroutine(AnimateButtonIn());
	}

	//make button grow UwU
	IEnumerator AnimateButtonIn(){
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
		myMenu.selected = myAction;
        myMenu.label.text = myAction.context;
        circle.color = hoverColor;
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		myMenu.selected = null;
        myMenu.label.text = "";
        circle.color = baseColor;
    }


}
