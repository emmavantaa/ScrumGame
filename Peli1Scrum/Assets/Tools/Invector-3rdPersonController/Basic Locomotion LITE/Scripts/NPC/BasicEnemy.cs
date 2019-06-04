using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
[DebuggerStepThrough]

// Start is called before the first frame update
public class BasicEnemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float turnSpeed;
    public float dashSpeed;
    public float dasHRange;
    public int attack1Damage = 1;
    public float hp = 1;
    public float kaatumisSpeed=1;
    public float attackTime;
    public float attackStartTime;
    public float attackStopTime;
    public float backOffTime;
    public float backOffTimeFromPlayer;
    public float collisionTime;
    public float collisionWithPlayerTime;
    public float forwardStartTime;
    public float timeToGoForward;
    public float timeBetweenDashAttacks;
    public float waitAfterDash;
    public float timeToGoToRest;
    public float collisionCounterTime;
    public float restingTime;
    public int hitCount;
    public GameObject player;
    public bool dashAttack;
    public bool miekkaCollision;
    public bool wait;
    public bool waitForDash;
    public bool backOff;
    public bool forward;
    public bool backOffFromPlayer;
    public bool GoToRest;
    public bool stop;
    public bool targetTooHigh;
    public bool resti;
    public Vector3 startingPoint;
    public Vector3 targetDir;
    public Vector3 newDir;
    public Vector3 targetPoint;
    public Vector3 backOffDir;
    public Vector3 newBackOffDir;
    public Vector3 newDirDash;
    public Quaternion newDirDashTuleva;
    public Quaternion direction;

    // Use this for initialization
    void Start()
    {
        hitCount = 0;
        timeToGoToRest = 22f;
        backOffTimeFromPlayer = 0.3f;
        speed = 3f;
        turnSpeed = 5f;
        dashSpeed = 13f;
        attackStopTime = 0f;
        dasHRange = 9f;
        attackTime = 1f;
        attackStartTime = 0f;
        waitAfterDash= 2f;
        timeBetweenDashAttacks = 3f;
        backOffTime = 0.3f;
        timeToGoForward = 0.5f;
        player = GameObject.FindGameObjectWithTag("Player");
        startingPoint = transform.position;
        Rest();
    }

    // Update is called once per frame
    void Update()
    {
        Ray collisionCheckRay = new Ray(transform.position + new Vector3(0f, 0.1f, 0f), transform.forward);
        UnityEngine.Debug.DrawRay(collisionCheckRay.origin, transform.forward,color: Color.cyan,2f);
        if(Physics.Raycast(collisionCheckRay, maxDistance: 1f))
        {
            GoToRest = true;
            restingTime = Time.time;
        }
        if (GoToRest == true)
        {
            if (Time.time > restingTime + timeToGoToRest)
            {
                GoToRest = false;
            }
            else
            {
                hitCount = 0;
                backOff = false;
                backOffFromPlayer = false;
                dashAttack = false;
                Rest();
                
            }

        }
        else
        {


            if (Time.time < collisionCounterTime + timeToGoToRest && hitCount >= 18)
            {
                GoToRest = true;
                restingTime = Time.time;
            }
            if (Time.time > collisionCounterTime + timeToGoToRest)
            {
                hitCount = 0;
            }
            if (miekkaCollision)
            {
                if (hitCount == 0)
                {
                    collisionCounterTime = Time.time;
                }


                collisionTime = Time.time;
                backOff = true;
                if (dashAttack)
                {
                    dashAttack = false;
                    attackStopTime = Time.time;
                }



                //Rest();
            }
            if (backOffFromPlayer == true)
            {
                transform.Translate(-Vector3.forward * ((speed) * Time.deltaTime));
                dashAttack = false;
                if (collisionWithPlayerTime + backOffTimeFromPlayer < Time.time)
                {
                    backOffFromPlayer = false;
                    attackStopTime = Time.time;
                }
            }
            else if (backOff)
            {
                dashAttack = false;
                transform.Translate(-Vector3.forward * ((speed) * Time.deltaTime));
                if(Time.time < collisionTime + backOffTime&& wait == false)
                {
                    float step = (turnSpeed) * Time.deltaTime;

                    backOffDir = ((target.position - transform.position) - transform.position);
                    newBackOffDir = Vector3.RotateTowards(transform.forward, backOffDir, step, 0.0f);
                    newBackOffDir.y = 0;

                    transform.rotation = Quaternion.LookRotation(newBackOffDir);

                }
            }
            if (backOff && Time.time > collisionTime + backOffTime)
            {

                forward = true;
                backOff = false;
                forwardStartTime = Time.time;
            }
            if (forward&&stop==false)
            {
                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }
            if (Time.time > forwardStartTime + timeToGoForward)
            {
                forward = false;
            }
            if (dashAttack == true)
            {
                transform.Translate(Vector3.forward * ((dashSpeed) * Time.deltaTime));
                if (Time.time > attackStartTime + attackTime)
                {
                    dashAttack = false;
                    attackStopTime = Time.time;
                }
            }
            if (Time.time < attackStopTime + waitAfterDash)
            {
                wait = true;
            }
            else
            {
                wait = false;
            }
            if (Time.time < attackStopTime + timeBetweenDashAttacks)
            {
                waitForDash = true;
            }
            else
            {
                waitForDash = false;
            }
            if (Vector3.Distance(transform.position, target.position) < dasHRange && dashAttack == false && wait == false && waitForDash == false && backOffFromPlayer == false && backOff == false && forward == false)
            {

                var ad = new Vector3(0.1f, 0f, 0.1f);


                targetDir = target.position - transform.position;
                var targetDirY0 = targetDir;
                targetDirY0.y = 0;
                var targetDirY0Normalized = targetDirY0.normalized;
                var thisDir = transform.forward;
                var thisDirY0 = thisDir;
                thisDirY0.y = 0;
                var thisDirNormalized = thisDirY0.normalized;

                float step = (turnSpeed) * Time.deltaTime;
                newDirDash = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                UnityEngine.Debug.DrawRay(transform.position, newDirDash, Color.red);
                newDirDash.y = 0;
                Quaternion rotNewDir = Quaternion.LookRotation(targetDir);

                transform.rotation = Quaternion.LookRotation(newDirDash);
                var targetXPlus= targetDirY0Normalized.x+0.05f;
                var targetYPlus = targetDirY0Normalized.y;
                var targetZPlus = targetDirY0Normalized.z+0.05f;

                var targetXMinus = targetDirY0Normalized.x - 0.05f;
                var targetYMinus = targetDirY0Normalized.y;
                var targetZMinus = targetDirY0Normalized.z - 0.05f;

                var thisx = thisDirNormalized.x;
                var thisy = thisDirNormalized.y;
                var thisz = thisDirNormalized.z;

                if ((thisx< targetXPlus && thisx>targetXMinus) ||thisz< targetZPlus && thisz>targetZMinus)
                {
                    dashAttack = true;
                    attackStartTime = Time.time;
                }

                //if (transform.forward.normalized == targetDirY0Normalized|| transform.forward.normalized< targetDirY0Normalized+ad)
                //{
                //    dashAttack = true;
                //    attackStartTime = Time.time;
                //}
                else
                {
                    stop = true;
                }
                
            }
            else if((Vector3.Distance(transform.position, target.position) > dasHRange))
            {
                stop = false;
            }
        }
        
        //if (hp<=0)
        //{
        //    float step = kaatumisSpeed * Time.deltaTime;
        //    //transform.position = Vector3.MoveTowards(transform.position, OpenDrawerPosition.transform.position, step);

        //    gameObject.SetActive(false);
        //    //Destroy(gameObject);
        //}
    }

    public void MoveToPlayer()
    {
        if (GoToRest == false&&stop==false)
        {


            ////rotate to look at player
            //var ylösKatsomus = new Vector3 (0, 1f, 0);
            //var hh = new Vector3(0, player.transform.position.y,0);

            ////Vector3 dif = target.position - transform.position;
            ////dif.y = 0;

            ////Quaternion lookAngle = Quaternion.LookRotation(dif, transform.up);

            //transform.LookAt(new Vector3(target.position.x,transform.position.y,target.position.z));
            //transform.Rotate(new Vector3(0, -90, 0), Space.Self);
            targetDir = target.position - transform.position;


            // The step size is equal to speed times frame time.
            float step = (turnSpeed) * Time.deltaTime;

            newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            UnityEngine.Debug.DrawRay(transform.position, newDir, Color.red);
            newDir.y = 0;
            Quaternion rotNewDir = Quaternion.LookRotation(targetDir);

            if (dashAttack == false && backOff == false)
            {
                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);
            }

            //move towards player
            if (player.GetComponent<Health>().immortalMoment == false && dashAttack == false && wait == false && backOff == false)
            {
                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }
        }
    }


    public void Rest()
    {
        if (transform.position!=startingPoint)
        {
            resti = true;

            Vector3 targetDir = startingPoint+target.position.normalized - transform.position;


            // The step size is equal to speed times frame time.
            float step = turnSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            UnityEngine.Debug.DrawRay(transform.position, (newDir.normalized*11f), Color.blue);
            newDir.y = 0;

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Lava")
        {
            gameObject.GetComponent<VihuHealth>().takeDamage();
        }
    }


}