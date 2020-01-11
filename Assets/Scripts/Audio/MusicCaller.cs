using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class MusicCaller : MonoBehaviour
    {
        [SerializeField] int songToPlay = 0;
        [SerializeField] float volume = .5f;
        [SerializeField] bool playFromLastScene = false;

        MusicPlayer musicPlayer;

        private void Start()
        {
            musicPlayer = GameObject.FindWithTag("Music Player").GetComponent<MusicPlayer>();
            if (playFromLastScene) { return; }
            musicPlayer.StopSong();
        }
        private void Update()
        {
            if (!musicPlayer.SongIsPlaying())
            {
                musicPlayer.PlaySong(songToPlay, volume);
            }
        }
    }

}
