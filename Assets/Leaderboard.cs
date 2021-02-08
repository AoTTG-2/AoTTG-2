using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Leaderboard : UiContainer
    {

        public GameObject playerInfoPrefab;
        public GameObject playerList;

        public void OnEnable()
        {
            
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
                GameObject playerInfo = Instantiate(playerInfoPrefab, transform.position, transform.rotation, playerList.transform);

                PlayerInfo playerLabel = playerInfo.GetComponent<PlayerInfo>();
                //SET DETAILS
                playerLabel.playerName.text = name.ToString();
                playerLabel.playerKills.text = kills.ToString();
                playerLabel.playerDeaths.text = deaths.ToString();
                playerLabel.playerHighest.text = maxDamage.ToString();
                playerLabel.playerTotal.text = totalDamage.ToString();
            }
        }


    }
}