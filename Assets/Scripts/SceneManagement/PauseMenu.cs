using RPG.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class PauseMenu : MonoBehaviour
    {

        SavingWrapper savingWrapper;
        AudioListener audioListener;

        private void Start()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void UnPauseGame()
        {
            Time.timeScale = 1;
        }

        public void SaveGame()
        {
            savingWrapper.Save();
        }

        public void ResetGame()
        {
            savingWrapper.Delete();
            SceneManager.LoadSceneAsync(0);
        }


        public void QuitGame()
        {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
    Application.Quit();
#elif (UNITY_WEBGL)
    Application.OpenURL("https://adhemazzabi.com/");
#endif
        }

    }
}