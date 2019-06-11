using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TriggerLoad : MonoBehaviour
{
    [SerializeField] private string loadLevel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(loadLevel);
        }
    }
}
