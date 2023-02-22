using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollider : MonoBehaviour
{
    public ParticleSystem particleSystem;
    // Start is called before the first frame update
    //public On
    void OnEnable()
    {
        //GameController.Instance.playerCat.addParticleSystem(particleSystem);
    }

    /*public void OnParticleCollision(GameObject other)
    {
        List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
        Debug.Log($"ParticleCollider OnParticleCollision");
    }*/
    
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
            
            GameController.Instance.gainPower(gameObject.tag == "LightningBolt", numEnter);
        }
    }

}
