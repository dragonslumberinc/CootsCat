using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPainting : MonoBehaviour
{
    private bool wasTriggered = false;
    
    public Rigidbody body;

    private void Start()
    {
        GameController.Instance.addTotalLaserFiresCounter();
    }

    public virtual void laserHit(Vector3 pos, Vector3 normal)
    {
        if (!wasTriggered)
        {
            wasTriggered = true;
            if(body != null)
                body.isKinematic = false;
            GameController.Instance.spawner.spawnAt("Fire", pos, Vector3.up, this.transform);

            GameController.Instance.spawner.spawnAt("Madness", this.transform.position, normal, GameController.Instance.transform);
            GameController.Instance.addLaserFiresCounter();
            GameController.Instance.pictureTaker.takePicture(GameController.Instance.playerCat.transform, this.transform, GameController.Instance.pictureFireTitle(), 0.15f);
        }
    }
}
