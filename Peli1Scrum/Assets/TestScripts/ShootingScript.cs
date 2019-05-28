using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{ 
public Rigidbody bunnyPrefab; // Ammo
public Transform instantiateFrom; // Mistä se ammo tulee

public float fireRate = 0.5f;
private float nextFire = 0.0f;


    public float ammoSpeed;



private void Start()
{

}


// Update is called once per frame
void Update()
{
       

    if (Input.GetButtonDown("Fire1") && Time.time > nextFire) // Kun painat vas hiirinappia ("Fire1) On hiirien vasen nappi
    {

        nextFire = Time.time + fireRate;

            Shoot();

        }





        void Shoot()
        {


            Rigidbody bulletInst;

            bulletInst = Instantiate(bunnyPrefab, instantiateFrom.position, instantiateFrom.rotation) as Rigidbody;
            bulletInst.AddForce(instantiateFrom.forward * ammoSpeed);


        }


    }

    }