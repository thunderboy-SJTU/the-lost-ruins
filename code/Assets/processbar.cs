using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class processbar : MonoBehaviour {

    [SerializeField]
    float speed;

    [SerializeField]
    float targetProcess;

    [SerializeField]
    Transform processTransform;

    [SerializeField]
    Transform indicatorTransform;

    [SerializeField]
    gameManager gamemanager;

    private void OnEnable()
    {
        //gamemanager.setProcess(0.0f);
    }

    public void updateProcess()
    {
        float currentProcess = gamemanager.getProcess();
        if (currentProcess >= targetProcess)
        {
            gamemanager.setProcess(targetProcess);
            if (gamemanager.getMagic() == 0)
                gamemanager.setMagic(1);

        }
        if (gamemanager.getMagic() != 2)
        {
            currentProcess = gamemanager.getProcess();
            indicatorTransform.GetComponent<Text>().text = ((int)(currentProcess * 100)).ToString() + " %";
            processTransform.GetComponent<Image>().fillAmount = currentProcess;
        }
        else
        {
            indicatorTransform.GetComponent<Text>().text = "Jump";
            processTransform.GetComponent<Image>().fillAmount = currentProcess;
        }
        

    }
    private void FixedUpdate()
    {
        if (gamemanager.getMagic() == 0 && gameManager.start)
        {
            float currentProcess = gamemanager.getProcess();
            if (currentProcess < targetProcess)
            {
                currentProcess += speed;
                gamemanager.setProcess(currentProcess);
            }
        }
        updateProcess();
    }
}
