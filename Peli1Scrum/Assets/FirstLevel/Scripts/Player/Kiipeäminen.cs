using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]

//////////Tähän scriptiin tuli paljon muutosta, niin pitää päivittää selitykset//////////

public class Kiipeäminen : MonoBehaviour
{
    vThirdPersonMotor pm;

    public Transform Ray0Origin;
    public Transform Ray1Origin;
    public Transform Ray2Origin;
    public Transform Ray3Origin;
    public Transform Ray4Origin;
    public Transform Ray5Origin;
    public Transform RayNormalOrigin;
    public Transform RayRoikkumisTasoKorkeusOrigin;

    Vector3 tulevaPaikka;
    Vector3 tulevaPaikka1;
     Vector3 tulevaPaikka2;
    Vector3 paikka;
     Vector3 roikkumispaikka;
     float roikkumispaikkaY;
     Vector3 korjattuRoikkumisPaikka;
     Vector3 kielekkeenPaikka;
     Vector3 KielekkeenPaikkaHypystä;
     Vector3 seinänPaikkaTiputtautumisesta;

    Rigidbody rb;
    CapsuleCollider col;

     bool liiku = false;
    [HideInInspector]
    public bool kesken = false;
     bool valmis = true;
     bool kiipeys = false;
    [HideInInspector]
    public bool roiku;
     bool kiipeilyVasemmalle;
     bool kiipeilyOikealle;
     bool kiipeysRoikkumisesta;
     bool tipuRoikkumisesta;
     bool eka;
     bool roikuKielekkeellä;
     bool roikkuminenHypystä;
     bool tiputtautuminen;
     bool eiVoiKiivetäVasemmalle;
     bool eiVoiKiivetäOikealle;
     bool capsuleOsuuVaultingissa;

     bool ray5PoisToiminnasta;
     bool ray4PoisToiminnasta;
     bool ray3PoisToiminnasta;
     bool ray2PoisToiminnasta;
     bool ray1PoisToiminnasta;
     bool ray0PoisToiminnasta;

     Vector3 orig;
     Vector3 dir;

    Quaternion oikeaSuunta;

    vThirdPersonController cc;

    RaycastHit RaycastHitNormal;
    RaycastHit RaycastHitNormal1;
    RaycastHit rayCastHitSuunnanVaihto;
    RaycastHit kiipeemisenAlotusHit;

    Vector3 ray0JosEiOsuHitPiste;

    RaycastHit hitKielekkeelleRay0;
    RaycastHit hitKielekkeelleRay1;
    RaycastHit hitKielekkeelleRay2;
    RaycastHit hitKielekkeelleRay3;
    RaycastHit hitKielekkeelleRay4;
    RaycastHit hitKielekkeelleRay5;
    RaycastHit taakseRayHit;
    RaycastHit rayYlösHit;

    // Start is called before the first frame update

    private void Awake()
    {
        pm = gameObject.GetComponent<vThirdPersonMotor>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        cc = gameObject.GetComponent<vThirdPersonController>();
    }
    void Start()
    {

    }

    // Update is called once per frame

