using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class AttackArea : MonoBehaviour
{
    [SerializeField]
    BoxCollider territory;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    BasicEnemy basicEnemy;
    bool playerInTerritory;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = transform.gameObject;
        basicEnemy = enemy.GetComponent<BasicEnemy>();
        territory = enemy.GetComponent<BoxCollider>();
        playerInTerritory = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Ray lavaFallCheckRay = new Ray(basicenemy.transform.position + (basicenemy.transform.forward * 2.5f), -basicenemy.transform.up);
        //RaycastHit lavaFallCheckRayHit = new RaycastHit();
        //UnityEngine.Debug.DrawRay(lavaFallCheckRay.origin, -basicenemy.transform.up.normalized * 2, Color.black, 10f);
        ////Physics.Raycast(lavaFallCheckRay, maxDistance: 2f, hitInfo: out lavaFallCheckRayHit);

        //if (Physics.Raycast(lavaFallCheckRay, maxDistance: 2f, hitInfo: out lavaFallCheckRayHit) && lavaFallCheckRayHit.collider.tag != ("Lava"))
        //{

        if (playerInTerritory == true && basicEnemy.backOffFromPlayer == false && basicEnemy.backOff == false && basicEnemy.wait == false && basicEnemy.dashAttack == false)
        {
            if (basicEnemy.target.position.y > basicEnemy.transform.position.y + 1f && basicEnemy.target.position.y < basicEnemy.transform.position.y + 3f)
            {
                basicEnemy.targetTooHigh = true;
                basicEnemy.MoveToPlayer();
            }
            if (basicEnemy.target.position.y < basicEnemy.transform.position.y - 1.5f && basicEnemy.target.position.y > basicEnemy.transform.position.y - 3.5f)
            {
                basicEnemy.targetTooLow = true;
                basicEnemy.MoveToPlayer();
            }
            else
            {
                basicEnemy.targetTooLow = false;
                basicEnemy.targetTooHigh = false;
                basicEnemy.MoveToPlayer();
            }

        }

        if (playerInTerritory == false && basicEnemy.GoToRest == false)
        {
            basicEnemy.targetTooLow = false;
            basicEnemy.targetTooHigh = false;
            basicEnemy.Rest();
        //basicEnemy.GoToRest = true;
        //basicEnemy.restingTime = Time.time;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTerritory = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            basicEnemy.GoToRest = false;
            playerInTerritory = false;
        }
    }
    [DebuggerStepThrough]
    private void OnTriggerStay(Collider other)
    {
        if (basicEnemy.GoToRest==true&&Time.time> basicEnemy.restingTime+ 5f)
        {
            basicEnemy.GoToRest = false;
        }
        else if (other.gameObject == player)
        {
            playerInTerritory = true;
        }
    }

}


