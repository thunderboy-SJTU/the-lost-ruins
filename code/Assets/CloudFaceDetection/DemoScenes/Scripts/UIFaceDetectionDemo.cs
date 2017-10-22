using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using System.IO;

public class UIFaceDetectionDemo : MonoBehaviour
{
    [Tooltip("UIWebCam source used for camera shots.")]
    public UIWebcamSource webcamSource;

    [Tooltip("UI RawImage used for camera shot rendering.")]
    public RawImage cameraShot;

//	[Tooltip("Whether to draw rectangles around the detected faces on the picture.")]
//	public bool displayFaceRectangles = true;

	[Tooltip("Whether to draw arrow pointing to the head direction.")]
	public bool displayHeadDirection = false;

	[Tooltip("UI Text used for hints and status messages.")]
    public Text hintText;

    [Tooltip("UI Text used to display result.")]
    public Text resultText;

    // whether webcamSource has been set or there is web camera at all
    private bool hasCamera = false;

    // initial hint message
    private string hintMessage;

    // AspectRatioFitter component;
    private AspectRatioFitter ratioFitter;

    void Start()
    {
        if (cameraShot)
        {
            ratioFitter = cameraShot.GetComponent<AspectRatioFitter>();
        }

        hasCamera = webcamSource && webcamSource.HasCamera;

        hintMessage = hasCamera ? "Click on the camera image to make a shot" : "No camera found";
        
        SetHintText(hintMessage);
    }

    // camera panel onclick event handler
    public void OnCameraClick()
    {
        if (!hasCamera) 
			return;
        
        ClearResultText();

        if (DoCameraShot())
        {
            StartCoroutine(DoFaceDetection());
        }        
    }

    // camera-shot panel onclick event handler
    public void OnShotClick()
    {
        ClearResultText();

        if (DoImageImport())
        {
            StartCoroutine(DoFaceDetection());
        }
    }

    // camera shot step
    private bool DoCameraShot()
    {
        if (cameraShot && webcamSource)
        {
            SetShotImageTexture(webcamSource.GetSnapshot());
            return true;
        }

        return false;
    }

    // imports image and displays it on the camera-shot object
    private bool DoImageImport()
    {
        Texture2D tex = FaceDetectionUtils.ImportImage();
        if (!tex) return false;

        SetShotImageTexture(tex);

        return true;
    }

    // performs face detection
    private IEnumerator DoFaceDetection()
    {
        // get the image to detect
        Face[] faces = null;
        Texture2D texCamShot = null;

        if (cameraShot)
        {
            texCamShot = (Texture2D)cameraShot.material.mainTexture;
            SetHintText("Wait...");
        }

        // get the face manager instance
		CloudFaceManager faceManager = CloudFaceManager.Instance;

        if (!faceManager)
        {
            SetHintText("Check if the FaceManager component exists in the scene.");
        }
        else if(texCamShot)
        {
			yield return faceManager.DetectFaces(texCamShot);
			faces = faceManager.faces;

            if (faces != null && faces.Length > 0)
            {
				//if(displayFaceRectangles)
				{
					faceManager.DrawFaceRects(texCamShot, faces, FaceDetectionUtils.FaceColors, displayHeadDirection);
				}

				SetHintText(hintMessage);
                SetResultText(faces);
            }
            else
            {
                SetHintText("No face(s) detected.");
            }
        }

        yield return null;
    }

    // display image on the camera-shot object
    private void SetShotImageTexture(Texture2D tex)
    {        
        if (ratioFitter)
        {
            ratioFitter.aspectRatio = (float)tex.width / (float)tex.height;
        }

        if (cameraShot)
        {
            cameraShot.material.mainTexture = cameraShot.texture = tex;
        }
    }

    // display results
    private void SetResultText(Face[] faces)
    {
        StringBuilder sbResult = new StringBuilder();

        if (faces != null && faces.Length > 0)
        {
            for (int i = 0; i < faces.Length; i++)
            {
                Face face = faces[i];
                string faceColorName = FaceDetectionUtils.FaceColorNames[i % FaceDetectionUtils.FaceColors.Length];

                string res = FaceDetectionUtils.FaceToString(face, faceColorName);

                sbResult.Append(string.Format("<color={0}>{1}</color>", faceColorName, res));
            }
        }

        string result = sbResult.ToString();

        if (resultText)
        {
            resultText.text = result;
            // ------------------------------------------------ do sth  -------------------------------------------------------------
            AudioPlayer.user.age = faces[0].faceAttributes.age;
            if (faces[0].faceAttributes.gender == "female") AudioPlayer.user.gender = 0;
            else AudioPlayer.user.gender = 1;
            AudioPlayer.user.smile = faces[0].faceAttributes.smile;
            AudioPlayer.user.anger = faces[0].faceAttributes.emotion.anger;
            AudioPlayer.user.contempt = faces[0].faceAttributes.emotion.contempt;
            AudioPlayer.user.disgust=faces[0].faceAttributes.emotion.disgust;
            AudioPlayer.user.fear = faces[0].faceAttributes.emotion.fear;
            AudioPlayer.user.happiness = faces[0].faceAttributes.emotion.happiness;
            AudioPlayer.user.neutral = faces[0].faceAttributes.emotion.neutral;

            AudioPlayer.user.sadness = faces[0].faceAttributes.emotion.sadness;
            AudioPlayer.user.surprise = faces[0].faceAttributes.emotion.surprise;

            /*
            FileStream fs = new FileStream("emotion.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(faces[0].faceAttributes.age + " ");
            if (faces[0].faceAttributes.gender == "female") sw.Write("0 ");
            else sw.Write("1 ");
            sw.Write(faces[0].faceAttributes.smile + " ");
            sw.Write(faces[0].faceAttributes.emotion.anger + " ");
            sw.Write(faces[0].faceAttributes.emotion.contempt + " ");
            sw.Write(faces[0].faceAttributes.emotion.disgust + " ");
            sw.Write(faces[0].faceAttributes.emotion.fear + " ");
            sw.Write(faces[0].faceAttributes.emotion.happiness + " ");
            sw.Write(faces[0].faceAttributes.emotion.neutral + " ");
            sw.Write(faces[0].faceAttributes.emotion.sadness + " ");
            sw.Write(faces[0].faceAttributes.emotion.surprise + " ");
            sw.Flush();
            sw.Close();
            fs.Close();*/

            webcamSource.webcamTex.Stop(); 
            Global.loadName = "main";
            SceneManager.LoadScene("Loading");
        }
        else
        {
            Debug.Log(result);
        }
    }

    // clear result
    private void ClearResultText()
    {
        if (resultText)
        {
            resultText.text = "";
        }
    }

    // displays hint or status text
    private void SetHintText(string sHintText)
    {
        if (hintText)
        {
            hintText.text = sHintText;
        }
        else
        {
            Debug.Log(sHintText);
        }
    }

}
