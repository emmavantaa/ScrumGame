using System.Collections;
using System.Collections.Generic;
using Invector.CharacterController;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]

public class Hyppii : MonoBehaviour
{
    vThirdPersonController cc;
    public bool hyppii;
    // Start is called before the first frame update
    private void Awake()
    {
        cc = gameObject.GetComponent<vThirdPersonController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (cc.isGrounded)
        //{
        //    hyppii = false;
        //}
    }
}
