using System;

namespace Assets.Scripts.Services.Interface
{
    public interface IPauseService
    {
        PhotonView photonView { get; }
        float PauseTimer { get; }

        bool IsUnpausing();
        bool IsPaused();
        /// <summary>
        /// Pauses the game (Sets Time.timeScale to 0)
        /// </summary>
        /// <param name="shouldPause"></param>
        /// <param name="immediate">True if the game should pause immediately, false if a timer should be used</param>
        void Pause(bool shouldPause, bool immediate = false);
        /// <summary>
        /// The RPC version of <see cref="Pause"/>
        /// </summary>
        /// <param name="shouldPause"></param>
        /// <param name="immediate"></param>
        /// <param name="info"></param>
        void PauseRpc(bool shouldPause, bool immediate, PhotonMessageInfo info);

        /// <summary>
        /// Invoked when the game is paused
        /// </summary>
        event EventHandler OnPaused;
        /// <summary>
        /// Invoked when the game is unpaused
        /// </summary>
        event EventHandler OnUnPaused;
    }
}
