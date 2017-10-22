using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class moveforward : MonoBehaviour
{

    public float speed = 1;
	public GameObject obj;
	public GameObject obj2;
    private float rotateSpeed = 4.0f;
    //static public bool start = false;
    public ControllerManager manager;
    public gameManager gamemanager;
    public AudioSource Sound;

    public GameObject eyecamera;
    public GameObject portal;
    float force = 145000;
    bool isJump = false;

    bool TurningL = false;
    bool TurningR = false;

    bool isRightDown = false;
    bool isLeftDown = false;
    float y;

    private bool canShift;
    private float turnTag;
    private float originspeed;

    private float speedstep;

    int head_dir = 1;//1：x正方向，2：z正方向，3：x负方向，4：z负方向 右拐+，左拐-
    int now_road = 0; /*-1 0 1*/
    bool headingLeft = false;
    bool headingRight = false;
    float now_position;

    //public float start_height = 0;

    private float EndZ;
    private float EndX;
    public void Fun1()
    {
        Quaternion quater = Quaternion.Euler(0, y - 90, 0);
        obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, quater, rotateSpeed);
        if ((int)obj.transform.rotation.eulerAngles.y - 1 <= (int)quater.eulerAngles.y && (int)quater.eulerAngles.y <= (int)obj.transform.rotation.eulerAngles.y + 1)
        {
            TurningL = false;
            TurningR = false;
            canShift = false;
            head_dir--;
        }
    }


    public void Fun2()
    {
        Quaternion quater = Quaternion.Euler(0, y + 90, 0);
        obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, quater, rotateSpeed);
        if ((int)obj.transform.rotation.eulerAngles.y - 1 <= (int)quater.eulerAngles.y && (int)quater.eulerAngles.y <= (int)obj.transform.rotation.eulerAngles.y + 1)
        {
            TurningR = false;
            TurningL = false;
            canShift = false;
            head_dir++;
        }
    }
    // Use this for initialization
    void Start()
    {
        EndX = portal.transform.position.x;
        EndZ = portal.transform.position.z;
        originspeed = speed;
        speedstep = speed / 500;
    }

    // Update is called once per frame
    void Update()
	{
		obj2.transform.rotation=Quaternion.Euler(0, 0, 0);
        //if(Input.GetKeyDown(KeyCode.T))
        if (manager.leftControllerTouchDown.trigger)
        {
            if(gameManager.start == false){
        //if (Input.GetKeyDown(KeyCode.P))
                   //gameManager.height = eyecamera.transform.position.y;
                   //start_height = eyecamera.transform.position.y;
                   gameManager.playerHeight = eyecamera.transform.position.y;
                   gameManager.start = true;
            }
        }
    
        
        float distance = Vector3.Distance(obj.transform.position, GameObject.Find("Bear").transform.position);
        //Debug.Log(distance);
        if (distance < 100)
        {
			if (distance < 20) 
				gameManager.die ();
            else
            {
                if(!Sound.isPlaying)Sound.Play() ;
            }
        }

		if(obj.transform.eulerAngles.x>5||obj.transform.eulerAngles.x<-5||obj.transform.eulerAngles.z>5||obj.transform.eulerAngles.z<-5)
			Debug.Log(obj.transform.eulerAngles.x  + " " +obj.transform.eulerAngles.z);
		if (obj.transform.eulerAngles.x < -45
			|| (obj.transform.eulerAngles.x > 45&&obj.transform.eulerAngles.x <315)
			|| obj.transform.eulerAngles.z < -45
			|| (obj.transform.eulerAngles.z > 45&&obj.transform.eulerAngles.z <315)
			|| obj.transform.position.y < -1) gameManager.die();
        if (speed < originspeed)
        {
            speed += speedstep;
        }
        //start = true;
        if (gameManager.start == true)
        {
            //if (Input.GetKeyDown(KeyCode.Q))
            if (manager.leftControllerTouchDown.grip)
            {
                if (gamemanager.getMagic() == 1)
                {
                    gamemanager.setMagic(2);
                    force = force * 3;
                }
            }
            if (gamemanager.getMagic() == 2)
            {
                gamemanager.minorTimecount();
                if (gamemanager.getMagic() == 0)
                    force = force / 3;
            }
            //if (Input.GetKeyDown(KeyCode.Space))
            if (eyecamera.transform.position.y - gameManager.playerHeight >= 0.15)
            {
                if (!isJump)//如果还在跳跃中，则不重复执行   
                {
                    obj.GetComponent<Rigidbody>().AddForce(Vector3.up * force);
                    isJump = true;
                }
            }
            //if (Input.GetKeyDown(KeyCode.A) && canShift == false)
            if (manager.leftControllerTouchDown.touchpad && canShift == false)
            {
                if (now_road >= 0 && headingRight == false && headingLeft == false)
                {
                    now_road--;
                    headingLeft = true;
                    if (head_dir == 1) now_position = obj.transform.position.x;
                    else now_position = obj.transform.position.z;
                }
            }
            //if (Input.GetKeyDown(KeyCode.D) && canShift == false)
            if (manager.rightControllerTouchDown.touchpad && canShift == false)
            {
                if (now_road <= 0 && headingLeft == false && headingRight == false)
                {
                    now_road++;
                    headingRight = true;
                    if (head_dir == 1) now_position = obj.transform.position.x;
                    else now_position = obj.transform.position.z;
                }
            }
            
            float offset = obj.transform.eulerAngles.y - manager.leftControllerRotation.eulerAngles.y;
			float offset2 = obj.transform.eulerAngles.y - manager.rightControllerRotation.eulerAngles.y;
			float offset3 = obj.transform.eulerAngles.y - eyecamera.transform.rotation.eulerAngles.y;
            if (offset < 0)
                offset += 360;
			if (offset2 < 0)
				offset2 += 360;
			if (offset3 < 0)
				offset3 += 360;


			if (isRightDown==false&& offset>=45 && offset<=135 && offset2>=45 && offset2<=135 && offset3>=45 && offset3<=135 && TurningL == false && TurningR == false && canShift == true)
            //if (isRightDown == false && Input.GetKeyDown(KeyCode.J) && TurningL == false && TurningR == false && canShift == true)
            {
                isLeftDown = true;
            }
			if (isLeftDown==false&&  offset>=225 && offset<=315  && offset2>=225 && offset2<=315 && offset3>=225 && offset3<=315 && TurningL == false && TurningR == false && canShift == true)
            //if (isLeftDown == false && Input.GetKeyDown(KeyCode.K) && TurningL == false && TurningR == false && canShift == true)
            {
                isRightDown = true;
            }
            if (canShift)
            {
                if (isLeftDown && obj.transform.position.z >= turnTag)
                {
                    y = obj.transform.eulerAngles.y;
                    TurningL = true;
                    canShift = false;
                }
                else if (isRightDown && obj.transform.position.z >= turnTag)
                {
                    y = obj.transform.eulerAngles.y;
                    TurningR = true;
                    canShift = false;
                }
                else
                {

                }
            }
            if (TurningL == true)
            {
                Fun1();
            }
            if (TurningR == true)
            {
                Fun2();
            }
            ///快到终点
            if (obj.transform.position.x >= EndX - 80)
            {
                if (obj.transform.position.x >= EndX - 5 && obj.transform.position.z >= EndZ - 30 && obj.transform.position.z <= EndZ + 30)
                {
                    Global.loadName = "Scene1";
                    SceneManager.LoadScene("Loading");
                }
                else obj.transform.Translate(Vector3.forward * speed / 1.5f * Time.deltaTime);
            }

            //到达终点
            else obj.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (headingLeft)
            {
                if (head_dir == 1)
                {
                    if (obj.transform.position.x > now_position - 10)
                        obj.transform.Translate(Vector3.left * speed * 0.8f * Time.deltaTime);
                    else headingLeft = false;
                }
                else if (head_dir == 2)
                {
                    if (obj.transform.position.z < now_position + 10)
                        obj.transform.Translate(Vector3.left * speed * 0.8f * Time.deltaTime);
                    else headingLeft = false;
                }
            }
            if (headingRight)
            {
                if (head_dir == 1)
                {
                    if (obj.transform.position.x < now_position + 10)
                        obj.transform.Translate(Vector3.right * speed * 0.8f * Time.deltaTime);
                    else headingRight = false;
                }
                else if (head_dir == 2)
                {
                    if (obj.transform.position.z > now_position - 10)
                        obj.transform.Translate(Vector3.right * speed * 0.8f * Time.deltaTime);
                    else headingRight = false;
                }
            }
        }
    }
    void OnTriggerEnter(Collider theCollision)
    {
        //turnTag=theCollision.gameObject.transform.position.z-3;
        //Debug.Log(theCollision.name);
        if (theCollision.transform.gameObject.layer == 13)
        {
            Debug.Log(now_road);
            int offset = Mathf.Abs(now_road) * 10 + 2 * now_road;
            turnTag = 1325 - now_road * offset;
            canShift = true;
        }
    }

    void OnTriggerExit(Collider theCollision)
    {
        canShift = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Plane" && isJump == true)//碰撞的是Plane    
        {

            int angle = 90 * (int)Mathf.Round(this.transform.rotation.eulerAngles.y / 90f);
            Debug.Log(angle);
            obj.transform.rotation = Quaternion.Euler(0, angle, 0);
            isJump = false;
        }
    }
}
