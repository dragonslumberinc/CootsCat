using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PossessionCamera : MonoBehaviour
{
    public Camera camera;

    public AudioListener audioListener;

    public AudioClip audioPossess;

    public void possess(Transform startPos, Transform endPos, bool catActiveTarget)
    {
        GameController­.Instance.audioSource.PlayOneShot(audioPossess);
        this.gameObject.SetActive(true);
        GameController.Instance.playerCat.setActive(false);
        GameController.Instance.playerHuman.setActive(false);

        camera.enabled = true;
        audioListener.enabled = true;

        camera.fieldOfView = 60f;

        camera.transform.position = startPos.position;
        camera.transform.LookAt(endPos.position);
        camera.transform.DOMove(endPos.position, 1f).OnComplete(() => { GameController.Instance.setCatActive(catActiveTarget); this.gameObject.SetActive(false); });
        camera.DOFieldOfView(120f, 1f);
    }
}
