using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public bool bIsCat;

    public Animator animator;
    
    public Rigidbody body;
    
    public Camera camera;

    public AudioListener audioListener;

    public Volume volume;

    public FirstPersonController firstPersonController;

    public AudioClip[] sfxVoices;

    public AudioSource audioVoice;

    public AudioClip jumpClip;

    public int jumpClipCount = 0;

    public Transform initPosition;

    private int lastSFXVoice;

    private bool jumping = false;

    private Vector3 jumpingStartPos;

    void Awake()
    {
        lastSFXVoice = UnityEngine.Random.Range(0, sfxVoices.Length);
    }

    void Start()
    {
        init();
    }

    void init()
    {
        transform.position = initPosition.position;
        transform.eulerAngles = initPosition.eulerAngles;
    }

    void Update()
    {
        if(firstPersonController.readInputs)
        {
            float _playerSpeed = body.velocity.magnitude;
            animator.SetFloat("MoveSpeed", _playerSpeed);

            if(!firstPersonController.isGrounded && !jumping)
            {
                jumpingStartPos = this.transform.position;
                jumping = true;
            }
        }
    }

    public void setActive(bool _bActive)
    {
        camera.enabled = _bActive;
        audioListener.enabled = _bActive;
        firstPersonController.readInputs = _bActive;
        animator.SetFloat("MoveSpeed", 0f);
        //volume.enabled = _bActive;

        //body.isKinematic = !_bActive;
    }

    public void jump()
    {

    }

    public void talk()
    {
        audioVoice.PlayOneShot(sfxVoices[lastSFXVoice]);

        if(sfxVoices.Length > 1)
        {
            int random = 0;
            do
            {
                random = UnityEngine.Random.Range(0, sfxVoices.Length);
            } while(random == lastSFXVoice);
            lastSFXVoice = random;
        }
    }

    /*public void OnParticleCollision(GameObject other)
    {
        Debug.Log($"PLAYER OnParticleCollision: { other }");
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, this.gameObject, collisionEvents);

        //Debug.Log($"PLAYER OnParticleTrigger: { collisionEvents.Count }");
        ParticleSystem.Particle[] particles= new ParticleSystem.Particle[0];

        int count = particleSystem.GetParticles(particles);

        particles[0].
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            collisionEvents[i].colliderComponent
            collisionEvents[i].
            particle = collisionEvents[i].colliderComponent.GetComponent<Particle>();
            particle.remainingLifetime = 0;
        }

        if(particleSystem.tag == "LightningBolt")
        {

        }
    }*/


    public void OnCollisionEnter(Collision collision)
    {
        ///Debug.Log($"OnCollisionEnter: { collision.gameObject} // { collision.gameObject.layer }  // { collision.gameObject.tag }");
        if (jumping)
        {
            Array.ForEach<ContactPoint>(collision.contacts, e =>
            {
                if (e.normal.y == 1)
                {
                    jumping = false;
                    if (jumpClipCount <= 0)
                    {
                        audioVoice.PlayOneShot(jumpClip);
                        jumpClipCount = 10;
                    }
                    jumpClipCount--;
                }
            });
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CardBlack" || other.gameObject.tag == "CardWhite") // CAH Black / White
            GameHud.Instance.catsAgainstHumanity.getCard(other.transform);
    }

    public void sayNoCouch()
    {
        Debug.Log($"sayNoCouch");
    }
}
