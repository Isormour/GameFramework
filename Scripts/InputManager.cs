using System;

namespace GameFramework
{
    public class InputManager
    {
        public static event Action OnAnyClicked;
        public InputManager()
        {
           
        }
        public virtual void Initialize()
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
