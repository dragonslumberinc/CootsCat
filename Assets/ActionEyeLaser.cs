using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEyeLaser : MonoBehaviour
{
    public Transform startLeft;
    public Transform startRight;

    public Transform laserLeft;
    public Transform laserRight;

    public ParticleSystem laserParticle;
    public ParticleSystem laserParticleSource;

    public AudioSource laserSoundSFX;

    public ParticleSystem psBurn;

    public Vector3 lastNormal;

    void Start()
    {
        endLaser();
    }

    public void startLaser()
    {
        laserLeft.gameObject.SetActive(true);
        laserRight.gameObject.SetActive(true);

        laserParticle.Play();
        laserParticleSource.Play();
        laserSoundSFX.Play();

        lastNormal = Vector3.zero;
        psBurn.Play();

        GameHud.Instance.questManager.completeQuest("questcateyelasers");
        StartCoroutine("UpdateLaser");
    }

    public IEnumerator UpdateLaser()
    {
        do
        {
            RaycastHit hit = GameHud.Instance.getLastRaycast();
            laserLeft.transform.position = (hit.point + startLeft.transform.position) / 2f;
            laserLeft.up = (hit.point - startLeft.transform.position).normalized;
            laserLeft.localScale = new Vector3(0.03f, hit.distance * 2f, 0.03f);

            laserRight.transform.position = (hit.point + startRight.transform.position) / 2f;
            laserRight.up = (hit.point - startRight.transform.position).normalized;
            laserRight.localScale = new Vector3(0.03f, hit.distance * 2f, 0.03f);

            laserParticle.transform.position = hit.point;
            laserParticle.transform.forward = hit.normal;

            if (hit.transform != null)
            {
                LaserPainting _laserPainting = hit.transform.GetComponent<LaserPainting>();
                if (_laserPainting != null)
                    _laserPainting.laserHit(hit.point, hit.normal);
                else
                {
                    FlowerPot _flowerPot = hit.transform.GetComponent<FlowerPot>();
                    if (_flowerPot != null)
                        _flowerPot.breakPot();

                    DoorDestroy doorDestroy = hit.transform.GetComponent<DoorDestroy>();
                    if (doorDestroy != null)
                        doorDestroy.doorDestroy(hit.point, hit.normal);

                    if (lastNormal != hit.normal)
                            psBurn.Stop();
                        psBurn.transform.forward = hit.normal;
                        psBurn.transform.position = hit.point + (hit.normal * 0.01f);

                        if (lastNormal != hit.normal)
                        {
                            psBurn.Play();
                            lastNormal = hit.normal;
                        }
                }
            }
            yield return new WaitForEndOfFrame();
        } while (true);
    }

    public void endLaser()
    {
        StopAllCoroutines();

        laserLeft.gameObject.SetActive(false);
        laserRight.gameObject.SetActive(false);
        laserParticle.Stop();
        laserSoundSFX.Stop();
        laserParticleSource.Stop();
        psBurn.Stop();
    }
}
