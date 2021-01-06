using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    
    class SyncTimeButtonHandler : MonoBehaviour
    {
        public Button SyncTimeButton;
        DayAndNightControl DayNightCycle = null;
        void Start ()
        {
            Button btn = SyncTimeButton.GetComponent<Button>();
            btn.onClick.AddListener(Synch);
        }

        void Synch()
        {
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            Service.Settings.SyncSettings();
                
            Debug.Log("System Synched");
        }
    
    }
}
