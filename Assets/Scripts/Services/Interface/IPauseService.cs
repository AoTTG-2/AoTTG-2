using System;

namespace Assets.Scripts.Services.Interface
{
    public interface IPauseService
    {
        PhotonView photonView { get; }
        float PauseTimer { get; }

        bool IsUnpausing();
        bool IsPaused();
        void Pause(bool shouldPause, bool immediate = false);
        void PauseRpc(bool shouldPause, bool immediate, PhotonMessageInfo info);

        event EventHandler OnPaused;
        event EventHandler OnUnPaused;
    }
}
