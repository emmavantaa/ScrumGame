using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Start is called before the first frame update
public class BasicEnemy : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float attack1Range = 1f;
    public int attack1Damage = 1;
    public float timeBetweenAttacks;
    public float hp = 1;
    public float kaatumisSpeed=1;
    public GameObject player;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Rest();
    }

    // Update is called once per frame
    void Update()
    {
        //if (hp<=0)
        //{
        //    float step = kaatumisSpeed * Time.deltaTime;
        //    //transform.position = Vector3.MoveTowards(transform.position, OpenDrawerPosition.transform.position, step);

        //    gameObject.SetActive(false);
        //    //Destroy(gameObject);
        //}
    }

    public void MoveToPlayer()
    {
        ////rotate to look at player
        //var ylösKatsomus = new Vector3 (0, 1f, 0);
        //var hh = new Vector3(0, player.transform.position.y,0);

        ////Vector3 dif = target.position - transform.position;
        ////dif.y = 0;

        ////Quaternion lookAngle = Quaternion.LookRotation(dif, transform.up);

        //transform.LookAt(new Vector3(target.position.x,transform.position.y,target.position.z));
        //transform.Rotate(new Vector3(0, -90, 0), Space.Self);


        Vector3 targetDir = target.position - transform.position;


        // The step size is equal to speed times frame time.
        float step = speed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);
        newDir.y = 0;

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);



        //move towards player
        if (/*Vector3.Distance(transform.position, target.position) > attack1Range&&*/player.GetComponent<Health>().immortalMoment==false)
        {
            transform.Translate(Vector3.forward * (speed* Time.deltaTime));
        }
    }

    //public void takeDamage()
    //{
    //    if (immortal==false)
    //    {
    //        hp -= 1;
    //    }
        
    //}

    public void Rest()
    {

    }


}