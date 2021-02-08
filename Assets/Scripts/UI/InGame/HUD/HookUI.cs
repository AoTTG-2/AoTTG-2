using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    /// <summary>
    /// Class to Contain, Manage and Disable/Enable all Hook UI GameObjects
    /// </summary>
    [Serializable]
    public class HookUI
    {
        public Transform cross;
        public Transform crossL;
        public Transform crossR;

        public Image crossImage;
        public Image crossImageL;
        public Image crossImageR;

        public Text distanceLabel;

        public bool enabled = false;

        /// <summary>
        /// Find and Enable all Hook UI Elements required
        /// </summary>
        public void Find()
        {
            // Todo: Implement system that does not use GameObject.Find()

            cross = GameObject.Find("cross1").transform;
            crossImage = cross.GetComponentInChildren<Image>();
            crossL = GameObject.Find("crossL1").transform;
            crossImageL = crossL.GetComponentInChildren<Image>();
            crossR = GameObject.Find("crossR1").transform;
            crossImageR = crossR.GetComponentInChildren<Image>();

            distanceLabel = GameObject.Find("Distance").GetComponent<Text>();

            Enable();
        }

        /// <summary>
        /// Disable all Hook UI GameObjects
        /// </summary>
        public void Disable()
        {
            if (enabled)
            {
                cross.gameObject.SetActive(false);
                crossImage.gameObject.SetActive(false);
                crossL.gameObject.SetActive(false);
                crossImageL.gameObject.SetActive(false);
                crossR.gameObject.SetActive(false);
                crossImageR.gameObject.SetActive(false);

                distanceLabel.gameObject.SetActive(false);

                enabled = false;
            }
        }

        /// <summary>
        /// Enable all Hook UI GameObjects
        /// </summary>
        public void Enable()
        {
            if (!enabled)
            {
                cross.gameObject.SetActive(true);
                crossImage.gameObject.SetActive(true);
                crossL.gameObject.SetActive(true);
                crossImageL.gameObject.SetActive(true);
                crossR.gameObject.SetActive(true);
                crossImageR.gameObject.SetActive(true);

                distanceLabel.gameObject.SetActive(true);

                enabled = true;
            }
        }
    }
}