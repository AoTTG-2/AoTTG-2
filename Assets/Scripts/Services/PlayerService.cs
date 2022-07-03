using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Services.Interface;
using UnityEngine;
using System;
namespace Assets.Scripts.Services
{
    /// <inheritdoc/>
    public class PlayerService : IPlayerService
    {
        public event OnTitanDamaged OnTitanDamaged;
        public event OnTitanHit OnTitanHit;
        public event OnHeroHit OnHeroHit;
        public event OnHeroKill OnHeroKill;

        /// <inheritdoc/>
        public void TitanDamaged(TitanDamagedEvent titanDamagedEvent)
        {
            Debug.Log("TitanDamaged event triggered;");
            OnTitanDamaged?.Invoke(titanDamagedEvent);
        }

        /// <inheritdoc/>
        public void TitanHit(TitanHitEvent titanHitEvent)
        {
            Debug.Log("TitanHit event triggered;");
            OnTitanHit?.Invoke(titanHitEvent);
        }

        /// <inheritdoc/>
        public void HeroHit(HeroHitEvent heroHitEvent)
        {
            Debug.Log("HeroHit event triggered;");
            OnHeroHit?.Invoke(heroHitEvent);
        }

        /// <inheritdoc/>
        public void HeroKill(HeroKillEvent heroKillEvent)
        {
            Debug.Log("HeroKill event triggered;");
            OnHeroKill?.Invoke(heroKillEvent);
        }

        public void OnRestart()
        {
        }

        public Entity Self { get; set; }
        public Faction Faction { get; private set; }
        public void SetFaction(Faction faction)
        {
            Faction = faction;
            if (Self != null)
            { Self.Faction = faction; }
        }

        public void OnJoinedRoom()
        {
            SetFaction(null);
        }
        /*private void PlayerService_OnConnectedToPhoton(object sender, EventArgs e)
        {
            Debug.Log("PlayerService_OnConnectedToPhoton");
            SetFaction(null);
        }
        private void PlayerService_OnDisconnectedFromPhoton(object sender, EventArgs e)
        {
            Debug.Log("PlayerService_OnDisconnectedFromPhoton");
            SetFaction(null);
        }
        private void Awake()
        {
            Service.Photon.PhotonService_OnDisconnectedFromPhoton += PlayerService_OnDisconnectedFromPhoton;
            Service.Photon.PhotonService_OnConnectedToPhoton += PlayerService_OnConnectedToPhoton;
        }
        private void OnDestroy()
        {
            Service.Photon.PhotonService_OnDisconnectedFromPhoton -= PlayerService_OnDisconnectedFromPhoton;
            Service.Photon.PhotonService_OnConnectedToPhoton -= PlayerService_OnConnectedToPhoton;
        }*/
        
    }
}
