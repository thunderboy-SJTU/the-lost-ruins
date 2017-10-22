using UnityEngine;

[RequireComponent(typeof(SteamVR_ControllerManager))]
public class ControllerManager : MonoBehaviour {

    // the left/right controller of HTC Vive we kept
    [HideInInspector]
    public GameObject left, right;
    [HideInInspector]
    public SteamVR_TrackedObject leftObject, rightObject;
    [HideInInspector]
    public SteamVR_Controller.Device leftDevice, rightDevice;

    // a new class we defined,including all the buttom input from HTC Vive controller
    [System.Serializable]
    public class ButtomInfo
    {
        public bool applicationMenu;
        public bool touchpad;
        public bool system;
        public bool trigger;
        public bool grip;
    }

    // Active information
    //is left/right controller actived?
    public bool leftControllerActive;
    public bool rightControllerActive;

    // Transform information of controllers
    [Header("Transform")]
    public Vector3 leftControllerPosition;
    public Quaternion leftControllerRotation;
    public Vector3 rightControllerPosition;
    public Quaternion rightControllerRotation;

    public Vector3 headPosition;

    // Buttom information
    // have the buttoms been touched down?
    [Header("Touch Down")]
    public ButtomInfo leftControllerTouchDown;
    public ButtomInfo rightControllerTouchDown;

    // Axis information
    // the axis datas of touchpads and triggers on controllers
    [Header("Axis")]
    public Vector2 leftTouchpadAxis;
    public Vector2 leftTriggerAxis;
    public Vector2 rightTouchpadAxis;
    public Vector2 rightTriggerAxis;
	public Vector3 lefta;

    // the function used to get "TouchDown" information of buttoms
    /*
     * device: HTC Vive device, such as leftController/rightController
     * buttomMask: the button mask, you can see [SteamVR_Controller.ButtonMask] for help
     */
     private bool GetTouchDown(SteamVR_Controller.Device device,ulong buttomMask)
    {
        return device.GetTouchDown(buttomMask);
    }

    //the function used to get "Axis" information of buttoms
    /*
     * device: HTC Vive device, such as leftController/rightController
     * buttomID: the button ID, you can see [Valve.VR.EVRButtonId] for help
     */
     private Vector2 GetAxis(SteamVR_Controller.Device device,Valve.VR.EVRButtonId buttomId)
    {
        return device.GetAxis(buttomId);
    }

    //Awake is called at the beginning, and is earlier than Start
    void Awake()
    {
        //get left/right [GameObjects] and [SteamVR_TrackedObjects] from [SteamVR_ControllerManager]
        left = GetComponent<SteamVR_ControllerManager>().left;
        right = GetComponent<SteamVR_ControllerManager>().right;
        if(left != null)
        {
            leftObject = left.GetComponent<SteamVR_TrackedObject>();
        }
        if(right != null)
        {
            rightObject = right.GetComponent<SteamVR_TrackedObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* get Transform informations from [GameObject] */
        //left
        if(left != null)
        {
            leftControllerPosition = left.transform.position;
            leftControllerRotation = left.transform.rotation;
			lefta = leftControllerRotation.eulerAngles;
        }
        //right
        if(right != null)
        {
            rightControllerPosition = right.transform.position;
            rightControllerRotation = right.transform.rotation;
        }

        /*
         * get Active informations from [SteamVR_TrackedObject]
         * and if Active, get Device informations using index of [SteamVR_TrackedObject]
         */
         //left
        if(leftObject != null)
        {
            leftControllerActive = (leftObject.index != SteamVR_TrackedObject.EIndex.None);
            leftDevice = leftControllerActive ? SteamVR_Controller.Input((int)leftObject.index) : null;
        }
        else
        {
            leftControllerActive = false;
            leftDevice = null;
        }
        //right
        if(rightObject != null)
        {
            rightControllerActive = (rightObject.index != SteamVR_TrackedObject.EIndex.None);
            rightDevice = rightControllerActive ? SteamVR_Controller.Input((int)rightObject.index) : null;
        }
        else
        {
            rightControllerActive = false;
            rightDevice = null;
        }

        //get Buttom/Axis informations from [SteamVR_TrackedObject]
        //left
        if(leftDevice != null)
        {
            leftControllerTouchDown.applicationMenu = GetTouchDown(leftDevice, SteamVR_Controller.ButtonMask.ApplicationMenu);
            leftControllerTouchDown.touchpad = GetTouchDown(leftDevice, SteamVR_Controller.ButtonMask.Touchpad);
            leftControllerTouchDown.system = GetTouchDown(leftDevice, SteamVR_Controller.ButtonMask.System);
            leftControllerTouchDown.trigger = GetTouchDown(leftDevice, SteamVR_Controller.ButtonMask.Trigger);
            leftControllerTouchDown.grip = GetTouchDown(leftDevice, SteamVR_Controller.ButtonMask.Grip);

            leftTouchpadAxis = GetAxis(leftDevice, Valve.VR.EVRButtonId.k_EButton_Axis0);
            leftTriggerAxis = GetAxis(leftDevice, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        }
        else
        {
            leftControllerTouchDown.applicationMenu = false;
            leftControllerTouchDown.touchpad = false;
            leftControllerTouchDown.system = false;
            leftControllerTouchDown.trigger = false;
            leftControllerTouchDown.grip = false;

            leftTouchpadAxis = new Vector2(0, 0);
            leftTriggerAxis = new Vector2(0, 0);
        }
        //right
        if (rightDevice != null)
        {
            rightControllerTouchDown.applicationMenu = GetTouchDown(rightDevice, SteamVR_Controller.ButtonMask.ApplicationMenu);
            rightControllerTouchDown.touchpad = GetTouchDown(rightDevice, SteamVR_Controller.ButtonMask.Touchpad);
            rightControllerTouchDown.system = GetTouchDown(rightDevice, SteamVR_Controller.ButtonMask.System);
            rightControllerTouchDown.trigger = GetTouchDown(rightDevice, SteamVR_Controller.ButtonMask.Trigger);
            rightControllerTouchDown.grip = GetTouchDown(rightDevice, SteamVR_Controller.ButtonMask.Grip);

            rightTouchpadAxis = GetAxis(rightDevice, Valve.VR.EVRButtonId.k_EButton_Axis0);
            rightTriggerAxis = GetAxis(rightDevice, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        }
        else
        {
            rightControllerTouchDown.applicationMenu = false;
            rightControllerTouchDown.touchpad = false;
            rightControllerTouchDown.system = false;
            rightControllerTouchDown.trigger = false;
            rightControllerTouchDown.grip = false;

            rightTouchpadAxis = new Vector2(0, 0);
            rightTriggerAxis = new Vector2(0, 0);
        }
    }	
}
