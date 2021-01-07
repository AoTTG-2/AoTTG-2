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
            dayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            Service.Settings.SyncSettings();
                
            Debug.Log("System Synched");
        }
    
    }
}
