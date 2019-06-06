using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyAmmo : MonoBehaviour
{


    private void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.CompareTag("CarrotWall") || col.gameObject.CompareTag("Ground"))
        {

            Debug.Log("Hits Carrot wall");
            Destroy(gameObject, 1f);
        }
    }

}

