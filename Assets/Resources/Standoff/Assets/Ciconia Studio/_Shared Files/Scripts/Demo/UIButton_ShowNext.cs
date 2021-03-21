using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class UIButton_ShowNext : MonoBehaviour
{
    // GameObjects list
    public GameObject[] GameObjectsList;
    private int shownGameObjectIndex = -1;


    private void Start()
    {
        for (int i = 0; i < GameObjectsList.Length; ++i)
            GameObjectsList[i].SetActive(false);
        SelectNextGameObject();
    }
   
    
    // Next or previous GameObjects onClick
    public void SelectNextGameObject()
    {
        int index = shownGameObjectIndex >= GameObjectsList.Length - 1 ? -1 : shownGameObjectIndex;
        SelectGameObject(index + 1);
    }
    public void SelectPreviousGameObject()
    {
        int index = shownGameObjectIndex <= 0 ? GameObjectsList.Length : shownGameObjectIndex;
        SelectGameObject(index - 1);
    }
    public void SelectGameObject(int index)
    {
        if (shownGameObjectIndex >= 0)
            GameObjectsList[shownGameObjectIndex].SetActive(false);
        shownGameObjectIndex = index;
        GameObjectsList[shownGameObjectIndex].SetActive(true);

    }


}


