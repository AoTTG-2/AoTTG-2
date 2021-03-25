using Assets.Scripts.Services;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        public TMP_Text distanceLabel;
        public TMP_Text speedLabel;

        public bool enabled = false;

        /// <summary>
        /// Find and Enable all Hook UI Elements required
        /// </summary>
        public void Find()
        {
            var crosshair = Service.Ui.GetUiHandler().InGameUi.HUD.Crosshair;
            cross = crosshair.Cross1.transform;
            crossImage = cross.GetComponentInChildren<Image>(true);
            crossL = crosshair.CrossL1.transform;
            crossImageL = crossL.GetComponentInChildren<Image>(true);
            crossR = crosshair.CrossR1.transform;
            crossImageR = crossR.GetComponentInChildren<Image>(true);

            distanceLabel = crosshair.Distance.transform.GetComponent<TMP_Text>();
            speedLabel = crosshair.Speedometer.transform.GetComponent<TMP_Text>();

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
                speedLabel.gameObject.SetActive(false);

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
                speedLabel.gameObject.SetActive(true);

                enabled = true;
            }
        }
    }

}