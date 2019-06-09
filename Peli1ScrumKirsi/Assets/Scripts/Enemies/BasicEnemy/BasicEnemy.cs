using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]

// Start is called before the first frame update

    
//////////Tähän scriptiin tuli paljon muutosta, niin pitää päivittää selitykset//////////
public class BasicEnemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float turnSpeed;
    public float dashSpeed;
    public float dasHRange;
    public int attack1Damage = 1;
    public float hp = 1;
    public float kaatumisSpeed=1;
    public float attackTime;
    public float attackStartTime;
    public float attackStopTime;
    public float backOffTime;
    public float backOffTimeFromPlayer;
    public float collisionTime;
    public float collisionWithPlayerTime;
    public float forwardStartTime;
    public float timeToGoForward;
    public float timeBetweenDashAttacks;
    public float waitAfterDash;
    public float timeToGoToRest;
    public float collisionCounterTime;
    public float restingTime;
    public float hitCountTimer;
    public int hitCount;
    public GameObject player;
    //osan näistä booleista vois tehä metoideiks
    public bool dashAttack;
    public bool miekkaCollision;
    public bool wait;
    public bool waitForDash;
    public bool backOff; //backoff pitäis saada toimimaan paremmin GoToRest kanssa koska jää jumittamaan seinään jos sen starting point on suoraan sen takana
    public bool forward;
    public bool backOffFromPlayer;
    public bool GoToRest;
    public bool stop;
    public bool targetTooHigh;
    public bool targetTooLow;
    public bool rightDirection;
    public bool moveTowardsPlayer;
    public bool resti;
    public Vector3 startingPoint;
    public Vector3 targetDir;
    public Vector3 newDir;
    public Vector3 targetPoint;
    public Vector3 backOffDir;
    public Vector3 newBackOffDir;
    public Vector3 newDirDash;
    public Quaternion newDirDashTuleva;
    public Quaternion direction;

    // Use this for initialization
    void Start()
    {
        hitCount = 0;
        hitCountTimer = 9;
        timeToGoToRest = 22f;
        backOffTimeFromPlayer = 0.3f;
        speed = 3f;
        turnSpeed = 5f;
        dashSpeed = 13f;
        attackStopTime = 0f;
        dasHRange = 7f;
        attackTime = 1f;
        attackStartTime = 0f;
        waitAfterDash= 2f;
        timeBetweenDashAttacks = 3f;
        backOffTime = 0.3f;
        timeToGoForward = 0.5f;
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;

        startingPoint = transform.position;
        Rest();
    }

    // Update is called once per frame
    void Update()
    {
        //Jos vihollisen transform.position.y muuttuu paljon eli jos se esim tippuu alemmalle tasolle, niin sen startingPoint(paikka mihin se menee kun Rest();) muutetaan siihen mihin se tippui
        if (transform.position.y+2.5<startingPoint.y|| transform.position.y  > startingPoint.y + 2.5)
        {
            startingPoint = transform.position;
        }


        //Jos on ongelmia, että miekka menee seinän sisään, niin tämä käyttöön
        //Jos miekka on seinän sisällä (koska silloin raycast osuisi seinään), niin vihollinen menee restaamaan
        //if (Physics.Raycast(collisionCheckRay, maxDistance: 0.5f))
        //{
        //    //backOff = true;
        //    GoToRest = true;
        //    restingTime = Time.time;
        //}

        if (GoToRest == true)
        {
            //Jos on mennyt määritelty aika, niin voi lopettaa GoToRest
            if (Time.time > restingTime + timeToGoToRest)
            {
                GoToRest = false;
            }
            //Kun GoToRest alkaa, niin nollataan asioita ja vihu menee starting pointia kohti
            else
            {
                hitCount = 0;
                backOff = false;
                backOffFromPlayer = false;
                dashAttack = false;
                stop = false;
                wait = false;
                waitForDash = false;
                Rest();
                
            }
        }

        //Jos GoToRest==false, niin vihollinen voi tehdä asioita
        else
        {
            //Jos vihollisen miekka on törmäännyt muuhun kuin pelaajaan 18 kertaa määritellyn ajan sisällä (hitCountTimer) niin vihollinen alkaa liikkumaan starting pointia kohti määritellyn ajan verran (timeToGoToRest)
            //(ellei GoToRestiä laiteta falseksi AttackArea-scriptistä), koska se lannistuu, kun törmäilee kokoajan seinään
            if (Time.time < collisionCounterTime + hitCountTimer && hitCount >= 18)
            {
                GoToRest = true;
                restingTime = Time.time;
            }

            //Jos on kulunut määritelty aika ilman miekan törmäystä muuhun kuin pelaajaan, niin hitCount nollataan
            if (Time.time > collisionCounterTime + hitCountTimer)
            {
                hitCount = 0;
            }

            //Jos vihollisen miekka on törmäännyt muuhun kuin pelaajaan
            if (miekkaCollision)
            {
                //Lasketaan 
                if (hitCount == 0)
                {
                    //Otetaan aika talteen törmäyksestä jos hitCount oli nolla
                    collisionCounterTime = Time.time;
                }

                //Otetaan aika talteen törmäyksestä backOffia varten
                collisionTime = Time.time;
                

                //Jos osui dashilla, niin lopetetaan se ja otetaan aika talteen
                if (dashAttack)
                {
                    backOff = true;
                    dashAttack = false;
                    attackStopTime = Time.time;
                }

            }

            //Jos vihollisen miekka on törmännyt pelaajaan, niin vihollinen liikkuu taaksepäin
            if (backOffFromPlayer == true)
            {
                transform.Translate(-Vector3.forward * ((speed) * Time.deltaTime));
                dashAttack = false;

                //Lopetetaan backOffFromPlayer, kun on kulunut backOffTimeFromPlayer aika
                if (collisionWithPlayerTime + backOffTimeFromPlayer < Time.time)
                {
                    backOffFromPlayer = false;
                    attackStopTime = Time.time;
                }
            }

            //Jos vihollisen miekka on törmännyt johonkin muuhun kuin pelaajaan, niin vihollinen alkaa liikkumaan taaksepäin ja kääntyy, että ei jäisi seinään jumittamaan
            //Nää backoffit vois laittaa samaan tyyliin kun backOffFromPlayer
            else if (backOff)
            {
                dashAttack = false;
                transform.Translate(-Vector3.forward * ((speed) * Time.deltaTime));
                if(Time.time < collisionTime + backOffTime&& wait == false)
                {
                    float step = (turnSpeed) * Time.deltaTime;

                    backOffDir = ((target.position - transform.position) - transform.position);
                    newBackOffDir = Vector3.RotateTowards(transform.forward, backOffDir, step, 0.0f);
                    newBackOffDir.y = 0;

                    transform.rotation = Quaternion.LookRotation(newBackOffDir);

                }
            }
            //Jos määritelty aika on kulunut törmäyksestä johonkin muuhun kuin pelaajaan, niin backOff pois päältä
            if (backOff && Time.time > collisionTime + backOffTime)
            {

                forward = true;
                backOff = false;
                forwardStartTime = Time.time;
            }
            //Vihollinen liikkuu eteenpäin backOffin jälkeen yrittäen kiertää esteen
            if (forward&&stop==false)
            {
                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }
            //forward pois päältä kun on kulunut määritelty aika(timeToGoForward)
            if (Time.time > forwardStartTime + timeToGoForward)
            {
                forward = false;
            }

            //Dash hyökkäys
            if (dashAttack == true)
            {
                transform.Translate(Vector3.forward * ((dashSpeed) * Time.deltaTime));

                //Kun on kulunut aika joka dash hyökkäykselle on määritelty (attackTime), niin se lopetetaan
                if (Time.time > attackStartTime + attackTime)
                {
                    dashAttack = false;

                    //Otetaan talteen aika jolloin dash lopetettiin, niin voidaan aloittaa odotus dashin jälkeen
                    attackStopTime = Time.time;
                }
            }

            //Odotus liikkumatta dashin jälkeen
            if (Time.time < attackStopTime + waitAfterDash)
            {
                wait = true;
            }
            else
            {
                wait = false;
            }

            //Odotus ennenkuin saa tehdä uuden dashin
            if (Time.time < attackStopTime + timeBetweenDashAttacks)
            {
                waitForDash = true;
            }
            else
            {
                waitForDash = false;
            }

            //Dash hyöäkkäys pelaajaa kohti, jos pelaajaa ei ole juuri osunut (immortalMoment) + muut ehdot
            //Jos haluaa, että viholliset dashaa pelaaja kohti kun immortalMoment, niin voi commentoida ton -> player.GetComponent<Health>().immortalMoment == false
            if (Vector3.Distance(transform.position, target.position) < dasHRange && dashAttack == false && wait == false && waitForDash == false &&
                backOffFromPlayer == false && backOff == false && forward == false&& (player.GetComponent<Health>().immortalMoment == false)&&!targetTooLow)
            {
                //Muutetaan suunnat normaaleiksi
                targetDir = target.position - transform.position;
                var targetDirY0 = targetDir;
                targetDirY0.y = 0;
                var targetDirY0Normalized = targetDirY0.normalized;
                var thisDir = transform.forward;
                var thisDirY0 = thisDir;
                thisDirY0.y = 0;
                var thisDirNormalized = thisDirY0.normalized;

                float step = (turnSpeed) * Time.deltaTime;
                newDirDash = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                newDirDash.y = 0;

                //Kääntyminen
                var rayCastDirection= Quaternion.LookRotation(newDirDash);

                Ray collisionCheckRay = new Ray(transform.position + new Vector3(0f, 0.1f, 0f), newDirDash);
                //UnityEngine.Debug.DrawRay(collisionCheckRay.origin, transform.forward, color: Color.cyan, 2f);

                //Pelaajan x ja z suunta + 0.001f vihollisesta katsottuna
                var targetXPlus= targetDirY0Normalized.x+0.001f;
                var targetZPlus = targetDirY0Normalized.z+0.001f;

                //Pelaajan x ja z suunta - 0.001f vihollisesta katsottuna
                var targetXMinus = targetDirY0Normalized.x - 0.001f;
                var targetZMinus = targetDirY0Normalized.z - 0.001f;

                //Vihollisen transform.forward x ja z normalisoituna
                var thisx = thisDirNormalized.x;
                var thisz = thisDirNormalized.z;

                //Dash hyökkäys pelaajan epätarkkaa suuntaa kohti (jos olisi tarkka suunta, niin viholliset pysähtyisivät odottamaan pelaajan pysähtymistä)
                if (!Physics.Raycast(collisionCheckRay, maxDistance: Vector3.Distance(transform.position, target.position) - 0.5f))
                {
                    //Tarkistetaan että vihollisen ja pelaajan välissä ei ole estettä
                    transform.rotation = Quaternion.LookRotation(newDirDash);
                    if ((thisx < targetXPlus && thisx > targetXMinus) || thisz < targetZPlus && thisz > targetZMinus)
                    {

                    
                    //UnityEngine.Debug.DrawRay(transform.position, transform.forward.normalized * 7, Color.red, 2f);
                    dashAttack = true;

                    //Otetaan aika talteen jolloin hyökkäys alkaa
                    attackStartTime = Time.time;
                    }

                    else
                    {
                        //stop = true;
                    }
                    //Muuten vihollinen liikkuu ensin estettä kohti ja törmäyksestä alkaa kiertää estettä

                }

                //Muuten odottaa, että on kääntynyt suunnilleen pelaajaa kohti
                else
                {
                    MoveToPlayer();
                    stop = false;

                }

            }

            //Jos pelaajan ja vihollisen välimatka on kasvanut suuremmaksi kuin dasHRange
            else if ((Vector3.Distance(transform.position, target.position) > dasHRange))
            {
                stop = false;
            }
        }
    }

    //Vihollinen koittaa aloittaa liikkumisen pelaaja kohti
    public void MoveToPlayer()
    {
        if (GoToRest == false&&stop==false)
        {
            moveTowardsPlayer = true;
            resti = false;

            Ray lavaFallCheckRay = new Ray(transform.position + (transform.forward * 0.5f), -transform.up);
            Ray lavaFallCheckRayLeft = new Ray(transform.position + (-transform.right * 0.5f), -transform.up);
            Ray lavaFallCheckRayRight = new Ray(transform.position + (transform.right * 0.5f), -transform.up);
            Ray collisionCheckRay = new Ray(transform.position + new Vector3(0f, -0.3f, 0f), transform.forward);
            Ray collisionCheckRayLeft = new Ray(transform.position + new Vector3(0f, -0.3f, 0f) + (-transform.right * 0.5f), -transform.right);
            Ray collisionCheckRayRight = new Ray(transform.position + new Vector3(0f, -0.3f, 0f) + (transform.right * 0.5f), transform.right);
            RaycastHit lavaFallCheckRayHit;

            //UnityEngine.Debug.DrawRay(lavaFallCheckRay.origin, -transform.up.normalized * 2, Color.black, 10f);

            //Jos edessä ei ole tyhjää tai laavaa
            if (Physics.Raycast(lavaFallCheckRay, maxDistance: 2f, hitInfo: out lavaFallCheckRayHit) && lavaFallCheckRayHit.collider.tag != ("Lava")&&rightDirection==false &&
                    (!Physics.Raycast(collisionCheckRay, 0.9f)))
            {
                //Jos oikealle eikä vasemalle osu raycast, niin suunta startingpointia kohti
                if (!Physics.Raycast(collisionCheckRayLeft, 0.9f) && !Physics.Raycast(collisionCheckRayRight, 0.9f))
                {
                    targetDir = target.position - transform.position;
                }
                //Jos oikealle tai vasemalle osuu raycast, niin suunta eteenpäin
                else
                {
                    targetDir = transform.forward;
                }

                // The step size is equal to speed times frame time.
                float step = (turnSpeed) * Time.deltaTime;

                newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                //UnityEngine.Debug.DrawRay(transform.position, newDir, Color.red);
                newDir.y = 0;
                Quaternion rotNewDir = Quaternion.LookRotation(targetDir);

                if (dashAttack == false && backOff == false)
                {
                    // Move our position a step closer to the target.
                    transform.rotation = Quaternion.LookRotation(newDir);
                }

                //move towards player
                //Jos haluaa, että viholliset ei liiku pelaaja kohti kun immortalMoment, niin voi uncommentoida ton -> player.GetComponent<Health>().immortalMoment == false &&
                if (/*player.GetComponent<Health>().immortalMoment == false &&*/ dashAttack == false && wait == false && backOff == false&&!targetTooHigh)
                {
                    transform.Translate(Vector3.forward * (speed * Time.deltaTime));
                }
            }

            //Ray check, että vasemmalla ei ole tyhjää tai laavaa tai estettä, jos ei ole niin kääntyy vasemman kautta starting pointin suuntaan ja liikkuu starting pointin suuntaan
            else if (Physics.Raycast(lavaFallCheckRayLeft, maxDistance: 2f, hitInfo: out lavaFallCheckRayHit) && lavaFallCheckRayHit.collider.tag != ("Lava") && !rightDirection &&
                    (!Physics.Raycast(collisionCheckRayLeft, 0.9f)))
            {
                targetDir =  target.position.normalized - (-transform.right.normalized);

                // The step size is equal to speed times frame time.
                float step = turnSpeed * Time.deltaTime;

                newDir = Vector3.RotateTowards(-transform.right, targetDir, step, 0.0f);
                //UnityEngine.Debug.DrawRay(transform.position, (newDir.normalized * 11f), Color.blue);
                newDir.y = 0;

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);

                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }

            //Ray check, että oikealla ei ole tyhjää tai laavaa tai estettä, jos ei ole niin kääntyy oikean kautta starting pointin suuntaan ja liikkuu starting pointin suuntaan
            else if (Physics.Raycast(lavaFallCheckRayRight, maxDistance: 2f, hitInfo: out lavaFallCheckRayHit) && lavaFallCheckRayHit.collider.tag != ("Lava") && !rightDirection &&
                    (!Physics.Raycast(collisionCheckRayRight, 0.9f)))
            {
                targetDir = target.position.normalized - (transform.right.normalized);

                // The step size is equal to speed times frame time.
                float step = turnSpeed * Time.deltaTime;

                newDir = Vector3.RotateTowards(transform.right, targetDir, step, 0.0f);
                //UnityEngine.Debug.DrawRay(transform.position, (newDir.normalized * 11f), Color.blue);
                newDir.y = 0;

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);

                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }


            else
            {
                Rest();
            }
        }
    }

    //Tuli muutoksia. Vois päivittää selitykset
    //Vihollinen menee aloituspaikkaansa kohti, kuitenkaan koskaan pääsemättä sinne joten se pyörii sen ympärillä
    public void Rest()
    {
        if (transform.position!=startingPoint)
        {
            resti = true;
            moveTowardsPlayer = false;

            Ray lavaFallCheckRay = new Ray(transform.position + (transform.forward * 0.5f), -transform.up);
            Ray lavaFallCheckRayLeft = new Ray(transform.position + (-transform.right * 0.5f), -transform.up);
            Ray lavaFallCheckRayRight = new Ray(transform.position + (transform.right * 0.5f), -transform.up);
            Ray collisionCheckRay = new Ray(transform.position + new Vector3(0f, -0.3f, 0f), transform.forward);
            Ray collisionCheckRayLeft = new Ray(transform.position + new Vector3(0f, -0.3f, 0f) + (-transform.right * 0.5f), -transform.right);
            Ray collisionCheckRayRight = new Ray(transform.position + new Vector3(0f, -0.3f, 0f) + (transform.right * 0.5f), transform.right);
            RaycastHit lavaFallCheckRayHit = new RaycastHit();
            //UnityEngine.Debug.DrawRay(lavaFallCheckRay.origin, -transform.up.normalized * 2, Color.black, 10f);
            Vector3 targetDir;
            Vector3 newDir;

            //Ray check, että edessä ei ole tyhjää tai laavaa tai estettä, jos ei ole niin kääntyy ja liikkuu starting pointin suuntaan
            if (Physics.Raycast(lavaFallCheckRay, maxDistance: 2f, hitInfo: out lavaFallCheckRayHit) && lavaFallCheckRayHit.collider.tag != ("Lava")&&!rightDirection &&
                    (!Physics.Raycast(collisionCheckRay, 0.9f)))
            {
                //Jos oikealle eikä vasemalle osu raycast, niin suunta startingpointia kohti
                if (!Physics.Raycast(collisionCheckRayLeft, 0.9f)&& !Physics.Raycast(collisionCheckRayRight, 0.9f))
                {
                    targetDir = startingPoint + target.position.normalized - transform.position;
                }
                //Jos oikealle tai vasemalle osuu raycast, niin suunta eteenpäin
                else
                {
                    targetDir = transform.forward;
                }
                

                // The step size is equal to speed times frame time.
                float step = turnSpeed * Time.deltaTime;

                newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                //UnityEngine.Debug.DrawRay(transform.position, (newDir.normalized * 11f), Color.blue);
                newDir.y = 0;

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);

                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }

            //Ray check, että vasemmalla ei ole tyhjää tai laavaa tai estettä, jos ei ole niin kääntyy vasemman kautta starting pointin suuntaan ja liikkuu starting pointin suuntaan
            else if (Physics.Raycast(lavaFallCheckRayLeft, maxDistance: 2f, hitInfo: out lavaFallCheckRayHit) && lavaFallCheckRayHit.collider.tag != ("Lava") && !rightDirection&&
                    (!Physics.Raycast(collisionCheckRayLeft, 0.9f)))
            {
                targetDir = startingPoint + target.position.normalized - (-transform.right.normalized);

                // The step size is equal to speed times frame time.
                float step = turnSpeed * Time.deltaTime;

                newDir = Vector3.RotateTowards(-transform.right, targetDir, step, 0.0f);
                //UnityEngine.Debug.DrawRay(transform.position, (newDir.normalized * 11f), Color.blue);
                newDir.y = 0;

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);

                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }

            //Ray check, että oikealla ei ole tyhjää tai laavaa tai estettä, jos ei ole niin kääntyy oikean kautta starting pointin suuntaan ja liikkuu starting pointin suuntaan
            else if (Physics.Raycast(lavaFallCheckRayRight, maxDistance: 2f, hitInfo: out lavaFallCheckRayHit) && lavaFallCheckRayHit.collider.tag != ("Lava") && !rightDirection &&
                    (!Physics.Raycast(collisionCheckRayRight, 0.9f)))
            {
                targetDir = startingPoint + target.position.normalized - (transform.right.normalized );

                // The step size is equal to speed times frame time.
                float step = turnSpeed * Time.deltaTime;

                newDir = Vector3.RotateTowards(transform.right, targetDir, step, 0.0f);
                //UnityEngine.Debug.DrawRay(transform.position, (newDir.normalized * 11f), Color.blue);
                newDir.y = 0;

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);

                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }

            //Muuten kääntyy starting pointin suuntaan
            //else
            //{
            //    rightDirection = true;
            //    Vector3 targetDir = startingPoint + target.position.normalized - transform.position;

            //    // The step size is equal to speed times frame time.
            //    float step = turnSpeed * Time.deltaTime;

            //    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir.normalized, step, 0.0f);
            //    //UnityEngine.Debug.DrawRay(transform.position, (newDir.normalized * 11f), Color.blue);
            //    newDir.y = 0;

            //    // Move our position a step closer to the target.
            //    transform.rotation = Quaternion.LookRotation(newDir);

            //    //Suunta normaalit stringeiksi, että voi verrata
            //    var forwardString = transform.forward.normalized.ToString();
            //    var targetDirString = newDir.normalized.ToString();

            //    //Jos on kääntynyt starting pointin suuntaan, niin rightDirection = false, joten voi alkaa taas testaamaan jos pääsee liikkumaan pelaajaa kohti
            //    if (forwardString==targetDirString)
            //    {
            //        rightDirection = false;
            //    }
            //}

        }

        //Jos pääsee starting pointiin
        else
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

    }

    //Vihollinen ottaa damagea jos menee laavaan
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Lava")
        {
            gameObject.GetComponent<VihuHealth>().takeDamage();
        }
    }


}