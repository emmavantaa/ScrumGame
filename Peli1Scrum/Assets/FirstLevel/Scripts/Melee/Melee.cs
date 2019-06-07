using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]

public class Melee : MonoBehaviour
{
    public bool painettu = false;
    GameObject peruspaikka;
    GameObject lyontipaikka;
    GameObject latauspaikka;
    Keräily keräilyScript;
    GameObject player;
    GameObject säilytyspaikka;
    ShootingScript shootingScript;
    GameObject gun;
    //public GameObject player;
    public float latausSpeed = 1f;
    public float lyontiSpeed = 6;
    public bool lyo;
    public bool takas;
    public bool esillä;
    Quaternion rotation;

    string tf;
    string sp;
    string pp;
    string lp;
    string lyp;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lyontipaikka = GameObject.Find("MeleeWeaponEtuPlace");
        latauspaikka = GameObject.Find("MeleeWeaponTakaPlace");
        säilytyspaikka = GameObject.Find("MeleeWeaponSäilytysPlace");

        peruspaikka = GameObject.Find("MeleeWeaponPlace");
        keräilyScript = player.GetComponent<Keräily>();
        shootingScript = player.GetComponent<ShootingScript>();
        takas = false;
        esillä = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (shootingScript.isOut)
        {
            gun = GameObject.Find("Bunnyzooka");
        }

        if (gameObject.GetComponent<Collider>().isTrigger == false)
        {
            Physics.IgnoreCollision(player.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());

        }
        tf = transform.localPosition.ToString();
        sp = säilytyspaikka.transform.localPosition.ToString();
        pp = peruspaikka.transform.localPosition.ToString();
        lp = latauspaikka.transform.localPosition.ToString();
        lyp = lyontipaikka.transform.localPosition.ToString();


        if (tf == sp)
        {
            esillä = false;
        }
        if (tf == pp)
        {
            esillä = true;
        }
        //Otetaan melee ase esille jos painetaan nappia
        if (Input.GetKeyDown("1") && keräilyScript.collectedMelee && !esillä)
        {
            UseMeleeWeapon();
        }

        //Laitetaan melee ase selkään jos painetaan nappia tai laitetaan automaattisesti selkään jos pyssy otetaan esille
        else if (Input.GetKeyDown("1") && keräilyScript.collectedMelee && esillä|| esillä && shootingScript.isOut)
        {
            DontUseMeleeWeapon();
        }

        if (esillä)
        {
            if (Input.GetMouseButton(0) && painettu == false && lyo == false)
            {
                if (tf == pp)
                {
                    painettu = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && keräilyScript.collectedMelee == true && painettu && tf == lp)
            {
                lyo = true;
                painettu = false;
            }

            else if (Input.GetMouseButtonUp(0) && keräilyScript.collectedMelee == true && painettu==true && lyo==false&&(tf != lp || tf != lyp || tf != pp))

            {
                lyo = false;
                painettu = false;
                takas = true;
            }

            if (painettu)
            {
                float step = latausSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, latauspaikka.transform.position, step);
            }
            if (lyo && takas == false)
            {
                float step = lyontiSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, lyontipaikka.transform.position, step);

                if (tf == lyp)
                {
                    takas = true;
                    lyo = false;
                }
            }
            else if (takas == true)
            {
                float step = latausSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, peruspaikka.transform.position, step);

                if (tf == pp)
                {
                    takas = false;
                    

                }
            }

        }
    }

    public void UseMeleeWeapon() //Otetaan melee ase esille
    {

        if (Input.GetKeyDown("1") && keräilyScript.collectedMelee && !esillä)
        {
            if (shootingScript.isOut)
            {
                gun.SetActive(false);
                shootingScript.isOut = false;
            }

            transform.localRotation = rotation;
            transform.localPosition = peruspaikka.transform.localPosition;
        }
    }


    public void DontUseMeleeWeapon() //Laitetaan melee ase selkään
    {
        rotation = transform.localRotation;
        transform.localRotation = Quaternion.Euler(0, 0, 90);
        transform.localPosition = säilytyspaikka.transform.localPosition;
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Enemy" && collision.gameObject != null)
        {
            //collision.gameObject.GetComponent<BasicEnemy>().takeDamage();
            collision.gameObject.GetComponent<VihuHealth>().takeDamage();

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VihunAlue")
        {
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }

        if (other.gameObject.tag == "Enemy" && other.gameObject != null && lyo)
        {
            //collision.gameObject.GetComponent<BasicEnemy>().takeDamage();
            other.gameObject.GetComponent<VihuHealth>().takeDamage();

        }
    }

}

