using UnityEngine;

namespace GameFramework
{
    public abstract class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; protected set; }
        public SceneLoader sceneLoader { get; } = new();
    }
    public abstract class GameManager<TSelf,TInput> : GameManager where TSelf : GameManager<TSelf,TInput> where TInput : InputManager, new()
    {
        public static new TSelf Instance { get; private set; }
        public TInput inputManager { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = (TSelf)this;
                GameManager.Instance = this;
            }
        }
        protected virtual void Start()
        {
            inputManager = new();
        }
        protected virtual void Update()
        {
            sceneLoader.Update();
        }
    }
}