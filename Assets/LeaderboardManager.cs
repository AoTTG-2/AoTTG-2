using Assets.Scripts.UI.InGame.Controls;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class LeaderboardManager : UiMenu
    {
        
        protected override void OnEnable()
        {
            base.OnEnable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // TODO: This only works in multiplayer and will probably definitely cause issues in singleplayer.
            // Do a check to see if it's multiplayer before allowing a use to open the leaderboard menu. <- somehow, idk how.

            // This is based off of InGameMenu.cs
            // InGameUi.cs is important.
            foreach (PhotonPlayer player in PhotonNetwork.playerList){
                var name = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
                var kills = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.kills]);
                var deaths = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.deaths]);
                var maxDamage = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.max_dmg]);
                var totalDamage = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.total_dmg]);
                Debug.Log("name: "+name);
                Debug.Log("kills: "+kills);
                Debug.Log("deaths: "+deaths);
                Debug.Log("maxDamage: "+maxDamage);
                Debug.Log("totalDamage: "+totalDamage);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}


