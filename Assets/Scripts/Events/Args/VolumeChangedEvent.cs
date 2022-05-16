using System;

namespace Assets.Scripts.Events.Args
{
    public abstract class VolumeChangedEvent : EventArgs
    {
        #region Constructors
        protected VolumeChangedEvent(float volume)
        {
            Volume = volume;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the current volume.
        /// </summary>
        public float Volume { get; }
        #endregion
    }

    public class MusicVolumeChangedEvent : VolumeChangedEvent
    {
        #region Constructors
        public MusicVolumeChangedEvent(float volume) : base(volume) {}
        #endregion
    }
}
