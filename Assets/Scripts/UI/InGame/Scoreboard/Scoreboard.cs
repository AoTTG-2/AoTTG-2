using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.UI.InGame.Scoreboard
{
    /// <summary>
    /// The UI Scoreboard class, which is used to display various details of the round
    /// </summary>
    public class Scoreboard : UiMenu
    {
        public GameObject playerInfoPrefab;
        public GameObject playerListParent;

        public SortButton[] sortButtons;

        public TMP_Text roomName;
        public TMP_Text playerCount;
        public TMP_Text gameMode;
        public TMP_Text sortContainer;
        public string sortLabel;
        private float timeLast = 0;


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }


        void Start()
        {
            sortButtons = GameObject.FindObjectsOfType<SortButton>();
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
            // Remove all instances of playerList to prevent duplications when instantiating.
            foreach (Transform child in playerListParent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            List<PhotonPlayer> playerList = PhotonNetwork.playerList.ToList();

            switch (sortLabel)
            {
                case "ID":
                    playerList.Sort(SortByID);
                    break;
                case "NAME":
                    playerList.Sort(SortByName);
                    break;
                case "SCORE":
                    playerList.Sort(SortByScore);
                    break;
                case "K":
                    playerList.Sort(SortByKills);
                    break;
                case "D":
                    playerList.Sort(SortByDeaths);
                    break;
                case "MAX DAMAGE":
                    playerList.Sort(SortByMaxDamage);
                    break;
                case "TOTAL DAMAGE":
                    playerList.Sort(SortByTotalDamage);
                    break;
            }

            sortContainer.text = "SORTED BY " + sortLabel;
            
            
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
                    playerInfo.GetComponent<PlayerInfo>().isMine = true;
                }
                else
                {
                    playerInfo = Instantiate(playerInfoPrefab, transform.position, transform.rotation, playerListParent.transform);
                }

                PlayerInfo playerLabel = playerInfo.GetComponent<PlayerInfo>();
                //SET DETAILS
                playerLabel.playerId.text = id.ToString();
                playerLabel.playerName.text = name.ToString();
                playerLabel.playerKills.text = kills.ToString();
                playerLabel.playerDeaths.text = deaths.ToString();
                playerLabel.playerHighest.text = maxDamage.ToString();
                playerLabel.playerTotal.text = totalDamage.ToString();
                if(!PhotonNetwork.offlineMode){
                    playerLabel.playerPing.text = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.ping]);
                }else{
                    playerLabel.playerPing.text = "N/A";
                }
                float score = CalculateScore(kills, deaths, maxDamage, totalDamage);
                playerLabel.playerScore.text = score.ToString();
            }

            roomName.text = PhotonNetwork.room.CustomProperties["name"].ToString();;
            gameMode.text = PhotonNetwork.room.CustomProperties["gamemode"].ToString() + " - " + PhotonNetwork.room.CustomProperties["level"].ToString();
            playerCount.text = PhotonNetwork.room.PlayerCount + "/" + PhotonNetwork.room.MaxPlayers;
        }

    #region Sorting Functions

        public static int SortByID (PhotonPlayer b, PhotonPlayer a)
        {
            return b.ID.CompareTo(a.ID);
        }

        public static int SortByName (PhotonPlayer b, PhotonPlayer a)
        {
            return RCextensions.returnStringFromObject(b.CustomProperties[PhotonPlayerProperty.name]).CompareTo(RCextensions.returnStringFromObject(a.CustomProperties[PhotonPlayerProperty.name]));
        }

        public static int SortByKills (PhotonPlayer a, PhotonPlayer b)
        {
            return RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.kills]).CompareTo(RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.kills]));
        }

        public static int SortByDeaths (PhotonPlayer a, PhotonPlayer b)
        {
            return RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.deaths]).CompareTo(RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.deaths]));
        }
        
        public static int SortByMaxDamage (PhotonPlayer a, PhotonPlayer b)
        {
            return RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.max_dmg]).CompareTo(RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.max_dmg]));
        }

        public static int SortByTotalDamage (PhotonPlayer a, PhotonPlayer b)
        {
            return RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.total_dmg]).CompareTo(RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.total_dmg]));
        }

        public static int SortByScore (PhotonPlayer a, PhotonPlayer b)
        {
            var killsA = RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.kills]);
            var deathsA = RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.deaths]);
            var maxDamageA = RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.max_dmg]);
            var totalDamageA = RCextensions.returnIntFromObject(a.CustomProperties[PhotonPlayerProperty.total_dmg]);

            var killsB = RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.kills]);
            var deathsB = RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.deaths]);
            var maxDamageB = RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.max_dmg]);
            var totalDamageB = RCextensions.returnIntFromObject(b.CustomProperties[PhotonPlayerProperty.total_dmg]);

            float aScore = CalculateScore(killsA, deathsA, maxDamageA, totalDamageA);
            float bScore = CalculateScore(killsB, deathsB, maxDamageB, totalDamageB);

            return bScore.CompareTo(aScore);
        }

    #endregion

        // Formula for calculating score using svork balance
        private static float CalculateScore(int k, int d, int m, int t)
        {
            float score;
            score = (10*(1000*k + t)*(10 - Mathf.Sqrt(d))+m*m/2)/Mathf.Pow(10,5);
            score *= 100;
            return Mathf.RoundToInt(score);
        }

    }
}