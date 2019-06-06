using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class CheckSurroundings : MonoBehaviour
{

    vThirdPersonController cc;
    Ray checkGroundRay;
    RaycastHit groundHit;
    public bool falling;
    public bool checkSurroundingsGrounded;
    public float fallDistanceToTakeDamage;
    public float fallDistance;
    public Vector3 lastGroundHitPoint;
    public Vector3 afterFallHitPoint;
    Kiipeäminen kiipeäminen;
    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<vThirdPersonController>();
        kiipeäminen = gameObject.GetComponent<Kiipeäminen>();
        lastGroundHitPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        checkGroundRay = new Ray(transform.position + new Vector3(0f, 0.2f, 0f), Vector3.down);
        if(Physics.Raycast(checkGroundRay, maxDistance: 0.05f, hitInfo: out groundHit))
        {
            checkSurroundingsGrounded = true;
        }
        else
        {
            checkSurroundingsGrounded = false;
        }

        //if (Physics.Raycast(checkGroundRay,maxDistance: 0.05f,hitInfo: out groundHit))
        //{
        //    UnityEngine.Debug.DrawRay(checkGroundRay.origin, Vector3.down * 0.05f, color: Color.blue, 55f);
        //    lastGroundHitPoint = transform.position;
        //}
        if (kiipeäminen.kesken)
        {
            lastGroundHitPoint = transform.position;
            falling = false;
        }
        else if (cc.isGrounded)
        {
            falling = false;
            UnityEngine.Debug.DrawRay(checkGroundRay.origin, Vector3.down * 0.05f, color: Color.blue, 55f);
            lastGroundHitPoint = transform.position;
        }
        else
        {
            falling = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lava" && !gameObject.GetComponent<Health>().immortalMoment)
        {
            gameObject.GetComponent<Health>().hitByLava = true;
            gameObject.GetComponent<Health>().TakeDamage();
        }
        if (falling)
        {
            afterFallHitPoint = collision.contacts[0].point;
            fallDistance = Vector3.Distance(lastGroundHitPoint, afterFallHitPoint);
            if (Vector3.Distance(lastGroundHitPoint,afterFallHitPoint)> fallDistanceToTakeDamage)
            {
                falling = false;
                gameObject.GetComponent<Health>().hitByFall = true;
                gameObject.GetComponent<Health>().TakeDamage();
            }
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Lava" && !gameObject.GetComponent<Health>().immortalMoment)
        {
            gameObject.GetComponent<Health>().hitByLava = true;
            gameObject.GetComponent<Health>().TakeDamage();
        }
    }
}
