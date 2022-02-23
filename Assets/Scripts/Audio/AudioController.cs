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
    public abstract class AudioController<Tclass> : SingeltonMonoBehaviour<Tclass> where Tclass : class
    {
        public AudioMixerGroup Mixer;
        [Range(0.0001f, 1f)]
        public float Volume;
        protected readonly IMusicService musicService = Service.Music;
        private float minVolume = 0.0001f;
        private float maxVolume = 1;

        protected AudioController()
        {
            Volume = .5f;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected float GetLogVolume(float volume)
        {
            return volume == 0 ? minVolume : volume > 1 ? maxVolume : volume;
        }
    }
}
