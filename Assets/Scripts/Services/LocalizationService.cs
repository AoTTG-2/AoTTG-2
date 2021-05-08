using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Assets.Scripts.Services
{
    public class LocalizationService : MonoBehaviour
    {
        [SerializeField]
        private LocalizedStringTable catchGamemodeTable;

        public StringTable CatchGamemode { get; set; }

        private IEnumerator Start()
        {
            var tableLoading = catchGamemodeTable.GetTable();
            yield return tableLoading;
            CatchGamemode = tableLoading.Result;
        }
    }
}
