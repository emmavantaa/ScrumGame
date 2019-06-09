using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTrigger : MonoBehaviour
{
     void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(2);
    }
}
