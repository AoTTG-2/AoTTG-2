using System;
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
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                    {
                        if (other.gameObject.GetPhotonView().isMine)
                        {
                            other.gameObject.GetComponent<Hero>().NetDieLocal((Vector3) (base.GetComponent<Rigidbody>().velocity * 50f), false, -1, string.Empty, true);
                        }
                    }
                    else
                    {
                        other.gameObject.GetComponent<Hero>().Die((Vector3) (other.gameObject.GetComponent<Rigidbody>().velocity * 50f), false);
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

