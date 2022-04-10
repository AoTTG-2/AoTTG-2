using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Includes common parameters and logic that is to be shared between controllers dealing with audio
    /// </summary>
    internal abstract class AudioController : MonoBehaviour
    {
        #region Protected Properties
        protected List<AudioSource> audioSources;
        protected const float MinVolume = 0.0001f;
        protected const float MaxVolume = 1;
        [SerializeField]
        [Tooltip("Audio mixer group that the controller has responsibility for.")]
        protected AudioMixerGroup mixerGroup;
        [SerializeField]
        [Tooltip("Current volume of the attatched audio mixer group.")]
        [Range(MinVolume, MaxVolume)]
        protected float volume;
        [SerializeField]
        [Tooltip("The suffix of the exposed parameter for mixer groups (i.e. if the group is named Master and the name of the exposed volume parameter is MasterVol then the suffix should be \"Vol\").")]
        protected string exposedParameterSuffix;
        #endregion

        #region Constructors
        protected AudioController()
        {
            audioSources = new List<AudioSource>();
        }
        #endregion

        #region Monobehaviours
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region Protected Methods
        protected float NormalizeVolume(float volume)
        {
            volume = volume <= 0 ? MinVolume : volume;
            volume = volume > 1 ? MaxVolume : volume;
            return volume;
        }

        // ALL mixer group exposed parameters must have the same suffix that is set in the field Suffix
        protected string GetExposedParameterName(AudioMixerGroup mixerGroup)
        {
            return $"{mixerGroup.name}{exposedParameterSuffix}";
        }
        #endregion
    }
}
