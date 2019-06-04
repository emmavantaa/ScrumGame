using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class ImmortalEmission : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmissionOn()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            var player = GameObject.FindWithTag("Player");
            if (player.GetComponent<ParticleSystem>())
            {
                var particleSystem= player.GetComponent<ParticleSystem>();
                particleSystem.Play();
                
            }

        }

    }
    public void EmissionOff()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            var player = GameObject.FindWithTag("Player");
            if (player.GetComponent<ParticleSystem>())
            {
                var particleSystem = player.GetComponent<ParticleSystem>();
                particleSystem.Stop();

            }

        }

    }

}
