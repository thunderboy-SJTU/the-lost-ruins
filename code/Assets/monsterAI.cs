using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class monsterAI : MonoBehaviour
{

    public Vector3 targetPosition;
    public Vector3 targetPosition2;
    private Seeker seeker;
    private int count = 0;
    public GameObject obj;
   // private CharacterController controller;

    //The calculated path  
    public Path path;

    //The AI's speed per second  
    public float speed = 200;

    //The max distance from the AI to a waypoint for it to continue to the next waypoint  
    public float nextWaypointDistance = 3;

    //The waypoint we are currently moving towards  
    private int currentWaypoint = 0;
    private float lasttime;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        // controller = GetComponent<CharacterController>();
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);
        //Start a new path to the targetPosition, return the result to the OnPathComplete function  
        //seeker.StartPath(transform.position, targetPosition, OnPathComplete);  
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter  
            currentWaypoint = 0;
        }
    }


    void FixedUpdate()
    {
        /*if (lasttime > 5)
        {
            lasttime = 0;
            path = null;
            targetPosition = obj.transform.position;
            seeker.StartPath(transform.position, targetPosition, OnPathComplete);
        }
        lasttime += Time.fixedDeltaTime;*/

        
        if(gameManager.start ==false){

            return;
        }
        
        if (path == null)
        {
            //We have no path to move after yet 
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
           // Debug.Log("End Of Path Reached");
            if (count == 0)
            {
                count++;
                seeker.StartPath(transform.position, targetPosition2, OnPathComplete);
            }
            return;
        }
        //Direction to the next waypoint  
        //Debug.Log(path.vectorPath[currentWaypoint]);
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 rotation = transform.localRotation.eulerAngles;
        //Debug.Log("locallocation:" + rotation);
       // Debug.Log("dir:" + dir);
        float deltax = dir.x;
        float deltaz = dir.z;
        float angle = 0;
        if(deltaz != 0)
           angle = Mathf.Atan(deltax / deltaz)/3.14f*180;
        angle = (int)(Mathf.RoundToInt(angle / 45) * 45);
        
        if (deltaz < 0)
            angle += 180;
       // Debug.Log("angle:" + angle);
        Vector3 dirVector = new Vector3(0, angle, 0);
        Vector3 rotate = dirVector - rotation;
       // Debug.Log("rotate:" + rotate);
        transform.Rotate(rotate);
       // Debug.Log("newdir:" + transform.localRotation.eulerAngles);
        float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        //dir *= speed * Time.fixedDeltaTime;
        //controller.SimpleMove(dir);
       
        /*Vector3 dirpara = new Vector3(Mathf.Cos(rotation.x/180*3.14f), Mathf.Cos(rotation.y / 180 * 3.14f), Mathf.Cos(rotation.z / 180 * 3.14f));
        Vector3 realdir = new Vector3(dir.x, dir.y, (-1)*dir.z);  */
        transform.Translate(new Vector3(0,0,1)*speed*Time.fixedDeltaTime);
       // Debug.Log(transform.forward);


        //Check if we are close enough to the next waypoint  
        //If we are, proceed to follow the next waypoint  
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

}

