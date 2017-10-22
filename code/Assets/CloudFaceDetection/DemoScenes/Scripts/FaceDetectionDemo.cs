using UnityEngine;
using System.Collections;

public class FaceDetectionDemo : MonoBehaviour 
{
	[Tooltip("WebCam source used for camera shots.")]
	public WebcamSource webcamSource;

	[Tooltip("Game object used for camera shot rendering.")]
	public Renderer cameraShot;

//	[Tooltip("Whether to draw rectangles around the detected faces on the picture.")]
//	public bool displayFaceRectangles = true;

	[Tooltip("Whether to draw arrow pointing to the head direction.")]
	public bool displayHeadDirection = false;
	
	[Tooltip("GUI text used for hints and status messages.")]
	public GUIText hintText;

	// list of detected faces
	private Face[] faces;

	// GUI scroll variable for the results' list
	private Vector2 scroll;


	void Start () 
	{
		SetHintText("Click on the camera image to make a shot");
	}
	
	void Update () 
	{
		// check for mouse click
		if (Input.GetMouseButtonDown(0))
		{
			DoMouseClick();
		}
	}

	// invoked on mouse clicks
	private void DoMouseClick()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Physics.Raycast(ray, out hit))
		{
			GameObject selected = hit.transform.gameObject;
			
			if(selected)
			{
				if(webcamSource && selected == webcamSource.gameObject)
				{
					if(DoCameraShot())
					{
						StartCoroutine(DoFaceDetection());
					}
				}
				else if(cameraShot && selected == cameraShot.gameObject)
				{
					if(DoImageImport())
					{
						StartCoroutine(DoFaceDetection());
					}
				}
			}
		}
		
	}
	
	// makes camera shot and displays it on the camera-shot object
	private bool DoCameraShot()
	{
		if(cameraShot && webcamSource)
		{
			Texture tex = webcamSource.GetSnapshot();
			cameraShot.GetComponent<Renderer>().material.mainTexture = tex;

			Vector3 localScale = cameraShot.transform.localScale;
			localScale.x = (float)tex.width / (float)tex.height * Mathf.Sign(localScale.x);
			cameraShot.transform.localScale = localScale;

			return true;
		}

		return false;
	}

	// imports image and displays it on the camera-shot object
	private bool DoImageImport()
	{
        Texture2D tex = FaceDetectionUtils.ImportImage();

        if (tex && cameraShot)
        {
            cameraShot.GetComponent<Renderer>().material.mainTexture = tex;

            Vector3 localScale = cameraShot.transform.localScale;
            localScale.x = (float)tex.width / (float)tex.height * Mathf.Sign(localScale.x);
            cameraShot.transform.localScale = localScale;

			return true;
        }

        return false;
	}

	// performs face detection
	private IEnumerator DoFaceDetection()
	{
		// get the image to detect
		faces = null;
		Texture2D texCamShot = null;

		if(cameraShot)
		{
			texCamShot = (Texture2D)cameraShot.GetComponent<Renderer>().material.mainTexture;
			SetHintText("Wait...");
		}

		// get the face manager instance
		CloudFaceManager faceManager = CloudFaceManager.Instance;

		if(texCamShot && faceManager)
		{
			yield return faceManager.DetectFaces(texCamShot);
			faces = faceManager.faces;
			
			if(faces != null && faces.Length > 0)
			{
				//if(displayFaceRectangles)
				{
					faceManager.DrawFaceRects(texCamShot, faces, FaceDetectionUtils.FaceColors, this.displayHeadDirection);
				}

				SetHintText("Click on the camera image to make a shot");
			}
			else
			{
				SetHintText("No faces detected.");
			}
		}
		else
		{
			SetHintText("Check if the FaceManager component exists in the scene.");
		}

		yield return null;
	}

	void OnGUI()
	{
		// set gui font
		GUI.skin.font = hintText ? hintText.GetComponent<GUIText>().font : GUI.skin.font;

		if(faces != null && faces.Length > 0)
		{
			Rect guiResultRect = new Rect(Screen.width / 2, 20, Screen.width / 2, Screen.height - 20);
			GUILayout.BeginArea(guiResultRect);
			scroll = GUILayout.BeginScrollView(scroll);

            Color[] faceColors = FaceDetectionUtils.FaceColors;
            string[] faceColorNames = FaceDetectionUtils.FaceColorNames;

            for (int i = 0; i < faces.Length; i++)
			{
				Face face = faces[i];

				Color faceColor = faceColors[i % faceColors.Length];
				string faceColorName = faceColorNames[i % faceColors.Length];

				Color guiColor = GUI.color;
				GUI.color = faceColor;

                string sRes = FaceDetectionUtils.FaceToString(face, faceColorName);

				GUILayout.Label(sRes);

				GUI.color = guiColor;
			}
			
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}


	// displays hint or status text
	private void SetHintText(string sHintText)
	{
		if(hintText)
		{
			hintText.GetComponent<GUIText>().text = sHintText;
		}
		else
		{
			Debug.Log(sHintText);
		}
	}

}
