using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class DefaultGameManager : GameManager<DefaultGameManager,InputManager>
    {
        protected override void Start()
        {
            base.Start();
            sceneLoader.LoadScene("Menu", () => { Debug.Log("Finished"); }, clickToChange: true);
        }
    }
}
