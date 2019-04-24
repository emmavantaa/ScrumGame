using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liike : MonoBehaviour
{
    Rigidbody rb;
    //Camera camera;
    public float thrust;
    
    public GameObject keila1;
    public GameObject keila2;
    public GameObject keila3;
    public GameObject keila4;
    public GameObject keila5;
    public GameObject keila6;
    public GameObject keila7;
    public GameObject keila8;
    public GameObject keila9;
    Vector3 targetDir;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //camera = GetComponent<Camera>();
        //transform.position = targetDir;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
        {
            targetDir = keila1.transform.position - transform.position;
        }
        else if (Input.GetKey("2"))
        {
            targetDir = keila2.transform.position - transform.position;
        }
        else if (Input.GetKey("3"))
        {
            targetDir = keila3.transform.position - transform.position;
        }
        else if (Input.GetKey("4"))
        {
            targetDir = keila4.transform.position - transform.position;
        }
        else if (Input.GetKey("5"))
        {
            targetDir = keila5.transform.position - transform.position;
        }
        else if (Input.GetKey("6"))
        {
            targetDir = keila6.transform.position - transform.position;
        }
        else if (Input.GetKey("7"))
        {
            targetDir = keila7.transform.position - transform.position;
        }
        else if (Input.GetKey("8"))
        {
            targetDir = keila8.transform.position - transform.position;
        }
        else if (Input.GetKey("9"))
        {
            targetDir = keila9.transform.position - transform.position;
        }
        

    }
    private void FixedUpdate()
    {
        


        //rb.AddRelativeForce(transform.forward * thrust);
        if (Input.GetKey("space"))
        {
            rb.AddForce(targetDir * thrust, ForceMode.Impulse);
        }
       
    }
}
