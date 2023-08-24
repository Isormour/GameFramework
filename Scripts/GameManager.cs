using UnityEngine;

namespace GameFramework
{
    public abstract class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; protected set; }
        public SceneLoader sceneLoader { get; } = new();
        public InputManager inputManager { get; protected set; }
    }
    public abstract class GameManager<T> : GameManager where T: GameManager<T>
    {
        public static new T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = (T)this;
                GameManager.Instance = this;
            }
        }
        protected virtual void Start()
        {
            inputManager = new InputManager();
        }
        protected virtual void Update()
        {
            sceneLoader.Update();
        }
    }
}