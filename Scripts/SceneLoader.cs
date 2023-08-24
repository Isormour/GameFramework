using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public class SceneLoader
    {
        public string SceneName { private set; get; }
        public bool IsDone => loadingOperation?.progress >= 0.9f;

        System.Action onLoaded;
        bool isAdditive;
        bool clickToChange;
        AsyncOperation loadingOperation;

        public SceneLoader()
        {

        }
        public void Update()
        {
            if (loadingOperation != null)
            {
                if (clickToChange)
                {
                    if (IsDone)
                    {
                        InputManager.OnAnyClicked += InputManager_OnClicked;
                    }
                }
            }
        }
        public void LoadScene(string sceneName, System.Action onLoaded, bool isAdditive = false, bool clickToChange = false)
        {
            SceneName = sceneName;
            this.onLoaded = onLoaded;
            this.isAdditive = isAdditive;
            this.clickToChange = clickToChange;

            if (this.isAdditive)
            {
                loadingOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
                loadingOperation.completed += SceneLoaded;
            }
            else
            {
                loadingOperation = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
                loadingOperation.completed += LoadingSceneLoaded;
            }
        }

        private void InputManager_OnClicked()
        {
            InputManager.OnAnyClicked -= InputManager_OnClicked;
            SceneManager.UnloadSceneAsync("Loading");
            loadingOperation.allowSceneActivation = true;
        }

        private void SceneLoaded(AsyncOperation obj)
        {
            onLoaded();
            if (!isAdditive)
            {
                SceneManager.UnloadSceneAsync("Loading");
                Cleanup();
            }
        }
        private void LoadingSceneLoaded(AsyncOperation obj)
        {
            loadingOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            loadingOperation.completed += SceneLoaded;
            if (clickToChange)
            {
                loadingOperation.allowSceneActivation = false;
            }
        }
        private void Cleanup()
        {
            loadingOperation = null;
        }
    }
}
