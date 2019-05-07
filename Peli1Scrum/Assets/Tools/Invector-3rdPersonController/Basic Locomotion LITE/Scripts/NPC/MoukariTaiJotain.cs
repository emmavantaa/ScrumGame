using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoukariTaiJotain : MonoBehaviour
{
    public Transform aseenPerusPaikka;
    public Transform aseenYläPaikka;
    public Transform aseenOsumaPaikka;
    public GameObject moukari;
    GameObject player;
    public float speed=1;
    public float lyontiSpeed=5;
    public bool nosta;
    public bool nostaKeskelle;
    public bool laske;
    public bool aloita;
    public bool pelaajaPoistui;
    public bool keskella;
    public bool nollaa;
    Vector3 perusPaikka;
    Vector3 yläPaikka;
    Vector3 osumaPaikka;
    float attack1Range = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        keskella = true;
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        perusPaikka = transform.position;
        yläPaikka = transform.position + (Vector3.up * 1.5f);
        osumaPaikka = transform.position - new Vector3(0, 0.5f, 0);

        if (nollaa)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, aseenPerusPaikka.position, step);
        }
        if (pelaajaPoistui == true)
        {
            
            aloita = false;
            nosta = false;
            laske = false;
            nollaa = true;
        }
        if (nosta)
        {
            keskella = false;
            //transform.Translate(yläPaikka * Time.deltaTime, Space.World);

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, aseenYläPaikka.position, step);
            if (transform.position == aseenYläPaikka.position)
            {
                nosta = false;
            }
        }
        else if (transform.position == aseenYläPaikka.position && Vector3.Distance(aseenPerusPaikka.position - new Vector3(0, 0.607f, 0), player.transform.position) < attack1Range || laske)
        {
            
            laske = true;

            //transform.Translate(osumaPaikka * Time.deltaTime, Space.World);

            float step = lyontiSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, aseenOsumaPaikka.position, step);
            if (transform.position == aseenOsumaPaikka.position)
            {
                laske = false;
            }
        }
        else if (transform.position== aseenOsumaPaikka.position || nostaKeskelle&&aloita)
        {
            laske = false;
            nostaKeskelle = true;

            //transform.Translate(perusPaikka * Time.deltaTime, Space.World);

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, aseenPerusPaikka.position, step);
            if (transform.position == aseenPerusPaikka.position)
            {
                nostaKeskelle = false;
                aloita = false;
            }
        }
        else if (transform.position == aseenPerusPaikka.position)
        {
            keskella = true;
            nollaa = false;
            //aloita = false;
        }
    }

    public void HitPlayer()
    {
        //moukari.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        if (keskella)
        {
            nosta = true;
            aloita = true;
        }
        
        
    }
}
