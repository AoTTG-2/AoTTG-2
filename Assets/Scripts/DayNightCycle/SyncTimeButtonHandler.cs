using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    class SyncTimeButtonHandler : MonoBehaviour
    {
        public Button syncTimeButton = null;
        DayAndNightControl dayNightCycle = null;
        void Start ()
        {
            Button btn = syncTimeButton.GetComponent<Button>();
            btn.onClick.AddListener(Synch);
        }

        void Synch()
        {
            Service.Settings.SyncSettings();
            Debug.Log("System Synched");
        }

        //grabbing the local scene's DayAndNightControl script
        
            void OnEnable()
            {
            if (PhotonNetwork.isMasterClient)
                 {
                dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
                Synch();
                }
            }

            void OnDisable()
            {
            if (PhotonNetwork.isMasterClient)
                {
                Synch();
                }
            }
        
    }
}
