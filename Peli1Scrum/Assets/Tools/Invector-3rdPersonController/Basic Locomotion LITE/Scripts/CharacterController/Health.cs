using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
//[DebuggerStepThrough]
public class Health : MonoBehaviour
    
{
    public float playerHp = 3;
    //public bool knockback;
    GameObject player;
    public float immortalityTime=2;
    float time1;
    public bool immortalMoment = false;
    public bool immortal;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("HP: "+playerHp);

        if (playerHp <= 0)
        {
            SceneManager.LoadScene("Level1");
        }

        if (immortalMoment&& Time.time>=+time1+ immortalityTime)
        {
            immortalMoment = false;


        }
        if (immortalMoment)
        {
            if (player.GetComponent<ImmortalEmission>())
            {
                player.GetComponent<ImmortalEmission>().EmissionOn();
            }
            
        }
        if (immortalMoment==false)
        {
            if (player.GetComponent<ImmortalEmission>())
            {
                player.GetComponent<ImmortalEmission>().EmissionOff();
            }
        }
    }
    private void FixedUpdate()
    {
        //if (knockback)
        //{
        //    player.GetComponent<Rigidbody>().AddExplosionForce
        //}
    }
    public void TakeDamage()
    {
        
        if (immortalMoment==false&&immortal==false)
        {
            
            playerHp -= 1;
            
        }
        time1 = Time.time;
        immortalMoment = true;



    }
    public void MoreHealth()
    {
        if (playerHp<3)
        {
            playerHp += 1;
        }
        
    }
}
