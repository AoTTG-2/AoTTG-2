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
        public GameObject playerListParent;

        public static int SortByKills (PhotonPlayer a, PhotonPlayer b){
                return RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.kills]).CompareTo(RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.kills]));
            }

        public static int SortByTotalDamage (PhotonPlayer a, PhotonPlayer b){
            return RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.total_dmg]).CompareTo(RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.total_dmg]));
        }
        public void OnEnable()
        {
            List<PhotonPlayer> playerList = PhotonNetwork.playerList.ToList();

            playerList.Sort(SortByKills);
            //playerList.Sort(SortByTotalDamage);

            
            foreach (PhotonPlayer player in playerList){

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
                GameObject playerInfo = Instantiate(playerInfoPrefab, transform.position, transform.rotation, playerListParent.transform);

                PlayerInfo playerLabel = playerInfo.GetComponent<PlayerInfo>();
                //SET DETAILS
                playerLabel.playerName.text = name.ToString();
                playerLabel.playerKills.text = kills.ToString();
                playerLabel.playerDeaths.text = deaths.ToString();
                playerLabel.playerHighest.text = maxDamage.ToString();
                playerLabel.playerTotal.text = totalDamage.ToString();
            }
        }

        public void OnDisable()
        {
            foreach (Transform child in playerListParent.transform)
            {
                GameObject.Destroy(child.gameObject); //REMOVE ALL CHILD (player infos) to prevent dupes
            }
        }


    }
}