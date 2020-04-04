using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class GameSettingsMenu : MonoBehaviour
    {
        private bool horses;
        public void SetHorse(bool horse)
        {
            horses = horse;
        }

        public void SyncSettings()
        {
            if (!PhotonNetwork.isMasterClient) return;
            var gamemode = FengGameManagerMKII.Gamemode;
            gamemode.Horse = horses;
            var json = JsonUtility.ToJson(gamemode);
            FengGameManagerMKII.instance.photonView.RPC("SyncSettings", PhotonTargets.All, json);
        }
    }
}