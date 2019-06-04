using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
[DebuggerStepThrough]
public class BridgeEnemyAttackArea : MonoBehaviour
{
    public BoxCollider territory;
    GameObject player;
    bool playerInTerritory;

    public GameObject enemy;
    BridgeEnemy bridgeEnemy;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bridgeEnemy = enemy.GetComponent<BridgeEnemy>();
        playerInTerritory = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTerritory == true)
        {
            bridgeEnemy.AttackPlayer();
        }

        if (playerInTerritory == false)
        {
            bridgeEnemy.Rest();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTerritory = true;
        }
    }
    private void OnTriggerStay(Collider other)
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
            playerInTerritory = false;
        }
    }
}
