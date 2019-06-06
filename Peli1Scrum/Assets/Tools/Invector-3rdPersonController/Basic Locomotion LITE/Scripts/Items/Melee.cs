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
        takas = false;
        esillä = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.GetComponent<Collider>().isTrigger == false)
        {
            Physics.IgnoreCollision(player.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());

        }
        Vector3 miekkaPiiloon = transform.position + new Vector3(-0.5f, 0, 0.5f);
        Vector3 miekkaEsiin = transform.position + new Vector3(0.5f, 0, -0.5f);
        Vector3 perusasento = player.transform.localPosition + new Vector3(-0.25f, 0, 0);
        Vector3 lyontipiste = player.transform.localPosition + new Vector3(-0.25f, 0, 1f);
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
        if (Input.GetKeyDown("1") && keräilyScript.collectedMelee && esillä == true && tf == pp)
        {
            rotation = transform.localRotation;

            transform.localRotation = Quaternion.Euler(0, 0, 90);
            //transform.Translate(parentti.transform.localPosition+miekkaPiiloon,Space.Self);
            transform.localPosition = säilytyspaikka.transform.localPosition;
        }

        else if (Input.GetKeyDown("1") && keräilyScript.collectedMelee && !esillä && tf == sp)
        {
            esillä = true;
            transform.localRotation = rotation;
            //transform.Translate (parentti.transform.localPosition - miekkaPiiloon,Space.Self);
            transform.localPosition = peruspaikka.transform.localPosition;
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

            //if (transform.position== latauspaikka.transform.position)
            //{
            //    var hemee = 1;
            //}
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
    private void FixedUpdate()
    {
        //tf = transform.localPosition.ToString();
        //sp = säilytyspaikka.transform.localPosition.ToString();
        //pp = peruspaikka.transform.localPosition.ToString();
        //lp = latauspaikka.transform.localPosition.ToString();
        //lyp = lyontipaikka.transform.localPosition.ToString();

        //if (esillä)
        //{
            
        //}
    }
}

//Vanha. Ei käytössä, mutta pitää vielä varmistaa, että uus varmasti toimii
public class Melee2 : MonoBehaviour
{
    bool painettu = false;
    GameObject peruspaikka;
    GameObject lyontipaikka;
    GameObject latauspaikka;
    Keräily keräilyScript;
    GameObject player;
    GameObject säilytyspaikka;
    //public GameObject player;
    float latausSpeed = 1f;
    float lyontiSpeed = 6;
    bool lyo;
    bool takas;
    bool esillä;
    Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lyontipaikka = GameObject.Find("MeleeWeaponEtuPlace");
        latauspaikka = GameObject.Find("MeleeWeaponTakaPlace");
        säilytyspaikka = GameObject.Find("MeleeWeaponSäilytysPlace");

        peruspaikka = GameObject.Find("MeleeWeaponPlace");
        keräilyScript = player.GetComponent<Keräily>();
        takas = false;
        esillä = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.GetComponent<Collider>().isTrigger == false)
        {
            Physics.IgnoreCollision(player.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());

        }
        Vector3 miekkaPiiloon = transform.position + new Vector3(-0.5f, 0, 0.5f);
        Vector3 miekkaEsiin = transform.position + new Vector3(0.5f, 0, -0.5f);
        Vector3 perusasento = player.transform.localPosition + new Vector3(-0.25f, 0, 0);
        Vector3 lyontipiste = player.transform.localPosition + new Vector3(-0.25f, 0, 1f);

        if (transform.localPosition == säilytyspaikka.transform.localPosition)
        {
            esillä = false;
        }
        if (transform.localPosition == peruspaikka.transform.localPosition)
        {
            esillä = true;
        }
        if (Input.GetKeyDown("1") && keräilyScript.collectedMelee && esillä == true && transform.position == peruspaikka.transform.position)
        {
            rotation = transform.localRotation;

            transform.localRotation = Quaternion.Euler(0, 0, 90);
            //transform.Translate(parentti.transform.localPosition+miekkaPiiloon,Space.Self);
            transform.localPosition = säilytyspaikka.transform.localPosition;
        }

        else if (Input.GetKeyDown("1") && keräilyScript.collectedMelee && !esillä && transform.position == säilytyspaikka.transform.position)
        {
            esillä = true;
            transform.localRotation = rotation;
            //transform.Translate (parentti.transform.localPosition - miekkaPiiloon,Space.Self);
            transform.localPosition = peruspaikka.transform.localPosition;
        }
        if (esillä)
        {


            if (Input.GetMouseButton(0) && painettu == false && lyo == false)
            {
                if (transform.position == peruspaikka.transform.position)
                {
                    painettu = true;
                }


            }
            if (Input.GetMouseButtonUp(0) && keräilyScript.collectedMelee == true)
            {
                lyo = true;
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

                if (transform.position == lyontipaikka.transform.position)
                {
                    takas = true;
                    lyo = false;
                }
            }
            else if (takas == true)
            {
                float step = latausSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, peruspaikka.transform.position, step);

                if (transform.position == peruspaikka.transform.position)
                {
                    takas = false;
                    painettu = false;

                }
            }
        }
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
    private void FixedUpdate()
    {

    }
}
