using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;

namespace Assets.Scripts.Gamemode
{
    public class InfectionGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Infection;
        private InfectionGamemodeSettings Settings => GameSettings.Gamemode as InfectionGamemodeSettings;

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
            var infectionMode = Settings.Infected.Value;
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
            FengGameManagerMKII.instance.restartGame2();
        }

        //TODO: In AoTTG this ran every 0.1s instead of per frame. Investigate
        private void Update()
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
                            photonView.RPC(nameof(SpawnPlayerTitanRpc), targetPlayer, new object[0]);
                        }
                        else if (FengGameManagerMKII.imatitan.ContainsKey(targetPlayer.ID))
                        {
                            foreach (var hero in EntityService.GetAll<Hero>())
                            {
                                if (hero.photonView.owner == targetPlayer)
                                {
                                    hero.MarkDie();
                                    hero.photonView.RPC(nameof(Hero.NetDie2), PhotonTargets.All, new object[] { -1, "noswitchingfagt" });
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
            if (num21 <= 0 && PhotonNetwork.isMasterClient)
            {
                FengGameManagerMKII.Gamemode.HumanScore++;
                FengGameManagerMKII.Gamemode.photonView.RPC(nameof(GamemodeBase.OnGameEndRpc), PhotonTargets.All, $"Humanity has won!\nRestarting in {{0}}s", FengGameManagerMKII.Gamemode.HumanScore, FengGameManagerMKII.Gamemode.TitanScore);
            }
        }


        [PunRPC]
        private void SpawnPlayerTitanRpc(PhotonMessageInfo info)
        {
            if (info.sender.IsMasterClient)
            {
                SpawnService.Spawn<PlayerTitan>();
            }
        }
    }
}
