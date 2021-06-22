using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Gamemode;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public class LevelTriggerRacingEnd : MonoBehaviour
    {
        private bool disable;

        private void OnTriggerStay(Collider other)
        {
            if (!this.disable && (other.gameObject.tag == "Player"))
            {
                if (other.gameObject.GetComponent<Hero>().photonView.isMine && FengGameManagerMKII.Gamemode is RacingGamemode racingGamemode)
                {
                    racingGamemode.OnRacingFinished();
                    this.disable = true;
                }
            }
        }

        private void Start()
        {
            this.disable = false;
        }
    }
}
