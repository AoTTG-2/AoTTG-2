using Assets.Scripts.Services.Interface;
using Photon;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class PauseService : PunBehaviour, IPauseService
    {
        private const float DefaultPauseTime = 3f;

        private bool isUnpausing;
        public bool IsUnpausing() => isUnpausing;

        private bool isPaused;
        public bool IsPaused() => isPaused;

        public float PauseTimer { get; private set; }

        public event EventHandler OnPaused;
        public event EventHandler OnUnPaused;

        public void Pause(bool shouldPause, bool immediate = false)
        {
            if (shouldPause && !isPaused)
            {
                isPaused = true;
                isUnpausing = false;
                PauseTimer = float.MaxValue;
                Time.timeScale = 1E-06f;
                OnPaused?.Invoke(this, EventArgs.Empty);
            }
            else if (!shouldPause && isPaused && !isUnpausing)
            {
                isUnpausing = true;
                PauseTimer = immediate ? 0f : DefaultPauseTime;
                OnUnPaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                isUnpausing = false;
                PauseTimer = float.MaxValue;
                OnPaused?.Invoke(this, EventArgs.Empty);
            }
        }

        [PunRPC]
        public void PauseRpc(bool shouldPause, bool immediate, PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient) return;
            Pause(shouldPause, immediate);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if (!PhotonNetwork.isMasterClient) return;
            if (isPaused)
            {
                photonView.RPC(nameof(PauseRpc), newPlayer, true, true);
                object[] parameters = new object[] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
                FengGameManagerMKII.instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), newPlayer, parameters);
            }
        }

        private void OnLevelWasLoaded()
        {
            if (isPaused)
            {
                Pause(false, true);
            }
        }
        
        private void LateUpdate()
        {
            if (!isPaused) return;

            if (PauseTimer <= DefaultPauseTime)
            {
                PauseTimer -= Time.deltaTime * 1000000f;
                if (PauseTimer <= 0f)
                {
                    Time.timeScale = 1f;
                    isUnpausing = false;
                    isPaused = false;
                }
            }
        }
    }
}
