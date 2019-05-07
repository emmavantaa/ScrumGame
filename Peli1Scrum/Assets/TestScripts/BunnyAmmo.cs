using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyAmmo : MonoBehaviour
{


   public GameObject[] checker;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("CarrotWall") || col.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hits Carrot wall");
            Destroy(gameObject,0.5f);
        }
    }

}
