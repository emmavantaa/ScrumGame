using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiipeäminen : MonoBehaviour
{
    vThirdPersonMotor pm;

    public Transform Ray0Origin;
    public Transform Ray1Origin;
    public Transform Ray2Origin;
    public Transform Ray3Origin;
    public Transform Ray4Origin;
    public Transform Ray5Origin;

    Vector3 tulevaPaikka;
    Vector3 tulevaPaikka1;
    Vector3 tulevaPaikka2;
    Vector3 paikka;

    Rigidbody rb;
    CapsuleCollider col;

    public bool liiku = false;
    public bool kesken = false;
    public bool valmis = true;
    public bool kiipeys = false;
    public bool kiipeilee = false;

    public Vector3 orig;
    public Vector3 dir;

    //GameObject obj = new GameObject("TestiCol");
    //Collider paikanTestausCollider;



    //var bCol = obj.AddComponent<BoxCollider>();

    //public GameObject alempiRayOriginGameObject;
    //public GameObject ylempiRayOriginGameObject;



    // Start is called before the first frame update

    private void Awake()
    {
        pm = gameObject.GetComponent<vThirdPersonMotor>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("e") && liiku == false)
        {
            kiipeys = true;
            VaultingParempi();
        }
        //else
        //{
        //    kiipeys = false;
        //}

    }
    private void FixedUpdate()
    {
        //if (kiipeys)
        //{

        //}
        //if (!kiipeys)
        //{

        //}
        //if (kesken)
        //{
        //    liiku = true;

        //    VaultingParempi();
        //}
        //else
        //{

        //    valmis = true;
        //}
        if (liiku)
        {
            kiipeilee = true;

            transform.position = tulevaPaikka2;
            if (transform.position == tulevaPaikka2)
            {
                liiku = false;

                tulevaPaikka2 = transform.position;
                kesken = false;
                valmis = true;
                kiipeilee = false;
            }
        }
    }
    public void Vaulting()
    {
        Ray ray0 = new Ray(Ray0Origin.position, transform.forward);
            
        if (Physics.Raycast(ray0, 0.55f) ==true)
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

                    if (Physics.Raycast(ray5, 0.55f) ==false)
                    {

                        Vector3 capsulenLähtöpaikka = ray5.origin;
                        float raycapsulenMatkaKeskeltäPointteihin = col.height / 2 - col.radius;
                        Vector3 point1 = capsulenLähtöpaikka + col.center + Vector3.up * raycapsulenMatkaKeskeltäPointteihin;
                        Vector3 point2 = capsulenLähtöpaikka + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                        float radius = col.radius * 0.95f;
                        float castDistance = 0.5f;

                        
                        if (!Physics.CapsuleCast(point1, point2, radius, transform.forward, castDistance))
                        {

                            Ray tulevaPaikkaRay = new Ray(ray5.origin+(ray5.direction/2), Vector3.down);

                            RaycastHit tulevapaikkaRaycastHit;

                            Physics.Raycast(tulevaPaikkaRay, maxDistance: Mathf.Infinity, hitInfo: out tulevapaikkaRaycastHit);

                            //if (Physics.Raycast( tulevaPaikkaRay, maxDistance: 1f, hitInfo: out tulevapaikkaRaycastHit ) == true)
                            //{
                                
                                Vector3 tokanCapsulenPoint1 = tulevaPaikkaRay.origin + col.center + Vector3.up*raycapsulenMatkaKeskeltäPointteihin;
                                Vector3 tokanCapsulenPoint2 = tulevaPaikkaRay.origin + col.center - Vector3.up * raycapsulenMatkaKeskeltäPointteihin;

                                if (!Physics.CapsuleCast(tokanCapsulenPoint1, tokanCapsulenPoint2, radius, -transform.up, tulevapaikkaRaycastHit.distance - 0.01f))
                                {
                                    kesken = true;
                                    valmis = false;
                                    tulevaPaikka1 = ray5.origin +/*new Vector3(0f,1f,0f)*/  ray5.direction * 0.5f;
                                    tulevaPaikka2 = tulevapaikkaRaycastHit.point;


                                    kiipeys = false;
                                    liiku = true;
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
        bool ray0OsuiBool=false;
        bool ray0YlösOsuiBool=false;

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

        bool ray1CapsuleCastOnnaa=true;
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

        if (Physics.Raycast(ray0, 0.55f) == true )
        {
            ray0OsuiBool = true;

        }
        if (ray0OsuiBool==false && Physics.Raycast(ray: ray0Ylös, maxDistance: 2.84f, hitInfo: out ray0YlosHit) == true)
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

                        kiipeys = false;
                        liiku = true;
                        
                    }
                    else
                    {
                        ray1CapsuleCastOnnaa = false;
                    }
                    //}
                }

                
            }
            if (Physics.Raycast(ray2, 0.55f) == false&& (ray1OsuiBool == true|| ray1CapsuleCastOnnaa==false))
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

                        kiipeys = false;
                        liiku = true;
                    }
                    else
                    {
                        ray2CapsuleCastOnnaa = false;
                    }
                    //}
                }
            }
            if (Physics.Raycast(ray3, 0.55f) == false&& (ray2OsuiBool == true|| ray2CapsuleCastOnnaa ==false))
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
                        liiku = true;
                    }
                    else
                    {
                        ray3CapsuleCastOnnaa = false;
                    }
                    //}
                }
            }
            if (Physics.Raycast(ray4, 0.55f) == false&& (ray3OsuiBool == true|| ray3CapsuleCastOnnaa==false))
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
                        liiku = true;
                    }
                    else
                    {
                        ray4CapsuleCastOnnaa = false;
                    }
                    //}
                }
            }
            if (Physics.Raycast(ray5, 0.55f) == false&& (ray4OsuiBool == true|| ray4CapsuleCastOnnaa==false))
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
                        liiku = true;
                    }
                    //}
                }
            }
            else
            {
                Debug.Log("Liian korkea. Nyt ei pysty.");
            }
        }

        else if (ray0YlösOsuiBool == true)
        {
            if (Physics.Raycast(ray1, 0.55f) == false)
            {
                ray1OsuiBool = false;
                Physics.Raycast(ray: ray1Alas, maxDistance: Mathf.Infinity, hitInfo: out ray1AlasHit);

                if (ray1AlasHit.point.y-0.01f > ray0Ylös.origin.y)
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

                            kiipeys = false;
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
            if (Physics.Raycast(ray2, 0.55f) == false && (ray1OsuiBool == true||ray1AlasOsuiBool==false || ray1CapsuleCastOnnaa == false))
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

                            kiipeys = false;
                            liiku = true;
                        }
                        else
                        {
                            ray2CapsuleCastOnnaa = false;
                        }
                        //}
                    }
                }
            }
            if (Physics.Raycast(ray3, 0.55f) == false && (ray2OsuiBool == true||ray2AlasOsuiBool==false || ray2CapsuleCastOnnaa == false))
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
                            liiku = true;
                        }
                        else
                        {
                            ray3CapsuleCastOnnaa = false;
                        }
                        //}
                    }
                }
            }
            if (Physics.Raycast(ray4, 0.55f) == false && (ray3OsuiBool == true||ray3AlasOsuiBool==false || ray3CapsuleCastOnnaa == false))
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
                            liiku = true;
                        }
                        else
                        {
                            ray4CapsuleCastOnnaa = false;
                        }
                        //}
                    }
                }
            }
            if (Physics.Raycast(ray5, 0.55f) == false && (ray4OsuiBool == true||ray4AlasOsuiBool==false || ray4CapsuleCastOnnaa == false))
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
                            liiku = true;
                        }
                        //}
                    }
                }
            }
            else
            {
                Debug.Log("Liian korkea. Nyt ei pysty.");
            }
        }

    }

}
