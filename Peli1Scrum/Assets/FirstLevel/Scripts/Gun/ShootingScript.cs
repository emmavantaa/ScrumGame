using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public bool ShootingisOn; // Katsoo onko hahmo saanut pyssyn
    public Text ammoAmountText; // Kuinka paljon ammoa on testinä
    public int ammoAmount;
    public Rigidbody bunnyPrefab; // Ammo
    public Transform instantiateFrom; // Mistä se ammo tulee
   


    public float ammoSpeed;



private void Start()
{
        ShootingisOn = false;
        
}


// Update is called once per frame
void Update()
{
       if (ShootingisOn == true)
        {
            if (Input.GetButtonDown("Fire1")) // Kun painat vas hiirinappia ("Fire1) On hiirien vasen nappi niin ammut
            {

                Shoot(); //Ampuminen


            }
        }

        AmmoUI();
  





        void Shoot()
        {
            if (ammoAmount >= 1)
            {
                Rigidbody bulletInst;

                bulletInst = Instantiate(bunnyPrefab, instantiateFrom.position, instantiateFrom.rotation) as Rigidbody;
                bulletInst.AddForce(instantiateFrom.forward * ammoSpeed);
                ammoAmount -= 1;

            }



        }


    }

    void AmmoUI()
    {
        ammoAmountText.text = "Ammo: " + ammoAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BunnyAmmo"))
        {
            ammoAmount += 1;
        }
    }
}