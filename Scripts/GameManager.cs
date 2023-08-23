using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public SceneLoader sceneLoader { get; } = new();
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        private void Start()
        {
            sceneLoader.LoadScene("Menu", () => { Debug.Log("Finished"); }, clickToChange: true);
            InputManager inputManager = new InputManager();
        }
        private void Update()
        {
            sceneLoader.Update();
        }
        public class InputManager
        {
            public static event Action OnAnyClicked;
            public InputManager()
            {
                GameControls controls = new GameControls();
                controls.MainMap.Enable();
                controls.MainMap.any.started += AnyClicked;
            }

            private void AnyClicked(UnityEngine.InputSystem.InputAction.CallbackContext obj)
            {
                OnAnyClicked?.Invoke();
            }
        }
    }
}