using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events.Args
{
    public abstract class VolumeChangedEvent
    {
        #region Constructors
        public VolumeChangedEvent(float volume)
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
