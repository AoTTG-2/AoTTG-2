using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;
using System;
using UnityEngine;
using static Assets.Scripts.FengGameManagerMKII;

namespace Assets.Scripts.Utility
{

    public static class SpectatorMode
    {

        private static bool _enable = false;

        public static void Initialize()
        {
            _enable = false;
        }

        public static bool IsEnable()
        {
            return _enable;
        }

        public static bool IsDisable()
        {
            return !_enable;
        }

        public static void Toggle()
        {
            _enable = !_enable;
        }

        public static void Disable()
        {
            _enable = false;
        }

        public static void Enable()
        {
            _enable = true;
        }

        public static void SetState(bool enable)
        {
            _enable = enable;
        }

        [Obsolete("Still using some of Fenglee Codes. Please migrate the instances once they have their place.")]
        public static void UpdateSpecMode()
        {
            if (_enable)
            {
                if (Service.Player.Self != null && Service.Player.Self.photonView.isMine)
                {
                    PhotonNetwork.Destroy(Service.Player.Self.photonView);
                }
                instance.needChooseSide = false; // FengCode
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if ((player != null) && (player.GetComponent<Hero>() != null))
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(player, true, false);
                }
                else
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
                }
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            }
            else
            {
                if (GameObject.Find("cross1") != null)
                {
                    GameObject.Find("cross1").transform.localPosition = (Vector3.up * 5000f);
                }
                instance.needChooseSide = true; // FengCode
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(true);
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            }
        }

    }

}