using UnityEngine;
using System.Collections;
using OpenCvSharp;

public class VideoTest : MonoBehaviour
{
    public WebCamTexture cameraTexture;
    Texture2D rt;
    public string cameraName = "";
    private bool isPlay = false;
    static int mPreviewWidth = 320;
    static int mPreviewHeight = 240;
    bool state = false;
    CascadeClassifier haarCascade;
    WebCamDevice[] devices;
    // Use this for initialization
    void Start()
    {
        rt = new Texture2D(mPreviewWidth, mPreviewHeight, TextureFormat.RGB565, false);
        temp = new Texture2D(mPreviewWidth, mPreviewHeight, TextureFormat.RGB565, false);
        StartCoroutine(Test());
        haarCascade = new CascadeClassifier(Application.streamingAssetsPath + "/haarcascades/haarcascade_frontalface_alt2.xml");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Test()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            devices = WebCamTexture.devices;
            cameraName = devices[0].name;
            cameraTexture = new WebCamTexture(cameraName, mPreviewWidth, mPreviewHeight, 15);
            cameraTexture.Play();
            isPlay = true;
        }
    }
    Mat haarResult;
    byte[] bs;
    void OnGUI()
    {
        if (isPlay)
        {
            GUI.DrawTexture(new UnityEngine.Rect(0, 0, mPreviewWidth, mPreviewHeight), cameraTexture, ScaleMode.ScaleToFit);

            if (GUI.Button(new UnityEngine.Rect(0, 240, 80, 80), "START"))
            {
                if (state)
                {
                    state = false;
                    return;
                }
                else
                {
                    state = true;
                }
            }

            if (state)
            {
                // Detect faces
                haarResult = DetectFace(haarCascade, GetTexture2D(cameraTexture));
                bs = haarResult.ToBytes(".png");
                rt.LoadImage(bs);
                rt.Apply();
                GUI.DrawTexture(new UnityEngine.Rect(mPreviewWidth, 0, mPreviewWidth, mPreviewHeight), rt, ScaleMode.StretchToFill);
            }

        }

    }

    Mat result;
    OpenCvSharp.Rect[] faces;
    Mat src;
    Mat gray = new Mat();
    Size axes = new Size();
    Point center = new Point();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cascade"></param>
    /// <returns></returns>
    private Mat DetectFace(CascadeClassifier cascade, Texture2D t)
    {
        src = Mat.FromImageData(t.EncodeToPNG(), ImreadModes.Color);
        result = src.Clone();
        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
        src = null;
        // Detect faces
        faces = cascade.DetectMultiScale(gray, 1.08, 2, HaarDetectionType.ScaleImage, new Size(30, 30));

        // Render all detected faces
        for (int i = 0; i < faces.Length; i++)
        {
            center.X = (int)(faces[i].X + faces[i].Width * 0.5);
            center.Y = (int)(faces[i].Y + faces[i].Height * 0.5);
            axes.Width = (int)(faces[i].Width * 0.5);
            axes.Height = (int)(faces[i].Height * 0.5);
            Cv2.Ellipse(result, center, axes, 0, 0, 360, new Scalar(255, 0, 255), 4);
        }
        return result;
    }
    Texture2D temp;
    Texture2D GetTexture2D(WebCamTexture wct)
    {
        temp.SetPixels(wct.GetPixels());
        temp.Apply();
        return temp;
    }
}
