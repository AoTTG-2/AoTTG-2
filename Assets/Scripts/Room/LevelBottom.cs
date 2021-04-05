using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

public class LevelBottom : MonoBehaviour
{
    public GameObject link;
    public BottomType type;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (this.type == BottomType.Die)
            {
                if (other.gameObject.GetComponent<Hero>() != null)
                {
                    if (other.gameObject.GetPhotonView().isMine)
                    {
                        other.gameObject.GetComponent<Hero>().NetDieLocal(base.GetComponent<Rigidbody>().velocity * 50f, false, -1, string.Empty, true);
                    }
                }
            }
            else if (this.type == BottomType.Teleport)
            {
                if (this.link != null)
                {
                    other.gameObject.transform.position = this.link.transform.position;
                }
                else
                {
                    other.gameObject.transform.position = Vector3.zero;
                }
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}

