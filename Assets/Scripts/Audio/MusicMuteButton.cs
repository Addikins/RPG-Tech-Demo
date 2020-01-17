using UnityEngine;
using UnityEngine.UI;

namespace RPG.Audio
{
    public class MusicMuteButton : MonoBehaviour
    {
        [SerializeField] Sprite musicOn;
        [SerializeField] Sprite musicOff;

        bool isMuted;

        MusicPlayer musicPlayer;
        Image imageComponent;

        private void Start()
        {
            musicPlayer = FindObjectOfType<MusicPlayer>();
            imageComponent = GetComponent<Image>();

            isMuted = musicPlayer.GetComponent<AudioSource>().mute;
            SetSprite();
        }

        public void MuteMusic()
        {
            isMuted = !isMuted;
            musicPlayer.GetComponent<AudioSource>().mute = isMuted;
            SetSprite();
        }

        private void SetSprite()
        {
            if (isMuted)
            {
                imageComponent.sprite = musicOff;
                return;
            }
            imageComponent.sprite = musicOn;
        }
    }
}