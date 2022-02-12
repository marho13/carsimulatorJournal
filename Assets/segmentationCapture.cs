using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class segmentationCapture : MonoBehaviour {

    public string folder = "D://Screenshots/SegImage/";
    public string imgNum = "";
    public string previousTime = "";
    private bool takeScreenshotOnNextFrame;
    private static segmentationCapture instance;
    public Camera myCamera;
    public List<byte[]> ImagesList;

    private void Awake()
    {
        instance = this;
        Debug.Log("Start");
        myCamera = gameObject.GetComponent<Camera>();
    }
    

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;

            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            //ImagesList.Add(byteArray);
            //Debug.Log(ImagesList.Count);
            System.IO.File.WriteAllBytes(folder + "/" + imgNum + ".png", byteArray);
            //Debug.Log("Saved CameraScreenshot.png");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
            Debug.Log("Segmentation done");
        }

    }

    IEnumerator saveImages(List<byte[]> listy)
    {
        foreach (byte[] il in listy)
        {
            System.IO.File.WriteAllBytes(folder + "/" + imgNum + ".png", il);
        }
        ImagesList.Clear();
        Debug.Log("Never gonna give you up, never gonna let you down");
        yield return new WaitForSecondsRealtime(3.0f);
    }

    private void TakeScreenshot(int width, int height, string time)
    {
        imgNum = time;
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height, string imgNum)
    {
        instance.TakeScreenshot(width, height, imgNum);
    }

}
