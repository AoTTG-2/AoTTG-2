using System;

namespace Assets.Scripts.Services.Interface
{
    public interface IPauseService
    {
        PhotonView photonView { get; }
        float PauseTimer { get; }

        bool IsPaused();
        void Pause(bool value, bool immediate = false);
        void PauseRpc(PhotonMessageInfo info);

        event EventHandler OnPaused;
        event EventHandler OnUnPaused;
    }
}
