using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;
using System;
using UnityEngine;
using static Assets.Scripts.FengGameManagerMKII;

namespace Assets.Scripts.Utility
{

    public static class SpectatorMode
    {

        private static bool _disable = true;

        public static void Initialize()
        {
            _disable = true;
        }

        public static bool IsDisable()
        {
            return _disable;
        }

        public static void Toggle()
        {
            _disable = !_disable;
        }

        public static void Disable()
        {
            _disable = true;
        }

        public static void Enable()
        {
            _disable = false;
        }

        public static void SetState(bool enable)
        {
            _disable = !enable;
        }

        [Obsolete("Still using some of Fenglee Codes. Please migrate the instances once they have their place.")]
        public static void EnterSpecMode(bool enter)
        {
            if (enter)
            {
                if (Service.Player.Self != null && Service.Player.Self.photonView.isMine)
                {
                    PhotonNetwork.Destroy(Service.Player.Self.photonView);
                }
                instance.needChooseSide = false; // FengCode
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
                GameObject obj4 = GameObject.FindGameObjectWithTag("Player");
                if ((obj4 != null) && (obj4.GetComponent<Hero>() != null))
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(obj4, true, false);
                }
                else
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
                }
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                instance.StartCoroutine(instance.reloadSky()); // FengCode
            }
            else
            {
                if (GameObject.Find("cross1") != null)
                {
                    GameObject.Find("cross1").transform.localPosition = (Vector3) (Vector3.up * 5000f);
                }
                instance.needChooseSide = true; // FengCode
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(true);
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            }
        }

    }

}