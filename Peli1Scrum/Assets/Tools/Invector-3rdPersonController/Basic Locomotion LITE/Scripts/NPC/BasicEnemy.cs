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


    // Use this for initialization
    void Start()
    {
        Rest();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToPlayer()
    {
        //rotate to look at player
        var ylösKatsomus = new Vector3 (0, 1f, 0);

        transform.LookAt(target.position+ylösKatsomus);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);

        //move towards player
        if (Vector3.Distance(transform.position, target.position) > attack1Range)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }

    public void Rest()
    {

    }

}