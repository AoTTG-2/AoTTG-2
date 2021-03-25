using Assets.Scripts.Characters.Humans;
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
                if (other.gameObject.GetComponent<Hero>().photonView.isMine)
                {
                    FengGameManagerMKII.instance.multiplayerRacingFinsih();
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
