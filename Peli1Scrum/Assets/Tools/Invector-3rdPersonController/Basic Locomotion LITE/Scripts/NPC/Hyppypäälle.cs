using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyppypäälle : MonoBehaviour
{
    GameObject player;
    VihuHealth vihuHealth;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        vihuHealth = enemy.GetComponent<VihuHealth>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)

    {
        if (other.gameObject == player&&enemy!=null&&vihuHealth.immortal==false)
        {
           enemy.GetComponent<VihuHealth>().takeDamage();

        }
    }
}
