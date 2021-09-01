using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.InGame.DebugWindow
{
    public class DebugStatsPanel : TabPanel
    {
        public TMP_Text Text;

        private Coroutine coroutine;
        private void OnEnable()
        {
            coroutine = StartCoroutine(OnUpdateEveryTenthSecond());
        }
        
        private void OnDisable()
        {
            StopCoroutine(coroutine);
        }

        protected IEnumerator OnUpdateEveryTenthSecond()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                RecalculateStats();
            }
        }

        private void RecalculateStats()
        {
            Text.text = "Some awesome updated text..." + Random.Range(0, 1000);
        }
    }
}
