using UnityEngine;
using TMPro;
using GameFramework;

public class LoadingSceneIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    // Update is called once per frame
    private void Start()
    {
        
    }
    void Update()
    {
        if (GameManager.Instance.sceneLoader.IsDone)
        {
            loadingText.text = "Click to continue";
        }
        else
        {
            loadingText.text = "Loading";
            int dotCount = (int)Time.time * 3;
            dotCount = (int)(dotCount % 4);
            for (int i = 0; i < dotCount; i++)
            {
                loadingText.text += ".";
            }
        }
    }
}
