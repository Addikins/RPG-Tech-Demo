using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector director)
        {
            // Cancels current action and prevents further actions during cinematics
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<ThirdPersonCharacterController>().enabled = false;
        }

        void EnableControl(PlayableDirector director)
        {
            // Re-enables control to player after cinematic finishes
            player.GetComponent<ThirdPersonCharacterController>().enabled = true;
        }

    }
}