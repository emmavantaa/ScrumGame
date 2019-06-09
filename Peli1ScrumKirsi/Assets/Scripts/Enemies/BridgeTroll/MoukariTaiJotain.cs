using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using System.Diagnostics;
//[DebuggerStepThrough]
public class MoukariTaiJotain : MonoBehaviour
{
    public Transform aseenPerusPaikka;
    public Transform aseenYläPaikka;
    public Transform aseenOsumaPaikka;
    public GameObject moukari;
    GameObject player;
    public bool fysiikkaKnockBack;
    public bool translateKnockBack;
    public float knockBackAlue;
    public float knockBackVoima;
    public float perusAsentoonSpeed;
    public float lyontiSpeed;
    public float nostoSpeed;
    public float lyöntiMatkaRaja;
    public float lyöntiMatkaRajaJosKiipeää;
    public float translateKesto;
    public float translateKnockBackVoima;
    float translateAlkamisaika;
    public bool nosta;
    public bool nostaKeskelle;
    public bool laske;
    public bool laskeKunKiipeää;
    public bool aloita;
    public bool pelaajaPoistui;
    public bool keskella;
    public bool nollaa;
    public bool forcea;
    bool translateKnockBackToteuta;
    public Vector3 voimanSuunta;
    vThirdPersonController cc;
    Transform knockBackSeuranta;
    GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        keskella = true;
        parent = gameObject.transform.parent.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<vThirdPersonController>();
        //parent = gameObject.transform.parent.gameObject;
        //knockBackSeuranta = parent.transform.Find("KnockBackSeuranta");
        knockBackSeuranta = transform.Find("KnockBackSeuranta");



    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = player.transform.position - knockBackSeuranta.transform.position;
        float steppi = 99 * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(knockBackSeuranta.transform.forward, targetDir, steppi, 0.0f);
        //UnityEngine.Debug.DrawRay(transform.position, newDir * 1.5f, Color.blue);
        newDir.y = 0;

        knockBackSeuranta.rotation = Quaternion.LookRotation(newDir);

        if (nollaa)
        {
            float step = perusAsentoonSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, aseenPerusPaikka.position, step);
        }
        if (pelaajaPoistui == true)
        {
            nostaKeskelle = false;
            aloita = false;
            nosta = false;
            laske = false;
            laskeKunKiipeää = false;
            nollaa = true;
        }
        if (nosta)
        {
            keskella = false;
            //transform.Translate(yläPaikka * Time.deltaTime, Space.World);

            float step = nostoSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, aseenYläPaikka.position, step);
            if (transform.position == aseenYläPaikka.position)
            {
                nosta = false;
            }
        }
        else if (laske || laskeKunKiipeää || (transform.position == aseenYläPaikka.position && (Vector3.Distance(aseenPerusPaikka.position - new Vector3(0, 0.607f, 0), player.transform.position) < lyöntiMatkaRaja)))
        {
            
            laske = true;
            laskeKunKiipeää = false;

            //transform.Translate(osumaPaikka * Time.deltaTime, Space.World);

            float step = lyontiSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, aseenOsumaPaikka.position, step);

            if (transform.position == aseenOsumaPaikka.position)
            {
                laske = false;
                
            }
        }
        else if ((transform.position == aseenYläPaikka.position &&
            (Vector3.Distance(aseenPerusPaikka.position - new Vector3(0, 0.607f, 0), player.transform.position) < lyöntiMatkaRajaJosKiipeää) &&
            player.GetComponent<Kiipeäminen>().roiku == true)&&!laskeKunKiipeää&&!laske)
        {
            laskeKunKiipeää = true;

        }
        else if (transform.position== aseenOsumaPaikka.position || nostaKeskelle&&aloita)
        {
            laske = false;
            laskeKunKiipeää = false;
            nostaKeskelle = true;

            //transform.Translate(perusPaikka * Time.deltaTime, Space.World);

            float step = perusAsentoonSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, aseenPerusPaikka.position, step);
            if (transform.position == aseenPerusPaikka.position)
            {
                nostaKeskelle = false;
                aloita = false;
            }
            if (forcea == true && Vector3.Distance(transform.position, player.transform.position) < knockBackAlue)
            {
                forcea = false;
                KnockBackPlayer();
            }
        }
        else if (transform.position == aseenPerusPaikka.position)
        {
            keskella = true;
            nollaa = false;
            //aloita = false;
        }
        if (translateKnockBackToteuta)
        {
            player.transform.Translate(Vector3.forward * ((translateKnockBackVoima) * Time.deltaTime), knockBackSeuranta);
            player.GetComponent<Kiipeäminen>().Nollaa();
            if (Time.time > translateAlkamisaika + translateKesto)
            {
                translateKnockBackToteuta = false;

                if (player.GetComponent<Health>().dead == false)
                {
                    cc.lockMovement = false;
                }
            }
        }
    }

    public void HitPlayer()
    {
        //moukari.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        if (keskella)
        {
            forcea = true;
            nosta = true;
            aloita = true;
        }
        
        
    }
    public void KnockBackPlayer()
    {
        if (player.GetComponent<Health>().immortalMoment == false)
        {
            if (fysiikkaKnockBack)
            {
                voimanSuunta = new Vector3(player.transform.position.x - aseenOsumaPaikka.position.x, 0f, player.transform.position.z - aseenOsumaPaikka.position.z);
                voimanSuunta.Normalize();
                player.GetComponent<Rigidbody>().AddForce((voimanSuunta) * knockBackVoima, /*transform.position + new Vector3(0, -0.5f, 0)*/ ForceMode.Impulse);
                player.GetComponent<Health>().hitByEnemy = true;
                player.GetComponent<Health>().TakeDamage();
            }
            else if (translateKnockBack)
            {
                translateKnockBackToteuta = true;
                cc.lockMovement = true;
                player.GetComponent<Health>().hitByEnemy = true;
                player.GetComponent<Health>().TakeDamage();
                translateAlkamisaika = Time.time;
                
            }

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player && player.GetComponent<Health>().immortalMoment == false)
        {
            if (forcea==true)
            {
                forcea = false;
               
                KnockBackPlayer();

            }
            
        }

    }
}
