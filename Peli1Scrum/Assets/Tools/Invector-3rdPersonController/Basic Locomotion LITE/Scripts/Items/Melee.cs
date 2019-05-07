using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
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

        if (gameObject.GetComponent<Collider>().isTrigger==false)
        {
            Physics.IgnoreCollision(player.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());
           
        }
        Vector3 miekkaPiiloon =transform.position+ new Vector3(-0.5f, 0, 0.5f);
        Vector3 miekkaEsiin = transform.position+ new Vector3(0.5f, 0, -0.5f);
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
        if (Input.GetKeyDown("r") && keräilyScript.collected&&esillä==true)
        {
            rotation = transform.localRotation;
            
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            //transform.Translate(parentti.transform.localPosition+miekkaPiiloon,Space.Self);
            transform.localPosition = säilytyspaikka.transform.localPosition;
        }

        else if (Input.GetKeyDown("r")&&keräilyScript.collected && !esillä)
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
            if (Input.GetMouseButtonUp(0) && keräilyScript.collected == true)
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

        if (collision.gameObject.tag=="Enemy"&&collision.gameObject!=null)
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

        if (other.gameObject.tag == "Enemy" && other.gameObject != null&&lyo)
        {
            //collision.gameObject.GetComponent<BasicEnemy>().takeDamage();
            other.gameObject.GetComponent<VihuHealth>().takeDamage();

        }
    }
    private void FixedUpdate()
    {
        
    }
}
