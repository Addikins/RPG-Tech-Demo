using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip[] audioClips = null;

        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void PlaySong(int songToPlay, float volume)
        {
            if (audioSource.isPlaying) { return; }
            audioSource.PlayOneShot(audioClips[songToPlay], volume);
        }
        public void StopSong()
        {
            audioSource.Stop();
        }
        public bool SongIsPlaying()
        {
            return audioSource.isPlaying;
        }
    }
}