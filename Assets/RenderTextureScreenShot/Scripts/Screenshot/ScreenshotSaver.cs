using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class ScreenshotSaver
{
    private static ScreenshotSaver instance = null;
    public static ScreenshotSaver Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScreenshotSaver();
            }

            return instance;
        }
    }
    private ScreenshotSaver() { }

    public void SaveAsPNG([NotNull] ref RenderTexture renderTexture, [NotNull] string path) =>
        SaveToFile(ref renderTexture, path, FileType.PNG);

    public void SaveAsJPG([NotNull] ref RenderTexture renderTexture, [NotNull] string path) =>
        SaveToFile(ref renderTexture, path, FileType.JPG);

    public void SaveAsTGA([NotNull] ref RenderTexture renderTexture, [NotNull] string path) =>
        SaveToFile(ref renderTexture, path, FileType.TGA);

    public void SaveAsPNG([NotNull] ref Texture2D texture, [NotNull] string path) =>
        SaveToFile(ref texture, path, FileType.PNG);

    public void SaveAsJPG([NotNull] ref Texture2D texture, [NotNull] string path) =>
        SaveToFile(ref texture, path, FileType.JPG);

    public void SaveAsTGA([NotNull] ref Texture2D texture, [NotNull] string path) =>
        SaveToFile(ref texture, path, FileType.TGA);

    private static void SaveToFile(ref RenderTexture renderTexture, string path, FileType fileType)
    {
        RenderTexture.active = renderTexture;
        var textureToSave = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        textureToSave.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        switch (fileType)
        {
            case FileType.PNG :
                path += ".png";
                bytes = textureToSave.EncodeToPNG();
                break;
            case FileType.JPG :
                path += ".jpg";
                bytes = textureToSave.EncodeToJPG();
                break;
            case FileType.TGA :
                path += ".tga";
                bytes = textureToSave.EncodeToTGA();
                break;
            default :
                path += ".png";
                bytes = textureToSave.EncodeToPNG();
                break;
        }

        File.WriteAllBytes(path, bytes);
        Debug.Log($"Screenshot saved to => {path}");
    }

    private void SaveToFile(ref Texture2D texture, string path, FileType fileType)
    {
        byte[] bytes;
        var textureToSave = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        textureToSave.SetPixels(texture.GetPixels());
        switch (fileType)
        {
            case FileType.PNG :
                path += ".png";
                bytes = textureToSave.EncodeToPNG();
                break;
            case FileType.JPG :
                path += ".jpg";
                bytes = textureToSave.EncodeToJPG();
                break;
            case FileType.TGA :
                path += ".tga";
                bytes = textureToSave.EncodeToTGA();
                break;
            default :
                path += ".png";
                bytes = textureToSave.EncodeToPNG();
                break;
        }

        File.WriteAllBytes(path, bytes);
        Object.DestroyImmediate(textureToSave);
        Debug.Log($"Screenshot saved to => {path}");
    }

    public List<Texture2D> GetAllSavedTextures(FileType fileType)
    {
        var savedTextures = new List<Texture2D>();
        var paths = GetAllSavedScreenshotPaths(fileType);

        paths.ForEach(path => { savedTextures.Add(LoadTextureFromPath(path)); });

        return savedTextures;
    }

    public Texture2D LoadTextureFromPath(string path)
    {
        if (!File.Exists(path))
        {
            return Texture2D.blackTexture;
        }

        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(path));

        return texture;
    }

    public static List<string> GetAllSavedScreenshotPaths(FileType fileType) =>
        Directory.GetFiles(Path.Combine(Application.persistentDataPath, "Screenshots"),
            GetSearchPattern(fileType)).ToList();

    public static string GetSearchPattern(FileType fileType)
    {
        switch (fileType)
        {
            case FileType.PNG :
                return "*.png";
            case FileType.JPG :
                return "*.jpg";
            case FileType.TGA :
                return "*.tga";
            default :
                return "*.png";
        }
    }
}

public enum FileType
{
    PNG,
    JPG,
    TGA
}
