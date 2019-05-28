using System.Collections;
using System.Collections.Generic;
//using Invector.CharacterController;
using UnityEngine;

public class Miekka : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    GameObject vihu;
    public bool knockBack;
    public bool teleportKnockback;
    public bool teleportKnockback2;
    public bool hyppii;
    float speed = 16;
    bool flyAway;
    Vector3 pos;
    GameObject collided;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        vihu = gameObject.transform.parent.gameObject;
        Physics.IgnoreCollision(vihu.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());
        var t = 1;


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
    private void OnCollisionEnter(Collision collision)

    {
        var osumaKohta=collision.contacts[0].point;
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

                collided = collision.gameObject;
                pos = collided.transform.position;
                flyAway = true;
                
            }
            else if (knockBack)
            {


                if (hyppii == false && !Input.GetKey("space"))
                {
                    if (collision.gameObject.transform.position.y > (transform.position.y - 0.5f))
                    {
                        collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(16, osumaKohta /*+ new Vector3(0, 1f, 0)*/, 0.5f, 0f, ForceMode.Impulse);
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(88, osumaKohta /*transform.position + new Vector3(0, -0.5f, 0)*/, 0.5f, 0.45f, ForceMode.Impulse);
                    }
 //                   else
	//{
 //                       collision.gameObject.GetComponent<Rigidbody>().AddRelativeForce((collision.gameObject.transform.position - transform.position) * 33333, /*transform.position + new Vector3(0, -0.5f, 0)*/ ForceMode.Impulse);
 //                   }

                }
                else if (hyppii == true || Input.GetKey("space"))
                {
                    collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(16, osumaKohta /*+ new Vector3(0, 1f, 0)*/, 0.5f, -0.1f, ForceMode.Impulse);
                }
            }

            //player.GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, 1);
            //rb.AddExplosionForce(999, transform.position, 999);
            player.GetComponent<Health>().TakeDamage();

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
