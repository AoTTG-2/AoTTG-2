using System;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Class to manage all <see cref="Hero"/> related Audio
    /// </summary>
    [Serializable]
    public class HeroAudio
    {
        /// <summary>
        /// Reference to the <see cref="AudioSource"/>
        /// </summary>
        public AudioSource source;

        public AudioClip clipDie;
        public AudioClip clipHit;
        public AudioClip clipRope;
        public AudioClip clipSlash;

        /// <summary>
        /// World Position of <see cref="source"/>
        /// </summary>
        public Vector3 Position => source.transform.position;

        /// <summary>
        /// Play an <see cref="AudioClip"/> once. Internally calls <see cref="AudioSource.PlayOneShot(AudioClip)"/>
        /// </summary>
        /// <param name="clip"><see cref="AudioClip"/> to play</param>
        /// <returns><see cref="HeroAudio"/> to allow chaining of Methods: <code>heroAudio.PlayOneShot(clip).PlayOneShot(clip2);</code></returns>
        public HeroAudio PlayOneShot(AudioClip clip)
        {
            source.PlayOneShot(clip);
            return this;
        }
        /// <summary>
        /// Play an <see cref="AudioClip"/> once. Internally calls <see cref="AudioSource.PlayOneShot(AudioClip, float)"/>
        /// </summary>
        /// <param name="clip"><see cref="AudioClip"/> to play</param>
        /// <param name="volume">Volume to play the <see cref="AudioClip"/> at</param>
        /// <returns><see cref="HeroAudio"/> to allow chaining of Methods: <code>heroAudio.PlayOneShot(clip).PlayOneShot(clip2);</code></returns>
        public HeroAudio PlayOneShot(AudioClip clip, float volume)
        {
            source.PlayOneShot(clip, volume);
            return this;
        }

        /// <summary>
        /// Set the <see cref="Transform.parent"/> to NULL, then Destroy the GameObject after <paramref name="destroyTime"/>
        /// </summary>
        /// <param name="destroyTime">Time to destroy <see cref="source"/> in seconds</param>
        public void Disconnect(float destroyTime)
        {
            source.transform.SetParent(null, true);
            GameObject.Destroy(source, destroyTime);
        }
        /// <summary>
        /// Set the <see cref="Transform.parent"/> to NULL, then Destroy the GameObject after the duration of <paramref name="clip"/>
        /// </summary>
        /// <param name="clip">Destroy <see cref="source"/> after <see cref="AudioClip.length"/>+1 seconds. Internally calls <see cref="Disconnect(float)"/></param>
        public void Disconnect(AudioClip clip)
        {
            Disconnect(clip.length + 1);
        }
    }
}
