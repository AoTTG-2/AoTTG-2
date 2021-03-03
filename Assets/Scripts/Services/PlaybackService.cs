using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.Input;
using System.IO;

namespace Assets.Scripts.Services
{
    public class PlaybackService : MonoBehaviour, IPlaybackService
    {
        public static bool IsPlaying { get; private set; }
        public static bool IsRecording { get; private set; }

        private static bool _onPlayingInput;
        private static bool _onRecordingInput;

        private void Awake()
        {
            InputManager.Playback.RegisterDown(InputPlayback.StartRecording, OnStartRecord);
            InputManager.Playback.RegisterDown(InputPlayback.StopRecording, OnStopRecord);
            InputManager.Playback.RegisterDown(InputPlayback.StartPlaying, OnStartPlay);
            InputManager.Playback.RegisterDown(InputPlayback.StopPlaying, OnStopPlay);
        }

        private void LateUpdate()
        {
            _onPlayingInput = false;
            _onRecordingInput = false;
        }

        private void OnStartRecord()
        {
            if (IsRecording || IsPlaying || _onRecordingInput)
                return;

            StartCoroutine(Record());

            IsRecording = true;
            _onRecordingInput = true;
            Debug.Log("Recording Started");
        }

        private void OnStopRecord()
        {
            if (!IsRecording || IsPlaying || _onRecordingInput)
                return;

            StopCoroutine(Record());

            IsRecording = false;
            _onRecordingInput = true;
            Debug.Log("Recording Stopped");
        }

        private void OnStartPlay()
        {
            if (IsPlaying || IsRecording || _onPlayingInput)
                return;

            StartCoroutine(Play());

            IsPlaying = true;
            _onPlayingInput = true;
            Debug.Log("Playback Started");
        }

        private void OnStopPlay()
        {
            if (!IsPlaying || IsRecording || _onPlayingInput)
                return;

            StopCoroutine(Play());

            IsPlaying = false;
            _onPlayingInput = true;
            Debug.Log("Playback Stopped");
        }

        IEnumerator Record()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                int tick = Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);
            }
        }

        IEnumerator Play()
        {
            float start = Time.fixedDeltaTime;
            Hero hero = FindObjectOfType<Hero>();
            Debug.Log(hero);

            while (true)
            {
                yield return new WaitForFixedUpdate();
                int tick = Mathf.RoundToInt((Time.fixedTime - start)/ Time.fixedDeltaTime);
            }
        }

        private void OnDestroy()
        {
            InputManager.Playback.DeregisterDown(InputPlayback.StartRecording, OnStartRecord);
            InputManager.Playback.DeregisterDown(InputPlayback.StopRecording, OnStopRecord);
            InputManager.Playback.DeregisterDown(InputPlayback.StartPlaying, OnStartPlay);
            InputManager.Playback.DeregisterDown(InputPlayback.StopPlaying, OnStopPlay);
        }

        public bool Save(string filePath)
        {

            return true;
        }

        public void OnRestart()
        {

        }
    }
}