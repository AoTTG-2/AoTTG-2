using Assets.Scripts.Services.Interface;
using Photon;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class PauseService : PunBehaviour, IPauseService
    {

        private bool isPaused;
        
        public float PauseTimer { get; private set; }
        public bool IsPaused() => isPaused;

        public event EventHandler OnPaused;
        public event EventHandler OnUnPaused;

        public void Pause(bool value, bool immediate = false)
        {
            if (!isPaused)
            {
                PauseTimer = float.MaxValue;
                Time.timeScale = 1E-06f;
                isPaused = true;
                OnPaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                PauseTimer = immediate ? 0f : 3f;
                OnUnPaused?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnLevelWasLoaded()
        {
            if (this.IsPaused())
            {
                this.Pause(false, true);
            }
        }
        
        private void LateUpdate()
        {
            if (!isPaused) return;
            if (PauseTimer <= 3f)
            {
                PauseTimer -= Time.deltaTime * 1000000f;
                if (PauseTimer <= 0f)
                {
                    Time.timeScale = 1f;
                    isPaused = false;
                }
            }
        }

        [PunRPC]
        public void PauseRpc(PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient) return;
            Pause(!isPaused);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if (!PhotonNetwork.isMasterClient) return;
            if (IsPaused())
            {
                photonView.RPC(nameof(PauseRpc), newPlayer);
                object[] parameters = new object[] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
                FengGameManagerMKII.instance.photonView.RPC("Chat", newPlayer, parameters);
            }
        }
    }
}
