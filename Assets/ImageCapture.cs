using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCapture : MonoBehaviour
{
    public static string folder = "D://Screenshots/Image/";
    private bool takeScreenshotOnNextFrame;
    private static ImageCapture instance;

    public Camera myCamera;
    public static Dictionary<string, Texture2D> ImagesList = new Dictionary<string, Texture2D>();
    public List<byte> tempList;

    private void Awake()
    {
        instance = this;
    }
    public void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;

            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);


            ImagesList.Add(DateTime.Now.Ticks.ToString(), renderResult);

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

    public static void SaveImages()
    {
        foreach (var texture2D in ImagesList)
        {
            byte[] byteArray = texture2D.Value.EncodeToPNG();
            System.IO.File.WriteAllBytes(folder + "/" + texture2D.Key + ".png", byteArray);
        }
        ImagesList.Clear();
    }

    private void TakeScreenshot(int width, int height, string time)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height, string time)
    {
        instance.TakeScreenshot(width, height, time);
    }
}