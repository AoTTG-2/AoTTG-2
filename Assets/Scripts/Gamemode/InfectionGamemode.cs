using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode
{
    public class InfectionGamemode : GamemodeBase
    {
        public InfectionGamemode()
        {
            GamemodeType = GamemodeType.Infection;
            RespawnMode = RespawnMode.NEVER;
            IsPlayerTitanEnabled = true;
        }

        [UiElement("Start Infected", "The amount of players that start as an infected")]
        public int Infected { get; set; } = 1;

        public override void OnRestart()
        {
            int num;
            FengGameManagerMKII.imatitan.Clear();
            for (num = 0; num < PhotonNetwork.playerList.Length; num++)
            {
                var player = PhotonNetwork.playerList[num];
                var propertiesToSet = new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.isTitan, 1 }
                };
                player.SetCustomProperties(propertiesToSet);
            }
            var length = PhotonNetwork.playerList.Length;
            var infectionMode = Infected;
            for (num = 0; num < PhotonNetwork.playerList.Length; num++)
            {
                PhotonPlayer player2 = PhotonNetwork.playerList[num];
                if ((length > 0) && (UnityEngine.Random.Range((float)0f, (float)1f) <= (((float)infectionMode) / ((float)length))))
                {
                    ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
                    hashtable2.Add(PhotonPlayerProperty.isTitan, 2);
                    player2.SetCustomProperties(hashtable2);
                    FengGameManagerMKII.imatitan.Add(player2.ID, 2);
                    infectionMode--;
                }
                length--;
            }
            FengGameManagerMKII.instance.gameEndCD = 0f;
            FengGameManagerMKII.instance.restartGame2();
        }

        public override void OnUpdate(float interval)
        {
            int num21 = 0;
            for (var num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
            {
                PhotonPlayer targetPlayer = PhotonNetwork.playerList[num22];
                if ((!FengGameManagerMKII.ignoreList.Contains(targetPlayer.ID) && (targetPlayer.CustomProperties[PhotonPlayerProperty.dead] != null)) && (targetPlayer.CustomProperties[PhotonPlayerProperty.isTitan] != null))
                {
                    if (RCextensions.returnIntFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.isTitan]) == 1)
                    {
                        if (RCextensions.returnBoolFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.dead]) && (RCextensions.returnIntFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.deaths]) > 0))
                        {
                            if (!FengGameManagerMKII.imatitan.ContainsKey(targetPlayer.ID))
                            {
                                FengGameManagerMKII.imatitan.Add(targetPlayer.ID, 2);
                            }
                            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                            propertiesToSet.Add(PhotonPlayerProperty.isTitan, 2);
                            targetPlayer.SetCustomProperties(propertiesToSet);
                            FengGameManagerMKII.instance.photonView.RPC("spawnTitanRPC", targetPlayer, new object[0]);
                        }
                        else if (FengGameManagerMKII.imatitan.ContainsKey(targetPlayer.ID))
                        {
                            for (int k = 0; k < FengGameManagerMKII.instance.getPlayers().Count; k++)
                            {
                                Hero hero = (Hero)FengGameManagerMKII.instance.getPlayers()[k];
                                if (hero.photonView.owner == targetPlayer)
                                {
                                    hero.markDie();
                                    hero.photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "noswitchingfagt" });
                                }
                            }
                        }
                    }
                    else if (!((RCextensions.returnIntFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.isTitan]) != 2) || RCextensions.returnBoolFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.dead])))
                    {
                        num21++;
                    }
                }
            }
            if (num21 <= 0)
            {
                FengGameManagerMKII.instance.gameWin2();
            }
            
        }
    }
}
