using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class moveforward2 : MonoBehaviour
{

    public float speed = 4.2f;
    public GameObject obj;

    
	public GameObject obj2;
	public GameObject portal;
    private float rotateSpeed = 4.0f;
    //bool start = false;
    public ControllerManager manager;
    public gameManager gamemanager;

    float force = 73000;
    bool isJump = false;

    bool TurningL = false;
    bool TurningR = false;

    bool isRightDown = false;
    bool isLeftDown = false;
    float y;

    private bool canShift;
    private float originspeed;

    private float speedstep;

    public GameObject eyecamera;

    int head_dir = 1;//1：x正方向，2：z正方向，3：x负方向，4：z负方向 右拐+，左拐-
    int now_road = 0; /*-1 0 1*/
    bool headingLeft = false;
    bool headingRight = false;
    float now_position;
    int i = 0;//第几个弯

	private float EndZ;
	private float EndX;

    float[] turnTag = new float[] { 119, 49, 81, 99, 31, 61 };
    private int fix_dir(int dir)
    {
        if (dir > 4) dir -= 4;
        if (dir < 1) dir += 4;
        return dir;
    }
    public void Fun1()
    {
        Quaternion quater = Quaternion.Euler(0, y - 90, 0);
        //Quaternion quater2 = Quaternion.Euler(0, y + 90, 0);
        obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, quater, rotateSpeed);
        //obj2.transform.rotation = Quaternion.RotateTowards(obj2.transform.rotation, quater2, rotateSpeed);
        if ((int)obj.transform.rotation.eulerAngles.y - 1 <= (int)quater.eulerAngles.y && (int)quater.eulerAngles.y <= (int)obj.transform.rotation.eulerAngles.y + 1)
        {
            TurningL = false;
            TurningR = false;
            canShift = false;
            head_dir--;
            head_dir = fix_dir(head_dir);
            speed += 0.3f;
            i++;
        }
    }


    public void Fun2()
    {
        Quaternion quater = Quaternion.Euler(0, y + 90, 0);
        //Quaternion quater2 = Quaternion.Euler(0, y - 90, 0);
        obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, quater, rotateSpeed);
        //obj2.transform.rotation = Quaternion.RotateTowards(obj2.transform.rotation, quater2, rotateSpeed);
        if ((int)obj.transform.rotation.eulerAngles.y - 1 <= (int)quater.eulerAngles.y && (int)quater.eulerAngles.y <= (int)obj.transform.rotation.eulerAngles.y + 1)
        {
            TurningR = false;
            TurningL = false;
            canShift = false;
            head_dir++;
            head_dir = fix_dir(head_dir);
            speed += 0.3f;
            i++;
        }
    }
    // Use this for initialization
    void Start()
    {
		EndX = -3.07f;
		EndZ = -13;
        originspeed = speed;
        speedstep = speed / 500;
		gameManager.playerHeight = eyecamera.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        obj2.transform.rotation=Quaternion.Euler(0, 0, 0);
        /*if (manager.leftControllerTouchDown.trigger){
            //if (Input.GetKeyDown(KeyCode.P))
            gameManager.start = true;

        }*/
        if (speed < originspeed)
        {
            speed += speedstep;
        }
        if (gameManager.start == true)
        {
			if(obj.transform.eulerAngles.x>5||obj.transform.eulerAngles.x<-5||obj.transform.eulerAngles.z>5||obj.transform.eulerAngles.z<-5)
			Debug.Log(obj.transform.eulerAngles.x  + " " +obj.transform.eulerAngles.z);
			if (obj.transform.eulerAngles.x < -90
				|| (obj.transform.eulerAngles.x > 90&&obj.transform.eulerAngles.x <270)
				|| obj.transform.eulerAngles.z < -90
				|| (obj.transform.eulerAngles.z > 90&&obj.transform.eulerAngles.z <270)
				|| obj.transform.position.y < -5) gameManager.die();
            if (manager.leftControllerTouchDown.grip)
            {
                if (gamemanager.getMagic() == 1)
                {
                    gamemanager.setMagic(2);
                    force = force * 2;
                }
            }
            if (gamemanager.getMagic() == 2)
            {
                gamemanager.minorTimecount();
                if (gamemanager.getMagic() == 0)
                    force = force / 2;
            }
            //if (Input.GetKeyDown(KeyCode.Space))
            if (eyecamera.transform.position.y - gameManager.playerHeight >= 0.022)
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
                    if (head_dir == 1 || head_dir == 3) now_position = obj.transform.position.x;
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
                    if (head_dir == 1 || head_dir == 3) now_position = obj.transform.position.x;
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
                switch (head_dir)
                {
                    case 1:
                        if (isLeftDown && obj.transform.position.z >= turnTag[i])
                        {
                            y = obj.transform.eulerAngles.y;
                            TurningL = true;
                            canShift = false;
                            isLeftDown = false;
                            isRightDown = false;
                        }
                        else if (isRightDown && obj.transform.position.z >= turnTag[i])
                        {
                            y = obj.transform.eulerAngles.y;
                            TurningR = true;
                            canShift = false;
                            isLeftDown = false;
                            isRightDown = false;
                        }
                        else
                        {

                        }

                        break;
                    case 2:
                        if (isLeftDown && obj.transform.position.x >= turnTag[i])
                        {
                            y = obj.transform.eulerAngles.y;
                            TurningL = true;
                            canShift = false;
                            isLeftDown = false;
                            isRightDown = false;
                        }
                        else if (isRightDown && obj.transform.position.x >= turnTag[i])
                        {
                            y = obj.transform.eulerAngles.y;
                            TurningR = true;
                            canShift = false;
                            isLeftDown = false;
                            isRightDown = false;
                        }
                        else
                        {

                        }

                        break;
                    case 3:
                        if (isLeftDown && obj.transform.position.z <= turnTag[i])
                        {
                            y = obj.transform.eulerAngles.y;
                            TurningL = true;
                            canShift = false;
                            isLeftDown = false;
                            isRightDown = false;
                        }
                        else if (isRightDown && obj.transform.position.z <= turnTag[i])
                        {
                            y = obj.transform.eulerAngles.y;
                            TurningR = true;
                            canShift = false;
                            isLeftDown = false;
                            isRightDown = false;
                        }
                        else
                        {

                        }

                        break;
                    case 4:
                        if (isLeftDown && obj.transform.position.x <= turnTag[i])
                        {
                            y = obj.transform.eulerAngles.y;
                            TurningL = true;
                            canShift = false;
                            isLeftDown = false;
                            isRightDown = false;
                        }
                        else if (isRightDown && obj.transform.position.x <= turnTag[i])
                        {
                            y = obj.transform.eulerAngles.y;
                            TurningR = true;
                            canShift = false;
                            isLeftDown = false;
                            isRightDown = false;
                        }
                        else
                        {

                        }

                        break;
                }
                Debug.Log(y);
            }
            if (TurningL == true)
            {
                Fun1();
            }
            if (TurningR == true)
            {
                Fun2();
            }
			if (i==6&&obj.transform.position.x >= EndZ +10)
			{

				if (obj.transform.position.x >= EndX+1  && obj.transform.position.z >= EndZ - 2.5 && obj.transform.position.z <= EndZ + 2.5)
				{
					SceneManager.LoadScene("Success");
				}
				else obj.transform.Translate(Vector3.forward * speed / 1.5f * Time.deltaTime);
			}
            else obj.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (headingLeft)
            {
                if (head_dir == 1)
                {
                    if (obj.transform.position.x > now_position - 1.5)
                        obj.transform.Translate(Vector3.left * speed * 0.6f * Time.deltaTime);
                    else headingLeft = false;
                }
                else if (head_dir == 2)
                {
                    if (obj.transform.position.z < now_position + 1.5)
                        obj.transform.Translate(Vector3.left * speed * 0.6f * Time.deltaTime);
                    else headingLeft = false;
                }
                else if (head_dir == 3)
                {
                    if (obj.transform.position.x < now_position + 1.5)
                        obj.transform.Translate(Vector3.left * speed * 0.6f * Time.deltaTime);
                    else headingLeft = false;
                }
                else if (head_dir == 4)
                {
                    if (obj.transform.position.z > now_position - 1.5)
                        obj.transform.Translate(Vector3.left * speed * 0.6f * Time.deltaTime);
                    else headingLeft = false;
                }
            }
            if (headingRight)
            {
                if (head_dir == 1)
                {
                    if (obj.transform.position.x < now_position + 1.5)
                        obj.transform.Translate(Vector3.right * speed * 0.6f * Time.deltaTime);
                    else headingRight = false;
                }
                else if (head_dir == 2)
                {
                    if (obj.transform.position.z > now_position - 1.5)
                        obj.transform.Translate(Vector3.right * speed * 0.6f * Time.deltaTime);
                    else headingRight = false;
                }
                else if (head_dir == 3)
                {
                    if (obj.transform.position.x > now_position - 1.5)
                        obj.transform.Translate(Vector3.right * speed * 0.6f * Time.deltaTime);
                    else headingRight = false;
                }
                else if (head_dir == 4)
                {
                    if (obj.transform.position.z < now_position + 1.5)
                        obj.transform.Translate(Vector3.right * speed * 0.6f * Time.deltaTime);
                    else headingRight = false;
                }
            }
        }
    }
    void OnTriggerEnter(Collider theCollision)
    {

        if (theCollision.transform.gameObject.layer == 13)
        {
            float offset = Mathf.Abs(now_road) + now_road * 0.2f;
            turnTag[i] = turnTag[i] - now_road * offset;
            canShift = true;
        }
    }

    void OnTriggerExit(Collider theCollision)
    {
        canShift = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.gameObject.layer == 12 && isJump == true)//碰撞的是Plane    
        {
            int angle = 90 * (int)Mathf.Round(this.transform.rotation.eulerAngles.y / 90f);
            obj.transform.rotation = Quaternion.Euler(0, angle, 0);
            isJump = false;
        }
    }
}
