using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFromPlayerTouch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 0.05f);
        }
    }
}
