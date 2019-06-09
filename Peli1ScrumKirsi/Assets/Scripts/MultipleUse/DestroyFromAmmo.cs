using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFromAmmo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BunnyAmmo"))
        {
            Destroy(gameObject, 0.2f);
        }
    }
}
