using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : MonoBehaviour
{
    public float range = 1.2f;

    public float longrange = 3.2f;

    public float power = 20;

    public float longpower = 40;

    public int pictureOffset;

    private float timeStart;

    public AudioClip audioSwipe;

    public AudioClip audioHit;

    public void Awake()
    {
        pictureOffset = Random.Range(0, 8);
        timeStart = -1;
        GameHud.Instance.pawSwipe.clearSwipe();
    }

    public void startAttack(Player playerSource)
    {
        timeStart = Time.realtimeSinceStartup;
        RaycastHit hit = GameHud.Instance.getLastRaycast();
        playerSource.firstPersonController.playerCanMove = false;
        playerSource.firstPersonController.cameraCanMove = false;
        GameHud.Instance.pawSwipe.startSwipe();
    }

    public void endAttack(Player playerSource)
    {
        if(timeStart > 0 && (Time.realtimeSinceStartup - timeStart) > 1f)
            GameHud.Instance.questManager.completeQuest("questcatstrike");
        GameController.Instance.playerCat.animator.SetTrigger("Attack");
        GameHud.Instance.pawSwipe.endSwipe();

        playerSource.firstPersonController.playerCanMove = true;
        playerSource.firstPersonController.cameraCanMove = true;

        GameController­.Instance.audioSource.PlayOneShot(audioSwipe);

        RaycastHit hit = GameHud.Instance.getLastRaycast();
        if (!hitValid(hit))
        {
            hit = offsetRaycast(GameHud.Instance.getLastRay(), true, true);
            if (!hitValid(hit))
            {
                hit = offsetRaycast(GameHud.Instance.getLastRay(), false, false);
                if (!hitValid(hit))
                {
                    hit = offsetRaycast(GameHud.Instance.getLastRay(), true, false);
                    if (!hitValid(hit))
                    {
                        hit = offsetRaycast(GameHud.Instance.getLastRay(), false, true);
                    }
                }

            }
        }
        //

        if (hit.rigidbody != null && ((GameController.Instance.canStrongPower() && hit.distance < longrange) || hit.distance < range))
        {
            Vector3 angle = GameHud.Instance.getLastRay().direction;
            angle.y = Mathf.Max(angle.y, 0.35f);
            float attackTime = Time.realtimeSinceStartup - timeStart;

            if (GameController.Instance.canStrongPower())
            {
                FlowerPot flowerPot = hit.transform.GetComponent<FlowerPot>();
                if (flowerPot != null && (Time.realtimeSinceStartup - timeStart) > 0.6f)
                    flowerPot.breakPot();
                else
                {
                    DoorDestroy doorDestroy = hit.transform.GetComponent<DoorDestroy>();
                    if (doorDestroy != null && (Time.realtimeSinceStartup - timeStart) > 0.6f)
                    {
                        doorDestroy.doorDestroy(hit.point, hit.normal);
                        GameController­.Instance.audioSource.PlayOneShot(audioHit);
                    }
                    else
                        hit.rigidbody.AddForce(angle * attackTime * longpower, ForceMode.Force);
                }
            }
            else
                hit.rigidbody.AddForce(angle * attackTime * power, ForceMode.Force);

            if (hit.rigidbody != null)
            {
                GameController­.Instance.audioSource.PlayOneShot(audioHit);
                hit.rigidbody.AddRelativeTorque(new Vector3(Random.Range(30, 150), Random.Range(30, 150), Random.Range(30, 150)));
            }
            
            if(hit.transform.gameObject.tag == "Picture")
                GameController.Instance.pictureTaker.takePicture(playerSource.transform, hit.transform, pictureTitle());
        }
    }
    
    public RaycastHit offsetRaycast(Ray ray, bool up, bool right)
    {
        RaycastHit hit;
        if(!up)
            ray.origin -= Camera.main.transform.up * 20f;
        else
            ray.origin += Camera.main.transform.up * 20f;

        if (!right)
            ray.origin -= Camera.main.transform.right * 20f;
        else
            ray.origin += Camera.main.transform.right * 20f;

        if (GameController.Instance.catActive)
            Physics.Raycast(ray, out hit, 100f, GameHud.Instance.layerMaskCat);
        else
            Physics.Raycast(ray, out hit, 100f, GameHud.Instance.layerMaskHuman);

        return hit;
    }

    public bool hitValid(RaycastHit hit)
    {
        return (hit.rigidbody != null && ((GameController.Instance.canStrongPower() && hit.distance < longrange) || hit.distance < range));
    }

    public string pictureTitle()
    {
        string[] titleList = new string[]
        {
            "Shattering discovery",
            "Do you even lift, bro?",
            "So jarring",
            "More than One Piece",
            "Knockout!",
            "Cracked the case",
            "Pathetic",
            "You got Cootsed!"
        };

        pictureOffset = (pictureOffset + 1 + titleList.Length) % titleList.Length;

        return titleList[pictureOffset];
    }
}
