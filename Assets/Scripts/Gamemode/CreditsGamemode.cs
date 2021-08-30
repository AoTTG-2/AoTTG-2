using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Extensions;
using Assets.Scripts.Gamemode.Credits;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Game.Gamemodes;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode
{
    public class CreditsGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Credits;
        private CreditsGamemodeSetting Settings => Setting.Gamemode as CreditsGamemodeSetting;

        protected override void OnFactionDefeated(Faction faction)
        {
            if (IsRoundOver || !PhotonNetwork.isMasterClient) return;
            string winner;
            if (faction == FactionService.GetHumanity())
            {
                TitanScore++;
                winner = "Titanity";
            }
            else
            {
                HumanScore++;
                winner = "Humanity";
            }

            photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"{winner} has won!\nRestarting in {{0}}s", HumanScore, TitanScore);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            if (!PhotonNetwork.isMasterClient) return;
            if (Setting.Gamemode.Name.Contains("Annie")) //TODO: Make this a setting
            {
                var ftSpawn = GameObject.Find("titanRespawn").transform;
                SpawnService.Spawn<FemaleTitan>(ftSpawn.position, ftSpawn.rotation, new TitanConfiguration());
            }
            
            var contributors = Settings.Contributors;
            WriteQuote(contributors.Where(x => !string.IsNullOrEmpty(x.Quote)).ToList().GetRandom());
            SpawnContributorTitans(contributors);
        }

        private static void WriteQuote(Contributor contributor)
        {
            Service.Message.Local($"<i>\"{contributor.Quote}\" - {contributor.Name}, {contributor.Role}</i>", DebugLevel.Notification);
        }

        protected void SpawnContributorTitans(List<Contributor> contributors)
        {
            Coroutines.Add(StartCoroutine(SpawnTitan(contributors)));
        }

        private IEnumerator SpawnTitan(List<Contributor> contributors)
        {
            foreach (var contributor in contributors)
            {
                if (EntityService.Count<MindlessTitan>() >= Settings.Titan.Limit.Value) break;
                var (position, rotation) = GetSpawnLocation();
                var configuration = GetTitanConfiguration(contributor.TitanType);
                var mindlessTitan = SpawnService.Spawn<MindlessTitan>(position, rotation);
                mindlessTitan.Contributor = contributor;
                configuration.Health = contributor.Health <= 0 ? 1 : contributor.Health;
                configuration.Size = contributor.Size <= 0.1 ? 3.0f : contributor.Size;
                configuration.Type = contributor.TitanType;
                mindlessTitan.Initialize(configuration);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
