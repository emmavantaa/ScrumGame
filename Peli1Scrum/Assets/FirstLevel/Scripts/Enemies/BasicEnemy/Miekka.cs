using System.Collections;
using System.Collections.Generic;
using Invector.CharacterController;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class Miekka : MonoBehaviour
{
    GameObject player;
    GameObject collided;
    GameObject vihu;
    BoxCollider triggerCollider;
    Rigidbody rb;
    public bool fysiikkaKnockBack;
    public bool teleportKnockback;
    public bool moveTowardsKnockback;
    public bool translateKnockBack;
    public bool hyppii;
    public bool collisionStay;
    bool forcea;
    bool forceaLess;
    public float timeOnCollisionStay;
    bool hidasta;
    bool collisionHappened;
    bool translateKnockBackToteuta;
    float speed = 16;
    public float translateKnockBackVoima;
    public float translateKesto;
    float translateAlkamisaika;
    bool flyAway;
    public Vector3 targetPositio;
    public float KnockBackVoimaIlmassa;
    public float KnockBackVoima;
    public Vector3 osumaKohta;
    public Vector3 voimanSuunta;
    public Vector3 voimanSuunta2;
    public Vector3 vihunSuunta;

    Hyppii hii;
    vThirdPersonMotor slippy;
    BasicEnemy basicEnemy;
    vThirdPersonController cc;


    
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        vihu = gameObject.transform.parent.gameObject;
        basicEnemy = vihu.GetComponent<BasicEnemy>();
        slippy = player.GetComponent<vThirdPersonMotor>();
        Physics.IgnoreCollision(vihu.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());
        cc = player.GetComponent<vThirdPersonController>();
        hii = player.GetComponent<Hyppii>();
        triggerCollider = player.GetComponentInChildren<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (collisionHappened && moveTowardsKnockback)
        {

            if (collided.transform.position == targetPositio)
            {
                flyAway = false;
                collisionHappened = false;
                //collided.GetComponent<Rigidbody>().isKinematic = false;

            }
            if (flyAway)
            {
                //collided.GetComponent<Rigidbody>().isKinematic = true;
                float step = speed * Time.deltaTime;
                collided.transform.position = Vector3.MoveTowards(collided.transform.position, targetPositio, step);
            }
        }
        if (translateKnockBackToteuta&&collisionHappened&&translateKnockBack)
        {
            cc.jumpCounter = 0;
            cc.isJumping = false;
            collided.transform.Translate(Vector3.forward * (translateKnockBackVoima * Time.fixedDeltaTime),vihu.transform);
            collided.GetComponent<Rigidbody>().AddForce(voimanSuunta * 55);//Jos on vikaa niin tämä rivi kommentointiin


            if (!player.GetComponent<CheckSurroundings>().checkSurroundingsGrounded)//Jos on vikaa niin tämä rivi kommentointiin
            {
                //collided.GetComponent<Rigidbody>().AddForce(voimanSuunta * 55);

                if (Time.time > translateAlkamisaika + translateKesto)
                {
                    translateKnockBackToteuta = false;
                    collisionHappened = false;

                    if (player.GetComponent<Health>().dead == false)
                    {
                        cc.lockMovement = false;
                    }

                }
            }

        }

        if (rb.velocity.magnitude>=200)
        {
        }
        if (hidasta)
        {
            collided.GetComponent<Rigidbody>().velocity = Vector3.zero;
            hidasta = false;
        }
        else if (forceaLess)
        {
            //triggerCollider.enabled = enabled;
            collided.GetComponent<Rigidbody>().AddForce(voimanSuunta * KnockBackVoimaIlmassa,ForceMode.Impulse);
            
            forceaLess = false;
            hidasta = true;
        }

        else if (forcea&&cc.isGrounded)
        {
            //collided.GetComponent<Rigidbody>().AddExplosionForce(KnockBackVoima, osumaKohta+(Vector3.down*0.5f), 1f);
            collided.GetComponent<Rigidbody>().AddForce(voimanSuunta * KnockBackVoima, ForceMode.Impulse);
            //gameObject.GetComponent<Collider>().isTrigger = false;
            //triggerCollider.enabled = enabled;
            forcea = false;
        }

    }
    private void OnCollisionEnter(Collision collision)

    {
        if (collision.gameObject==player)
        {
            collided = collision.gameObject;
            basicEnemy.backOffFromPlayer = true;
            basicEnemy.collisionWithPlayerTime = Time.time;

            osumaKohta = collision.contacts[0].point;

            voimanSuunta = new Vector3(player.transform.position.x - osumaKohta.x, 0f, player.transform.position.z - osumaKohta.z);
            voimanSuunta2 = player.transform.position - osumaKohta;

            voimanSuunta.Normalize();
            voimanSuunta2.Normalize();

            if (player.GetComponent<Health>().immortalMoment == false)
            {
                targetPositio = collided.transform.position + (gameObject.transform.forward * 2);
                collisionHappened = true;
            }
        }

        if (collision.gameObject!=player)
        {
            basicEnemy.miekkaCollision = true;
            if (basicEnemy.GoToRest==false)
            {
                ++basicEnemy.hitCount;
            }
            
        }
        

        //bool isJumping;
        //isJumping = GetComponent<vThirdPersonController>().isJumping;
        if (collision.gameObject == player && player.GetComponent<Health>().immortalMoment == false)
        {
            if (teleportKnockback)
            {
                collision.gameObject.transform.position = collision.gameObject.transform.position + (gameObject.transform.forward * 2);
            }
            else if (moveTowardsKnockback&&flyAway==false)
            {

                //collided = collision.gameObject;
                //pos = collided.transform.position;
                flyAway = true;
                
            }
            else if (fysiikkaKnockBack)
            {
                if (cc.isJumping||!cc.isGrounded)
                {
                    forceaLess = true;
                    hii.hyppii = true;
                    collided.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    cc.jumpCounter = 0;
                    cc.isJumping = false;
                    slippy._capsuleCollider.material = slippy.maxFrictionPhysics;
                }
                else
                {
                    //gameObject.GetComponent<Collider>().isTrigger = true;
                    forcea = true;
                }
                collided = collision.gameObject;
                




                //if (hyppii == false && !Input.GetKey("space"))
                //{
                //    if (collision.gameObject.transform.position.y > (transform.position.y - 0.5f))
                //    {
                //        collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(16, osumaKohta /*+ new Vector3(0, 1f, 0)*/, 0.5f, 0f, ForceMode.Impulse);
                //    }
                //    //else
                //    //{
                //    //    collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(88, osumaKohta /*transform.position + new Vector3(0, -0.5f, 0)*/, 0.5f, 0.45f, ForceMode.Impulse);
                //    //}
                //    else
                //    {
                //        collision.gameObject.GetComponent<Rigidbody>().AddForce((collision.gameObject.transform.position - osumaKohta) * 33333, /*transform.position + new Vector3(0, -0.5f, 0)*/ ForceMode.Impulse);
                //    }

                //}
                //else if (hyppii == true || Input.GetKey("space"))
                //{
                //    collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(16, osumaKohta /*+ new Vector3(0, 1f, 0)*/, 0.5f, -0.1f, ForceMode.Impulse);
                //}
            }
            else if (translateKnockBack&&!translateKnockBackToteuta)
            {
                
                hidasta = true; //Jos on vikaa niin tämä rivi kommentointiin
                cc.lockMovement = true;
                vihunSuunta = transform.forward.normalized;
                translateKnockBackToteuta = true;
                translateAlkamisaika = Time.time;
            }

            //player.GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, 1);
            //rb.AddExplosionForce(999, transform.position, 999);
            player.GetComponent<Health>().hitByEnemy = true;
            player.GetComponent<Health>().TakeDamage();

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject==player)
        {
            //collisionHappened = false;
        }
        
        basicEnemy.miekkaCollision = false;
        
        collisionStay = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collisionStay==false)
        {
            collisionStay = true;
            timeOnCollisionStay = Time.time;
            //basicEnemy.backOff = true;//Tämä rivi on testi. En oo vielä varma toimiiko

        }
        else if (Time.time>timeOnCollisionStay+2)
        {
            basicEnemy.GoToRest = true;
            basicEnemy.restingTime = Time.time;
        }
        if (collision.gameObject != player)
        {
            basicEnemy.miekkaCollision = true;
        }
        
    }
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject == player && player.GetComponent<Health>().immortalMoment == false)
    //    {
    //        if (teleportKnockback)
    //        {
    //            collision.gameObject.transform.position = collision.gameObject.transform.position + (gameObject.transform.forward * 2);
    //        }
    //    }
    //}
}
