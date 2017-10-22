using UnityEngine;
using System.IO;
using System.Text;

public static class FaceDetectionUtils {

    private static readonly Color[] faceColors = new Color[] { Color.white, Color.yellow, Color.cyan, Color.magenta, Color.red ,Color.green };
    private static readonly string[] faceColorNames = new string[] { "White", "Yellow", "Cyan", "Magenta", "Red", "Green" };
                                                            
    public static Texture2D ImportImage()
    {
        Texture2D tex = null;

#if UNITY_EDITOR
		string filePath = UnityEditor.EditorUtility.OpenFilePanel("Open image file", "", "jpg");  // string.Empty; // 
#else
		string filePath = string.Empty;
#endif

        if (!string.IsNullOrEmpty(filePath))
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            tex = new Texture2D(2, 2);
            tex.LoadImage(fileBytes);
        }

        return tex;
    }

    public static string FaceToString(Face face, string faceColorName)
    {
        StringBuilder sbResult = new StringBuilder();

        //sbResult.Append(string.Format("{0}:", faceColorName)).AppendLine();
        if (face.faceAttributes.gender == "female") sbResult.Append("Hey, girl! Here's your report!").AppendLine();
        else sbResult.Append("Hey, boy! Here's your report!").AppendLine();
        sbResult.Append(string.Format("Your Age: about {0}", face.faceAttributes.age)).AppendLine();
        sbResult.Append(string.Format("You are currently {0:F0}% happy!", face.faceAttributes.smile * 100f)).AppendLine().AppendLine();
        sbResult.Append("Emotion Details:").AppendLine();
        sbResult.Append(string.Format("Happiness: {0:F0}%", face.faceAttributes.emotion.happiness * 100f)).AppendLine();
        sbResult.Append(string.Format("Anger: {0:F0}%", face.faceAttributes.emotion.anger * 100f)).AppendLine();
        sbResult.Append(string.Format("Contempt: {0:F0}%", face.faceAttributes.emotion.contempt * 100f)).AppendLine();
        sbResult.Append(string.Format("Disgust: {0:F0}%", face.faceAttributes.emotion.disgust * 100f)).AppendLine();
        sbResult.Append(string.Format("Fear: {0:F0}%", face.faceAttributes.emotion.fear * 100f)).AppendLine();
        sbResult.Append(string.Format("Sadness: {0:F0}%", face.faceAttributes.emotion.sadness * 100f)).AppendLine();
        sbResult.Append(string.Format("Surprise: {0:F0}%", face.faceAttributes.emotion.surprise * 100f)).AppendLine();
        sbResult.Append(string.Format("Neutral: {0:F0}%", face.faceAttributes.emotion.neutral * 100f)).AppendLine().AppendLine();
        sbResult.Append("Recommending songs for you ...").AppendLine().AppendLine();

        //			sbResult.Append(string.Format("    Beard: {0}", face.FaceAttributes.FacialHair.Beard)).AppendLine();
        //			sbResult.Append(string.Format("    Moustache: {0}", face.FaceAttributes.FacialHair.Moustache)).AppendLine();
        //			sbResult.Append(string.Format("    Sideburns: {0}", face.FaceAttributes.FacialHair.Sideburns)).AppendLine().AppendLine();

        return sbResult.ToString();
    }

    public static Color[] FaceColors { get { return faceColors; } }
    public static string[] FaceColorNames { get { return faceColorNames; } }
}
