using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollider : MonoBehaviour
{
    public ParticleSystem particleSystem;

    private int particleCount = 0;
    // Start is called before the first frame update
    //public On
    void OnEnable()
    {
        //GameController.Instance.playerCat.addParticleSystem(particleSystem);
        StartCoroutine("EndGrab");
    }

    public IEnumerator EndGrab()
    {
        particleCount = 35;
        yield return new WaitForSeconds(particleSystem.main.duration);
        if(particleCount > 0)
            GameController.Instance.gainPower(gameObject.tag == "LightningBolt", particleCount);

        Debug.Log($"EndGrab: { particleCount} / { GameController.Instance.getPower(gameObject.tag == "LightningBolt") }");
    }

    /*public void OnParticleCollision(GameObject other)
    {
        List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
        Debug.Log($"ParticleCollider OnParticleCollision");
    }*/

    public void OnParticleSystemStopped()
    {
        Debug.Log($"OnParticleSystemStopped");
    }

    public void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
        int numEnter = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        if (numEnter > 0)
        {
            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle particle = particles[i];
                particle.remainingLifetime = 0;
                particles[i] = particle;
            }
            particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

            particleCount -= numEnter;
            GameController.Instance.gainPower(gameObject.tag == "LightningBolt", numEnter);
        }
    }

}
