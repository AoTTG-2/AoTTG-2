using System;
using UnityEngine;

public class BtnSSNext : MonoBehaviour
{
    private void OnClick()
    {
        if (base.gameObject.transform.parent.gameObject.GetComponent<CharacterCreationComponent>() != null)
        {
            base.gameObject.transform.parent.gameObject.GetComponent<CharacterCreationComponent>().nextOption();
        }
        else
        {
            base.gameObject.transform.parent.gameObject.GetComponent<CharacterStatComponent>().nextOption();
        }
    }
}

