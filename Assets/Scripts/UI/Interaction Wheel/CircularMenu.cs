using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenu : MonoBehaviour 
{
    [Obsolete]
	private List<CircularMenuElement> Elements = new List<CircularMenuElement>();

    private List<Interactable> interactables;
    private GameObject player;

    public Image WheelImage;
    private Vector2 mousePosition;
    private Vector2 fromVectorMouse = new Vector2(0.5f, 1.0f);
	private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVectorMouse;

    private int menuItems;
    private int selectedMenuItem;
    private int oldMenuItem;

    private bool hasCursor;

    void OnEnable()
    {
        hasCursor = Cursor.visible;
        if (!hasCursor) 
            Cursor.visible = true;

        player = GameObject.FindGameObjectsWithTag("Player").Single(x => x.GetComponent<PhotonView>().isMine);
        interactables = player.GetComponent<PlayerInteractable>().Collisions;

        menuItems = interactables.Count;
        for (var i = 0; i < menuItems; i++)
        {
            var copy = Instantiate(WheelImage, gameObject.transform);

            var element = new CircularMenuElement();
            element.SceneImage = copy;
            element.SceneImage.fillAmount = 1f / menuItems;
            element.SceneImage.color = element.NormalColor;
            var rect = element.SceneImage.gameObject.GetComponent<RectTransform>();
            rect.rotation = Quaternion.Euler(0, 0, (360f / menuItems) * (i + 1));
            Elements.Add(element);
        }
    }

    void OnDisable()
    {
        if (!hasCursor)
            Cursor.visible = false;

        //TODO Perhaps cache the current list, then check if anything has changed
        foreach (Transform child in transform)
        {
            DestroyObject(child.gameObject);
        }

        Elements = new List<CircularMenuElement>();
    }
	
	void Update () 
    {
		GetCurrentMenuItem();
        if (Input.GetButtonDown("Fire1"))
        {
            ButtonAction();
        }
	}

    public void GetCurrentMenuItem()
    {
        mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        toVectorMouse = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        var angle = (Mathf.Atan2(fromVectorMouse.y - centerCircle.y, fromVectorMouse.x - centerCircle.x) 
                    - Mathf.Atan2(toVectorMouse.y - centerCircle.y, toVectorMouse.x - centerCircle.x))
                    * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360f;

        selectedMenuItem = (int)(angle / (360f / menuItems));

        if (selectedMenuItem == oldMenuItem) return;
        Elements[oldMenuItem].SceneImage.color = Elements[oldMenuItem].NormalColor;
        oldMenuItem = selectedMenuItem;
        Elements[selectedMenuItem].SceneImage.color = Elements[selectedMenuItem].HighLightedColor;
    }

    public void ButtonAction()
    {
        interactables[selectedMenuItem].Action(player);
        Debug.Log($"{interactables[selectedMenuItem].name} selected!");
    }
}
