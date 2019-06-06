using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class ImmortalEmission : MonoBehaviour
{
    public bool bloodEmission;
    public bool lavaEmission;
    public bool FallBloodEmission;
    public bool FallBlueEmission;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FallBloodEmission)
        {
            FallBlueEmission = false;
        }
        else if (FallBlueEmission)
        {
            FallBloodEmission = false;
        }
    }

    public void LavaEmissionOn()
    {

        if (lavaEmission)
        {
            var player = gameObject;
            if (player != null)
            {
                var lavaGameObject = transform.Find("ImmortalityMomentLava");
                if (lavaGameObject != null)
                {
                    var lavaParticleSystem = lavaGameObject.GetComponent<ParticleSystem>();
                    if (lavaParticleSystem != null)
                    {
                        lavaParticleSystem.Play();
                    }

                }
            }
        }


    }
    public void LavaEmissionOff()
    {
        if (lavaEmission)
        {
            var player = gameObject;
            if (player != null)
            {
                var lavaGameObject = transform.Find("ImmortalityMomentLava");
                if (lavaGameObject != null)
                {
                    var lavaParticleSystem = lavaGameObject.GetComponent<ParticleSystem>();
                    if (lavaParticleSystem != null)
                    {
                        lavaParticleSystem.Stop();
                    }

                }
            }
        }
       

    }
    public void BloodEmissionOn()
    {
        if (bloodEmission)
        {
            var player = gameObject;
            if (player != null)
            {
                var bloodGameObject = transform.Find("ImmortalityMomentBlood");
                if (bloodGameObject != null)
                {
                    var bloodParticleSystem = bloodGameObject.GetComponent<ParticleSystem>();
                    if (bloodParticleSystem != null)
                    {
                        bloodParticleSystem.Play();
                    }

                }
            }
        }
       

    }
    public void BloodEmissionOff()
    {
        if (bloodEmission)
        {
            var player = gameObject;
            if (player != null)
            {
                var bloodGameObject = transform.Find("ImmortalityMomentBlood");
                if (bloodGameObject != null)
                {
                    var bloodParticleSystem = bloodGameObject.GetComponent<ParticleSystem>();
                    if (bloodParticleSystem != null)
                    {
                        bloodParticleSystem.Stop();
                    }

                }
            }
        }
       

    }
    public void FallEmissionOn()
    {
        if (FallBloodEmission)
        {
            var player = gameObject;
            if (player != null)
            {
                var fallGameObject = transform.Find("ImmortalityMomentFallBlood");
                if (fallGameObject != null)
                {
                    var fallParticleSystem = fallGameObject.GetComponent<ParticleSystem>();
                    if (fallParticleSystem != null)
                    {
                        fallParticleSystem.Play();
                    }

                }
            }
        }
        if (FallBlueEmission)
        {
            var player = gameObject;
            if (player != null)
            {
                var fallGameObject = transform.Find("ImmortalityMomentFallBlue");
                if (fallGameObject != null)
                {
                    var fallParticleSystem = fallGameObject.GetComponent<ParticleSystem>();
                    if (fallParticleSystem != null)
                    {
                        fallParticleSystem.Play();
                    }

                }
            }
        }
        

    }
    public void FallEmissionOff()
    {
        if (FallBloodEmission)
        {
            var player = gameObject;
            if (player != null)
            {
                var fallGameObject = transform.Find("ImmortalityMomentFallBlood");
                if (fallGameObject != null)
                {
                    var fallParticleSystem = fallGameObject.GetComponent<ParticleSystem>();
                    if (fallParticleSystem != null)
                    {
                        fallParticleSystem.Stop();
                    }

                }
            }
        }
        if (FallBlueEmission)
        {
            var player = gameObject;
            if (player != null)
            {
                var fallGameObject = transform.Find("ImmortalityMomentFallBlue");
                if (fallGameObject != null)
                {
                    var fallParticleSystem = fallGameObject.GetComponent<ParticleSystem>();
                    if (fallParticleSystem != null)
                    {
                        fallParticleSystem.Stop();
                    }

                }
            }
        }
        

    }

}
