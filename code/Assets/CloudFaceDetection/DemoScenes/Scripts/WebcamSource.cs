using UnityEngine;
using System.Collections;
using System;

public class WebcamSource : WebcamSourceBase
{
    protected override void OnApplyTexture(WebCamTexture webcamTex)
    {
        GetComponent<Renderer>().material.mainTexture = webcamTex;
    }

    protected override void OnSetAspectRatio(int width, int height)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = (float)width / (float)height * Mathf.Sign(localScale.x);
        transform.localScale = localScale;
    }
}
