using System;
using UnityEngine;


public class Test : MonoBehaviour
{
    private void OnEnable()
    {
        ScreenShotManager.OnScreenShotTake += Save;
    }
    
    private void OnDisable()
    {
        ScreenShotManager.OnScreenShotTake -= Save;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ScreenShotManager.Instance.TakeScreenShot();
        }
    }

    private void Save(RenderTexture renderTexture)
    {
        ScreenShotSaver.Instance.SaveAsPNG(ref renderTexture, "Assets/ss1");
        ScreenShotSaver.Instance.SaveAsJPG(ref renderTexture, "Assets/ss1");
        ScreenShotSaver.Instance.SaveAsTGA(ref renderTexture, "Assets/ss1");
    }
}