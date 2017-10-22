using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunshoot : MonoBehaviour {

    public Animator _Animator;
    public Transform GunPoint;
    public ControllerManager controlManager;
    public gameManager gamemanager;
    public AudioSource sound;

    int maxBullet = 12;
    int currentBullet;
	// Use this for initialization
	void Start () {
        currentBullet = maxBullet;
	}
	
	// Update is called once per frame
	void Update () {
        if (controlManager.rightControllerTouchDown.trigger && gameManager.start == true)
        //if (Input.GetKey(KeyCode.F))
        {
            /*if (currentBullet > 0)
            {
                currentBullet--;
            }
            else
                return;*/
            _Animator.Play("shoot");
            sound.Play();
            Ray _Ray = new Ray(GunPoint.position, (-1)*GunPoint.up);
            RaycastHit hit;
            LayerMask layer = 1 << (LayerMask.NameToLayer("staticfence"));
            bool bHit = Physics.Raycast(_Ray, out hit, 1500, layer.value);
            if (bHit){
                fenceController controller = hit.collider.GetComponent<fenceController>();
                gamemanager.setScore(gamemanager.getScore() + 10);
                gamemanager.setProcess(gamemanager.getProcess()+0.03f);
                gamemanager.updateProcess();
                gamemanager.updateScore();
                controller.triggerBreak();
            }
            else
            {
                layer = 1 << (LayerMask.NameToLayer("monster"));
                bHit = Physics.Raycast(_Ray, out hit, 500, layer.value);
				if (bHit) {
					monsterFSMAI ai1 = null;
					monsterFSMAI2 ai2 = null;

					if ((ai1 = hit.collider.GetComponent<monsterFSMAI> ()) && ai1.death == 0){
						gamemanager.setScore (gamemanager.getScore () + 50);
					gamemanager.updateScore ();
					gamemanager.setProcess (gamemanager.getProcess () + 0.05f);
					gamemanager.updateProcess ();
					ai1.death = 1;
				    }
					else if((ai2 = hit.collider.GetComponent<monsterFSMAI2> ()) &&ai2.death == 0) {
						ai2 = hit.collider.GetComponent<monsterFSMAI2> ();
						gamemanager.setScore (gamemanager.getScore () + 50);
						gamemanager.updateScore ();
						gamemanager.setProcess (gamemanager.getProcess () + 0.05f);
						gamemanager.updateProcess ();
						ai2.death = 1;
					}
		       
                }
            }

        }
	}
}
