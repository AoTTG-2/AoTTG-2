using System;
using UnityEngine;

public class LevelTriggerRacingEnd : Assets.Scripts.Gamemode.Racing.RacingGameComponent
{
    private bool disable;

    private void OnTriggerStay(Collider other)
    {
        if (!this.disable && (other.gameObject.tag == "Player"))
        {
            if (other.gameObject.GetComponent<Hero>().photonView.isMine)
            {
                Gamemode.RacingFinsihEvent();
                this.disable = true;
            }
        }
    }

    private void Start()
    {
        this.disable = false;
    }
}

