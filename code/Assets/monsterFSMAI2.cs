using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class monsterFSMAI2 : MonoBehaviour
{

    public GameObject player;
    public GameObject monster;
    private Seeker seeker;
    public gameManager gamemanager;
    public float walkspeed;
    public float chasespeed;
    public float attackrange;
    public float seerange;
    public float walkrange;
    public int type;
	public int death = 0;

    public float distance;

    private FSMSystem fsm;

    public void SetTransition(Transition t)
    {
        fsm.PerformTransition(t);
    }

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        MakeFSM();
    }

    public void FixedUpdate()
    {
        fsm.CurrentState.Reason(player, monster);
        fsm.CurrentState.Act(player, monster);
    }



    private void MakeFSM()
    {
        RoundsState2 rounds = new RoundsState2(seeker, monster, walkspeed, seerange, walkrange, type, distance);
        rounds.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);
		rounds.AddTransition (Transition.GG, StateID.DIE);

        ChasePlayerState2 chase = new ChasePlayerState2(seeker, monster, player, gamemanager, chasespeed, attackrange, type);
        //chase.AddTransition(Transition.LostPlayer, StateID.FollowingPath);
		chase.AddTransition (Transition.GG, StateID.DIE);
		DieState2 die = new DieState2 (monster);
        fsm = new FSMSystem();
        fsm.AddState(rounds);
        fsm.AddState(chase);
		fsm.AddState (die);

    }
}


public class RoundsState2 : FSMState
{
    private Path path;
    private Vector3[] target = new Vector3[2];
    int i = 1;
    private Transform[] waypoints;
    public float distance = 0.1f;
    private float speed;
    private float seerange;
    private float walkrange;
    private int type;
    private int status = 0;

    //构造函数装填自己
    public RoundsState2(Seeker seeker, GameObject monster, float speed, float seerange, float walkrange, int type, float distance)
    {
        stateID = StateID.ROUNDS;
        target[0] = monster.transform.position;
        target[1] = target[0] - walkrange * monster.transform.forward;
        //Debug.Log(target[1]);
        this.speed = speed;
        this.seerange = seerange;
        this.walkrange = walkrange;
        this.type = type;
        this.distance = distance;
        //setPath(seeker, monster);
    }

    /*public void setPath(Seeker seeker, GameObject monster)
    {
        for (i = 0; i < 2; i++)
        {
            seeker.StartPath(target[i], target[(i + 1) % 2], OnPathComplete);
        }
        i = 0;           
    }*/

    public int walkto(GameObject monster, float speed, Vector3 dst)
    {
        if (status == 0)
        {
            Vector3 dir = (dst - monster.transform.position).normalized;
            Vector3 rotation = monster.transform.rotation.eulerAngles;
            float deltax = dir.x;
            float deltaz = dir.z;
            float angle = Mathf.Atan(deltax / deltaz) / 3.14f * 180;
            angle = Mathf.RoundToInt(angle / 45) * 45;
            if (deltaz < 0)
                angle += 180;
            Vector3 dirVector = new Vector3(0, angle, 0);
            Vector3 rotate = dirVector - rotation;
            monster.transform.Rotate(rotate);
            status = 1;
        }
        monster.transform.Translate(new Vector3(0, 0, 1) * speed * Time.fixedDeltaTime);
        if (Vector3.Distance(monster.transform.position, dst) < distance)
        {
            return 1;
        }
        return 0;
    }

