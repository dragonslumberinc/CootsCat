using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDestroy : MonoBehaviour
{
    public HumanInteractable humanInteractable;

    public BoxCollider collider;
    
    public void doorDestroy(Vector3 pos, Vector3 normalUp)
    {
        humanInteractable.enabled = false;
        collider.enabled = false;
        gameObject.SetActive(false);
        GameController.Instance.spawner.spawnAt("DoorShards", pos, normalUp, this.transform.parent);
        GameController.Instance.gainPower(false, 10);
    }
}
