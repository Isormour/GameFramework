using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public class SceneLoader
    {
        public string SceneName { private set; get; } = "";
        public bool IsDone => loadingOperation?.progress >= 0.9f;
        System.Action onLoaded;
        bool isAdditive;
        bool clickToChange;
        AsyncOperation loadingOperation;
        AsyncOperation unloadingOperation;
        string previousLoadedScene = "";
        bool sceneLoaded = false;
        bool previousSceneUnloaded = false;
        List<string> loadedScenes = new List<string>();
        public Action OnLoadedScene;
        public SceneLoader()
        {
          
        }
        public void Initialize()
        {
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            GetCurrentScenes();
        }
        void GetCurrentScenes()
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene tempScene = SceneManager.GetSceneAt(i);
                loadedScenes.Add(tempScene.name);
            }
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
        public void UnloadAdditiveScene(string sceneName)
        {
            if (loadedScenes.Contains(sceneName))
            {
                SceneManager.UnloadSceneAsync(sceneName);
                loadedScenes.Remove(sceneName);
            }
        }
        public void LoadScene(string sceneName, System.Action onLoaded, bool isAdditive = false, bool clickToChange = false)
        {
            if (isAdditive)
            {
                loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                loadingOperation.completed += SceneLoaded;
            }
            else
            {
                previousLoadedScene = SceneName;
                SceneName = sceneName;
                loadingOperation = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
                loadingOperation.completed += LoadingSceneLoaded;
            }
            
            this.onLoaded = onLoaded;
            this.isAdditive = isAdditive;
            this.clickToChange = clickToChange;
            sceneLoaded = false;
            
            loadingOperation.priority = 255;
            loadedScenes.Add(sceneName);
        }

        private void InputManager_OnClicked()
        {
            InputManager.OnAnyClicked -= InputManager_OnClicked;
            SceneManager.UnloadSceneAsync("Loading");
            loadingOperation.allowSceneActivation = true;
        }

        private void SceneLoaded(AsyncOperation obj)
        {
            sceneLoaded = true;
            if (previousSceneUnloaded || isAdditive)
            {
                FinishChangeScene();
            }
        }
        private void SceneUnloaded(AsyncOperation obj)
        {
            previousSceneUnloaded = true;
            if (sceneLoaded)
            {
                FinishChangeScene();
            }
        }
        private void FinishChangeScene()
        {
            if (!isAdditive)
            {
                SceneManager.UnloadSceneAsync("Loading");
                Cleanup();
            }

            onLoaded?.Invoke();
            OnLoadedScene?.Invoke();
        }
        private void LoadingSceneLoaded(AsyncOperation obj)
        {
            loadingOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            loadingOperation.completed += SceneLoaded;
            if (previousLoadedScene != "")
            {
                previousSceneUnloaded = false;
                unloadingOperation = SceneManager.UnloadSceneAsync(previousLoadedScene);
                unloadingOperation.completed += SceneUnloaded;
                loadedScenes.Remove(previousLoadedScene);
            }
            else
            {
                previousSceneUnloaded = true;
            }
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
