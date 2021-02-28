//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;

//public class InteractionWheel : MonoBehaviour
//{
//    public Text Label;
//    public WheelButton ButtonPrefab;
//    public Interactable Selected;
//    private List<Interactable> interactables;
//    private GameObject player;

//    public void OnEnable()
//    {
//        if (!Label)
//            Label = GetComponentInChildren<Text>();
//        Label.text = "";

//        // There may be 0 players, in the case of the player spawning as a titan
//        player = GameObject.FindGameObjectsWithTag("Player").SingleOrDefault(x => x.GetComponent<PhotonView>().isMine);
//        if (player)
//        {
//            interactables = player.GetComponent<PlayerInteractable>().Collisions;
//            base.StartCoroutine(SpawnButtons());
//        }
//    }

//    private IEnumerator SpawnButtons()
//    {
//        for (int i = 0; i < interactables.Count; i++)
//        {
//            WheelButton newButton = Instantiate(ButtonPrefab) as WheelButton;
//            newButton.transform.SetParent(transform, false);
//            newButton.InteractionWheel = this;
//            newButton.MyAction = interactables[i];
//            newButton.Icon.sprite = interactables[i].Icon;
//            float theta = (2 * Mathf.PI / interactables.Count) * i;
//            float xPos = Mathf.Sin(theta);
//            float yPos = Mathf.Cos(theta);
//            newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 200f;
//            newButton.InteractionWheel = this;
//            newButton.Animate();
//            yield return new WaitForSeconds(.05f);

//        }
//    }

//    private void OnDisable()
//    {
//        if (Selected && player)
//        {
//            Selected.Action(player);
//        }

//        //remove all buttons when menu is closed.
//        foreach (Transform t in transform)
//        {
//            if (t.name != "Context")
//            {
//                Destroy(t.gameObject);
//            }
//        }
//    }
//}
