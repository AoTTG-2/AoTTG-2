using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Gamemode.Settings;
using Assets.Scripts.UI.Input;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode
{
    public class StandoffGamemode : GamemodeBase
    {
        public sealed override GamemodeSettings Settings { get; set; }
        private StandoffSettings GamemodeSettings => Settings as StandoffSettings;
        private readonly StandoffGamemode gamemode = FengGameManagerMKII.Gamemode as StandoffGamemode;

        private int teamWinner;
        public readonly int[] teamScores = new int[2] {10, 10};

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
            if (!isMasterClient) return;
                SpawnTitans(10);

        }

        public override string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
        {

            object[] objArray = new object[6];
            objArray[0] = "Cyan: ";
            objArray[1] = (20-teamScores[0]).ToString();
            objArray[2] = "Magenta: ";
            objArray[3] = (20-teamScores[1]).ToString();
            objArray[4] = "\n\t\t\tTime: "; 
            //int length = totalRoomTime - (time);
            var length = GameObject.FindGameObjectsWithTag("titan").Length;
            objArray[5] = length.ToString();
            return string.Concat(objArray);
        }

        private void Awake()
        {
            //FengGameManagerMKII.instance.barrier.Add(gameObject);
        }

        public override void OnPlayerKilled(int id)
        {
            if (IsAllPlayersDead())
            {
                FengGameManagerMKII.instance.gameLose2();
                teamWinner = 0;
            }
            if (GamemodeBase.IsTeamAllDead(1))
            {
                teamWinner = 2;
                FengGameManagerMKII.instance.gameWin2();
            }
            if (GamemodeBase.IsTeamAllDead(2))
            {
                teamWinner = 1;
                FengGameManagerMKII.instance.gameWin2();
            }
        }

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (Settings.Pvp == PvpMode.Disabled && !Settings.PvPBomb)
            {
                return $"Team {teamWinner}, Win!\nGame Restart in {(int)timeUntilRestart}s\n\n";
            }
            return $"Round Ended!\nGame Restart in {(int)timeUntilRestart}s\n\n";
        }


        public void OnTitanKilledStandoff(string titanName, PhotonPlayer player)
        {

            int team = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.RCteam]);
                if(team==1)
                   {//1 - cyan 2- magenta
                        newTitanTeam1();
                        teamScores[0]++;
                        teamScores[1]--;
                   }
                else
                   {
                       newTitanTeam2();
                       teamScores[1]++;
                       teamScores[0]--;
                   }
               
                 CheckWinConditions();


        }
        private float getHeight(Vector3 pt)
        {
            RaycastHit hit;
            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(pt, -Vector3.up, out hit, 1000f, mask2.value))
            {
                return hit.point.y;
            }
            return 0f;
        }

        private void newTitanTeam1()//cyan
        {
            Vector3 position = new Vector3 (Random.Range(0, 500), 0f, Random.Range(-500, 500));
            Quaternion rotation=GameObject.Find("Barrier").transform.rotation;
            var configuration = GetTitanConfiguration();
            FengGameManagerMKII.instance.SpawnTitan(position, rotation, configuration).GetComponent<MindlessTitan>();
            
        }

        private void newTitanTeam2()//magenta
        {
            Vector3 position = new Vector3 (-Random.Range(0, 500), 0f, Random.Range(-500, 500));
            Quaternion rotation=GameObject.Find("Barrier").transform.rotation;
            var configuration = GetTitanConfiguration();
            FengGameManagerMKII.instance.SpawnTitan(position, rotation, configuration).GetComponent<MindlessTitan>();
            
        }

        public void CheckWinConditions()
        {
            if (teamScores[1]==20)
            {
                teamWinner = 1;
                FengGameManagerMKII.instance.gameWin2();
            }
            else if(teamScores[2]==20)
            {
                teamWinner = 2;
                FengGameManagerMKII.instance.gameWin2();
            }
        
        
        }
        
        public virtual void OnRestart()
        {
            if (Settings.PointMode > 0)
            {
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    PhotonPlayer player = PhotonNetwork.playerList[i];
                    ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, 0);
                    propertiesToSet.Add(PhotonPlayerProperty.deaths, 0);
                    propertiesToSet.Add(PhotonPlayerProperty.max_dmg, 0);
                    propertiesToSet.Add(PhotonPlayerProperty.total_dmg, 0);
                    player.SetCustomProperties(propertiesToSet);
                }
            }
            FengGameManagerMKII.instance.gameEndCD = 0f;
            FengGameManagerMKII.instance.restartGame2();
        }
        
        public virtual void OnUpdate(float interval) { }
        
        public virtual void OnPlayerSpawned(GameObject player)
        {
        }
        
        protected void SpawnTitans(int amount)
        {
            for(int i=0;i<amount;i++){
                newTitanTeam1();
                newTitanTeam2();}
        }
        
        public virtual void OnTitanSpawned(MindlessTitan titan)
        {
        }
        
        public virtual GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
        {
            var objArray = GameObject.FindGameObjectsWithTag(tag);
            return objArray[Random.Range(0, objArray.Length)];
        }
        
        public virtual string GetGamemodeStatusTopRight(int time = 0, int totalRoomTime = 0)
        {
            return string.Concat("Humanity ", Settings.HumanScore, " : Titan ", Settings.TitanScore, " ");
        
        }
        
        public virtual string GetRoundEndedMessage()
        {
            return $"Humanity {Settings.HumanScore} : Titan {Settings.TitanScore}";
        }
        
        public virtual void OnAllTitansDead() { }
        
        public virtual void OnGameWon()
        {
            Settings.HumanScore++;
            FengGameManagerMKII.instance.gameEndCD = FengGameManagerMKII.instance.gameEndTotalCDtime;
            var parameters = new object[] { Settings.HumanScore };
            FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
            if (((int) FengGameManagerMKII.settings[0xf4]) == 1)
            {
                //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
            }
        }
        
        public virtual void OnGameLost()
        {
            Settings.TitanScore++;
            var parameters = new object[] { Settings.TitanScore };
            FengGameManagerMKII.instance.photonView.RPC("netGameLose", PhotonTargets.Others, parameters);
            if ((int) FengGameManagerMKII.settings[0xf4] == 1)
            {
                //FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
            }
        }
        
        public virtual void OnNetGameLost(int score)
        {
            Settings.TitanScore = score;
        }
        
        public virtual void OnNetGameWon(int score)
        {
            Settings.HumanScore = score;
        }
        
        internal bool IsAllPlayersDead()
        {
            var num = 0;
            var num2 = 0;
            foreach (var player in PhotonNetwork.playerList)
            {
                if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 1) continue;
                num++;
                if (RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]))
                {
                    num2++;
                }
            }
            return (num == num2);
        }
        
        internal bool IsAllTitansDead()
        {
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if ((obj2.GetComponent<MindlessTitan>() != null) && obj2.GetComponent<MindlessTitan>().TitanState != MindlessTitanState.Dead)
                {
                    return false;
                }
                if (obj2.GetComponent<FEMALE_TITAN>() != null)
                {
                    return false;
                }
            }
            return true;
        }
        
        public virtual string GetDefeatMessage(float gameEndCd)
        {
            if (PhotonNetwork.offlineMode)
            {
                return $"Humanity Fail!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return "Humanity Fail!\nAgain!\nGame Restart in " + ((int) gameEndCd) + "s\n\n";
        }
    }
}
