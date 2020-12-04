
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class DayLengthController : MonoBehaviour
    {

        public Text Label;

        public InputField DayLengthInput;
        
        


        DayAndNightControl DayNightCycle;
      
        void Update()
        {
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            DayLengthInput = gameObject.GetComponent<InputField>();
            var se = new InputField.SubmitEvent();
            se.AddListener(SubmitDayLength);
            DayLengthInput.onEndEdit = se;

        }

        private void SubmitDayLength(string arg0)
        {
            double dayLength =float.Parse(arg0);
            DayNightCycle.DayLength = (float) dayLength;
            Debug.Log(DayNightCycle.DayLength);
        }

        

    }
}
