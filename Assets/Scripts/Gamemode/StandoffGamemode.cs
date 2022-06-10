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
using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.UI;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Events;

namespace Assets.Scripts.Gamemode
{
    public class StandoffGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Standoff;
        private StandoffSettings Settings => GameSettings.Gamemode as StandoffSettings;

        public Faction Team1Players;
        public Faction Team2Players;
        public Faction Team2Titans;
        public Faction Team1Titans;
        private int team1Score = 0;
        private int team2Score = 0;

        protected override void Awake()
        {
            base.Awake();
            #region Faction initialization
            Team1Players = new Faction
            {
                Name = "Team1 Humans",
                Prefix = "1P",
                Color = Color.red
            };

            Team2Titans = new Faction
            {
                Name = "Team2 Titans",
                Prefix = "1T",
                Color = Color.red
            };


            Team2Players = new Faction
            {
                Name = "Team2 Humans",
                Prefix = "2P",
                Color = Color.blue
            };


            Team1Titans = new Faction
            {
                Name = "Team1 Titans",
                Prefix = "2T",
                Color = Color.blue
            };

            Team1Players.Allies.Add(Team1Titans);
            Team1Titans.Allies.Add(Team1Players);

            Team2Players.Allies.Add(Team2Titans);
            Team2Titans.Allies.Add(Team2Players);

            Team2Titans.Allies.Add(Team1Titans);
            Team1Titans.Allies.Add(Team2Titans);

            FactionService.Add(Team1Players);
            FactionService.Add(Team2Titans);
            FactionService.Add(Team2Players);
            FactionService.Add(Team1Titans);
            #endregion

            //TODO Add player titans
            Service.Ui.GetUiHandler().InGameUi.SpawnMenu.SetFactionOptions(new List<Faction> { Team1Players, Team2Players/*, Team1Titans, Team2Titans */});

            Service.Spawn.OnTitanKilled += OnTitanKilled;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            FactionService.Remove(Team1Players);
            FactionService.Remove(Team2Players);
            FactionService.Remove(Team2Titans);
            FactionService.Remove(Team1Titans);
        }
        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            if (!PhotonNetwork.isMasterClient) return;
            SpawnTitans(Settings.Titan.Start.Value, GetStandoffTitanConfiguration, "titanRespawn", Team2Titans);
            SpawnTitans(Settings.Titan.Start.Value, GetStandoffTitanConfiguration, "titanRespawn2", Team1Titans);
        }

        /// <summary>
        /// Modifies TitanBehaviour to make titans always chase players. Currently is the same behaviour as the wave gamemode.
        /// </summary>
        /// <returns></returns>
        private TitanConfiguration GetStandoffTitanConfiguration()
        {
            var configuration = GetTitanConfiguration();
            configuration.Behaviors.Add(new WaveBehavior());
            configuration.ViewDistance = 999999f;
            return configuration;
        }

        protected override void OnPlayerSpawn(Entity entity)
        {
            //Since the gamemode is destroyed and re-created on restart, if a player selected a StandoffGamemode faction last round, that faction no longer exists after a restart.
            //This checks if the player's faction does not exist in FactionService and then assigns the player to a new faction based on the name of their old faction.
            if (!FactionService.GetAll().Any(x => x.Equals(entity.Faction)))
            {
                var newFaction = FactionService.GetAll().Select(x => x).Where(x => x.Name == entity.Faction.Name).ToList();
                if (newFaction.Count() == 1)
                { entity.Faction = newFaction[0];}
                else
                { Debug.LogError("Spawned player does not have a valid faction."); }
            }
            //Debug.Log($"In OnEntityRegistered: Faction is {entity.Faction.Name}");
            if (entity is Hero)
            {
                var hero = entity as Hero;
                Transform spawnLocation;
                
                if (entity.Faction.Equals(Team1Players))
                { spawnLocation = GetPlayerSpawnLocation("playerRespawn").transform; }
                else if (entity.Faction.Equals(Team2Players))
                { spawnLocation = GetPlayerSpawnLocation("playerRespawn2").transform; }
                else
                {
                    Debug.LogError("Hero is not a part of StandoffGamemode team1 or team2. Hero was spawned at default player respawn point.");
                    spawnLocation = GetPlayerSpawnLocation("playerRespawn").transform;
                }

                /*if (entity.Faction.Name == Team1Players.Name)
                { spawnLocation = GetPlayerSpawnLocation("playerRespawn").transform; }
                else if (entity.Faction.Name == Team2Players.Name)
                { spawnLocation = GetPlayerSpawnLocation("playerRespawn2").transform; }
                else
                {
                    Debug.LogError("StandoffGamemode: Hero is not a part of team1 or team 2. Hero was spawned at the default human respawn point.");
                    spawnLocation = GetPlayerSpawnLocation("playerRespawn").transform;
                }*/

                hero.transform.SetPositionAndRotation(spawnLocation.position, spawnLocation.rotation);
            }
            else if (entity is PlayerTitan)
            {
                var titan = entity as PlayerTitan;
                Transform spawnLocation;
                if (entity.Faction.Equals(Team1Titans))
                { spawnLocation = GetPlayerSpawnLocation("titanRespawn2").transform; }
                else if (entity.Faction.Equals(Team2Titans))
                { spawnLocation = GetPlayerSpawnLocation("titanRespawn").transform; }
                else
                {
                    Debug.LogError("PlayerTitan is not a part of StandoffGamemode team1 or team2. Titan was spawned at default titan respawn point.");
                    spawnLocation = GetPlayerSpawnLocation("titanRespawn").transform;
                }
                titan.transform.SetPositionAndRotation(spawnLocation.position, spawnLocation.rotation);
            }
        }

        private void OnTitanKilled(Entity human, Entity deadTitan, int damage)
        {
            if (deadTitan.GetType().IsSubclassOf(typeof(TitanBase)) && !IsRoundOver)
            {
                //TODO make damage threshold customizable after #604 is done.
                //TODO change 500 to 1000. I set it to 500 for testing purposes because I suck.
                int numTitansToSpawn = 0;
                if (damage < 500)
                { numTitansToSpawn = 1; }
                else if (damage >= 500)
                { numTitansToSpawn = 2; }

                //Respawns a titan on the other side.
                if (deadTitan.Faction == Team2Titans)
                { SpawnTitans(numTitansToSpawn, GetStandoffTitanConfiguration, "titanRespawn2", Team1Titans); }

                else if (deadTitan.Faction == Team1Titans)
                { SpawnTitans(numTitansToSpawn, GetStandoffTitanConfiguration, "titanRespawn", Team2Titans); }
            }
        }
        /*protected override void OnEntityUnRegistered(Entity entity)
        {
            if (entity.GetType().IsSubclassOf(typeof(TitanBase)) && !IsRoundOver)
            {
                //Respawns a titan on the other side.
                if (entity.Faction == Team2Titans)
                { SpawnTitans(1, GetStandoffTitanConfiguration, "titanRespawn2", Team1Titans); }

                else if (entity.Faction == Team1Titans)
                { SpawnTitans(1, GetStandoffTitanConfiguration, "titanRespawn", Team2Titans); }
            }
        }*/

        protected override void OnFactionDefeated(Faction faction)
        {
            string winner = "";
            if (faction == Team2Titans || faction == Team2Players)
            {
                winner = Localization.Gamemode.Shared.GetLocalizedString("TEAM", "1");
                team1Score++;
            }
            else if (faction == Team1Titans || faction == Team1Players)
            {
                winner = Localization.Gamemode.Shared.GetLocalizedString("TEAM", "2");
                team2Score++;
            }
            else { Debug.LogError($"StandoffGamemode.cs Invalid faction defeated ({faction.Name.ToString()})"); }

            photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"{winner} has won!\nRestarting in {{0}}s", 0, 0);
        }

        protected override void SetStatusTop()
        {
            int friendlyHumans=0, friendlyTitans=0, enemyHumans=0, enemyTitans=0;
            
            //The OnUpdateEverySecond() coroutine will stop running if it gets a Null Reference Exception error. So checking this is necessary.
            if (Service.Player.Self == null) return;

            else if (Service.Player.Self.Faction.Equals(Team1Players) || Service.Player.Self.Faction.Equals(Team2Titans))
            {
                friendlyHumans = FactionService.CountMembers(Team1Players);
                friendlyTitans = FactionService.CountMembers(Team1Titans);
                enemyHumans = FactionService.CountMembers(Team2Players);
                enemyTitans = FactionService.CountMembers(Team2Titans);
            }
            else if (Service.Player.Self.Faction.Equals(Team2Players) || Service.Player.Self.Faction.Equals(Team1Titans))
            {
                friendlyHumans = FactionService.CountMembers(Team2Players);
                friendlyTitans = FactionService.CountMembers(Team2Titans);
                enemyHumans = FactionService.CountMembers(Team1Players);
                enemyTitans = FactionService.CountMembers(Team1Titans);
            }

            var content = $"Friendly humans: {friendlyHumans,-2} Enemy humans: {enemyHumans,-2}\n" +
                          $"Friendly titans: {friendlyTitans,-2} Enemy titans: {enemyTitans,-2}";
            /* TODO Use localized strings
            var content = $"{Localization.Gamemode.Shared.GetLocalizedString("ENEMY_LEFT", Service.Player.Self.Faction } | " +
                          $"{Localization.Gamemode.Shared.GetLocalizedString("FRIENDLY_LEFT", FactionService.CountFriendly(Service.Player.Self))} | " +
                          $"{Localization.Common.GetLocalizedString("TIME")}: {TimeService.GetRoundDisplayTime()}";
            */
            UiService.SetMessage(LabelPosition.Top, content);
        }

        //protected override void SetStatusTopLeft()
        //{
        //    base.SetStatusTopLeft();
        //}

        protected override void SetStatusTopRight()
        {
            var context = string.Concat($"{Localization.Gamemode.Shared.GetLocalizedString("TEAM", "1:")} ", team1Score, $" : {Localization.Gamemode.Shared.GetLocalizedString("TEAM", "2:")} ", team2Score, " ");
            UiService.SetMessage(LabelPosition.TopRight, context);
        }
    }
}
