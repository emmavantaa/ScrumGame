using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class CursorVisible : MonoBehaviour
{
    GameObject mc;
    vThirdPersonCamera tp;
    // Start is called before the first frame update
    void Start()
    {
        mc = GameObject.Find("vThirdPersonController");
        tp = mc.GetComponent<vThirdPersonCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            tp.lockCamera = true;
            
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            tp.lockCamera = false;
        }
    }
}
