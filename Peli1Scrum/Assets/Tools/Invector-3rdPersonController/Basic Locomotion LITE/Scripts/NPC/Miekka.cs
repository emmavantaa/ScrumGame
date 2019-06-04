using System.Collections;
using System.Collections.Generic;
using Invector.CharacterController;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class Miekka : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    GameObject vihu;
    public bool knockBack;
    public bool teleportKnockback;
    public bool teleportKnockback2;
    public bool hyppii;
    public bool collisionStay;
    public bool forcea;
    public bool forceaLess;
    public float timeOnCollisionStay;
    public bool hidasta;
    float speed = 16;
    bool flyAway;
    Vector3 pos;
    public Vector3 osumaKohta;
    public Vector3 voimanSuunta;
    public Vector3 voimanSuunta2;
    GameObject collided;
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

    }

    // Update is called once per frame
    void Update()
    {
        if (teleportKnockback2)
        {
            if (collided.transform.position == pos + (gameObject.transform.forward * 2))
            {
                flyAway = false;
                collided.GetComponent<Rigidbody>().isKinematic = false;

            }
            if (flyAway)
            {
                collided.GetComponent<Rigidbody>().isKinematic = true;
                float step = speed * Time.deltaTime;
                collided.transform.position = Vector3.MoveTowards(collided.transform.position, pos + (gameObject.transform.forward * 2), step);
            }
        }
    }
    //private void OnTriggerEnter(Collider other)

    //{
    //    //bool isJumping;
    //    //isJumping = GetComponent<vThirdPersonController>().isJumping;
    //    if (other.gameObject == player&&player.GetComponent<Health>().immortalMoment==false)
    //    {
    //        if (hyppii==false&&!Input.GetKey("space"))
    //        {
    //            other.GetComponent<Rigidbody>().AddExplosionForce(6999, transform.position + new Vector3(0, -1f, 0), 3);
    //        }
    //        else if(hyppii==true||Input.GetKey("space"))
    //        {
    //            other.GetComponent<Rigidbody>().AddExplosionForce(155, transform.position + new Vector3(0, 1f, 0), 3);
    //        }

    //        //player.GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, 1);
    //        //rb.AddExplosionForce(999, transform.position, 999);
    //        player.GetComponent<Health>().TakeDamage();

    //    }
    //}
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude>=200)
        {
            var k = 9;
        }
        if (hidasta)
        {
            collided.GetComponent<Rigidbody>().velocity = Vector3.zero;
            hidasta = false;
        }
        else if (forceaLess)
        {
            var t = 3;
            collided.GetComponent<Rigidbody>().AddForce((voimanSuunta) * 277f, /*transform.position + new Vector3(0, -0.5f, 0)*/ ForceMode.Impulse);
            forceaLess = false;
            hidasta = true;
        }

        else if (forcea&&cc.isGrounded)
        {
            collided.GetComponent<Rigidbody>().AddForce((voimanSuunta) * 277, /*transform.position + new Vector3(0, -0.5f, 0)*/ ForceMode.Impulse);
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
        }

        if (collision.gameObject!=player)
        {
            basicEnemy.miekkaCollision = true;
            if (basicEnemy.GoToRest==false)
            {
                ++basicEnemy.hitCount;
            }
            
        }
        
        osumaKohta=collision.contacts[0].point;

        voimanSuunta = new Vector3(player.transform.position.x - osumaKohta.x, 0f, player.transform.position.z - osumaKohta.z);
        voimanSuunta2 = player.transform.position - osumaKohta;

        voimanSuunta.Normalize();
        voimanSuunta2.Normalize();
        //bool isJumping;
        //isJumping = GetComponent<vThirdPersonController>().isJumping;
        if (collision.gameObject == player && player.GetComponent<Health>().immortalMoment == false)
        {
            if (teleportKnockback)
            {
                collision.gameObject.transform.position = collision.gameObject.transform.position + (gameObject.transform.forward * 2);
            }
            else if (teleportKnockback2&&flyAway==false)
            {

                //collided = collision.gameObject;
                pos = collided.transform.position;
                flyAway = true;
                
            }
            else if (knockBack)
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

            //player.GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, 1);
            //rb.AddExplosionForce(999, transform.position, 999);
            player.GetComponent<Health>().TakeDamage();

        }
    }
    private void OnCollisionExit(Collision collision)
    {

            basicEnemy.miekkaCollision = false;
        
        collisionStay = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collisionStay==false)
        {
            collisionStay = true;
            timeOnCollisionStay = Time.time;

        }
        else if (Time.time>timeOnCollisionStay+2)
        {
            basicEnemy.GoToRest = true;
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
