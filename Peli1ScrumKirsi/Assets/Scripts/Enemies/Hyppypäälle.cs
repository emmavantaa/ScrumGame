using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class Hyppypäälle : MonoBehaviour
{
    GameObject player;
    GameObject colliderPäähän;
    VihuHealth vihuHealth;
     GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.transform.parent.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        colliderPäähän = GameObject.Find("VihunPäälleHyppyCollider");
        vihuHealth = enemy.GetComponent<VihuHealth>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)

    {
        if (colliderPäähän!=null&&other.gameObject==colliderPäähän&& enemy != null && vihuHealth.immortal == false)
        {
            enemy.GetComponent<VihuHealth>().takeDamage();
        }
        //if (other.gameObject == player&&enemy!=null&&vihuHealth.immortal==false)
        //{
        //   enemy.GetComponent<VihuHealth>().takeDamage();

        //}
    }
}
