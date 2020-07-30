using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public class ScreenShotSaver
{
    private static ScreenShotSaver instance = null;
    public static ScreenShotSaver Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScreenShotSaver();
            }

            return instance;
        }
    }
    private ScreenShotSaver() { }

    public void SaveAsPNG([NotNull] ref RenderTexture renderTexture,[NotNull] string path)=>
        SaveToFile(ref renderTexture, path, FileType.PNG);
    
    public void SaveAsJPG([NotNull] ref RenderTexture renderTexture,[NotNull] string path)=>
        SaveToFile(ref renderTexture, path, FileType.JPG);
    
    public void SaveAsTGA([NotNull] ref RenderTexture renderTexture,[NotNull] string path)=>
        SaveToFile(ref renderTexture, path, FileType.TGA);
    

    private static void SaveToFile(ref RenderTexture renderTexture, string path, FileType fileType)
    {
        RenderTexture.active = renderTexture;
        var tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        switch (fileType)
        {
            case FileType.PNG :
                path += ".png";
                bytes = tex.EncodeToPNG();
                break;
            case FileType.JPG :
                path += ".jpg";
                bytes = tex.EncodeToJPG();
                break;
            case FileType.TGA :
                path += ".tga";
                bytes = tex.EncodeToTGA();
                break;
            default :
                path += ".png";
                bytes = tex.EncodeToPNG();
                break;
        }

        File.WriteAllBytes(path, bytes);
        Debug.Log($"Screenshot saved to => {path}");
    }
}

public enum FileType
{
    PNG,
    JPG,
    TGA
}