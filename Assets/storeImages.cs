using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeImages : MonoBehaviour
{
    public string segFolder = "D://Screenshots/SegImage/";
    public string folder = "D://Screenshots/Image/";
    public int imgNum = 1;
    public string fileName;

    public Camera cam;
    public Camera segCam;

    public List<string> fileNameList;

    private bool CaptureImages = true;


    public string day = System.DateTime.Now.ToString("MM/dd");

    
    
    public void Start()
    {
        cam.enabled = false;
        segCam.enabled = true;
        createDirectory();

        StartCoroutine(CaptureSegments());
    }

    private void createDirectory() 
    {
        if (!System.IO.Directory.Exists(folder + System.DateTime.Now.ToString("MM/dd")))
        {
            System.IO.Directory.CreateDirectory(folder + "/" + System.DateTime.Now.ToString("MM/dd"));
            System.IO.Directory.CreateDirectory(segFolder + "/" + System.DateTime.Now.ToString("MM/dd"));
        }
    }

    public void AddFileName(string timeName) 
    {
        fileNameList.Add(timeName);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            cam.enabled = !cam.enabled;
            segCam.enabled = !segCam.enabled;
        }
        if (System.DateTime.Now.ToString("MM/dd") != day) {
            createDirectory();
        }

        if (Input.GetKeyDown(KeyCode.N))
            CaptureImages = false;

    }

    private IEnumerator CaptureSegments()
    {
        while (true)
        {
            while (CaptureImages)
            {
                captureImages(720, 480, imgNum.ToString());
                yield return new WaitForSeconds(0.2f);
            }

            SaveImages();
            CaptureImages = true;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    private void captureImages(int width, int height, string timeNow)
    {
        // get the stuff on another thread 
        // await Task.Run(() => segmentationCapture.TakeScreenshot_Static(width, height, timeNow));
        SegCapture.TakeScreenshot_Static(width, height, timeNow);
        // await Task.Run(() => ImageCapture.TakeScreenshot_Static(width, height, timeNow));
    }

    private void SaveImages()
    {
        SegCapture.SaveImages();
    }
}