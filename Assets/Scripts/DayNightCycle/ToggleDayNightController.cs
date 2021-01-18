using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.DayNightCycle
{
    public class ToggleDayNightController : MonoBehaviour
    {
        public Toggle ToggleDayNight;
        public GameObject DayNightController;
        public GameObject MainLight;
        public Text Label;

        DayAndNightControl dayNightCycle;
        // Start is called before the first frame update
        void Start()
        {
            ToggleDayNight.isOn = false;
            ToggleDayNight.onValueChanged.AddListener(delegate {
                ToggleValueChanged(ToggleDayNight);
            });

        }

        void ToggleValueChanged(Toggle change)
        {
            if (PhotonNetwork.isMasterClient)
            {
                dayNightCycle.pause = !change.isOn;
                GameSettings.Time.pause = !change.isOn;
                Service.Settings.SyncSettings();
            }

        }
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }

        

    }
}

