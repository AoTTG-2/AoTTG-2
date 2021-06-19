using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Inventory
{

    public class LanternLightBehaviour : MonoBehaviour
    {

        private bool lightStatus;

        [PunRPC]
        public void ToggleLight()
        {

            lightStatus = !lightStatus;
            gameObject.SetActive(lightStatus);

        }

    }
}