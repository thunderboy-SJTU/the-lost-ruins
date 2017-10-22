using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircleProcessBar : MonoBehaviour
{
    public float speed;

    public float targetProcess;

    public Transform processTransform;

    public Transform indicatorTransform;

    private float currentProcess = 0.0f;

    private void OnEnable()
    {
        currentProcess = 0.0f;
    }

    private void FixedUpdate()
    {
        if (currentProcess < targetProcess)
        {
            currentProcess += speed;

            if (currentProcess > targetProcess)
            {
                currentProcess = targetProcess;
            }

            indicatorTransform.GetComponent<Text>().text = ((int)(currentProcess * 100)).ToString() + " %";
            processTransform.GetComponent<Image>().fillAmount = currentProcess;
        }
    }
}
