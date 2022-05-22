using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.Settings;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using UnityEngine;
using System;
using Assets.Scripts.Extensions;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.Characters.Titan.Behavior;

namespace Assets.Scripts.Gamemode
{
    public class StandoffGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Standoff;
        private StandoffSettings Settings => GameSettings.Gamemode as StandoffSettings;

        public Faction Side1Players;
        public Faction Side2Players;
        public Faction Side1Titans;
        public Faction Side2Titans;
        private int Team1Score;
        private int Team2Score;

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            if (!PhotonNetwork.isMasterClient) return;
            #region Factions
            Side1Players = new Faction
            {
                Name = "Side1Players",
                Prefix = "1P",
                Color = Color.red
            };

            Side1Titans = new Faction
            {
                Name = "Side1Titans",
                Prefix = "1T",
                Color = Color.red
            };
            

            Side2Players = new Faction
            {
                Name = "Side2Players",
                Prefix = "2P",
                Color = Color.blue
            };
            

            Side2Titans = new Faction
            {
                Name = "Side2Titans",
                Prefix = "2T",
                Color = Color.blue
            };

            Side1Players.Allies.Add(Side2Titans);
            Side2Titans.Allies.Add(Side1Players);

            Side2Players.Allies.Add(Side1Titans);
            Side1Titans.Allies.Add(Side2Players);

            Side1Titans.Allies.Add(Side2Titans);
            Side2Titans.Allies.Add(Side1Titans);

            FactionService.Add(Side1Players);
            FactionService.Add(Side1Titans);
            FactionService.Add(Side2Players);
            FactionService.Add(Side2Titans);
            #endregion

            SpawnTitans(Settings.Titan.Start.Value, GetStandoffTitanConfiguration, "titanRespawn", Side1Titans);
            SpawnTitans(Settings.Titan.Start.Value, GetStandoffTitanConfiguration, "titanRespawn2", Side2Titans);
        }


        /// <summary>
        /// Modifies TitanBehaviour to make titans always chase players.
        /// </summary>
        /// <returns></returns>
        private TitanConfiguration GetStandoffTitanConfiguration()
        {
            var configuration = GetTitanConfiguration();

            //uses the same behaviour as the wave gamemode. If wave behaviour changes standoff may need a unique behaviour.
            configuration.Behaviors.Add(new WaveBehavior());
            configuration.ViewDistance = 999999f;
            return configuration;
        }


        //public override GameObject GetPlayerSpawnLocation(string tag)
        //{
        //    var objArray = GameObject.FindGameObjectsWithTag(tag);
        //    if (objArray.Length == 0) return null;
        //    return objArray[Random.Range(0, objArray.Length)];
        //}

        /// <summary>
        /// Respawns a titan on the opposite team/side of the faction given.
        /// </summary>
        /// <param name="faction"></param>
        //private void SpawnTitanOnOtherSide (Faction faction)
        //{
        /*Transform titanSpawnLocation;
        if (faction == Side1Titans)
        { titanSpawnLocation = GetPlayerSpawnLocation("titanRespawn1").transform; }

        else if (faction == Side2Titans)
        { titanSpawnLocation = GetPlayerSpawnLocation("titanRespawn").transform; }

        else
        {
            Debug.LogError("Error: StandoffGamemode.cs Titan is not a part of a standoff gamemode faction. Titan was spawned at default titan respawn point");
            titanSpawnLocation = GetPlayerSpawnLocation("titanRespawn").transform;
        }

        var newTitan = SpawnService.Spawn<MindlessTitan>(titanSpawnLocation.position, titanSpawnLocation.rotation, GetTitanConfiguration());
        */
        /*SpawnTitans(1, GetTitanConfiguration, "titanRespawn1", Side2Titans);
        var newTitanFaction =
        if (faction == Side1Titans)
        { newTitan.Faction = Side2Titans; }

        else if (faction == Side2Titans)
        { newTitan.Faction = Side1Titans; }

        else
        {
            Debug.LogError("Error: StandoffGamemode.cs Killed titan was not part of a standoff gamemode faction.");
            newTitan.Faction = Side1Titans;
        }
    }*/
        protected override void OnEntityRegistered(Entity entity)
        {

            
            if (entity is Hero)
            {
                var hero = entity as Hero;
                Transform spawnLocation;

                //TODO Remove this when a UI for selecting a team is added.
                entity.Faction = Side1Players;

                if (entity.Faction == Side1Players)
                { spawnLocation = GetPlayerSpawnLocation("playerRespawn").transform; }
                else if (entity.Faction == Side2Players)
                { spawnLocation = GetPlayerSpawnLocation("playerRespawn2").transform; }

                //TODO use actual in-game debug log.
                else
                {
                    Debug.LogError("Error: StandoffGamemode.cs Hero is not a part of team1 or team2. Hero was spawned at default player respawn point.");
                    spawnLocation = GetPlayerSpawnLocation("playerRespawn").transform;
                }

                //TODO This is a workaround for now. SpawnService needs to be changed.
                hero.transform.SetPositionAndRotation(spawnLocation.position, spawnLocation.rotation);
            }
        }

        protected override void OnEntityUnRegistered(Entity entity)
        {
            if (entity.GetType().IsSubclassOf(typeof(TitanBase)))
            {
                Debug.Log("Titan was killed");
                if (entity.Faction == Side1Titans)
                { SpawnTitans(1, GetStandoffTitanConfiguration, "titanRespawn2", Side2Titans); Debug.Log("Spawned titan on side 2"); }

                else if (entity.Faction == Side2Titans)
                { SpawnTitans(1, GetStandoffTitanConfiguration, "titanRespawn", Side1Titans); Debug.Log("Spawned titan on side 1");  }
            }
        }
        protected override void OnFactionDefeated(Faction faction)
        {
            string winner = "";
            if (faction == Side1Titans || faction == Side2Players)
            { winner = Localization.Gamemode.Shared.GetLocalizedString("TEAM", "1"); }
            else if (faction == Side2Titans || faction == Side1Players)
            { winner = Localization.Gamemode.Shared.GetLocalizedString("TEAM", "2"); }
            else { Debug.LogError("StandoffGamemode.cs Not valid faction defeated"); }
            photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"{winner} has won!\nRestarting in {{0}}s", 0, 0);
        }

        protected override void SetStatusTopRight()
        {
            var context = string.Concat($"{Localization.Gamemode.Shared.GetLocalizedString("TEAM")} ", Team1Score, $" : {Localization.Gamemode.Shared.GetLocalizedString("TEAM")} ", Team2Score, " ");
            UiService.SetMessage(LabelPosition.TopRight, context);
        }
        //protected override void SetStatusTop()
        //{
        //    var content = $"{Localization.Gamemode.Shared.GetLocalizedString("ENEMY_LEFT", FactionService.CountHostile(Service.Player.Self))} | " +
        //                  $"{Localization.Gamemode.Shared.GetLocalizedString("FRIENDLY_LEFT", FactionService.CountFriendly(Service.Player.Self))} | " +
        //                  $"{Localization.Common.GetLocalizedString("TIME")}: {TimeService.GetRoundDisplayTime()}";
        //    UiService.SetMessage(LabelPosition.Top, content);
        //}


    }
}
