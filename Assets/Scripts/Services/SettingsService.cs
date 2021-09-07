using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.Settings.Titans;
using Assets.Scripts.Settings.Titans.Attacks;
using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class SettingsService : PunBehaviour, ISettingsService
    {
        private GameSettings Settings { get; set; }

        public event OnGamemodeSettingsChanged OnGamemodeSettingsChanged;
        public event OnPvpSettingsChanged OnPvpSettingsChanged;
        public event OnHorseSettingsChanged OnHorseSettingsChanged;
        public event OnRespawnSettingsChanged OnRespawnSettingsChanged;
        public event OnTitanSettingsChanged OnTitanSettingsChanged;
        public event OnTimeSettingsChanged OnTimeSettingsChanged;
        public event OnGlobalSettingsChanged OnGlobalSettingsChanged;

        /// <inheritdoc/>
        public GameSettings Get()
        {
            return Settings;
        }

        /// <inheritdoc/>
        public void SetRoomPropertySettings()
        {
            if (PhotonNetwork.offlineMode) return;
            if (PhotonNetwork.isMasterClient)
            {
                var hash = new Hashtable();
                hash.Add("Settings", JsonConvert.SerializeObject(Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                PhotonNetwork.room.SetCustomProperties(hash);
            }
            else
            {
                var json = (string) PhotonNetwork.room.CustomProperties["Settings"];
                Settings = new GameSettings();
                Settings.Initialize(json);
                OnSettingsChanged();
            }
        }

        /// <inheritdoc/>
        public void SetGamemodeType(GamemodeType type)
        {
            Settings.Initialize(type);
            OnGamemodeSettingsChanged?.Invoke(GameSettings.Gamemode);
        }

        /// <inheritdoc/>
        public void SyncSettings(GameSettings settings)
        {
            if (!PhotonNetwork.isMasterClient) return;

            Settings = settings;
            OnSettingsChanged();
            SyncSettings();
        }

        /// <inheritdoc/>
        public void SyncSettings(string json)
        {
            if (!PhotonNetwork.isMasterClient) return;

            Settings = new GameSettings();
            Settings.Initialize(json);
            SyncSettings();
        }

        /// <inheritdoc/>
        public void SyncSettings()
        {
            if (!PhotonNetwork.isMasterClient) return;

            Settings.Update();
            SetRoomPropertySettings();
            var json = JsonConvert.SerializeObject(Settings, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            photonView.RPC(nameof(SyncSettingsRpc), PhotonTargets.Others, json);
        }

        /// <inheritdoc/>
        public void SyncSettings(Difficulty difficulty)
        {
            Settings = new GameSettings();
            Settings.Initialize(
                GamemodeSettings.GetAll(difficulty),
                new PvPSettings(difficulty),
                new SettingsTitan(difficulty)
                {
                    Mindless = new MindlessTitanSettings(difficulty)
                    {
                        AttackSettings = AttackSetting.GetAll<MindlessTitan>(difficulty)
                    },
                    Female = new FemaleTitanSettings(difficulty),
                    Colossal = new ColossalTitanSettings(difficulty),
                    Eren = new TitanSettings(difficulty)
                },
                new HorseSettings(difficulty),
                new RespawnSettings(difficulty),
                new TimeSettings(),
                new GlobalSettings(difficulty)
            );

            if (!PhotonNetwork.isMasterClient) return;
            SetRoomPropertySettings();
            photonView.RPC(nameof(SyncSettingsRpc), PhotonTargets.Others, difficulty);
        }

        [PunRPC]
        public void SyncSettingsRpc(string settings, PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient || PhotonNetwork.isMasterClient) return;

            Settings = new GameSettings();
            Settings.Initialize(settings);
            OnSettingsChanged();
        }

        [PunRPC]
        public void SyncSettingsRpc(Difficulty difficulty, PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient || PhotonNetwork.isMasterClient) return;

            Debug.Log($"Setting difficulty to: {difficulty}");
            SyncSettings(difficulty);
            OnSettingsChanged();
        }

        private void OnSettingsChanged()
        {
            OnGamemodeSettingsChanged?.Invoke(GameSettings.Gamemode);
            OnPvpSettingsChanged?.Invoke(GameSettings.PvP);
            OnHorseSettingsChanged?.Invoke(GameSettings.Horse);
            OnRespawnSettingsChanged?.Invoke(GameSettings.Respawn);
            OnTitanSettingsChanged?.Invoke(GameSettings.Titan);
            OnGlobalSettingsChanged?.Invoke(GameSettings.Global);
            OnTimeSettingsChanged?.Invoke(GameSettings.Time);
        }
    }
}