    public void walk(GameObject monster, float speed)
    {
        //Debug.Log(i);
        /* if (path == null)
         {
             //We have no path to move after yet 
             //Debug.Log("No Path");
             return;
         }

         if (currentWaypoint >= path.vectorPath.Count)
         {
             //Debug.Log("End Of Path Reached");
             currentWaypoint = 0;
             return;
         }
         //Direction to the next waypoint  
         //Debug.Log(path.vectorPath[currentWaypoint]);
         Vector3 dir = (path.vectorPath[currentWaypoint] - monster.transform.position).normalized;
         Vector3 rotation = monster.transform.rotation.eulerAngles;
         float deltax = dir.x;
         float deltaz = dir.z;
         float angle = Mathf.Atan(deltax / deltaz) / 3.14f * 180;
         angle = Mathf.RoundToInt(angle / 45) * 45;
         if (deltaz < 0)
             angle += 180;
         Vector3 dirVector = new Vector3(0, angle, 0);
         Vector3 rotate = dirVector - rotation;
         monster.transform.Rotate(rotate);
         float distance = Vector3.Distance(monster.transform.position, path.vectorPath[currentWaypoint]);
         //dir *= speed * Time.fixedDeltaTime;
         //controller.SimpleMove(dir);

         monster.transform.Translate(new Vector3(0, 0, 1) * speed * Time.fixedDeltaTime);
         // Debug.Log(transform.forward);


         //Check if we are close enough to the next waypoint  
         //If we are, proceed to follow the next waypoint  
         if (Vector3.Distance(monster.transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
         {
             currentWaypoint++;
             return;
         }*/
        int ret = walkto(monster, speed, target[i]);
        if (ret == 1)
        {
            status = 0;
            i = (i + 1) % 2;
        }
    }


    /*public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter  
            //i++;
            currentWaypoint = 0;
        }
    }*/


    public override void DoBeforeEntering()
    {
        //Debug.Log("Rounds BeforeEntering--------");
    }

    public override void DoBeforeLeaving()
    {
        // Debug.Log("Rounds BeforeLeaving---------");
    }

    //重写动机方法
    public override void Reason(GameObject player, GameObject monster)
    {
        // If the Player passes less than 15 meters away in front of the NPC
        /*RaycastHit hit;
        if (Physics.Raycast(npc.transform.position, npc.transform.forward, out hit, 15F))
        {
            if (hit.transform.gameObject.tag == "Player")
                npc.GetComponent<NPCControl>().SetTransition(Transition.SawPlayer);
        }*/
		if (monster.GetComponent<monsterFSMAI2> ().death == 1)
			monster.GetComponentInParent<monsterFSMAI2> ().SetTransition (Transition.GG);
        if (Vector3.Distance(monster.transform.position, player.transform.position) <= seerange)
            monster.GetComponent<monsterFSMAI2>().SetTransition(Transition.SawPlayer);
    }


    //重写表现方法
    public override void Act(GameObject player, GameObject monster)
    {
        walk(monster, speed);
    }

}

public class ChasePlayerState2 : FSMState
{

    private List<Vector3> path;
    private int currentWaypoint;
    public float nextWaypointDistance = 2;
    private Seeker seeker;
    private GameObject monster;
    private GameObject player;
    private gameManager gamemanager;
    private float speed;
    private float range;
    private int type;
    private int attacked = 0;

    //构造函数装填自己
    public ChasePlayerState2(Seeker seeker, GameObject monster, GameObject player, gameManager gamemanager, float speed, float range, int type)
    {
        currentWaypoint = 0;
        stateID = StateID.ChasingPlayer;
        this.seeker = seeker;
        this.monster = monster;
        this.player = player;
        this.gamemanager = gamemanager;
        this.speed = speed;
        this.range = range;
        this.type = type;

    }

    public override void DoBeforeEntering()
    {
        setpath(monster, player);
    }

    public override void DoBeforeLeaving()
    {
        //Debug.Log("ChasingPlayer BeforeLeaving---------");
    }
    /*public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;

            currentWaypoint = 0;
        }
    }

    public void setPath(Seeker seeker, GameObject monster, GameObject player)
    {
        Vector3 position = monster.transform.position;
        Vector3 rotation = monster.transform.rotation.eulerAngles;
        Vector3 target = player.transform.position;
        seeker.StartPath(position, target, OnPathComplete);
    }*/

