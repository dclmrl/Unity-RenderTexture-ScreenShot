using System;
using System.Collections;
using UnityEngine;


public class ScreenshotManager : MonoBehaviour
{
    #region Singleton

    private static ScreenshotManager instance = null;
    public static ScreenshotManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(ScreenshotManager)) as ScreenshotManager;
                if (instance == null)
                {
                    instance =
                        new GameObject(nameof(ScreenshotManager))
                            .AddComponent(typeof(ScreenshotManager)) as ScreenshotManager;
                }

                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }

    #endregion Singleton

    [SerializeField]
    private Camera screenShotCamera = null;

    [SerializeField]
    private RenderTexture targetRenderTexture = null;

    public static Action<RenderTexture> OnScreenShotTake = null;

    private void Awake()
    {
        screenShotCamera.targetTexture = targetRenderTexture;
        screenShotCamera.gameObject.SetActive(false);
    }

    public void TakeScreenShot()
    {
        StartCoroutine(nameof(ScreenShotRoutine));
    }

    private IEnumerator ScreenShotRoutine()
    {
        screenShotCamera.gameObject.SetActive(true);
        screenShotCamera.Render();
        yield return new WaitForEndOfFrame();
        screenShotCamera.gameObject.SetActive(false);
        
        OnScreenShotTake?.Invoke(targetRenderTexture);
    }
}
