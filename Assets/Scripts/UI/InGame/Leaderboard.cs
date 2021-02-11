using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using Photon;

namespace Assets.Scripts.UI.InGame
{
    public class Leaderboard : UiMenu
    {

        public GameObject playerInfoPrefab;
        public GameObject otherPlayerInfoPrefab;
        public GameObject playerListParent;
        public TMP_Text roomName;
        public TMP_Text playerCount;
        public TMP_Text gameMode;
        public TMP_Text sortLabel;
        public bool isSortedByKills = false;
        public bool isSortedByScore = true;
        public bool isSortedByDmg = false;
        private float timeLast = 0;

        public static int SortByKills (PhotonPlayer a, PhotonPlayer b)
        {
            return RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.kills]).CompareTo(RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.kills]));
        }

        public static int SortByTotalDamage (PhotonPlayer a, PhotonPlayer b)
        {
            return RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.total_dmg]).CompareTo(RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.total_dmg]));
        }

        public static int SortBySvork (PhotonPlayer a, PhotonPlayer b)
        {
            var killsA = RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.kills]);
            var deathsA = RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.deaths]);
            var maxDamageA = RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.max_dmg]);
            var totalDamageA = RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.total_dmg]);

            var killsB = RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.kills]);
            var deathsB = RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.deaths]);
            var maxDamageB = RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.max_dmg]);
            var totalDamageB = RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.total_dmg]);

            float aScore = svorkBalance(killsA, deathsA, maxDamageA, totalDamageA);
            float bScore = svorkBalance(killsB, deathsB, maxDamageB, totalDamageB);

            return bScore.CompareTo(aScore);
        }

        // Formula for calculating score.
        private static float svorkBalance(int k, int d, int m, int t)
        {
            float score;
            score = (10*(1000*k + t)*(10 - Mathf.Sqrt(d))+m*m/2)/Mathf.Pow(10,5);
            return score;
        }
       
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        private void LateUpdate()
        {
            // UpdateScoreboard should be called 8 times a second. 
            float timeSince = Time.time * 1000;
            if(timeSince - timeLast > 125){
                UpdateScoreboard();
                timeLast = timeSince;
            }
        }
        
        private void UpdateScoreboard()
        {
            foreach (Transform child in playerListParent.transform)
            {
                GameObject.Destroy(child.gameObject); //REMOVE ALL CHILD (player infos) to prevent dupes
            }

            List<PhotonPlayer> playerList = PhotonNetwork.playerList.ToList();

            if(isSortedByScore)
            {
                playerList.Sort(SortBySvork);
            }else if(isSortedByDmg)
            {
                playerList.Sort(SortByTotalDamage);
            }else if (isSortedByKills)
            {
                playerList.Sort(SortByKills);
            }
            
            // Loop through each player and display them on the scoreboard.
            foreach (PhotonPlayer player in playerList)
            {
                var id = player.ID;
                var name = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
                var kills = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.kills]);
                var deaths = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.deaths]);
                var maxDamage = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.max_dmg]);
                var totalDamage = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.total_dmg]);
                var isDead = RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]);
                var isMaster = player.isMasterClient;
                
                GameObject playerInfo;

                if(player.IsLocal)
                {
                    playerInfo = Instantiate(playerInfoPrefab, transform.position, transform.rotation, playerListParent.transform);
                }
                else
                {
                    playerInfo = Instantiate(otherPlayerInfoPrefab, transform.position, transform.rotation, playerListParent.transform);
                }

                PlayerInfo playerLabel = playerInfo.GetComponent<PlayerInfo>();
                //SET DETAILS
                playerLabel.playerId.text = id.ToString();
                playerLabel.playerName.text = name.ToString();
                playerLabel.playerKills.text = kills.ToString();
                playerLabel.playerDeaths.text = deaths.ToString();
                playerLabel.playerHighest.text = maxDamage.ToString();
                playerLabel.playerTotal.text = totalDamage.ToString();
                playerLabel.playerPing.text = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.ping]);
                
                //if svork
                float score = svorkBalance(kills, deaths, maxDamage, totalDamage);
                playerLabel.playerScore.text = score.ToString();

            }

            roomName.text = PhotonNetwork.room.CustomProperties["name"].ToString();;
            gameMode.text = PhotonNetwork.room.CustomProperties["gamemode"].ToString() + " - " + PhotonNetwork.room.CustomProperties["level"].ToString();
            playerCount.text = PhotonNetwork.room.PlayerCount + "/" + PhotonNetwork.room.MaxPlayers;
        }

        public void SortByKills()
        {
            isSortedByKills = true;
            isSortedByDmg = false;
            isSortedByScore = false;
            sortLabel.text = "Kills";
        }

        public void SortByDmg()
        {
            
            isSortedByKills = false;
            isSortedByDmg = true;
            isSortedByScore = false;
            sortLabel.text = "Dmg";
        }

        public void SortByScore()
        {
            isSortedByKills = false;
            isSortedByDmg = false;
            isSortedByScore = true;
            sortLabel.text = "Score";
        }

        public void ToggleSort()
        {
            if(isSortedByScore)
            {
                SortByKills();
            }else if(isSortedByKills)
            {
                SortByDmg();
            }else if (isSortedByDmg)
            {
                SortByScore();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

    }
}