        public void Nollaa()
    {
        roiku = false;
        liiku = false;
        kesken = false;
        valmis = true;
        kiipeys = false;
        kiipeilyVasemmalle = false;
        kiipeilyOikealle = false;
        roikuKielekkeellä = false;
        roikkuminenHypystä = false;
        eiVoiKiivetäVasemmalle = false;
        eiVoiKiivetäOikealle = false;
        roikkumispaikka = Vector3.zero;
        roikkumispaikkaY = 0f;

        cc.lockMovement = false;
        cc.keepDirection = false;
        cc.isStrafing = false;
        cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Collider>().isTrigger = false;
        tipuRoikkumisesta = false;
    }
    void Update()
    {
        //Kiipeemisen aloittaminen jos on ilmassa
        if (liiku == false && roiku == false && eka == false && cc.isGrounded == false && Input.GetKey("r") == false)
        {
            Ray hyppyRayAlempi = new Ray(RayNormalOrigin.position, transform.forward);
            RaycastHit hyppyRayAlempiHit;

            //Jos edessä on seinä, joka ei ole kasvi tai vihollinen
            if (Physics.Raycast(hyppyRayAlempi, maxDistance: 0.5f, hitInfo: out hyppyRayAlempiHit) &&
                hyppyRayAlempiHit.collider.tag != "Kasvis" && hyppyRayAlempiHit.collider.tag != "Enemy"&& hyppyRayAlempiHit.collider.tag != "Miekka" &&
                hyppyRayAlempiHit.collider.tag != "Lava" && hyppyRayAlempiHit.collider.tag != "Moukari")
            {
                //Jos seinän päällä on tasanne
                Ray hyppyRayYlempi = new Ray(RayRoikkumisTasoKorkeusOrigin.position, transform.forward);
                if (!Physics.Raycast(hyppyRayYlempi, maxDistance: 0.55f))
                {
                    Ray hyppyRayYlempiAlas = new Ray(RayRoikkumisTasoKorkeusOrigin.position + (transform.forward.normalized * 0.55f), -transform.up);
                    RaycastHit hyppyRayYlempiAlasHit;

                    //Tsekataan, että tasanteella on lattia ja otetaan sen koordinaatit talteen
                    if (Physics.Raycast(hyppyRayYlempiAlas, maxDistance: 0.2f, hitInfo: out hyppyRayYlempiAlasHit) &&
                        hyppyRayYlempiAlasHit.collider.tag != "Kasvis" && hyppyRayYlempiAlasHit.collider.tag != "Enemy" && hyppyRayYlempiAlasHit.collider.tag != "Miekka"&&
                        hyppyRayAlempiHit.collider.tag != "Lava" && hyppyRayAlempiHit.collider.tag != "Moukari")
                    {
                        //Tsekataan, että lattian pinta ei ole liian kalteva
                        if (hyppyRayYlempiAlasHit.normal.y>=0.9f)
                        {
                            Vector3 capsulenPaikkaKeski;
                            Vector3 point1Keski;
                            Vector3 point2Keski;

                            Quaternion osumanSuunta = Quaternion.FromToRotation(transform.forward, new Vector3(-hyppyRayAlempiHit.normal.x, 0f, -hyppyRayAlempiHit.normal.z)) * transform.rotation;
                            capsulenPaikkaKeski = new Vector3(hyppyRayAlempiHit.point.x, col.transform.position.y, hyppyRayAlempiHit.point.z) - (-hyppyRayAlempiHit.normal * 0.4f);


                            float capsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                            point1Keski = capsulenPaikkaKeski + col.center + Vector3.up * capsulenMatkaKeskeltäPointteihin;
                            point2Keski = capsulenPaikkaKeski + col.center - Vector3.up * capsulenMatkaKeskeltäPointteihin;
                            float radius = col.radius * 0.1f;

                            UnityEngine.Debug.DrawRay(point1Keski, osumanSuunta * (-Vector3.right.normalized * 0.37f), color: Color.black, 22);
                            UnityEngine.Debug.DrawRay(point1Keski, osumanSuunta * (Vector3.right.normalized * 0.37f), color: Color.blue, 22);
                            RaycastHit hitInfoVasen;
                            RaycastHit hitInfoOikea;

                            //Varmistetaan, että tuleva paikka ei laita pelaajaa seinän sisään vasemmalla eli että pelaaja mahtuu tulevaan paikkaan
                            if (Physics.CapsuleCast(point1Keski, point2Keski, radius, osumanSuunta * (-Vector3.right), maxDistance: 0.37f, layerMask: 1, hitInfo: out hitInfoVasen, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
                            {
                                eiVoiKiivetäVasemmalle = true;
                            }
                            //Varmistetaan, että tuleva paikka ei laita pelaajaa seinän sisään oikealla eli että pelaaja mahtuu tulevaan paikkaan
                            if (Physics.CapsuleCast(point1Keski, point2Keski, radius, osumanSuunta * (Vector3.right), maxDistance: 0.37f, layerMask: 1, hitInfo: out hitInfoOikea, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
                            {
                                eiVoiKiivetäOikealle = true;
                            }
                            //Jos pelaaja mahtuu tulevaan paikkaan
                            if (eiVoiKiivetäVasemmalle == false && eiVoiKiivetäOikealle == false)
                            {
                                rb.velocity = Vector3.zero;
                                cc.lockMovement = true;
                                cc.keepDirection = true;
                                cc.locomotionType = vThirdPersonMotor.LocomotionType.OnlyStrafe;
                                rb.isKinematic = true;
                                cc.isSprinting = false;
                                gameObject.GetComponent<Collider>().isTrigger = true;

                                kesken = true;
                                valmis = false;
                                tulevaPaikka2 = hyppyRayYlempiAlasHit.point;
                                kiipeys = false;
                                roikkumispaikkaY = hyppyRayYlempiAlasHit.point.y;
                                roiku = true;
                                roikkuminenHypystä = true;
                                KielekkeenPaikkaHypystä = hyppyRayAlempiHit.point;

                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-hyppyRayAlempiHit.normal.x, 0f, -hyppyRayAlempiHit.normal.z)) * transform.rotation;
                                oikeaSuunta = transform.rotation;
                            }

                            //Jos pelaaja joutuisi liian lähelle seinää oikealla
                            if (eiVoiKiivetäVasemmalle == false && eiVoiKiivetäOikealle == true)
                            {
                                rb.velocity = Vector3.zero;
                                cc.lockMovement = true;
                                cc.keepDirection = true;
                                cc.locomotionType = vThirdPersonMotor.LocomotionType.OnlyStrafe;
                                rb.isKinematic = true;
                                cc.isSprinting = false;
                                gameObject.GetComponent<Collider>().isTrigger = true;

                                kesken = true;
                                valmis = false;
                                tulevaPaikka2 = hyppyRayYlempiAlasHit.point;
                                kiipeys = false;
                                roikkumispaikkaY = hyppyRayYlempiAlasHit.point.y;
                                roiku = true;
                                roikkuminenHypystä = true;
                                KielekkeenPaikkaHypystä = hyppyRayAlempiHit.point + (osumanSuunta * (-Vector3.right * 0.07f));

                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-hyppyRayAlempiHit.normal.x, 0f, -hyppyRayAlempiHit.normal.z)) * transform.rotation;
                                oikeaSuunta = transform.rotation;
                            }

                            //Jos pelaaja joutuisi liian lähelle seinää vasemmalla
                            if (eiVoiKiivetäVasemmalle == true && eiVoiKiivetäOikealle == false)
                            {
                                rb.velocity = Vector3.zero;
                                cc.lockMovement = true;
                                cc.keepDirection = true;
                                cc.locomotionType = vThirdPersonMotor.LocomotionType.OnlyStrafe;
                                rb.isKinematic = true;
                                cc.isSprinting = false;
                                gameObject.GetComponent<Collider>().isTrigger = true;

                                kesken = true;
                                valmis = false;
                                tulevaPaikka2 = hyppyRayYlempiAlasHit.point;
                                kiipeys = false;
                                roikkumispaikkaY = hyppyRayYlempiAlasHit.point.y;
                                roiku = true;
                                roikkuminenHypystä = true;
                                KielekkeenPaikkaHypystä = hyppyRayAlempiHit.point + (osumanSuunta * (Vector3.right * 0.07f));

                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-hyppyRayAlempiHit.normal.x, 0f, -hyppyRayAlempiHit.normal.z)) * transform.rotation;
                                oikeaSuunta = transform.rotation;
                            }

                            //Jos pelaaja ei mahdu roikkumaan tulevaan paikkaan 
                            else
                            {
                                eiVoiKiivetäVasemmalle = false;
                                eiVoiKiivetäOikealle = false;
                            }
                        }
                        
                    }
                }
            }
        }
        //Kiipeemisen aloittaminen jos on maassa
        else if (Input.GetKeyDown("e") && liiku == false && roiku == false && eka == false && cc.isGrounded == true)
        {
            //var speed = rb.velocity.magnitude;
            //if (speed < 0.5)
            //{
            //    rb.velocity = new Vector3(0, 0, 0);
            //    //Or
            //    //gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            //}

            Ray ray0 = new Ray(Ray0Origin.position, transform.forward);
            Ray rayYlös = new Ray(Ray0Origin.position, transform.up);

            //Tsekataan että ei oo kattoa jos on niin laitetaan rayt jotka on katon rajaa korkemmalla pois päältä, että ei kiivetä katon läpi 
            if (Physics.Raycast(rayYlös, out rayYlösHit, 3f))
            {

                if (rayYlösHit.point.y < Ray5Origin.position.y)
                {
                    ray5PoisToiminnasta = true;
                }
                if (rayYlösHit.point.y < Ray4Origin.position.y)
                {
                    ray4PoisToiminnasta = true;
                }
                if (rayYlösHit.point.y < Ray3Origin.position.y)
                {
                    ray3PoisToiminnasta = true;
                }
                if (rayYlösHit.point.y < Ray2Origin.position.y)
                {
                    ray2PoisToiminnasta = true;
                }
            }

            UnityEngine.Debug.DrawRay(ray0.origin, ray0.direction.normalized * 0.5f, color: Color.black, 33);

            //Tsekataan että alin ray osuu seinään
            if (Physics.Raycast(ray0, out kiipeemisenAlotusHit, maxDistance: 0.5f))
            {
                //Estetään kiipely kasvien ja vihollisten päälle
                if (kiipeemisenAlotusHit.collider.tag != "Kasvis" && kiipeemisenAlotusHit.collider.tag != "Enemy" && kiipeemisenAlotusHit.collider.tag != "Miekka" &&
                        kiipeemisenAlotusHit.collider.tag != "Lava" && kiipeemisenAlotusHit.collider.tag != "Moukari")
                {
                    var tulevaSuunta = Quaternion.FromToRotation(transform.forward, new Vector3(-kiipeemisenAlotusHit.normal.x, 0f, -kiipeemisenAlotusHit.normal.z)) * transform.rotation;
                    //var tt = tulevaSuunta * Vector3.forward;
                    //var tt1 = tt.normalized;
                    //var tt2 = tt1.ToString();
                    var tulevaSuuntaToisinpäin = tulevaSuunta * -Vector3.forward;
                    var tulevaSuuntaToisinpäinNormaali = tulevaSuuntaToisinpäin.normalized;
                    var tulevaSuuntaToisinpäinNormaaliString = tulevaSuuntaToisinpäinNormaali.ToString();

                    var hitinsuunta = kiipeemisenAlotusHit.normal.normalized;
                    var hitinsuuntaString = hitinsuunta.ToString();

                    //Tsekataan että seinä jota pitkin aiotaan kiivetä on suora y suunnassa, ettei esim kiivetä kaltevalla lattialla 
                    if (hitinsuuntaString == tulevaSuuntaToisinpäinNormaaliString)
                    {
                        rb.velocity = Vector3.zero;
                        cc.lockMovement = true;
                        cc.keepDirection = true;
                        cc.locomotionType = vThirdPersonMotor.LocomotionType.OnlyStrafe;
                        rb.isKinematic = true;
                        cc.isSprinting = false;
                        gameObject.GetComponent<Collider>().isTrigger = true;
                        transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-kiipeemisenAlotusHit.normal.x, 0f, -kiipeemisenAlotusHit.normal.z)) * transform.rotation;
                        oikeaSuunta = transform.rotation;
                        eka = true;
                    }
                }
            }
            //Jos alin ray ei osu seinään, niin tsekataan voiko kiivetä kielekkeelle
            else
            {
                KiipeilyKielekkeelle();
                if (roikuKielekkeellä == true)
                {
                    rb.velocity = Vector3.zero;
                    cc.lockMovement = true;
                    cc.keepDirection = true;
                    cc.isSprinting = false;
                    cc.locomotionType = vThirdPersonMotor.LocomotionType.OnlyStrafe;
                    rb.isKinematic = true;
                    gameObject.GetComponent<Collider>().isTrigger = true;
                }
                else
                {

                }
            }
        }

        //Roikkumiseen tiputtautuminen
        else if (Input.GetKeyDown("r") && liiku == false && roiku == false && eka == false && cc.isGrounded == true)
        {
            //Tsekataan, että edessä on tyhjää alhaalla
            Ray ray0Alas= new Ray(Ray0Origin.position+(transform.forward*0.55f), -transform.up);
            RaycastHit tt;
            if (!Physics.Raycast(ray0Alas , maxDistance: 1f, hitInfo: out tt))
            {
                //Tsekataan, että ray osuu sen lattian sivuun missä seistään
                Ray roikkumispaikanCheckRay = new Ray(ray0Alas.origin - (transform.up * 0.15f), -transform.forward);
                if (Physics.Raycast(roikkumispaikanCheckRay, maxDistance: 0.55f, hitInfo: out taakseRayHit))
                {
                    //Estetään roikkuminen kasvien ja vihollisten päällä
                    if (taakseRayHit.collider.tag != "Kasvis" && taakseRayHit.collider.tag != "Enemy" && taakseRayHit.collider.tag != "Miekka" &&
                        taakseRayHit.collider.tag != "Lava" && taakseRayHit.collider.tag != "Moukari")
                    {
                        var tulevaSuunta = Quaternion.FromToRotation(transform.forward, new Vector3(-taakseRayHit.normal.x, 0f, -taakseRayHit.normal.z)) * transform.rotation;
                        var tulevaSuuntaToisinpäin = tulevaSuunta * -Vector3.forward;
                        var tulevaSuuntaToisinpäinNormaali = tulevaSuuntaToisinpäin.normalized;
                        var tulevaSuuntaToisinpäinNormaaliString = tulevaSuuntaToisinpäinNormaali.ToString();

                        var hitinsuunta = taakseRayHit.normal.normalized;
                        var hitinsuuntaString = hitinsuunta.ToString();

                        hitinsuunta.Normalize();
                        tulevaSuuntaToisinpäin.Normalize();

                        //UnityEngine.Debug.DrawRay()

                        //Tsekataan että seinä jota pitkin aiotaan kiivetä on suora y suunnassa, ettei esim kiivetä kaltevalla lattialla 
                        if (hitinsuunta.y <=0.15f)
                        {
                            
                            Vector3 point1Keski;
                            Vector3 point2Keski;
                            

                            float capsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                            point1Keski = transform.position + col.center + Vector3.up * capsulenMatkaKeskeltäPointteihin;
                            point2Keski = transform.position + col.center - Vector3.up * capsulenMatkaKeskeltäPointteihin;
                            float radius = col.radius *0.95f;
                            var maxDistanssi = Vector3.Distance(new Vector3(taakseRayHit.point.x, 0f, taakseRayHit.point.z), new Vector3(transform.position.x, 0f, transform.position.z)) + 0.55f;

                            //CapsuleCast eteenpäin
                            if (!Physics.CapsuleCast(point1Keski, point2Keski, radius, transform.forward, maxDistance: maxDistanssi, layerMask: 1, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
                            {
                                point1Keski = (transform.position + (transform.forward * maxDistanssi)) + col.center + Vector3.up * capsulenMatkaKeskeltäPointteihin;
                                point2Keski = (transform.position + (transform.forward * maxDistanssi)) + col.center - Vector3.up * capsulenMatkaKeskeltäPointteihin;

                                //CapsuleCast alaspäin
                                if (!Physics.CapsuleCast(point1Keski, point2Keski, radius, -transform.up, maxDistance: 1f, layerMask: 1, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
                                {
                                    var positio = transform.position;
                                    rb.velocity = Vector3.zero;
                                    cc.lockMovement = true;
                                    cc.keepDirection = true;
                                    cc.locomotionType = vThirdPersonMotor.LocomotionType.OnlyStrafe;
                                    rb.isKinematic = true;
                                    cc.isSprinting = false;
                                    gameObject.GetComponent<Collider>().isTrigger = true;

                                    kesken = true;
                                    valmis = false;
                                    tulevaPaikka2 = positio;
                                    kiipeys = false;
                                    roikkumispaikkaY = positio.y;
                                    roiku = true;
                                    tiputtautuminen = true;
                                    seinänPaikkaTiputtautumisesta = taakseRayHit.point;

                                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-taakseRayHit.normal.x, 0f, -taakseRayHit.normal.z)) * transform.rotation;
                                    oikeaSuunta = transform.rotation;
                                }
                            }
                        }
                    }
                }

            }
        }

        //kiipeysRoikkumisesta ja kiipeys ilman roikkumista matalan esteen yli
        else if (liiku && kiipeys || liiku && Input.GetKeyDown("e") && roiku)
        {
            kiipeysRoikkumisesta = true;
        }

        //tipuRoikkumisesta
        else if (roiku && Input.GetKeyDown("r") && transform.position == korjattuRoikkumisPaikka)
        {
            roiku = false;
            tipuRoikkumisesta = true;
        }

        //kiipeilyVasemmalle
        if (roiku && Input.GetKey("a") && transform.position == korjattuRoikkumisPaikka && transform.rotation == oikeaSuunta && eiVoiKiivetäVasemmalle == false)
        {
            Ray rayRoikkumistasoVasemmalle = new Ray(RayRoikkumisTasoKorkeusOrigin.position + (-transform.right.normalized * 0.1f), transform.forward);
            UnityEngine.Debug.DrawRay(rayRoikkumistasoVasemmalle.origin, (rayRoikkumistasoVasemmalle.direction.normalized * 0.6f), color: Color.white, 33);

            //Tsekataan, että roikkumistasanne ei muutu seinäksi 0.1 yksikköä roikkumis seinästä pelaajan katsomissuuntaan (eli kun pelaaja on 0.5 yksikköä seinästä ja ray menee 0.6 yksikköä eteenpäin)
            if (Physics.Raycast(rayRoikkumistasoVasemmalle, maxDistance: 0.6f) == false)
            {
                
                kiipeilyVasemmalle = true;
            }
        }

        //kiipeilyOikealle
        else if (roiku && Input.GetKey("d") && transform.position == korjattuRoikkumisPaikka && transform.rotation == oikeaSuunta && eiVoiKiivetäOikealle == false)
        {
            Ray rayRoikkumistasoOikealle = new Ray(RayRoikkumisTasoKorkeusOrigin.position + (transform.right.normalized * 0.1f), transform.forward);
            UnityEngine.Debug.DrawRay(rayRoikkumistasoOikealle.origin, (rayRoikkumistasoOikealle.direction.normalized * 0.6f), color: Color.white, 33);

            //Tsekataan, että roikkumistasanne ei muutu seinäksi 0.1 yksikköä roikkumis seinästä pelaajan katsomissuuntaan (eli kun pelaaja on 0.5 yksikköä seinästä ja ray menee 0.6 yksikköä eteenpäin)
            if (Physics.Raycast(rayRoikkumistasoOikealle, maxDistance: 0.6f) == false)
            {
               
                kiipeilyOikealle = true;
            }
        }

        //Tässä tapahtuu kiipeily suoraa seinää pitkin. Tän vois muuttaa samanalaiseksi kuin kiipeilyn kielekkeelle
        if (eka)
        {
            eka = false;

            VaultingUusin();
        }

        //roikkumisposition korjaus
        if (roiku && transform.rotation.normalized == oikeaSuunta.normalized)
        {
            //roikkumispaikka.y = roikkumispaikkaY - 1f;

            //Tällä pidetään pelaaja oikeassa kohdassa roikkumassa
            if (transform.position == korjattuRoikkumisPaikka)
            {
                liiku = true;
                Ray rayKorkeusCheck = new Ray((RayRoikkumisTasoKorkeusOrigin.position+(transform.forward.normalized*0.6f)), -transform.up);
                RaycastHit rayKorkeusCheckHit;
                UnityEngine.Debug.DrawRay(rayKorkeusCheck.origin, rayKorkeusCheck.direction.normalized * 0.2f, color: Color.magenta, 8);
                if (Physics.Raycast(rayKorkeusCheck, maxDistance: 0.2f, hitInfo: out rayKorkeusCheckHit))
                {
                    
                    roikkumispaikka.y = rayKorkeusCheckHit.point.y - 1f;

                    Ray rayNormal = new Ray(RayNormalOrigin.position, transform.forward);
                    UnityEngine.Debug.DrawRay(rayNormal.origin, rayNormal.direction, color: Color.grey, 3);
                    //Pidetään etäisyys seinästä 0.5 yksikköä
                    if (Physics.Raycast(rayNormal, maxDistance: 1, hitInfo: out RaycastHitNormal))
                    {
                        korjattuRoikkumisPaikka = RaycastHitNormal.point - (transform.forward *0.5f);
                        korjattuRoikkumisPaikka.y = roikkumispaikka.y;
                        transform.position = korjattuRoikkumisPaikka;
                        roikkumispaikka = korjattuRoikkumisPaikka;
                    }
                }
            }

            //Nämä tapahtuu eka, kun synkronisoidaan roikkumispaikat
            else
            {
                liiku = true;

                Ray rayNormal = new Ray(new Vector3(RayNormalOrigin.position.x, roikkumispaikkaY, RayNormalOrigin.position.z) - (transform.forward * 0.5f), transform.forward);
                UnityEngine.Debug.DrawRay(rayNormal.origin, rayNormal.direction, color: Color.yellow, 55);

                //Hyppyroikkumis position korjaus
                if (roikkuminenHypystä == true)
                {
                    korjattuRoikkumisPaikka = KielekkeenPaikkaHypystä - (transform.forward * 0.5f);
                    korjattuRoikkumisPaikka.y = roikkumispaikkaY - 1f;

                    transform.position = korjattuRoikkumisPaikka;
                    roikkumispaikka = korjattuRoikkumisPaikka;
                    roikkuminenHypystä = false;
                }
                //Suoran seinän roikkumis position korjaus

                //Kieleke roikkumisen position korjaus
                else if (roikuKielekkeellä == true)
                {
                    korjattuRoikkumisPaikka = kielekkeenPaikka - (transform.forward * 0.5f);
                    korjattuRoikkumisPaikka.y = roikkumispaikkaY - 1f;

                    transform.position = korjattuRoikkumisPaikka;
                    roikkumispaikka = korjattuRoikkumisPaikka;
                    roikuKielekkeellä = false;
                }
                else if (tiputtautuminen)
                {
                    korjattuRoikkumisPaikka = seinänPaikkaTiputtautumisesta - (transform.forward * 0.5f);
                    korjattuRoikkumisPaikka.y = roikkumispaikkaY - 1f;

                    transform.position = korjattuRoikkumisPaikka;
                    roikkumispaikka = korjattuRoikkumisPaikka;
                    tiputtautuminen = false;
                }
                else if (Physics.Raycast(rayNormal, maxDistance: 1, hitInfo: out RaycastHitNormal))
                {

                    korjattuRoikkumisPaikka = RaycastHitNormal.point - (transform.forward * 0.5f);
                    korjattuRoikkumisPaikka.y = roikkumispaikkaY - 1f;

                    transform.position = korjattuRoikkumisPaikka;
                    roikkumispaikka = korjattuRoikkumisPaikka;

                }
            }
        }

        //kiipeysRoikkumisesta ja kiipeys ilman roikkumista matalan esteen yli toteutus
        if (kiipeysRoikkumisesta)
        {
            Ray rayNormalTulevaPositio = new Ray(RayNormalOrigin.position + new Vector3(0f, 1f, 0f), transform.forward);
            RaycastHit rr;
            UnityEngine.Debug.DrawRay(rayNormalTulevaPositio.origin, rayNormalTulevaPositio.direction, color: Color.red, 333f);

            //Jos pisteestä raynormal origin +1 ylöspäin ray +1 eteenpäin ei osu, eli ei ole seinää tasanteen päällä 
            if (Physics.Raycast(rayNormalTulevaPositio, maxDistance: 0.5f, hitInfo: out rr) == false)
            {
                Ray rayNormalTulevaPositio1 = new Ray(RayNormalOrigin.position + new Vector3(0f, 1f, 0f) + (transform.forward * 0.7f), -transform.up);
                RaycastHit rayNormalTulevaPositio1Hit;
                UnityEngine.Debug.DrawRay(rayNormalTulevaPositio1.origin, rayNormalTulevaPositio1.direction, color: Color.green, 333f);

                //Jos pisteestä raynormal origin +1 ylöspäin +0.7 eteenpäin ray alaspäin +1 osuu, niin siitä tulee tulevapaikka
                if (Physics.Raycast(rayNormalTulevaPositio1, maxDistance: Mathf.Infinity, hitInfo: out rayNormalTulevaPositio1Hit))
                {

                    Vector3 capsulenLähtöPaikka = (rayNormalTulevaPositio1Hit.point + (Vector3.up.normalized * 0.05f) - (transform.forward.normalized*0.7f));
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 tokanCapsulenPoint1 = capsulenLähtöPaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 tokanCapsulenPoint2 = capsulenLähtöPaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.95f;

                    //Tsekataan mahtuuko pelaajan collider capsulecastin alkamispisteeseen, joka on nykyisen paikan x ja z ja tulevan paikan y+0.05
                    if (!Physics.CheckCapsule(tokanCapsulenPoint1, tokanCapsulenPoint2, radius))
                    {
                        //Tsekataan mahtuuko pelaajan collider liikkumaan nykyisen paikan x ja z koordinaateista ja tulevan paikan y koordinaatista tulevaan paikkaan
                        if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, transform.forward, maxDistance: 0.70f))
                        {
                            transform.position = rayNormalTulevaPositio1Hit.point;
                            tulevaPaikka2 = transform.position;
                            roiku = false;
                        }
                    }
                }
            }

            //Kiipeys roikkumisesta
            if (transform.position == tulevaPaikka2)
            {
                liiku = false;
                tulevaPaikka2 = transform.position;
                kesken = false;
                valmis = true;
                kiipeys = false;
                kiipeilyVasemmalle = false;
                kiipeilyOikealle = false;
                roikuKielekkeellä = false;
                roikkuminenHypystä = false;
                eiVoiKiivetäVasemmalle = false;
                eiVoiKiivetäOikealle = false;
                roikkumispaikka = Vector3.zero;
                roikkumispaikkaY = 0f;

                cc.lockMovement = false;
                cc.keepDirection = false;
                cc.isStrafing = false;
                cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Collider>().isTrigger = false;

            }

            //kiipeys ilman roikkumista matalan esteen yli
            else if (kiipeys == true)
            {
                liiku = false;
                kiipeys = false;
                kesken = false;
                valmis = true;
                kiipeys = false;
                kiipeilyVasemmalle = false;
                kiipeilyOikealle = false;
                roikuKielekkeellä = false;
                roikkuminenHypystä = false;
                eiVoiKiivetäVasemmalle = false;
                eiVoiKiivetäOikealle = false;
                roikkumispaikka = Vector3.zero;
                roikkumispaikkaY = 0f;

                cc.lockMovement = false;
                cc.keepDirection = false;
                cc.isStrafing = false;
                cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Collider>().isTrigger = false;
            }
            //Jos ei pysty kiipeämään, koska seinä
            else
            {
                roiku = true;
            }
            kiipeysRoikkumisesta = false;
        }

        //tipuRoikkumisesta toteutus
        if (tipuRoikkumisesta)
        {
            liiku = false;
            kesken = false;
            valmis = true;
            kiipeys = false;
            kiipeilyVasemmalle = false;
            kiipeilyOikealle = false;
            roikuKielekkeellä = false;
            roikkuminenHypystä = false;
            eiVoiKiivetäVasemmalle = false;
            eiVoiKiivetäOikealle = false;
            roikkumispaikka = Vector3.zero;
            roikkumispaikkaY = 0f;

            cc.lockMovement = false;
            cc.keepDirection = false;
            cc.isStrafing = false;
            cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
            tipuRoikkumisesta = false;
        }

        //kiipeilyVasemmalle toteutus
        if (kiipeilyVasemmalle)
        {
            Vector3 capsulenLähtöpaikka;
            Vector3 point1;
            Vector3 point2;

            //Tsekataan onko pelaajan roikkumispositio sama kuin korjatturoikkumispositio ja onko pelaajan suunta seinän suuntaisesti
            if (transform.position == korjattuRoikkumisPaikka && transform.rotation == oikeaSuunta)
            {

                Ray rayNormal1 = new Ray(RayNormalOrigin.position + (-transform.right * 0.1f), transform.forward);
                Ray rayKorkeusCheck = new Ray(RayRoikkumisTasoKorkeusOrigin.position + (transform.forward.normalized * 0.6f) + (-transform.right * 0.1f), -transform.up);
                RaycastHit rayKorkeusCheckHit;
                Ray rayVasemmalle = new Ray(RayNormalOrigin.position, -transform.right);

                //UnityEngine.Debug.DrawRay(rayNormal1.origin, rayNormal1.direction, color: Color.red);
                UnityEngine.Debug.DrawRay(rayVasemmalle.origin, rayVasemmalle.direction.normalized * 0.7f, color: Color.yellow, 25f);
                UnityEngine.Debug.DrawRay(rayNormal1.origin, rayNormal1.direction.normalized * 0.6f, color: Color.black, 25f);

                Ray rayRoikkumistasoVasemmalle = new Ray(RayRoikkumisTasoKorkeusOrigin.position, -transform.right);
                UnityEngine.Debug.DrawRay(rayRoikkumistasoVasemmalle.origin, (rayRoikkumistasoVasemmalle.direction.normalized * 0.75f), color: Color.magenta, 33);

                capsulenLähtöpaikka = col.transform.position;
                float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                float radius = col.radius;
                float castDistance = 0.1f;

                //Tsekataan onko vasemmalla seinä, jos on niin käännytään sitä kohti ja jos osuu niin että roikkumistasanne ei muutu seinäksi
                if (Physics.Raycast(rayVasemmalle, maxDistance: 0.55f, hitInfo: out RaycastHitNormal1) && Physics.Raycast(rayRoikkumistasoVasemmalle, maxDistance: 0.75f) == false)
                {
                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;
                }

                //Tsekataan mahtuuko pelaajan collider vasemalle +0.1 yksikköä
                else if (!Physics.CapsuleCast(point1, point2, radius, -transform.right, castDistance))
                {

                    //Tsekataan onko seinää vasemmalle+0.1 yksikköä
                    if (Physics.Raycast(rayNormal1, maxDistance: 0.7f, hitInfo: out RaycastHitNormal1) && Physics.Raycast(rayKorkeusCheck, hitInfo: out rayKorkeusCheckHit) && rayKorkeusCheckHit.normal.y > 0.9f)
                    {

                        //Tsekataan onko seinä johon osui raycast saman suuntaisesti kuin pelaaja
                        if (-RaycastHitNormal1.normal == transform.forward)
                        {
                            transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;
                            korjattuRoikkumisPaikka += (-transform.right * 0.1f);
                            transform.position = korjattuRoikkumisPaikka;
                        }
                        //Jos seinä ei ole samansuuntaisesti kuin pelaaja
                        else
                        {
                            Ray raySuunnanVaihto = new Ray(RayNormalOrigin.position + (-transform.right * 0.3f) + (transform.forward * 0.7f), transform.right);
                            UnityEngine.Debug.DrawRay(raySuunnanVaihto.origin, raySuunnanVaihto.direction, color: Color.blue, 25f);
 
                            capsulenLähtöpaikka = col.transform.position + (-transform.right * 0.4f);
                            point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                            point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                            //Tsekataan kääntyykö seinä 
                            if (Physics.Raycast(raySuunnanVaihto, maxDistance: 0.6f, hitInfo: out rayCastHitSuunnanVaihto) == true)
                            {
                                castDistance = rayCastHitSuunnanVaihto.distance - 0.01f;

                                //Tsekataan mahtuuko pelaajan collider liikkumaan vasemmalta +0.4yksikköä uudelle seinälle
                                if (Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance) == false)
                                {
                                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-rayCastHitSuunnanVaihto.normal.x, 0f, -rayCastHitSuunnanVaihto.normal.z)) * transform.rotation;
                                    korjattuRoikkumisPaikka = rayCastHitSuunnanVaihto.point - (transform.forward / 2);
                                    korjattuRoikkumisPaikka.y = roikkumispaikka.y;
                                    transform.position = korjattuRoikkumisPaikka;
                                }
                            }
                            //Jos seinä ei ole samansuuntaisesti kuin pelaaja eikä seinä käänny tarpeeksi
                            else
                            {
                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;
                                korjattuRoikkumisPaikka = RaycastHitNormal1.point - (transform.forward * 0.5f);
                                korjattuRoikkumisPaikka.y = roikkumispaikka.y;
                                transform.position = korjattuRoikkumisPaikka;
                            }
                        }
                    }

                    // Jos ei ole seinää vasemmalle +0.1 yksikköä
                    else
                    {
                        capsulenLähtöpaikka = col.transform.position + (-transform.right * 0.5f);
                        castDistance = 0.7f;
                        point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        //Tsekataan mahtuuko pelaajan collider liikkumaan vasemmalta +0.5yksikköä eteenpäin 0.7 yksikköä
                        if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                        {
                            Ray raySuunnanVaihto = new Ray(RayNormalOrigin.position + (-transform.right * 0.3f) + (transform.forward * 0.7f), transform.right);
                            UnityEngine.Debug.DrawRay(raySuunnanVaihto.origin, raySuunnanVaihto.direction, color: Color.blue, 25f);

                            //Tsekataan kääntyykö seinä
                            if (Physics.Raycast(raySuunnanVaihto, maxDistance: 0.6f, hitInfo: out rayCastHitSuunnanVaihto))
                            {
                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-rayCastHitSuunnanVaihto.normal.x, 0f, -rayCastHitSuunnanVaihto.normal.z)) * transform.rotation;
                                korjattuRoikkumisPaikka = rayCastHitSuunnanVaihto.point - (transform.forward / 2);
                                korjattuRoikkumisPaikka.y = roikkumispaikka.y;
                                transform.position = korjattuRoikkumisPaikka;

                            }
                        }
                    }
                }
                kiipeilyVasemmalle = false;
                eiVoiKiivetäOikealle = false;
                oikeaSuunta = transform.rotation;
            }
            //Jos pelaajan roikkumispositio ei ole sama kuin korjatturoikkumispositio tai pelaajan suunta ei ole seinän suuntaisesti
            else
            {
                kiipeilyVasemmalle = false;
            }
        }
        //kiipeilyOikealle toteutus
        if (kiipeilyOikealle)
        {
            Vector3 capsulenLähtöpaikka;
            Vector3 point1;
            Vector3 point2;

            if (transform.position == korjattuRoikkumisPaikka && transform.rotation == oikeaSuunta)
            {
                Ray rayNormal1 = new Ray(RayNormalOrigin.position + (transform.right * 0.1f), transform.forward);
                Ray rayKorkeusCheck = new Ray(RayRoikkumisTasoKorkeusOrigin.position + (transform.forward.normalized * 0.6f) + (transform.right * 0.1f), -transform.up);
                RaycastHit rayKorkeusCheckHit;
                Ray rayOikealle = new Ray(RayNormalOrigin.position, transform.right);
                Ray rayRoikkumistasoOikealle = new Ray(RayRoikkumisTasoKorkeusOrigin.position, transform.right);
                //UnityEngine.Debug.DrawRay(rayNormal1.origin, rayNormal1.direction, color: Color.red);
                //UnityEngine.Debug.DrawRay(rayOikealle.origin, rayOikealle.direction.normalized * 0.7f, color: Color.yellow, 25f);
                //UnityEngine.Debug.DrawRay(rayNormal1.origin, rayNormal1.direction.normalized * 0.6f, color: Color.black, 25f);
                //UnityEngine.Debug.DrawRay(rayRoikkumistasoOikealle.origin, (rayRoikkumistasoOikealle.direction.normalized * 0.75f), color: Color.magenta, 33);
                UnityEngine.Debug.DrawRay(rayKorkeusCheck.origin, rayKorkeusCheck.direction.normalized * 0.2f, color: Color.yellow, 25f);


                capsulenLähtöpaikka = col.transform.position;
                float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                float radius = col.radius;
                float castDistance = 0.1f;

                //Tsekataan onko oikealla seinä, jos on niin käännytään sitä kohti ja jos osuu niin että roikkumistasanne ei muutu seinäksi
                if (Physics.Raycast(rayOikealle, maxDistance: 0.55f, hitInfo: out RaycastHitNormal1) && Physics.Raycast(rayRoikkumistasoOikealle, maxDistance: 0.75f) == false)
                {
                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;
                }

                //Tsekataan mahtuuko pelaajan collider liikkumaan oikealle +0.1 yksikköä
                else if (!Physics.CapsuleCast(point1, point2, radius, transform.right, castDistance))
                {
                    //Tsekataan onko seinää oikealle +0.1 yksikköä
                    if (Physics.Raycast(rayNormal1, maxDistance: 0.7f, hitInfo: out RaycastHitNormal1)&& Physics.Raycast(rayKorkeusCheck, maxDistance: 0.2f, hitInfo: out rayKorkeusCheckHit)&&rayKorkeusCheckHit.normal.y>0.9f)
                    {
                        //Tsekataan onko seinä johon osui raycast saman suuntaisesti kuin pelaaja
                        if (-RaycastHitNormal1.normal == transform.forward)
                        {
                            transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;
                            korjattuRoikkumisPaikka += (transform.right * 0.1f);
                            transform.position = korjattuRoikkumisPaikka;
                        }

                        //Jos seinä ei ole samansuuntaisesti kuin pelaaja
                        else
                        {
                            Ray raySuunnanVaihto = new Ray(RayNormalOrigin.position + (transform.right * 0.3f) + (transform.forward * 0.7f), -transform.right);
                            UnityEngine.Debug.DrawRay(raySuunnanVaihto.origin, raySuunnanVaihto.direction, color: Color.blue, 25f);

                            capsulenLähtöpaikka = col.transform.position + (transform.right * 0.4f);
                            point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                            point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                            //Tsekataan kääntyykö seinä 
                            if (Physics.Raycast(raySuunnanVaihto, maxDistance: 0.6f, hitInfo: out rayCastHitSuunnanVaihto))
                            {
                                castDistance = rayCastHitSuunnanVaihto.distance - 0.01f;

                                //Tsekataan mahtuuko pelaajan collider liikkumaan oikealta +0.4yksikköä uudelle seinälle
                                if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                                {
                                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-rayCastHitSuunnanVaihto.normal.x, 0f, -rayCastHitSuunnanVaihto.normal.z)) * transform.rotation;
                                    korjattuRoikkumisPaikka = rayCastHitSuunnanVaihto.point - (transform.forward / 2);
                                    korjattuRoikkumisPaikka.y = roikkumispaikka.y;
                                    transform.position = korjattuRoikkumisPaikka;
                                }
                            }

                            //Jos seinä ei ole samansuuntaisesti kuin pelaaja eikä seinä käänny tarpeeksi
                            else
                            {
                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;
                                korjattuRoikkumisPaikka = RaycastHitNormal1.point - (transform.forward * 0.5f);
                                korjattuRoikkumisPaikka.y = roikkumispaikka.y;
                                transform.position = korjattuRoikkumisPaikka;
                            }
                        }
                    }

                    // Jos ei ole seinää oikealle +0.1 yksikköä
                    else
                    {
                        capsulenLähtöpaikka = col.transform.position + (transform.right * 0.5f);
                        castDistance = 0.7f;
                        point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        //Tsekataan mahtuuko pelaajan collider liikkumaan oikealta +0.5yksikköä eteenpäin 0.7 yksikköä
                        if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                        {
                            Ray raySuunnanVaihto = new Ray(RayNormalOrigin.position + (transform.right * 0.3f) + (transform.forward * 0.7f), -transform.right);
                            UnityEngine.Debug.DrawRay(raySuunnanVaihto.origin, raySuunnanVaihto.direction, color: Color.blue, 25f);

                            //Tsekataan kääntyykö seinä
                            if (Physics.Raycast(raySuunnanVaihto, maxDistance: 0.6f, hitInfo: out rayCastHitSuunnanVaihto))
                            {
                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-rayCastHitSuunnanVaihto.normal.x, 0f, -rayCastHitSuunnanVaihto.normal.z)) * transform.rotation;
                                korjattuRoikkumisPaikka = rayCastHitSuunnanVaihto.point - (transform.forward / 2);
                                korjattuRoikkumisPaikka.y = roikkumispaikka.y;
                                transform.position = korjattuRoikkumisPaikka;

                            }
                        }

                    }
                }
                kiipeilyOikealle = false;
                eiVoiKiivetäVasemmalle = false;
                oikeaSuunta = transform.rotation;
            }

            //Jos pelaajan roikkumispositio ei ole sama kuin korjatturoikkumispositio tai pelaajan suunta ei ole seinän suuntaisesti
            else
            {
                kiipeilyOikealle = false;
            }
        }
    }


    //VaultingUusin() ja KiipeilyKielekkeelle() vois siistiä ja muuttaa vähän fiksummaks


    //Kiipeily suoraa seinää pitkin eli kiipeily seinää pitkin, joka on lattiasta ylöspäin
    //Kiipeily tapahtuu periaatteella: jos aiempi ray osuu ja nykyinen ray ei osu
    public void VaultingUusin()
    {
        bool ray0OsuiBool = false;
        bool ray0YlösOsuiBool = false;

        bool ray1OsuiBool = true;
        bool ray1AlasOsuiBool = false;

        bool ray2OsuiBool = true;
        bool ray2AlasOsuiBool = false;

        bool ray3OsuiBool = true;
        bool ray3AlasOsuiBool = false;

        bool ray4OsuiBool = true;
        bool ray4AlasOsuiBool = false;

        bool ray5OsuiBool = true;
        bool ray5AlasOsuiBool = false;

        bool ray1CapsuleCastOnnaa = true;
        bool ray2CapsuleCastOnnaa = true;
        bool ray3CapsuleCastOnnaa = true;
        bool ray4CapsuleCastOnnaa = true;
        bool ray5CapsuleCastOnnaa = true;

        Ray ray0 = new Ray(Ray0Origin.position, transform.forward);
        Ray ray0Ylös = new Ray(ray0.origin + (ray0.direction * 0.55f), Vector3.up);
        RaycastHit ray0YlosHit;

        Ray ray1 = new Ray(Ray1Origin.position, transform.forward);
        Ray ray1Alas = new Ray(ray1.origin + (ray1.direction * 0.55f), -Vector3.up);
        RaycastHit ray1AlasHit;

        Ray ray2 = new Ray(Ray2Origin.position, transform.forward);
        Ray ray2Alas = new Ray(ray2.origin + (ray2.direction * 0.55f), -Vector3.up);
        RaycastHit ray2AlasHit;

        Ray ray3 = new Ray(Ray3Origin.position, transform.forward);
        Ray ray3Alas = new Ray(ray3.origin + (ray3.direction * 0.55f), -Vector3.up);
        RaycastHit ray3AlasHit;

        Ray ray4 = new Ray(Ray4Origin.position, transform.forward);
        Ray ray4Alas = new Ray(ray4.origin + (ray4.direction * 0.55f), -Vector3.up);
        RaycastHit ray4AlasHit;

        Ray ray5 = new Ray(Ray5Origin.position, transform.forward);
        Ray ray5Alas = new Ray(ray5.origin + (ray5.direction * 0.55f), -Vector3.up);
        RaycastHit ray5AlasHit;

        RaycastHit hitSuoralleSeinälleRay0;
        RaycastHit hitSuoralleSeinälleRay1;
        RaycastHit hitSuoralleSeinälleRay2;
        RaycastHit hitSuoralleSeinälleRay3;
        RaycastHit hitSuoralleSeinälleRay4;
        RaycastHit hitSuoralleSeinälleRay5;
        RaycastHit hitTasanteenKulmanCheck;

        //alimmainen ray
        if (Physics.Raycast(ray0, maxDistance: 0.55f, hitInfo: out hitSuoralleSeinälleRay0) == true)
        {
            ray0OsuiBool = true;
        }

        if (ray0OsuiBool == true)
        {
            //toka ray. Tällä kiivetää suoraan ilman roikkumista
            if (Physics.Raycast(ray1, maxDistance: 0.55f, hitInfo: out hitSuoralleSeinälleRay1) == false)
            {
                ray1OsuiBool = false;

                //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray1AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                Vector3 capsulenLähtöpaikka = ray1.origin;
                float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                float radius = col.radius * 0.95f;
                float castDistance = 0.5f;

                if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                {
                    Ray tulevaPaikkaRay = new Ray(ray1.origin + (ray1.direction / 2), Vector3.down);
                    RaycastHit tulevapaikkaRaycastHit;

                    Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                    Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    if (Physics.Raycast(ray1.origin + (ray1.direction * hitSuoralleSeinälleRay0.distance) + (ray1.direction * 0.1f), Vector3.down, maxDistance: 1f, hitInfo: out hitTasanteenKulmanCheck))
                    {
                        if (hitTasanteenKulmanCheck.normal.y>=0.9f)
                        {
                            if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                            {
                                kesken = true;
                                valmis = false;
                                tulevaPaikka1 = ray1.origin + ray1.direction * 0.5f;
                                tulevaPaikka2 = tulevapaikkaRaycastHit.point;
                                kiipeys = true;
                                liiku = true;

                            }
                            else
                            {
                                ray1CapsuleCastOnnaa = false;
                            }
                        }
                    }
                }
            }

            //kolmas ray. Tällä kiivetää suoraan ilman roikkumista
            if (Physics.Raycast(ray2, maxDistance: 0.55f, hitInfo: out hitSuoralleSeinälleRay2) == false && ray2PoisToiminnasta == false)
            {
                ray2OsuiBool = false;
                if (ray1OsuiBool == true || ray1CapsuleCastOnnaa == false)
                {
                    //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray2AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                    Vector3 capsulenLähtöpaikka = ray2.origin;
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.8f;
                    float castDistance = 0.5f;

                    if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                    {
                        Ray tulevaPaikkaRay = new Ray(ray2.origin + (ray2.direction / 2), Vector3.down);
                        RaycastHit tulevapaikkaRaycastHit;
                        RaycastHit tokanCapsulenHitti;

                        Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                        Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        if (Physics.Raycast(ray2.origin + (ray2.direction * hitSuoralleSeinälleRay1.distance) + (ray2.direction * 0.1f), Vector3.down, maxDistance: 1f, hitInfo: out hitTasanteenKulmanCheck))
                        {
                            if (hitTasanteenKulmanCheck.normal.y >= 0.9f)
                            {

                                //omituisesti capsulecast törmää ennen kuin raycast joten piti laittaa tulevapaikkaRaycastHit.distance - 0.01->tulevapaikkaRaycastHit.distance - 0.4f
                                if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, Vector3.down, out tokanCapsulenHitti, tulevapaikkaRaycastHit.distance - 0.4f))
                                {
                                    kesken = true;
                                    valmis = false;
                                    tulevaPaikka1 = ray2.origin + ray2.direction * 0.5f;
                                    tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                                    kiipeys = true;
                                    liiku = true;
                                }
                                else
                                {
                                    ray2CapsuleCastOnnaa = false;
                                }
                            }
                        }
                    }
                }
            }

            //neljäs ray. Tällä kiivetään roikkumispaikkaan
            if (Physics.Raycast(ray3, maxDistance: 0.55f, hitInfo: out hitSuoralleSeinälleRay3) == false && ray3PoisToiminnasta == false)
            {
                ray3OsuiBool = false;
                if (ray2OsuiBool == true || ray2CapsuleCastOnnaa == false)
                {
                    Ray tulevaPaikkaRay = new Ray(ray3.origin + ((ray3.direction * hitSuoralleSeinälleRay2.distance) + (ray3.direction * 0.1f)), Vector3.down);
                    RaycastHit tulevapaikkaRaycastHit;

                    if (Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit))
                    {
                        if (tulevapaikkaRaycastHit.normal.y >= 0.9f)
                        {


                            kesken = true;
                            valmis = false;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;
                            kiipeys = false;
                            roikkumispaikkaY = tulevapaikkaRaycastHit.point.y;
                            roiku = true;
                        }
                    }
                }
            }

            //viides ray. Tällä kiivetään roikkumispaikkaan
            if (Physics.Raycast(ray4, maxDistance: 0.55f, hitInfo: out hitSuoralleSeinälleRay4) == false && ray4PoisToiminnasta == false)
            {
                ray4OsuiBool = false;
                if ((ray3OsuiBool == true))
                {
                    //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray3AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon

                    Ray tulevaPaikkaRay = new Ray(ray4.origin + ((ray4.direction * hitSuoralleSeinälleRay3.distance) + (ray4.direction * 0.1f)), Vector3.down);
                    RaycastHit tulevapaikkaRaycastHit;

                    if (Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit))
                    {
                        if (tulevapaikkaRaycastHit.normal.y >= 0.9f)
                        {
                            kesken = true;
                            valmis = false;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;
                            kiipeys = false;
                            roikkumispaikkaY = tulevapaikkaRaycastHit.point.y;
                            roiku = true;
                        }

                    }
                }
            }

            //kuudes ray. Tällä kiivetään roikkumispaikkaan
            if (Physics.Raycast(ray5, maxDistance: 0.55f, hitInfo: out hitSuoralleSeinälleRay5) == false && ray5PoisToiminnasta == false)
            {
                ray5OsuiBool = false;

                if (ray4OsuiBool == true)
                {
                    //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray3AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon

                    Ray tulevaPaikkaRay = new Ray(ray5.origin + ((ray5.direction * hitSuoralleSeinälleRay4.distance) + (ray5.direction * 0.1f)), Vector3.down);
                    RaycastHit tulevapaikkaRaycastHit;

                    if (Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit))
                    {
                        if (tulevapaikkaRaycastHit.normal.y >= 0.9f)
                        {
                            kesken = true;
                            valmis = false;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;
                            kiipeys = false;
                            roikkumispaikkaY = tulevapaikkaRaycastHit.point.y;
                            roiku = true;
                        }
                    }
                }
            }

            // jos ei löydy kiipeilypaikkaa, kun kaikki rayt osuu
            if (kiipeys == false && roiku == false)
            {
                cc.lockMovement = false;
                cc.keepDirection = false;
                gameObject.GetComponent<Collider>().isTrigger = false;
                rb.isKinematic = false;
                cc.keepDirection = false;
                cc.isStrafing = false;
                cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
                UnityEngine.Debug.Log("Liian korkea. Nyt ei pysty.");
            }
        }

        // jos ei löydy kiipeilypaikkaa, kun alin ray ei osu
        else
        {
            cc.lockMovement = false;
            cc.keepDirection = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
            rb.isKinematic = false;
            cc.keepDirection = false;
            cc.isStrafing = false;
            cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
        }

        // katto tarkistuksen tulokset pois käytöstä
        ray5PoisToiminnasta = false;
        ray4PoisToiminnasta = false;
        ray3PoisToiminnasta = false;
        ray2PoisToiminnasta = false;
        ray1PoisToiminnasta = false;
        ray0PoisToiminnasta = false;

    }

    //kiipeily kielekkeelle jonka alapuolella on tyhjää
    //Kiipeily tapahtuu periaatteella: jos eka ray ylös osuu ja aiempi ray osuu ja nykyinen ray ei osu
    //tai jos eka ray ylöspäin osuu ja mikään muu ray ei osu niin sitten ray ylhäältä alas ja jos sen osumakohta on korkeemmalla kuin eka ray ylös osumakohta
    public void KiipeilyKielekkeelle()
    {
        bool ray0OsuiBool = false;
        bool ray0YlösOsuiBool = false;

        bool ray1OsuiBool = true;
        bool ray1AlasOsuiBool = false;

        bool ray2OsuiBool = true;
        bool ray2AlasOsuiBool = false;

        bool ray3OsuiBool = true;
        bool ray3AlasOsuiBool = false;

        bool ray4OsuiBool = true;
        bool ray4AlasOsuiBool = false;

        bool ray5OsuiBool = true;
        bool ray5AlasOsuiBool = false;

        bool ray1CapsuleCastOnnaa = true;
        bool ray2CapsuleCastOnnaa = true;
        bool ray3CapsuleCastOnnaa = true;
        bool ray4CapsuleCastOnnaa = true;
        bool ray5CapsuleCastOnnaa = true;

        Ray ray0 = new Ray(Ray0Origin.position, transform.forward);
        Ray ray0Ylös = new Ray(ray0.origin + (ray0.direction * 0.5f), Vector3.up);
        RaycastHit ray0YlosHit;

        Ray ray1 = new Ray(Ray1Origin.position, transform.forward);
        Ray ray1Alas = new Ray(ray1.origin + (ray1.direction * 0.55f), -Vector3.up);
        RaycastHit ray1AlasHit;

        Ray ray2 = new Ray(Ray2Origin.position, transform.forward);
        Ray ray2Alas = new Ray(ray2.origin + (ray2.direction * 0.55f), -Vector3.up);
        RaycastHit ray2AlasHit;

        Ray ray3 = new Ray(Ray3Origin.position, transform.forward);
        Ray ray3Alas = new Ray(ray3.origin + (ray3.direction * 0.55f), -Vector3.up);
        RaycastHit ray3AlasHit;

        Ray ray4 = new Ray(Ray4Origin.position, transform.forward);
        Ray ray4Alas = new Ray(ray4.origin + (ray4.direction * 0.55f), -Vector3.up);
        RaycastHit ray4AlasHit;

        Ray ray5 = new Ray(Ray5Origin.position, transform.forward);
        Ray ray5Alas = new Ray(ray5.origin + (ray5.direction * 0.55f), -Vector3.up);
        RaycastHit ray5AlasHit;

        RaycastHit tulevapaikkaRaycastHit;
        RaycastHit capsuleCheckHit;

        UnityEngine.Debug.DrawRay(ray0Ylös.origin, ray0Ylös.direction.normalized * 2.84f, color: Color.yellow, 33);

        // eka ray eteenpäin. Jos ei osu, niin ray ylöspäin jonka pitää osua
        if (Physics.Raycast(ray0, maxDistance: 0.5f, hitInfo: out hitKielekkeelleRay0) == false && Physics.Raycast(ray: ray0Ylös, maxDistance: 2.84f, hitInfo: out ray0YlosHit) == true)
        {
            if (ray0YlosHit.collider.tag != "Kasvis" && ray0YlosHit.collider.tag != "Enemy" && ray0YlosHit.collider.tag != "Miekka" &&
                        ray0YlosHit.collider.tag != "Lava" && ray0YlosHit.collider.tag != "Moukari")
            {
                ray0YlösOsuiBool = true;
                ray0JosEiOsuHitPiste = ray0YlosHit.point;

                Ray taakseRay = new Ray(ray0JosEiOsuHitPiste - (ray0.direction * 0.6f) + new Vector3(0f, 0.05f, 0f), ray0.direction);

                Physics.Raycast(taakseRay, maxDistance: 1f, hitInfo: out taakseRayHit);
            }
        }
        if (ray0YlösOsuiBool == true)
        {
            //toka ray. Tällä kiivetää suoraan ilman roikkumista
            if (Physics.Raycast(ray1, maxDistance: 0.55f, hitInfo: out hitKielekkeelleRay1) == false)
            {
                ray1OsuiBool = false;
                Physics.Raycast(ray: ray1Alas, maxDistance: Mathf.Infinity, hitInfo: out ray1AlasHit);

                if (ray1AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                {
                    ray1AlasOsuiBool = true;

                    //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray1AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                    Vector3 capsulenLähtöpaikka = ray1.origin;
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.95f;
                    float castDistance = 0.5f;

                    if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                    {
                        Ray tulevaPaikkaRay = new Ray(ray1.origin + (ray1.direction / 2), Vector3.down);

                        if (Physics.Raycast(ray1.origin + (ray1.direction * taakseRayHit.distance) + (ray1.direction * 0.1f), Vector3.down, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit)&&
                            Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out capsuleCheckHit))
                        {
                            if (tulevapaikkaRaycastHit.normal.y >= 0.9f)
                            {
                                Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                                Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                                if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, capsuleCheckHit.distance - 0.01f))
                                {
                                    kesken = true;
                                    valmis = false;
                                    tulevaPaikka1 = ray1.origin + ray1.direction * 0.5f;
                                    tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                                    kiipeys = true;
                                    liiku = true;
                                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-hitKielekkeelleRay0.normal.x, 0f, -hitKielekkeelleRay0.normal.z)) * transform.rotation;
                                }
                                else
                                {
                                    ray1CapsuleCastOnnaa = false;
                                }
                            }
                        }
                    }
                }
            }

            //kolmas ray. Tällä kiivetää suoraan ilman roikkumista
            if (Physics.Raycast(ray2, maxDistance: 0.55f, hitInfo: out hitKielekkeelleRay2) == false && ray2PoisToiminnasta == false)
            {
                ray2OsuiBool = false;

                if (ray1OsuiBool == true|| ray1AlasOsuiBool==false)
                {
                    Physics.Raycast(ray: ray2Alas, maxDistance: Mathf.Infinity, hitInfo: out ray2AlasHit);

                    if (ray2AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                    {
                        ray2AlasOsuiBool = true;

                        Vector3 capsulenLähtöpaikka = ray2.origin;
                        float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                        Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        float radius = col.radius * 0.95f;
                        float castDistance = 0.5f;

                        if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                        {
                            Ray tulevaPaikkaRay = new Ray(ray2.origin + (ray2.direction / 2), Vector3.down);

                            if (Physics.Raycast(ray2.origin + (ray2.direction * taakseRayHit.distance) + (ray2.direction * 0.1f), Vector3.down, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit)&&
                                Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out capsuleCheckHit))
                            {
                                if (tulevapaikkaRaycastHit.normal.y >= 0.9f)
                                {
                                    Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                                    Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                                    if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, capsuleCheckHit.distance - 0.01f))
                                    {
                                        kesken = true;
                                        valmis = false;
                                        tulevaPaikka1 = ray2.origin + ray2.direction * 0.5f;
                                        tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                                        kiipeys = true;
                                        liiku = true;
                                        transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-hitKielekkeelleRay1.normal.x, 0f, -hitKielekkeelleRay1.normal.z)) * transform.rotation;
                                    }
                                    else
                                    {
                                        ray2CapsuleCastOnnaa = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //neljäs raý. Tällä kiivetään roikkumispaikkaan
            if (Physics.Raycast(ray3, maxDistance: 0.55f, hitInfo: out hitKielekkeelleRay3) == false && ray3PoisToiminnasta == false)
            {
                ray3OsuiBool = false;

                if (ray2OsuiBool == true)
                {
                    Physics.Raycast(ray: ray3Alas, maxDistance: Mathf.Infinity, hitInfo: out ray3AlasHit);

                    if (ray3AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                    {
                        ray3AlasOsuiBool = true;

                        Ray tulevaPaikkaRay = new Ray(ray3.origin + ((ray3.direction * taakseRayHit.distance) + (ray3.direction * 0.1f)), Vector3.down);
                        

                        if (Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit))
                        {
                            if (tulevapaikkaRaycastHit.point.y - 0.01f > ray0Ylös.origin.y && tulevapaikkaRaycastHit.normal.y >= 0.9f)
                            {
                                kesken = true;
                                valmis = false;
                                tulevaPaikka2 = tulevapaikkaRaycastHit.point;
                                kiipeys = false;
                                roikkumispaikkaY = tulevapaikkaRaycastHit.point.y;
                                roiku = true;
                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-hitKielekkeelleRay2.normal.x, 0f, -hitKielekkeelleRay2.normal.z)) * transform.rotation;
                                roikuKielekkeellä = true;
                                kielekkeenPaikka = hitKielekkeelleRay2.point;
                            }

                        }
                    }
                }
            }

            //viides ray. Tällä kiivetään roikkumispaikkaan
            if (Physics.Raycast(ray4, maxDistance: 0.55f, hitInfo: out hitKielekkeelleRay4) == false && ray4PoisToiminnasta == false)
            {
                ray4OsuiBool = false;

                if (ray3OsuiBool == true)
                {
                    Physics.Raycast(ray: ray4Alas, maxDistance: Mathf.Infinity, hitInfo: out ray4AlasHit);

                    if (ray4AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                    {
                        ray4AlasOsuiBool = true;

                        Ray tulevaPaikkaRay = new Ray(ray4.origin + ((ray4.direction * taakseRayHit.distance) + (ray4.direction * 0.1f)), Vector3.down);

                        if (Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit))
                        {
                            if (tulevapaikkaRaycastHit.point.y - 0.01f > ray0Ylös.origin.y&& tulevapaikkaRaycastHit.normal.y >= 0.9f)
                            {

                                kesken = true;
                                valmis = false;
                                tulevaPaikka2 = tulevapaikkaRaycastHit.point;
                                kiipeys = false;
                                roikkumispaikkaY = tulevapaikkaRaycastHit.point.y;
                                roiku = true;
                                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-hitKielekkeelleRay3.normal.x, 0f, -hitKielekkeelleRay3.normal.z)) * transform.rotation;
                                roikuKielekkeellä = true;
                                kielekkeenPaikka = hitKielekkeelleRay3.point;
                            }

                        }
                    }
                }
            }

            //kuudes ray. Tällä kiivetään roikkumispaikkaan
            if (Physics.Raycast(ray5, maxDistance: 0.55f, hitInfo: out hitKielekkeelleRay5) == false && ray5PoisToiminnasta == false&&!kiipeys&&!roiku)
            {
                ray5OsuiBool = false;


                    Ray tulevaPaikkaRay = new Ray(ray5.origin + ((ray5.direction * taakseRayHit.distance) + (ray5.direction * 0.1f)), Vector3.down);

                    if (Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit))
                    {
                        if (tulevapaikkaRaycastHit.point.y - 0.01f > ray0Ylös.origin.y && tulevapaikkaRaycastHit.normal.y >= 0.9f)
                        {
                            kesken = true;
                            valmis = false;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;
                            kiipeys = false;
                            roikkumispaikkaY = tulevapaikkaRaycastHit.point.y;
                            roiku = true;
                            transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-taakseRayHit.normal.x, 0f, -taakseRayHit.normal.z)) * transform.rotation;
                            roikuKielekkeellä = true;
                            kielekkeenPaikka = taakseRayHit.point;
                        }
                    }

            }

            //jos ei löydy kiipeilypaikkaa, kun ei löydy koloa tai tasannetta
            if (kiipeys == false && roiku == false)
            {
                cc.lockMovement = false;
                cc.keepDirection = false;
                gameObject.GetComponent<Collider>().isTrigger = false;
                rb.isKinematic = false;
                cc.keepDirection = false;
                cc.isStrafing = false;
                cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
                UnityEngine.Debug.Log("Liian korkea. Nyt ei pysty.");

            }
        }

        //jos eka ray osuu tai jos eka ray ylös ei osu
        else
        {
            cc.lockMovement = false;
            cc.keepDirection = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
            rb.isKinematic = false;
            cc.keepDirection = false;
            cc.isStrafing = false;
            cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
        }

        // katto tarkistuksen tulokset pois käytöstä
        oikeaSuunta = transform.rotation;
        ray5PoisToiminnasta = false;
        ray4PoisToiminnasta = false;
        ray3PoisToiminnasta = false;
        ray2PoisToiminnasta = false;
        ray1PoisToiminnasta = false;
        ray0PoisToiminnasta = false;
    }




    //Nämä ei ole käytössä
    public void Vaulting()
    {
        Ray ray0 = new Ray(Ray0Origin.position, transform.forward);

        if (Physics.Raycast(ray0, 0.55f) == true)
        {
            Ray ray1 = new Ray(Ray1Origin.position, transform.forward);

            if (Physics.Raycast(ray1, 0.55f) == true)
            {
                Ray ray2 = new Ray(Ray2Origin.position, transform.forward);

                if (Physics.Raycast(ray2, 0.55f) == true)
                {
                    Ray ray5 = new Ray(Ray5Origin.position, transform.forward);
                    orig = ray5.origin;
                    dir = ray5.direction;

                    if (Physics.Raycast(ray5, 0.55f) == false)
                    {

                        Vector3 capsulenLähtöpaikka = ray5.origin;
                        float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                        Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        float radius = col.radius * 0.95f;
                        float castDistance = 0.5f;


                        if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                        {

                            Ray tulevaPaikkaRay = new Ray(ray5.origin + (ray5.direction / 2), Vector3.down);

                            RaycastHit tulevapaikkaRaycastHit;

                            Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                            //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                            //{

                            Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                            Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                            if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                            {
                                kesken = true;
                                valmis = false;
                                tulevaPaikka1 = ray5.origin +/*new Vector3(0f,1f,0f)*/  ray5.direction * 0.5f;
                                tulevaPaikka2 = tulevapaikkaRaycastHit.point;


                                kiipeys = false;
                                //liiku = true;
                            }
                            //}
                        }

                    }
                    //else
                    //{
                    //    ylempiRay.origin = Vector3.zero;
                    //    ylempiRay.direction = Vector3.zero;

                    //}
                }
            }
        }
        else
        {

        }


    }
    public void VaultingParempi()
    {
        bool ray0OsuiBool = false;
        bool ray0YlösOsuiBool = false;

        bool ray1OsuiBool = true;
        bool ray1AlasOsuiBool = false;

        bool ray2OsuiBool = true;
        bool ray2AlasOsuiBool = false;

        bool ray3OsuiBool = true;
        bool ray3AlasOsuiBool = false;

        bool ray4OsuiBool = true;
        bool ray4AlasOsuiBool = false;

        bool ray5OsuiBool = true;
        bool ray5AlasOsuiBool = false;

        bool ray1CapsuleCastOnnaa = true;
        bool ray2CapsuleCastOnnaa = true;
        bool ray3CapsuleCastOnnaa = true;
        bool ray4CapsuleCastOnnaa = true;
        bool ray5CapsuleCastOnnaa = true;

        Ray ray0 = new Ray(Ray0Origin.position, transform.forward);
        Ray ray0Ylös = new Ray(ray0.origin + (ray0.direction * 0.55f), Vector3.up);
        RaycastHit ray0YlosHit;

        Ray ray1 = new Ray(Ray1Origin.position, transform.forward);
        Ray ray1Alas = new Ray(ray1.origin + (ray1.direction * 0.55f), -Vector3.up);
        RaycastHit ray1AlasHit;

        Ray ray2 = new Ray(Ray2Origin.position, transform.forward);
        Ray ray2Alas = new Ray(ray2.origin + (ray2.direction * 0.55f), -Vector3.up);
        RaycastHit ray2AlasHit;

        Ray ray3 = new Ray(Ray3Origin.position, transform.forward);
        Ray ray3Alas = new Ray(ray3.origin + (ray3.direction * 0.55f), -Vector3.up);
        RaycastHit ray3AlasHit;

        Ray ray4 = new Ray(Ray4Origin.position, transform.forward);
        Ray ray4Alas = new Ray(ray4.origin + (ray4.direction * 0.55f), -Vector3.up);
        RaycastHit ray4AlasHit;

        Ray ray5 = new Ray(Ray5Origin.position, transform.forward);
        Ray ray5Alas = new Ray(ray5.origin + (ray5.direction * 0.55f), -Vector3.up);
        RaycastHit ray5AlasHit;

        if (Physics.Raycast(ray0, 0.55f) == true)
        {
            ray0OsuiBool = true;

        }
        if (ray0OsuiBool == false && Physics.Raycast(ray: ray0Ylös, maxDistance: 2.84f, hitInfo: out ray0YlosHit) == true)
        {
            ray0YlösOsuiBool = true;
            Vector3 ray0JosEiOsuHitPiste = ray0YlosHit.point;
        }





        if (ray0OsuiBool == true)
        {
            if (Physics.Raycast(ray1, 0.55f) == false)
            {
                ray1OsuiBool = false;

                //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray1AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                Vector3 capsulenLähtöpaikka = ray1.origin;
                float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                float radius = col.radius * 0.95f;
                float castDistance = 0.5f;

                if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                {
                    Ray tulevaPaikkaRay = new Ray(ray1.origin + (ray1.direction / 2), Vector3.down);

                    RaycastHit tulevapaikkaRaycastHit;

                    Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                    //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                    //{

                    Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                    {
                        kesken = true;
                        valmis = false;
                        tulevaPaikka1 = ray1.origin +/*new Vector3(0f,1f,0f)*/  ray1.direction * 0.5f;
                        tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                        kiipeys = true;
                        liiku = true;

                    }
                    else
                    {
                        ray1CapsuleCastOnnaa = false;
                    }
                    //}
                }


            }
            if (Physics.Raycast(ray2, 0.55f) == false && (ray1OsuiBool == true || ray1CapsuleCastOnnaa == false))
            {
                ray2OsuiBool = false;

                //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray2AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                Vector3 capsulenLähtöpaikka = ray2.origin;
                float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                float radius = col.radius * 0.8f;
                float castDistance = 0.5f;


                if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                {
                    Ray tulevaPaikkaRay = new Ray(ray2.origin + (ray2.direction / 2), Vector3.down);


                    RaycastHit tulevapaikkaRaycastHit;
                    RaycastHit tokanCapsulenHitti;

                    Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);



                    //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                    //{

                    Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    //omituisesti capsulecast törmää ennen kuin raycast joten piti laittaa tulevapaikkaRaycastHit.distance - 0.01->tulevapaikkaRaycastHit.distance - 0.4f
                    if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, Vector3.down, out tokanCapsulenHitti, tulevapaikkaRaycastHit.distance - 0.4f))
                    {
                        kesken = true;
                        valmis = false;
                        tulevaPaikka1 = ray2.origin +/*new Vector3(0f,1f,0f)*/  ray2.direction * 0.5f;
                        tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                        kiipeys = true;
                        liiku = true;

                        //roikkumispaikka = tulevapaikkaRaycastHit.point - new Vector3(0f, 1f, 0f) - (ray2.direction / 2);
                        //roiku = true;
                        //if (roikkumispaikka.y < -0.5f)
                        //{
                        //    var stoppi = "tässä";
                        //}
                    }
                    else
                    {
                        ray2CapsuleCastOnnaa = false;
                    }
                    //}
                }
            }
            if (Physics.Raycast(ray3, 0.55f) == false && (ray2OsuiBool == true || ray2CapsuleCastOnnaa == false))
            {
                ray3OsuiBool = false;

                //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray3AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                Vector3 capsulenLähtöpaikka = ray3.origin;
                float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                float radius = col.radius * 0.95f;
                float castDistance = 0.5f;


                if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                {
                    Ray tulevaPaikkaRay = new Ray(ray3.origin + (ray3.direction / 2), Vector3.down);

                    RaycastHit tulevapaikkaRaycastHit;

                    Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);



                    //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                    //{

                    Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                    {

                        kesken = true;
                        valmis = false;
                        tulevaPaikka1 = ray3.origin +/*new Vector3(0f,1f,0f)*/  ray3.direction * 0.5f;
                        tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                        kiipeys = false;
                        //liiku = true;

                        roikkumispaikka = tulevapaikkaRaycastHit.point - new Vector3(0f, 1f, 0f) - (ray3.direction / 2);
                        roiku = true;
                        if (roikkumispaikka.y < -0.5f)
                        {
                            var stoppi = "tässä";
                        }
                    }
                    else
                    {
                        ray3CapsuleCastOnnaa = false;
                    }
                    //}
                }
            }
            if (Physics.Raycast(ray4, 0.55f) == false && (ray3OsuiBool == true || ray3CapsuleCastOnnaa == false))
            {
                ray4OsuiBool = false;

                //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray4AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                Vector3 capsulenLähtöpaikka = ray4.origin;
                float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                float radius = col.radius * 0.95f;
                float castDistance = 0.5f;


                if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                {
                    Ray tulevaPaikkaRay = new Ray(ray4.origin + (ray4.direction / 2), Vector3.down);

                    RaycastHit tulevapaikkaRaycastHit;

                    Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);



                    //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                    //{

                    Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                    {
                        kesken = true;
                        valmis = false;
                        tulevaPaikka1 = ray4.origin +/*new Vector3(0f,1f,0f)*/  ray4.direction * 0.5f;
                        tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                        kiipeys = false;
                        //liiku = true;

                        roikkumispaikka = tulevapaikkaRaycastHit.point - new Vector3(0f, 1f, 0f) - (ray4.direction / 2);
                        roiku = true;
                        if (roikkumispaikka.y < -0.5f)
                        {
                            var stoppi = "tässä";
                        }
                    }
                    else
                    {
                        ray4CapsuleCastOnnaa = false;
                    }
                    //}
                }
            }
            if (Physics.Raycast(ray5, 0.55f) == false && (ray4OsuiBool == true || ray4CapsuleCastOnnaa == false))
            {
                ray5OsuiBool = false;

                //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray5AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                Vector3 capsulenLähtöpaikka = ray5.origin;
                float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                float radius = col.radius * 0.95f;
                float castDistance = 0.5f;


                if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                {
                    Ray tulevaPaikkaRay = new Ray(ray5.origin + (ray5.direction / 2), Vector3.down);

                    RaycastHit tulevapaikkaRaycastHit;

                    Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);



                    //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                    //{

                    Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                    {
                        kesken = true;
                        valmis = false;
                        tulevaPaikka1 = ray5.origin +/*new Vector3(0f,1f,0f)*/  ray5.direction * 0.5f;
                        tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                        kiipeys = false;
                        //liiku = true;

                        roikkumispaikka = tulevapaikkaRaycastHit.point - new Vector3(0f, 1f, 0f) - (ray5.direction / 2);
                        roiku = true;
                        if (roikkumispaikka.y < -0.5f)
                        {
                            var stoppi = "tässä";
                        }
                    }
                    //}
                }
            }
            if (tulevaPaikka2 == Vector3.zero)
            {
                roiku = true;
            }
            else
            {
                UnityEngine.Debug.Log("Liian korkea. Nyt ei pysty.");
            }
        }

        else if (ray0YlösOsuiBool == true)
        {
            if (Physics.Raycast(ray1, 0.55f) == false)
            {
                ray1OsuiBool = false;
                Physics.Raycast(ray: ray1Alas, maxDistance: Mathf.Infinity, hitInfo: out ray1AlasHit);

                if (ray1AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                {
                    ray1AlasOsuiBool = true;

                    //Näissä kohdissa vois vaihtaa Capsulen tulemaan ray1AlasHit.point->pelaajaa kohti, jotta Capsulen pää ei virheellisesti kolise kattoon
                    Vector3 capsulenLähtöpaikka = ray1.origin;
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.95f;
                    float castDistance = 0.5f;


                    if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                    {
                        Ray tulevaPaikkaRay = new Ray(ray1.origin + (ray1.direction / 2), Vector3.down);

                        RaycastHit tulevapaikkaRaycastHit;

                        Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                        //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                        //{

                        Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                        {
                            kesken = true;
                            valmis = false;
                            tulevaPaikka1 = ray1.origin +/*new Vector3(0f,1f,0f)*/  ray1.direction * 0.5f;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                            kiipeys = true;
                            liiku = true;

                        }
                        else
                        {
                            ray1CapsuleCastOnnaa = false;
                        }
                        //}
                    }
                }


            }
            if (Physics.Raycast(ray2, 0.55f) == false && (ray1OsuiBool == true || ray1AlasOsuiBool == false || ray1CapsuleCastOnnaa == false))
            {
                ray2OsuiBool = false;
                Physics.Raycast(ray: ray2Alas, maxDistance: Mathf.Infinity, hitInfo: out ray2AlasHit);



                if (ray2AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                {
                    ray2AlasOsuiBool = true;

                    Vector3 capsulenLähtöpaikka = ray2.origin;
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.95f;
                    float castDistance = 0.5f;


                    if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                    {
                        Ray tulevaPaikkaRay = new Ray(ray2.origin + (ray2.direction / 2), Vector3.down);

                        RaycastHit tulevapaikkaRaycastHit;

                        Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                        //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                        //{

                        Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                        {
                            kesken = true;
                            valmis = false;
                            tulevaPaikka1 = ray2.origin +/*new Vector3(0f,1f,0f)*/  ray2.direction * 0.5f;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                            kiipeys = true;
                            liiku = true;

                            //roikkumispaikka = ray2AlasHit.point - new Vector3(0f, 1f, 0f) - (ray2.direction / 2);
                            //roiku = true;
                            //if (roikkumispaikka.y < -0.5f)
                            //{
                            //    var stoppi = "tässä";
                            //}
                        }
                        else
                        {
                            ray2CapsuleCastOnnaa = false;
                        }
                        //}
                    }
                }
            }
            if (Physics.Raycast(ray3, 0.55f) == false && (ray2OsuiBool == true || ray2AlasOsuiBool == false || ray2CapsuleCastOnnaa == false))
            {
                ray3OsuiBool = false;
                Physics.Raycast(ray: ray3Alas, maxDistance: Mathf.Infinity, hitInfo: out ray3AlasHit);


                if (ray3AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                {
                    ray3AlasOsuiBool = true;

                    Vector3 capsulenLähtöpaikka = ray3.origin;
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.95f;
                    float castDistance = 0.5f;


                    if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                    {
                        Ray tulevaPaikkaRay = new Ray(ray3.origin + (ray3.direction / 2), Vector3.down);

                        RaycastHit tulevapaikkaRaycastHit;

                        Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                        //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                        //{

                        Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                        {
                            kesken = true;
                            valmis = false;
                            tulevaPaikka1 = ray3.origin +/*new Vector3(0f,1f,0f)*/  ray3.direction * 0.5f;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                            kiipeys = false;
                            //liiku = true;

                            roikkumispaikka = ray3AlasHit.point - new Vector3(0f, 1f, 0f) - (ray3.direction / 2);
                            roiku = true;
                            if (roikkumispaikka.y < -0.5f)
                            {
                                var stoppi = "tässä";
                            }
                        }
                        else
                        {
                            ray3CapsuleCastOnnaa = false;
                        }
                        //}
                    }
                }
            }
            if (Physics.Raycast(ray4, 0.55f) == false && (ray3OsuiBool == true || ray3AlasOsuiBool == false || ray3CapsuleCastOnnaa == false))
            {
                ray4OsuiBool = false;

                Physics.Raycast(ray: ray4Alas, maxDistance: Mathf.Infinity, hitInfo: out ray4AlasHit);


                if (ray4AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                {
                    ray4AlasOsuiBool = true;

                    Vector3 capsulenLähtöpaikka = ray4.origin;
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.95f;
                    float castDistance = 0.5f;


                    if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                    {
                        Ray tulevaPaikkaRay = new Ray(ray4.origin + (ray4.direction / 2), Vector3.down);

                        RaycastHit tulevapaikkaRaycastHit;

                        Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                        //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                        //{

                        Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                        {
                            kesken = true;
                            valmis = false;
                            tulevaPaikka1 = ray4.origin +/*new Vector3(0f,1f,0f)*/  ray4.direction * 0.5f;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;

                            kiipeys = false;
                            //liiku = true;

                            roikkumispaikka = ray4AlasHit.point - new Vector3(0f, 1f, 0f) - (ray4.direction / 2);
                            roiku = true;
                            if (roikkumispaikka.y < -0.5f)
                            {
                                var stoppi = "tässä";
                            }
                        }
                        else
                        {
                            ray4CapsuleCastOnnaa = false;
                        }
                        //}
                    }
                }
            }
            if (Physics.Raycast(ray5, 0.55f) == false && (ray4OsuiBool == true || ray4AlasOsuiBool == false || ray4CapsuleCastOnnaa == false))
            {
                ray5OsuiBool = false;

                Physics.Raycast(ray: ray5Alas, maxDistance: Mathf.Infinity, hitInfo: out ray5AlasHit);


                if (ray5AlasHit.point.y - 0.01f > ray0Ylös.origin.y)
                {
                    ray5AlasOsuiBool = true;

                    Vector3 capsulenLähtöpaikka = ray5.origin;
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.95f;
                    float castDistance = 0.5f;


                    if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                    {
                        Ray tulevaPaikkaRay = new Ray(ray5.origin + (ray5.direction / 2), Vector3.down);

                        RaycastHit tulevapaikkaRaycastHit;

                        Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                        //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                        //{

                        Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                        {
                            kesken = true;
                            valmis = false;
                            tulevaPaikka1 = ray5.origin +/*new Vector3(0f,1f,0f)*/  ray5.direction * 0.5f;
                            tulevaPaikka2 = tulevapaikkaRaycastHit.point;



                            kiipeys = false;
                            //liiku = true;

                            roikkumispaikka = ray5AlasHit.point - new Vector3(0f, 1f, 0f) - (ray5.direction / 2);
                            roiku = true;
                            if (roikkumispaikka.y < -0.5f)
                            {
                                var stoppi = "tässä";
                            }
                        }
                        //}
                    }
                }
            }
            if (tulevaPaikka2 == Vector3.zero)
            {
                roiku = true;
            }
            else
            {
                UnityEngine.Debug.Log("Liian korkea. Nyt ei pysty.");
                //kiipeys = false;

            }
        }
        else
        {
            cc.lockMovement = false;
            cc.keepDirection = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
            rb.isKinematic = false;
            cc.keepDirection = false;
            cc.isStrafing = false;
            cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
        }

    }
    void Updatet()
    {

        if (kiipeys)
        {

        }
        if (Input.GetKeyDown("e") && liiku == false && roiku == false && eka == false)
        {
            //var speed = rb.velocity.magnitude;
            //if (speed < 0.5)
            //{
            //    rb.velocity = new Vector3(0, 0, 0);
            //    //Or
            //    //gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            //}
            //kiipeys = true;
            Ray hh = new Ray(RayNormalOrigin.position, transform.forward);


            if (Physics.Raycast(hh, out kiipeemisenAlotusHit, maxDistance: 0.5f))
            {
                UnityEngine.Debug.DrawRay(hh.origin, hh.direction, color: Color.red, 3);
                var gg = Quaternion.FromToRotation(Vector3.up, -kiipeemisenAlotusHit.normal).eulerAngles;

                //transform.rotation = Quaternion.LookRotation(-kk.normal);
                //transform.eulerAngles =new Vector3(0, gg.y,0);
                //transform.forward = -kk.normal;

                rb.velocity = Vector3.zero;
                cc.lockMovement = true;
                cc.keepDirection = true;
                cc.locomotionType = vThirdPersonMotor.LocomotionType.OnlyStrafe;
                rb.isKinematic = true;
                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-kiipeemisenAlotusHit.normal.x, 0f, -kiipeemisenAlotusHit.normal.z)) * transform.rotation;
                oikeaSuunta = transform.rotation;
                eka = true;

            }

        }
        else if (liiku && kiipeys || liiku && Input.GetKeyDown("e") && roiku)
        {
            kiipeysRoikkumisesta = true;
        }
        else if (roiku && Input.GetKeyDown("r") && transform.position == korjattuRoikkumisPaikka)
        {
            roiku = false;
            tipuRoikkumisesta = true;
        }
        if (roiku && Input.GetKey("a") && transform.position == korjattuRoikkumisPaikka && transform.rotation == oikeaSuunta)
        {
            roiku = false;
            kiipeilyVasemmalle = true;
        }
        else if (roiku && Input.GetKey("d") && transform.position == korjattuRoikkumisPaikka && transform.rotation == oikeaSuunta)
        {
            roiku = false;
            kiipeilyOikealle = true;
        }



        if (eka)
        {
            eka = false;

            VaultingParempi();
        }

        //if (roiku && transform.rotation == oikeaSuunta)
        //{

        //    gameObject.GetComponent<Collider>().isTrigger = true;

        //    transform.position = roikkumispaikka;

        //    if (transform.forward != -kk.normal)
        //    {
        //        var t = 4;
        //    }

        //    if (transform.position == roikkumispaikka)
        //    {

        //        liiku = true;

        //        Ray rayNormal = new Ray(RayNormal.position, transform.forward);
        //        UnityEngine.Debug.DrawRay(rayNormal.origin, rayNormal.direction, color: Color.red, 3);

        //        if (Physics.Raycast(rayNormal, maxDistance: 1, hitInfo: out RaycastHitNormal))
        //        {

        //            korjattuRoikkumisPaikka = RaycastHitNormal.point - (transform.forward / 2);
        //            korjattuRoikkumisPaikka.y = roikkumispaikka.y;

        //            transform.position = korjattuRoikkumisPaikka;
        //            roikkumispaikka = korjattuRoikkumisPaikka;

        //            //transform.rotation= Quaternion.FromToRotation(transform.forward, -RaycastHitNormal.normal) * transform.rotation;
        //            //transform.Rotate(new Vector3 (0, -RaycastHitNormal.normal.y ,0));

        //        }
        //        else if (kiipeilyVasemmalle && Physics.Raycast(RayNormal.position + (transform.right * 0.1f), transform.forward, maxDistance: 1, hitInfo: out RaycastHitNormal))
        //        {
        //            korjattuRoikkumisPaikka = RaycastHitNormal.point - (transform.forward / 2);
        //            korjattuRoikkumisPaikka.y = roikkumispaikka.y;

        //            transform.position = korjattuRoikkumisPaikka;
        //            roikkumispaikka = korjattuRoikkumisPaikka;
        //        }
        //        else if (kiipeilyOikealle && Physics.Raycast(RayNormal.position + (-transform.right * 0.1f), transform.forward, maxDistance: 1, hitInfo: out RaycastHitNormal))
        //        {
        //            korjattuRoikkumisPaikka = RaycastHitNormal.point - (transform.forward / 2);
        //            korjattuRoikkumisPaikka.y = roikkumispaikka.y;

        //            transform.position = korjattuRoikkumisPaikka;
        //            roikkumispaikka = korjattuRoikkumisPaikka;
        //        }
        //    }

        //}

        if (roiku && transform.rotation == oikeaSuunta)
        {

            gameObject.GetComponent<Collider>().isTrigger = true;

            transform.position = transform.position - (transform.forward * 0.5f);
            roikkumispaikka = transform.position;
            roikkumispaikka.y = roikkumispaikkaY - 1f;
            transform.position = roikkumispaikka;

            if (transform.position == roikkumispaikka)
            {

                liiku = true;

                Ray rayNormal = new Ray(RayNormalOrigin.position, transform.forward);
                UnityEngine.Debug.DrawRay(rayNormal.origin, rayNormal.direction, color: Color.red, 3);

                if (Physics.Raycast(rayNormal, maxDistance: 1, hitInfo: out RaycastHitNormal))
                {

                    korjattuRoikkumisPaikka = RaycastHitNormal.point - (transform.forward / 2);
                    korjattuRoikkumisPaikka.y = roikkumispaikka.y;

                    transform.position = korjattuRoikkumisPaikka;
                    roikkumispaikka = korjattuRoikkumisPaikka;

                    //transform.rotation= Quaternion.FromToRotation(transform.forward, -RaycastHitNormal.normal) * transform.rotation;
                    //transform.Rotate(new Vector3 (0, -RaycastHitNormal.normal.y ,0));

                }
                //else if (kiipeilyVasemmalle && Physics.Raycast(RayNormal.position + (transform.right * 0.1f), transform.forward, maxDistance: 1, hitInfo: out RaycastHitNormal))
                //{
                //    korjattuRoikkumisPaikka = RaycastHitNormal.point - (transform.forward / 2);
                //    korjattuRoikkumisPaikka.y = roikkumispaikka.y;

                //    transform.position = korjattuRoikkumisPaikka;
                //    roikkumispaikka = korjattuRoikkumisPaikka;
                //}
                //else if (kiipeilyOikealle && Physics.Raycast(RayNormal.position + (-transform.right * 0.1f), transform.forward, maxDistance: 1, hitInfo: out RaycastHitNormal))
                //{
                //    korjattuRoikkumisPaikka = RaycastHitNormal.point - (transform.forward / 2);
                //    korjattuRoikkumisPaikka.y = roikkumispaikka.y;

                //    transform.position = korjattuRoikkumisPaikka;
                //    roikkumispaikka = korjattuRoikkumisPaikka;
                //}
            }

        }

        if (kiipeysRoikkumisesta)
        {

            Ray rayNormalTulevaPositio = new Ray(RayNormalOrigin.position + new Vector3(0f, 1f, 0f), transform.forward);
            RaycastHit rr;
            UnityEngine.Debug.DrawRay(rayNormalTulevaPositio.origin, rayNormalTulevaPositio.direction, color: Color.red, 333f);

            if (Physics.Raycast(rayNormalTulevaPositio, maxDistance: 0.5f, hitInfo: out rr) == false)
            {
                Ray rayNormalTulevaPositio1 = new Ray(RayNormalOrigin.position + new Vector3(0f, 1f, 0f) + (transform.forward * 0.7f), -transform.up);
                RaycastHit rayNormalTulevaPositio1Hit;
                UnityEngine.Debug.DrawRay(rayNormalTulevaPositio1.origin, rayNormalTulevaPositio1.direction, color: Color.green, 333f);
                if (Physics.Raycast(rayNormalTulevaPositio1, maxDistance: Mathf.Infinity, hitInfo: out rayNormalTulevaPositio1Hit))
                {
                    float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                    Vector3 tokanCapsulenPoint1 = rayNormalTulevaPositio1.origin + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                    Vector3 tokanCapsulenPoint2 = rayNormalTulevaPositio1.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                    float radius = col.radius * 0.95f;
                    float castDistance = 0.5f;

                    Physics.CapsuleCast(tokanCapsulenPoint1 - new Vector3(0f, 0.1f, 0f), tokanCapsulenPoint2 - new Vector3(0f, 0.1f, 0f), radius, -transform.forward, maxDistance: 0.70f);

                    if (!Physics.CapsuleCast(tokanCapsulenPoint1 - new Vector3(0f, 0.1f, 0f), tokanCapsulenPoint2 - new Vector3(0f, 0.1f, 0f), radius, -transform.forward, maxDistance: 0.70f))
                    {
                        transform.position = rayNormalTulevaPositio1Hit.point;
                        tulevaPaikka2 = transform.position;
                        roiku = false;
                    }
                }
            }


            //transform.position = tulevaPaikka2;
            if (transform.position == tulevaPaikka2)
            {
                liiku = false;
                tulevaPaikka2 = transform.position;
                kesken = false;
                valmis = true;
                kiipeys = false;
                kiipeilyVasemmalle = false;
                kiipeilyOikealle = false;

                cc.lockMovement = false;
                cc.keepDirection = false;
                cc.isStrafing = false;
                cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Collider>().isTrigger = false;

            }
            else
            {
                roiku = true;
            }
            kiipeysRoikkumisesta = false;
        }

        if (tipuRoikkumisesta)
        {
            roiku = false;
            liiku = false;
            kesken = false;
            valmis = true;
            kiipeys = false;
            kiipeilyVasemmalle = false;
            kiipeilyOikealle = false;

            cc.lockMovement = false;
            cc.keepDirection = false;
            cc.isStrafing = false;
            cc.locomotionType = vThirdPersonMotor.LocomotionType.FreeWithStrafe;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Collider>().isTrigger = false;
            tipuRoikkumisesta = false;
        }

        if (kiipeilyVasemmalle)
        {
            if (transform.position == korjattuRoikkumisPaikka && transform.rotation == oikeaSuunta)
            {

                Ray rayNormal1 = new Ray(RayNormalOrigin.position + (-transform.right * 0.1f), transform.forward);
                Ray rayVasemmalle = new Ray(RayNormalOrigin.position, -transform.right);

                //UnityEngine.Debug.DrawRay(rayNormal1.origin, rayNormal1.direction, color: Color.red);
                UnityEngine.Debug.DrawRay(rayVasemmalle.origin, rayVasemmalle.direction.normalized * 0.8f, color: Color.yellow, 25f);

                if (Physics.Raycast(rayVasemmalle, maxDistance: 0.8f, hitInfo: out RaycastHitNormal1))
                {
                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;
                }

                else if (Physics.Raycast(rayNormal1, maxDistance: 0.6f, hitInfo: out RaycastHitNormal1))
                {

                    roikkumispaikka += (-transform.right * 0.1f);
                    transform.position = roikkumispaikka;
                    //transform.rotation = Quaternion.LookRotation(-RaycastHitNormal1.normal);
                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;

                    //Ray rayNormal2 = new Ray(RayNormal.position, transform.forward);
                    ////UnityEngine.Debug.DrawRay(rayNormal2.origin, rayNormal2.direction, color: Color.red);

                    //if (Physics.Raycast(rayNormal2, maxDistance: 0.6f, hitInfo: out RaycastHitNormal2))
                    //{

                    //    transform.rotation = Quaternion.LookRotation(-RaycastHitNormal.normal);

                    //}

                }
                else
                {
                    Ray raySuunnanVaihto = new Ray(RayNormalOrigin.position + (-transform.right * 0.1f) + (transform.forward * 0.7f), transform.right);
                    UnityEngine.Debug.DrawRay(raySuunnanVaihto.origin, raySuunnanVaihto.direction, color: Color.blue, 25f);
                    if (Physics.Raycast(raySuunnanVaihto, maxDistance: 0.6f, hitInfo: out rayCastHitSuunnanVaihto))
                    {
                        roikkumispaikka = rayCastHitSuunnanVaihto.point - (raySuunnanVaihto.direction / 2);
                        roikkumispaikka.y = korjattuRoikkumisPaikka.y;
                        transform.position = roikkumispaikka;
                        //transform.rotation = Quaternion.LookRotation(-rayCastHitSuunnanVaihto.normal);
                        transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;

                    }

                }
                kiipeilyVasemmalle = false;
                roiku = true;
                oikeaSuunta = transform.rotation;
            }
            else
            {
                kiipeilyVasemmalle = false;
                roiku = true;
            }
        }

        if (kiipeilyOikealle)
        {
            if (transform.position == korjattuRoikkumisPaikka && transform.rotation == oikeaSuunta)
            {


                Ray rayNormal1 = new Ray(RayNormalOrigin.position + (transform.right * 0.1f), transform.forward);
                Ray rayOikealle = new Ray(RayNormalOrigin.position, transform.right);
                //UnityEngine.Debug.DrawRay(rayNormal1.origin, rayNormal1.direction, color: Color.red);
                UnityEngine.Debug.DrawRay(rayOikealle.origin, rayOikealle.direction.normalized * 0.8f, color: Color.yellow, 25f);
                UnityEngine.Debug.DrawRay(rayNormal1.origin, rayNormal1.direction.normalized * 0.6f, color: Color.yellow, 25f);

                if (Physics.Raycast(rayOikealle, maxDistance: 0.8f, hitInfo: out RaycastHitNormal1))
                {
                    transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;
                }

                else if (Physics.Raycast(rayNormal1, maxDistance: 0.6f, hitInfo: out RaycastHitNormal1))
                {
                    if (-RaycastHitNormal1.normal == transform.forward)
                    {


                        roikkumispaikka += (transform.right * 0.1f);
                        transform.position = roikkumispaikka;
                        //transform.rotation = Quaternion.LookRotation(-RaycastHitNormal1.normal);
                        transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;

                        //Ray rayNormal2 = new Ray(RayNormal.position, transform.forward);
                        ////UnityEngine.Debug.DrawRay(rayNormal2.origin, rayNormal2.direction, color: Color.red);

                        //if (Physics.Raycast(rayNormal2, maxDistance: 0.6f, hitInfo: out RaycastHitNormal2))
                        //{
                        //    transform.rotation = Quaternion.LookRotation(-RaycastHitNormal2.normal);

                        //}
                    }
                    else
                    {
                        Ray raySuunnanVaihto = new Ray(RayNormalOrigin.position + (transform.right * 0.3f) + (transform.forward * 0.7f), -transform.right);
                        UnityEngine.Debug.DrawRay(raySuunnanVaihto.origin, raySuunnanVaihto.direction, color: Color.blue, 25f);
                        if (Physics.Raycast(raySuunnanVaihto, maxDistance: 0.6f, hitInfo: out rayCastHitSuunnanVaihto))
                        {
                            roikkumispaikka = rayCastHitSuunnanVaihto.point - (raySuunnanVaihto.direction / 2);
                            roikkumispaikka.y = korjattuRoikkumisPaikka.y;
                            transform.position = roikkumispaikka;
                            //transform.rotation = Quaternion.LookRotation(-rayCastHitSuunnanVaihto.normal);
                            transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;

                        }
                    }
                }
                //else if (Physics.Raycast(rayNormal1, maxDistance: 0.6f, hitInfo: out RaycastHitNormal1) && -RaycastHitNormal1.normal != transform.forward)
                //{
                //    var t = 1;
                //}
                else
                {
                    //Pitää ehkä laittaa nää kaikki updateen
                    //Se ei tuu tänne siinä kulmassa
                    //Se osuu siihen seuraavaan seinään

                    Ray raySuunnanVaihto = new Ray(RayNormalOrigin.position + (transform.right * 0.1f) + (transform.forward * 0.7f), -transform.right);
                    UnityEngine.Debug.DrawRay(raySuunnanVaihto.origin, raySuunnanVaihto.direction, color: Color.blue, 25f);
                    if (Physics.Raycast(raySuunnanVaihto, maxDistance: 0.6f, hitInfo: out rayCastHitSuunnanVaihto))
                    {
                        roikkumispaikka = rayCastHitSuunnanVaihto.point - (raySuunnanVaihto.direction / 2);
                        roikkumispaikka.y = korjattuRoikkumisPaikka.y;
                        transform.position = roikkumispaikka;
                        //transform.rotation = Quaternion.LookRotation(-rayCastHitSuunnanVaihto.normal);
                        transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(-RaycastHitNormal1.normal.x, 0f, -RaycastHitNormal1.normal.z)) * transform.rotation;

                    }

                }
                kiipeilyOikealle = false;
                roiku = true;
                oikeaSuunta = transform.rotation;
            }
            else
            {
                kiipeilyOikealle = false;
                roiku = true;
            }
        }

    }

}