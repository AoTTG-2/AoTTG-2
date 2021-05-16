using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI;
using Assets.Scripts.UI.InGame;
using Assets.Scripts.UI.InGame.HUD;
using Photon;
using System;
using TMPro;

namespace Assets.Scripts.Services
{
    public class UiService : PunBehaviour, IUiService
    {
        public UiHandler MainUi;
        private InGameUi Ui { get; set; }

        private void Awake()
        {
            Ui = MainUi.InGameUi.GetComponent<InGameUi>();
        }

        private TMP_Text GetLabel(LabelPosition label)
        {
            var labels = Ui.HUD.Labels;
            switch (label)
            {
                case LabelPosition.Top:
                    return labels.Top;
                case LabelPosition.TopLeft:
                    return labels.TopLeft;
                case LabelPosition.TopRight:
                    return labels.TopRight;
                case LabelPosition.Center:
                    return labels.Center;
                default:
                    throw new ArgumentOutOfRangeException(nameof(label), label, null);
            }
        }

        public UiHandler GetUiHandler()
        {
            return MainUi;
        }

        public void ResetMessage(LabelPosition label)
        {
            GetLabel(label).text = string.Empty;
        }

        public void ResetMessagesAll()
        {
            var labels = Ui.HUD.Labels;
            labels.Top.text
                = labels.TopLeft.text
                = labels.TopRight.text
                = labels.Center.text
                    = string.Empty;

            Ui.HUD.ClearDamage();
        }

        public void SetMessage(LabelPosition label, string message)
        {
            GetLabel(label).text = message;
        }

        private void OnLevelWasLoaded(int level)
        {
            if (level == 0)
            {
                MainUi.ShowMenu();
            }
        }
    }
}
