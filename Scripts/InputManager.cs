using System;

namespace GameFramework
{
    public class InputManager
    {
        public static event Action OnAnyClicked;
        public InputManager()
        {
            Initialize();
        }
        protected virtual void Initialize()
        {   // using sample input map
            GameControls controls = new GameControls();
            controls.MainMap.Enable();
            controls.MainMap.any.started += AnyClicked;
        }
        private void AnyClicked(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnAnyClicked?.Invoke();
        }
        public virtual void Update()
        {
            
        }
    }
}
