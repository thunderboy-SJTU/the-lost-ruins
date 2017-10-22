using UnityEngine;
using UnityEngine.EventSystems;

public class AppManager : MonoBehaviour {

    public GameObject[] buttons;

    public void exitPlayer()
    {
        Application.Quit();
    }

    public void setButtonSelected(int i)
    {
        EventSystem.current.SetSelectedGameObject(buttons[i]);
    }


}
