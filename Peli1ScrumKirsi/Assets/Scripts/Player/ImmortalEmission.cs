using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
//[DebuggerStepThrough]
public class ImmortalEmission : MonoBehaviour
{
    public bool bloodEmissionOn;
    public bool lavaEmissionOn;
    public bool FallBloodEmissionOn;
    public bool FallBlueEmissionOn;
    GameObject player;
    Transform lavaGameObject;
    Transform bloodGameObject;
    Transform fallGameObject;
    ParticleSystem lavaParticleSystem;
    ParticleSystem bloodParticleSystem;
    ParticleSystem fallParticleSystem;
    ParticleSystem.EmissionModule lavaEmission;
    ParticleSystem.EmissionModule bloodEmission;
    ParticleSystem.EmissionModule fallEmission;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;

        if (lavaEmissionOn)
        {
            lavaGameObject = transform.Find("ImmortalityMomentLava");
            lavaParticleSystem = lavaGameObject.GetComponent<ParticleSystem>();
            lavaEmission = lavaParticleSystem.emission;
            lavaEmission.enabled = true;
        }
        else if (!lavaEmissionOn)
        {
            lavaGameObject = transform.Find("ImmortalityMomentLava");
            lavaParticleSystem = lavaGameObject.GetComponent<ParticleSystem>();
            lavaEmission = lavaParticleSystem.emission;
            lavaEmission.enabled = false;
        }
        if (bloodEmissionOn)
        {
            bloodGameObject = transform.Find("ImmortalityMomentBlood");
            bloodParticleSystem = bloodGameObject.GetComponent<ParticleSystem>();
            bloodEmission = bloodParticleSystem.emission;
            bloodEmission.enabled = true;
        }
        else if (!bloodEmissionOn)
        {
            bloodGameObject = transform.Find("ImmortalityMomentBlood");
            bloodParticleSystem = bloodGameObject.GetComponent<ParticleSystem>();
            bloodEmission = bloodParticleSystem.emission;
            bloodEmission.enabled = false;
        }
        if (FallBloodEmissionOn)
        {
            FallBlueEmissionOn = false;
            fallGameObject = transform.Find("ImmortalityMomentFallBlood");
            fallParticleSystem = fallGameObject.GetComponent<ParticleSystem>();
            fallEmission = fallParticleSystem.emission;
            fallEmission.enabled = true;
        }
        else if(!FallBloodEmissionOn)
        {
            fallGameObject = transform.Find("ImmortalityMomentFallBlood");
            fallParticleSystem = fallGameObject.GetComponent<ParticleSystem>();
            fallEmission = fallParticleSystem.emission;
            fallEmission.enabled = false;
        }
        if (FallBlueEmissionOn)
        {
            FallBloodEmissionOn = false;
            fallGameObject = transform.Find("ImmortalityMomentFallBlue");
            fallParticleSystem = fallGameObject.GetComponent<ParticleSystem>();
            fallEmission = fallParticleSystem.emission;
            fallEmission.enabled = true;
        }
        else if (!FallBlueEmissionOn)
        {
            fallGameObject = transform.Find("ImmortalityMomentFallBlue");
            fallParticleSystem = fallGameObject.GetComponent<ParticleSystem>();
            fallEmission = fallParticleSystem.emission;
            fallEmission.enabled = false;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LavaEmissionOn()
    {

        if (lavaEmissionOn)
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
        if (lavaEmissionOn)
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
        if (bloodEmissionOn)
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
        if (bloodEmissionOn)
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
        if (FallBloodEmissionOn)
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
        if (FallBlueEmissionOn)
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
        if (FallBloodEmissionOn)
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
        if (FallBlueEmissionOn)
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
