using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] float volume = .5f;
        [SerializeField] AudioClip audioClip = null;

        private void Awake()
        {
            
        }
        private void Start()
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position, volume);
        }
    }
}