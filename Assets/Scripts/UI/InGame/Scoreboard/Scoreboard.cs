using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.UI.InGame.Scoreboard
{
    public class Scoreboard : UiMenu
    {

        public GameObject playerInfoPrefab;
        public GameObject otherPlayerInfoPrefab;
        public GameObject playerListParent;
        public TMP_Text roomName;
        public TMP_Text playerCount;
        public TMP_Text gameMode;
        public TMP_Text sortContainer;
        public TMP_Text[] labels;
        public string sortLabel;
        private float timeLast = 0;

       
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        void Start()
        {
            SortByScore();
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
                case "id":
                    playerList.Sort(SortByID);
                    break;
                case "name":
                    playerList.Sort(SortByName);
                    break;
                case "score":
                    playerList.Sort(SortByScore);
                    break;
                case "kills":
                    playerList.Sort(SortByKills);
                    break;
                case "deaths":
                    playerList.Sort(SortByDeaths);
                    break;
                case "max damage":
                    playerList.Sort(SortByMaxDamage);
                    break;
                case "total damage":
                    playerList.Sort(SortByTotalDamage);
                    break;
            }

            sortContainer.text = "Sorted by " + sortLabel;
            
            
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

    #region OnClick events called by buttons

        public void SortByID()
        {
            sortLabel = "id";
            SetLabelIndicators();
        }
        public void SortByName()
        {
            sortLabel = "name";
            SetLabelIndicators();
        }

        public void SortByKills()
        {
            sortLabel = "kills";
            SetLabelIndicators();
        }

        public void SortByDeaths()
        {
            sortLabel = "deaths";
            SetLabelIndicators();
        }

        public void SortByMaxDamage()
        {
            sortLabel = "max damage";
            SetLabelIndicators();
        }

        public void SortByTotal()
        {
            sortLabel = "total damage";
            SetLabelIndicators();
        }

        public void SortByScore()
        {
            sortLabel = "score";
            SetLabelIndicators();
        }

        private void SetLabelIndicators()
        {
            foreach(TMP_Text label in labels)
            {
                if(label.gameObject.name == sortLabel)
                {
                    label.GetComponent<SortLabelIndicator>().SetIndicator();
                }
                else
                {
                    label.GetComponent<SortLabelIndicator>().SetDefault();
                }
            }
        }

    #endregion

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

        protected override void OnDisable()
        {
            base.OnDisable();
        }

    }
}