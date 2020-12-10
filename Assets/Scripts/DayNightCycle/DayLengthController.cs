
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    public class DayLengthController : MonoBehaviour
    {

        public Text Label;
        public InputField DayLengthInput;
        DayAndNightControl DayNightCycle;
       

        void Start()
        {
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            DayLengthInput = gameObject.GetComponent<InputField>();
        }
        void Update()
        {
            if (DayNightCycle == null)
            {
                DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            }
            var se = new InputField.SubmitEvent();
            se.AddListener(SubmitDayLength);
            DayLengthInput.onEndEdit = se;

        }

        private void SubmitDayLength(string arg0)
        {
            float dayLength =float.Parse(arg0);
            DayNightCycle.DayLength = (float) dayLength;
            Debug.Log(DayNightCycle.DayLength);
        }
      


    }
}
