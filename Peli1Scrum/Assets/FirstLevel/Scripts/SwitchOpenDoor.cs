using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOpenDoor : MonoBehaviour
{

    public GameObject playerCamera;
    public GameObject doorOpenCam;

    public Animator animSwich;

    public Animator animCamera;

    public Rigidbody rb;

   

    public bool isOpen;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        //animSwich = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isOpen== true && timer >= 300)
        {
            playerCamera.SetActive(true);
            doorOpenCam.SetActive(false);
            Time.timeScale = 1;
            rb.isKinematic =false;
        }

        if (isOpen == true)
        {
            timer++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("vipu"))
        {

            if (Input.GetKey(KeyCode.E))
            {
                isOpen = true;
                playerCamera.SetActive(false);
                doorOpenCam.SetActive(true);
                animSwich.SetBool("SwitchOn", true);
                animCamera.SetBool("camOn", true);
                rb.isKinematic = true;
                Debug.Log("Switch Down");
            }
          

        }

      
    }


}
