using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
[DebuggerStepThrough]
public class AttackArea : MonoBehaviour
{

    public BoxCollider territory;
    GameObject player;
    bool playerInTerritory;

    public GameObject enemy;
    BasicEnemy basicenemy;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        basicenemy = enemy.GetComponent<BasicEnemy>();
        playerInTerritory = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTerritory == true&&basicenemy.backOffFromPlayer==false&&basicenemy.backOff==false&&basicenemy.wait==false&&basicenemy.dashAttack==false)
        {
            if (basicenemy.target.position.y>basicenemy.transform.position.y+1f && basicenemy.target.position.y < basicenemy.transform.position.y + 3f)
            {
                basicenemy.targetTooHigh = true;
            }
            else
            {
                basicenemy.targetTooHigh = false;
                basicenemy.MoveToPlayer();
            }

        }

        if (playerInTerritory == false&&basicenemy.GoToRest==false)
        {
            basicenemy.Rest();
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
            basicenemy.GoToRest = false;
            playerInTerritory = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (basicenemy.GoToRest==true&&Time.time> basicenemy.restingTime+ 5f)
        {
            basicenemy.GoToRest = false;
        }
        else if (other.gameObject == player)
        {
            playerInTerritory = true;
        }
    }

}


