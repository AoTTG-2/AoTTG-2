using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
using Assets.Scripts.Settings.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.DebugWindow
{
    public class DebugToolsPanel : TabPanel
    {
        public Button NoClipButton;
        public Button TitanMovementButton;
        public Button TitanAttacksButton;

        public Color EnabledColor;
        public Color DisabledColor;

        private void OnEnable()
        {
            NoClipButton.image.color = Setting.Debug.NoClip.Value ? EnabledColor : DisabledColor;
            TitanMovementButton.image.color = Setting.Debug.TitanMovement.Value ? EnabledColor : DisabledColor;
            TitanAttacksButton.image.color = Setting.Debug.TitanAttacks.Value ? EnabledColor : DisabledColor;
        }
        
        private int? defaultHeroLayer;
        public void NoClip()
        {
            if (!PhotonNetwork.isMasterClient) return;

            if (Service.Player.Self == null)
            {
                //TODO: Localized message
                Debug.LogWarning("Player Object is null");
                return;
            }

            if (Service.Player.Self is Hero hero)
            {
                defaultHeroLayer ??= hero.gameObject.layer;
                DebugSettings.IsNoClip = !DebugSettings.IsNoClip;
                hero.gameObject.layer = DebugSettings.IsNoClip ? (int) Layers.Default : defaultHeroLayer.Value;
            }
        }

        public void DisableTitanMovement()
        {
            if (!PhotonNetwork.isMasterClient) return;
        }

        public void DisableTitanAttacks()
        {
            if (!PhotonNetwork.isMasterClient) return;
        }
    }
}
