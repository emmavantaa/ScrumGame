using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
[DebuggerStepThrough]
public class BridgeEnemy : MonoBehaviour
{

    public Transform target;
    public float speed = 3f;
    public float attack1Range = 2f;
    public int attack1Damage = 1;
    public float timeBetweenAttacks;
    public float hp = 1;
    public float kaatumisSpeed = 1;
    public GameObject player;

    public GameObject enemy;
    public GameObject ase;
    public MoukariTaiJotain moukari;
    BridgeEnemy bridgeEnemy;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bridgeEnemy = enemy.GetComponent<BridgeEnemy>();
        //ase = GameObject.Find("Ase");
        //moukari = ase.GetComponent<MoukariTaiJotain>();
        Rest();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackPlayer()
    {
        moukari.pelaajaPoistui = false;
        //rotate to look at player


        //Vector3 dif = target.position - transform.position;
        //dif.y = 0;

        //Quaternion lookAngle = Quaternion.LookRotation(dif, transform.up);
        if (moukari.laske == false&&moukari.nostaKeskelle==false)
        {


            //transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            //transform.Rotate(new Vector3(0, -90, 0), Space.Self);


            Vector3 targetDir = target.position - transform.position;
            

            // The step size is equal to speed times frame time.
            float step = speed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            UnityEngine.Debug.DrawRay(transform.position, newDir, Color.red);
            newDir.y = 0;

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);



            //move towards player
            //if (Vector3.Distance(transform.position, target.position) < attack1Range && player.GetComponent<Health>().immortalMoment == false)
            //{
            //transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));

            //}
            if (moukari.keskella)
            {
                moukari.HitPlayer();
            }

        }
        
    }



    public void Rest()
    {
        moukari.pelaajaPoistui = true;
    }


}
