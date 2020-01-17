using UnityEngine;

namespace RPG.Audio
{
    public class AudioMuter : MonoBehaviour
    {
        [SerializeField] bool isAudioMuted = false;


        private void Awake()
        {
            if (isAudioMuted)
            {
                AudioListener.pause = true;
            }
            else
            {
                AudioListener.pause = false;
            }
        }

        public void MuteAudio()
        {
            isAudioMuted = !AudioListener.pause;
            AudioListener.pause = isAudioMuted;
        }

        public bool GetAudioStatus()
        {
            return isAudioMuted;
        }
    }

}