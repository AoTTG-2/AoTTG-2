using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Game.Gamemodes;
using Assets.Scripts.Settings.Titans.Attacks;
using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class SettingsService : PunBehaviour, ISettingsService
    {
        private GamemodeSetting Settings { get; set; }

        public event OnPvpSettingsChanged OnPvpSettingsChanged;
        public event OnHorseSettingsChanged OnHorseSettingsChanged;
        public event OnRespawnSettingsChanged OnRespawnSettingsChanged;
        public event OnTimeSettingsChanged OnTimeSettingsChanged;
        public event OnGlobalSettingsChanged OnGlobalSettingsChanged;

        /// <inheritdoc/>
        public GamemodeSetting Get()
        {
            return Settings;
        }

        /// <inheritdoc/>
        public void SetRoomPropertySettings()
        {
            return;
            //if (PhotonNetwork.offlineMode) return;
            //if (PhotonNetwork.isMasterClient)
            //{
            //    var hash = new Hashtable();
            //    hash.Add("Settings", JsonConvert.SerializeObject(Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            //    PhotonNetwork.room.SetCustomProperties(hash);
            //}
            //else
            //{
            //    var json = (string) PhotonNetwork.room.CustomProperties["Settings"];
            //    Settings = new Setting.Gamemode();
            //    Settings.Initialize(json);
            //    OnSettingsChanged();
            //}
        }

        /// <inheritdoc/>
        public void SetGamemodeType(GamemodeType type)
        {
            return;
            //Settings.Initialize(type);
            //OnGamemodeSettingsChanged?.Invoke(Setting.Gamemode.Gamemode);
        }

        /// <inheritdoc/>
        public void SyncSettings(GamemodeSetting settings)
        {
            if (!PhotonNetwork.isMasterClient) return;

            Settings = settings;
            OnSettingsChanged();
            SyncSettings();
        }
        
        /// <inheritdoc/>
        public void SyncSettings(string json)
        {
            return;
            //if (!PhotonNetwork.isMasterClient) return;

            //Settings = new Setting.Gamemode();
            //Settings.Initialize(json);
            //SyncSettings();
        }

        /// <inheritdoc/>
        public void SyncSettings()
        {
            return;
            //if (!PhotonNetwork.isMasterClient) return;

            //Settings.Update();
            //SetRoomPropertySettings();
            //var json = JsonConvert.SerializeObject(Settings, new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //});
            //photonView.RPC(nameof(SyncSettingsRpc), PhotonTargets.Others, json);
        }
        
        [PunRPC]
        public void SyncSettingsRpc(string settings, PhotonMessageInfo info)
        {
            return;
            //if (!info.sender.IsMasterClient || PhotonNetwork.isMasterClient) return;

            //Settings = new Gamemode;
            //Settings.Initialize(settings);
            //OnSettingsChanged();
        }
        
        private void OnSettingsChanged()
        {
            OnPvpSettingsChanged?.Invoke(Setting.Gamemode.PvP);
            OnHorseSettingsChanged?.Invoke(Setting.Gamemode.Horse);
            OnRespawnSettingsChanged?.Invoke(Setting.Gamemode.Respawn);
            OnGlobalSettingsChanged?.Invoke(Setting.Gamemode.Global);
            OnTimeSettingsChanged?.Invoke(Setting.Gamemode.Time);
        }
    }
}
