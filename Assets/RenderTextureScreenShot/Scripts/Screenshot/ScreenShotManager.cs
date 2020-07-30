using System;
using System.Collections;
using UnityEngine;


public class ScreenShotManager : MonoBehaviour
{
    #region Singleton

    private static ScreenShotManager instance = null;
    public static ScreenShotManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(ScreenShotManager)) as ScreenShotManager;
                if (instance == null)
                {
                    instance =
                        new GameObject(nameof(ScreenShotManager))
                            .AddComponent(typeof(ScreenShotManager)) as ScreenShotManager;
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