using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Includes common parameters and logic that is to be shared between controller dealing with audio. Inherits <see cref="SingeltonMonoBehaviour{Tclass}"/>
    /// </summary>
    /// <typeparam name="Tclass"></typeparam>
    public abstract class AudioController<Tclass> : SingeltonMonoBehaviour<Tclass> where Tclass : class
    {
        #region Public Properties
        [Tooltip("Audio mixer group that the controller has responsibility for")]
        public AudioMixerGroup MixerGroup;
        [Tooltip("Current volume of the attatched audio mixer group")]
        [Range(0.0001f, 1f)]
        public float Volume;
        [Tooltip("The name of the attatched audio mixer group's exposed volume parameter")]
        public string VolumeParameterName;
        #endregion

        #region Private Properties
        private float minVolume = 0.0001f;
        private float maxVolume = 1;
        #endregion

        #region Protected Properties
        protected List<AudioSource> audioSources;
        #endregion

        #region Constructors
        protected AudioController()
        {
            Volume = .5f;
        }
        #endregion

        #region Monobehaviours
        protected override void Awake()
        {
            base.Awake();
        }
        #endregion

        #region Protected Methods
        protected float GetLogVolume(float volume)
        {
            return volume == 0 ? minVolume : volume > 1 ? maxVolume : volume;
        }
        #endregion
    }
}
