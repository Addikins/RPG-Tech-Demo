using UnityEngine;
using UnityEngine.UI;

namespace RPG.Audio
{
    public class AudioMuteButton : MonoBehaviour
    {
        [SerializeField] Sprite audioOn;
        [SerializeField] Sprite audioOff;

        AudioMuter audioMuter;
        Image imageComponent;

        bool isMuted;

        private void Start()
        {
            audioMuter = FindObjectOfType<AudioMuter>();
            imageComponent = GetComponent<Image>();

            SetSprite();
        }

        public void MuteSound()
        {
            audioMuter.MuteAudio();
            SetSprite();
        }

        private void SetSprite()
        {
            if (audioMuter.GetAudioStatus())
            {
                imageComponent.sprite = audioOff;
                return;
            }
            imageComponent.sprite = audioOn;
        }

    }
}
