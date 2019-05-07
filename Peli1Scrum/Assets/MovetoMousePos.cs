using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetoMousePos : MonoBehaviour
{
    public Camera cam;


    Vector3 mPoint;
    public GameObject Hand;
    //public Rigidbody2D rb;



    // Use this for initialization
    void Start()
    {
        cam = Camera.main;

        //rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        mPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        Hand.transform.position = new Vector3(mPoint.x, mPoint.y, 0);
        //EnableObjectCol();
    }

    //void EnableObjectCol()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        rb.isKinematic = false;
    //    }

    //}
}
