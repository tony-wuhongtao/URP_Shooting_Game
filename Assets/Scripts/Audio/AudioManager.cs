using System;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace TonyLearning.ShootingGame.Audio
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        [SerializeField] private AudioSource sFXPlayer;

        private const float MIN_PITCH = 0.9f;
        private const float MAX_PITCH = 1.1f;
        public void PlaySFX(AudioData audioData)
        {
            sFXPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
        }

        public void PlayRandomSFX(AudioData audioData)
        {
            sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
            PlaySFX(audioData);
        }

        public void PlayRandomSFX(AudioData[] audioData)
        {
            PlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
        }

    }

    [System.Serializable] public class AudioData
    {
        public AudioClip audioClip;
        public float volume;
    }
}


