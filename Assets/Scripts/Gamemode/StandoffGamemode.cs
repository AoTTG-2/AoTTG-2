using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.Settings;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using UnityEngine;
namespace Assets.Scripts.Gamemode
{
    public class StandoffGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Standoff;
        private StandoffSettings Settings => GameSettings.Gamemode as StandoffSettings;

        private Faction Team1Players;
        private Faction Team2Players;
        private Faction Team1Titans;
        private Faction Team2Titans;

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            if (!PhotonNetwork.isMasterClient) return;
            SpawnTitans(2);
            #region Factions
            Team1Players = new Faction
            {
                Name = "Team1Players",
                Prefix = "1P",
                Color = Color.red
            };
            FactionService.Add(Team1Players);

            Team1Titans = new Faction
            {
                Name = "Team1Titans",
                Prefix = "1T",
                Color = Color.red
            };
            FactionService.Add(Team1Titans);

            Team2Players = new Faction
            {
                Name = "Team2Players",
                Prefix = "2P",
                Color = Color.blue
            };
            FactionService.Add(Team2Players);

            Team2Titans = new Faction
            {
                Name = "Team2Titans",
                Prefix = "2T",
                Color = Color.blue
            };
            FactionService.Add(Team2Titans);
            #endregion
        }

        public override GameObject GetPlayerSpawnLocation(string tag)
        {
            var objArray = GameObject.FindGameObjectsWithTag(tag);
            if (objArray.Length == 0) return null;
            return objArray[Random.Range(0, objArray.Length)];
        }

        protected override void OnEntityRegistered(Entity entity)
        {

            
            if (entity is Hero)
            {
                var hero = entity as Hero;
                GameObject spawnLocation;

                if (entity.Faction == Team1Players)
                { spawnLocation = GetPlayerSpawnLocation("playerRespawn"); }
                else if (entity.Faction == Team2Players)
                { spawnLocation = GetPlayerSpawnLocation("playerRespawn1"); }

                //TODO use actual in-game debug log.
                else
                {
                    Debug.LogError("Error: StandoffGamemode.cs Hero is not a part of team1 or team2. Hero was spawned at default player respawn point.");
                    spawnLocation = GetPlayerSpawnLocation("playerRespawn");
                }

                //TODO This is a workaround for now. SpawnService needs to be changed.
                hero.transform.SetPositionAndRotation(spawnLocation.transform.position, spawnLocation.transform.rotation);
            }
        }

        protected override void OnFactionDefeated(Faction faction)
        {
            //var endMessage = 
            //if (hostileEntities.Count == 0)
            //{
            //    photonView.RPC(OnGameEndRpc, PhotonTargets.All, endMessage)
            //}
        }


    }
}
