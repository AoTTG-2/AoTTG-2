using System;
using Assets.Scripts.Characters.Titan;
using UnityEngine;

public class TitanTrigger : MonoBehaviour
{
    private MindlessTitan Titan { get; set; }

    void Start()
    {
        Titan = gameObject.GetComponentInParent<MindlessTitan>();
    }

    public void SetCollision(bool value)
    {
        Titan.IsColliding = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Titan != null && !Titan.IsColliding)
        {
            GameObject gameObject = other.transform.root.gameObject;
            if (gameObject.layer == 8)
            {
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                {
                    if (gameObject.GetPhotonView().isMine)
                    {
                        Titan.IsColliding = true;
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    GameObject obj3 = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
                    if ((obj3 != null) && (obj3 == gameObject))
                    {
                        Titan.IsColliding = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Titan != null && Titan.IsColliding)
        {
            GameObject gameObject = other.transform.root.gameObject;
            if (gameObject.layer == 8)
            {
                Debug.LogWarning($"Gameobject: {gameObject.name}");
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                {
                    if (gameObject.GetPhotonView().isMine)
                    {
                        Titan.IsColliding = false;
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    GameObject obj3 = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
                    if ((obj3 != null) && (obj3 == gameObject))
                    {
                        Titan.IsColliding = false;
                    }
                }
            }
        }
    }
}

