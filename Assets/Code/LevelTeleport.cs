using System;
using UnityEngine;

public class LevelTeleport : MonoBehaviour
{
    public string levelname = string.Empty;
    public GameObject link;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (this.levelname != string.Empty)
            {
                Application.LoadLevel(this.levelname);
            }
            else
            {
                other.gameObject.transform.position = this.link.transform.position;
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

