using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAmmo : MonoBehaviour
{
    private float timeBtwShots;
    private float timeBtwShotsSwish;

    public float startTimeBtwShots;

    public GameObject projectile;
    //public GameObject Attack;

   public  ShootingScript shootscript;

    public static int maxSpawns;



    void Start()
    {

        timeBtwShots = startTimeBtwShots;

        maxSpawns = 0;

    }

    void Update()
    {


        if (maxSpawns != 3)
        {
            SpawnSmallEnemies();
        }

        if (shootscript.ammoAmount == 0 && maxSpawns ==3)
        {

            maxSpawns -= 3;

           
        }


    }


    void SpawnSmallEnemies()
    {

      
        if (timeBtwShots <= 1f)
        {
            maxSpawns += 1;

            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }


        if (timeBtwShotsSwish <= 0)
        {

            //Instantiate(Attack, transform.position, Quaternion.identity);
            timeBtwShotsSwish = startTimeBtwShots;
        }

        else
        {

            timeBtwShots -= Time.deltaTime;

        }
    }
}
