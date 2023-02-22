using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPot : MonoBehaviour
{
    private bool bBroken = false;

    public bool bRemoveColliderOnBroken = false;
    
    public Rigidbody body;
    
    public AudioSource audioSourceBreak;

    public GameObject goFull;

    public GameObject goBroken;

    void Start()
    {
        GameController.Instance.addTotalPlantCounter();
        init();
    }

    void init()
    {
        bBroken = false;
        if(goFull != null)
            goFull.SetActive(true);
        if (goBroken != null)
            goBroken.SetActive(false);
        if(body != null)
            body.isKinematic = false;
    }

    public void breakPot()
    {
        if (!bBroken)
        {
            bBroken = true;
            if (goFull != null)
                goFull.SetActive(false);
            if (goBroken != null)
                goBroken.SetActive(true);

            if (bRemoveColliderOnBroken)
                Array.ForEach(GetComponentsInChildren<Collider>(), x => x.enabled = false);

            if(audioSourceBreak != null)
                audioSourceBreak.Play();
            if (body != null)
                body.velocity = new Vector3(0, body.velocity.y, 0);
            GameController.Instance.addPlantCounter();
            GameController.Instance.spawner.spawnAt("Madness", this.transform.position, Vector3.up, GameController.Instance.transform);

            this.transform.eulerAngles = Vector3.zero;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"OnCollisionEnter: { collision.contacts[0].otherCollider.name } // Mag:{ body.velocity.magnitude } // { collision.contacts[0].normal }");
        if (!bBroken && (body != null && body.velocity.magnitude > 1.5f))
        {
            breakPot();
        }

        if (bBroken)
        {
            Array.ForEach<ContactPoint>(collision.contacts, e =>
            {
                if (e.normal == Vector3.up)
                {
                    this.transform.position = collision.contacts[0].point;
                    body.isKinematic = true;
                }
            });
        }
    }
}
