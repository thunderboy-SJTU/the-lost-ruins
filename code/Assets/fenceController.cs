using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fenceController : MonoBehaviour {

    public Transform fragments;
    public ParticleSystem particles;
    public float waitForRemoveCollider = 3;
    public float waitForRemoveRigid = 4;
    public float waitForDestroy = 4;
    public float explosiveForce = 100;
    public float durability = 3;
    public bool mouseClickDestroy;
    public gameManager gamemanager;
    private Transform fragmentd;
    private bool broken = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (mouseClickDestroy)
        {
            /*gamemanager.setProcess(gamemanager.getProcess() + 0.05f);
            gamemanager.updateProcess();*/
            triggerBreak();
        }
    }

    IEnumerator removeRigids()
    {
        if (waitForRemoveRigid > 0 && waitForRemoveRigid != waitForDestroy)
        {
            yield return new WaitForSeconds(waitForRemoveRigid);
            foreach (Transform child in fragmentd.FindChild("fragments"))
            {
                child.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
  
    }

    IEnumerator removeColliders()
    {
        if (waitForRemoveCollider > 0)
        {
            yield return new WaitForSeconds(waitForRemoveCollider);
            foreach (Transform child in fragmentd.FindChild("fragments"))
            {
                child.GetComponent<Collider>().enabled = false;
            }
        }
    }
    IEnumerator breakObject()
    {
        if (!broken)
        {

            if (this.GetComponent< AudioSource > ())
            {
                GetComponent<AudioSource> ().Play();
            }
            broken = true;
            fragmentd = Instantiate(fragments, transform.position, transform.rotation); // adds fragments to stage (!memo:consider adding as disabled on start for improved performance > mem)
            fragmentd.localScale = transform.localScale; // set size of fragments
            Transform frags = fragmentd.FindChild("fragments");
            foreach (Transform child in frags)
            {
                child.gameObject.layer = 8;
                child.GetComponent<Rigidbody>().AddForce(Random.Range(-explosiveForce, explosiveForce), Random.Range(-explosiveForce, explosiveForce), Random.Range(-explosiveForce, explosiveForce));
                child.GetComponent<Rigidbody>().AddTorque(Random.Range(-explosiveForce, explosiveForce), Random.Range(-explosiveForce, explosiveForce), Random.Range(-explosiveForce, explosiveForce));
            }
            StartCoroutine(removeColliders());
            StartCoroutine(removeRigids());
                     
            if (waitForDestroy > 0)
            { // destroys fragments after "waitForDestroy" delay
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(waitForDestroy);
                GameObject.Destroy(fragmentd.gameObject);
                GameObject.Destroy(transform.gameObject);
            }
            else if (waitForDestroy <= 0)
            { // destroys gameobject
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(1);
                GameObject.Destroy(transform.gameObject);
            }
        }
    } 

    public void triggerBreak()
    {
        Destroy(transform.FindChild("object").gameObject);
        Destroy(transform.GetComponent<BoxCollider>());
        Destroy(transform.GetComponent<Rigidbody>());
        StartCoroutine(breakObject());
    }

    void OnTriggerEnter(Collider theCollision)
    {
        triggerBreak();
        theCollision.GetComponent<moveforward>().speed *= 0.55f;
        GameObject camerarig = theCollision.transform.FindChild("[CameraRig]").gameObject;
        /*if(camerarig.transform == null)
            Debug.Log("gg");
        Transform camerahead = camerarig.transform.FindChild("Camera(head)");
        if (camerahead == null)
            Debug.Log("gg2");*/
        StartCoroutine(breakrotate(camerarig.transform));
        //camerarig.transform.rotation = Quaternion.RotateTowards(theCollision.transform.rotation, quater,4);
        Debug.Log("break");
    }

    IEnumerator breakrotate(Transform transform)
    {
        //transform.transform.Rotate(new Vector3(45, 0, 0));
        Vector3 localrotate = transform.transform.localRotation.eulerAngles;
        transform.transform.Rotate(localrotate + new Vector3(45,0,0));
        yield return new WaitForSeconds(0.1f);
        transform.transform.Rotate(new Vector3(-45, 0, 0));
        //transform.transform.Rotate(localrotate + new Vector3(45, 0, 0));
    }
}