    public void setpath(GameObject monster,GameObject player)
    {
        Vector3 position = monster.transform.position;
        Vector3 rotation = monster.transform.rotation.eulerAngles;
        Vector3 target = player.transform.position;
        //Debug.Log("target"+target);
        path = new List<Vector3>();
        ArrayList nodes = Astar.findPath(position, target);
        if (nodes != null)
            foreach (Node node in nodes)
            {
                // Debug.Log("bb "+node.position);
                path.Add(node.position);
            }
    }

    public void walk(GameObject monster, float speed)
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.Count)
        {
            //Debug.Log("End Of Path Reached");
            return;
        }
        //Direction to the next waypoint  
        //Debug.Log(path.vectorPath[currentWaypoint]);
        Vector3 dir = (path[currentWaypoint] - monster.transform.position).normalized;
        //Debug.Log("dir:" + dir);
        Vector3 rotation = monster.transform.rotation.eulerAngles;
        float deltax = dir.x;
        float deltaz = dir.z;
        // float angle = Mathf.Atan(deltax / deltaz) / 3.14f * 180;
        float angle = 0;
        if (deltax != 0 || deltaz != 0)
        {

            if (deltaz != 0)
                angle = Mathf.Atan(deltax / deltaz) / 3.14f * 180;
            angle = Mathf.RoundToInt(angle / 45) * 45;
            //Debug.Log("angle:" + angle);
            if (deltaz < 0)
                angle += 180;
            Vector3 dirVector = new Vector3(0, angle, 0);
            Vector3 rotate = dirVector - rotation;
            //Debug.Log("rotate:" + rotate);
            monster.transform.Rotate(rotate);
            float distance = Vector3.Distance(monster.transform.position, path[currentWaypoint]);
            //dir *= speed * Time.fixedDeltaTime;
            //controller.SimpleMove(dir);

            /*Vector3 dirpara = new Vector3(Mathf.Cos(rotation.x/180*3.14f), Mathf.Cos(rotation.y / 180 * 3.14f), Mathf.Cos(rotation.z / 180 * 3.14f));
            Vector3 realdir = new Vector3(dir.x, dir.y, (-1)*dir.z);  */
            monster.transform.Translate(new Vector3(0, 0, 1) * speed * Time.fixedDeltaTime);
            // Debug.Log(transform.forward);

        }
        //Check if we are close enough to the next waypoint  
        //If we are, proceed to follow the next waypoint  
        if (Vector3.Distance(monster.transform.position, path[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    public override void Reason(GameObject player, GameObject monster)
    {
		if (monster.GetComponent<monsterFSMAI2> ().death == 1)
			monster.GetComponentInParent<monsterFSMAI2> ().SetTransition (Transition.GG);
    }

    public override void Act(GameObject player, GameObject monster)
    {
        walk(monster, speed);
        if (Vector3.Distance(monster.transform.position, player.transform.position) <= range)
        {
			if (monster.GetComponent<monsterFSMAI2> ().death == 0) {
				if (type == 0)
					monster.GetComponent<Animator> ().Play ("attack");
				if (type == 1 || type == 2)
					monster.GetComponent<Animator> ().Play ("ShootStart");
				if (attacked == 0) {
					damage.TakeDamage ();

					gamemanager.reduceLife ();
					gamemanager.updateLife ();
					attacked = 1;
					if (gamemanager.getlife () == 0)
						gamemanager.attacked = true;
				}
			}
        }
    }
}

public class DieState2 : FSMState
{
	GameObject monster;
	public DieState2(GameObject monster)
	{
		stateID = StateID.DIE;
		this.monster = monster;
	}
	public override void DoBeforeEntering()
	{

	}

	public override void DoBeforeLeaving()
	{
		//Debug.Log("ChasingPlayer BeforeLeaving---------");
	}

	public override void Reason(GameObject player, GameObject monster)
	{

	}

	public override void Act(GameObject player, GameObject monster)
	{
		monster.GetComponent<Animator>().Play("death");
		GameObject.Destroy(monster,0.8f);
	}

}