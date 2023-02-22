using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class EndGameSequence : MonoBehaviour
{
    public Camera camera;

    public Transform camera1;

    public Transform camera2;

    public Transform camera3;

    public Transform camera3b;

    public Transform cat1;

    public Transform cat2;

    public Transform cat2Jump;

    public Transform cat3;

    public Transform sittingPos;

    public Transform standingPos;

    public TextMeshProUGUI textWhite;

    public TextMeshProUGUI textWhiteProduced;

    public AudioClip audioPurr;

    private bool nextEvent;

    void Awake()
    {
        camera.gameObject.SetActive(false);
        textWhite.text = "";
        textWhiteProduced.color = new Color(1f, 1f, 1f, 0f);
        textWhiteProduced.gameObject.SetActive(true);
    }

    public void startEndGame()
    {
        StartCoroutine("EndGameCoRoutine");
    }

    public IEnumerator EndGameCoRoutine()
    {
        GameHud.Instance.madnessHud.clear();
        nextEvent = false;
        GameHud.Instance.fadeInOut.fadeToBlack(() => { nextEvent = true; });
        yield return new WaitUntil(() => { return nextEvent; });

        GameHud.Instance.humanHud.gameObject.SetActive(false);
        GameHud.Instance.catsAgainstHumanity.gameObject.SetActive(false);
        GameHud.Instance.questManager.gameObject.SetActive(false);

        GameController.Instance.playerHuman.setActive(false);
        GameController.Instance.playerCat.setActive(false);
        GameController.Instance.playerHuman.body.isKinematic = true;
        GameController.Instance.playerCat.body.isKinematic = true;

        camera.gameObject.SetActive(true);
        camera.transform.position = camera1.position;
        camera.transform.LookAt(sittingPos);

        GameController.Instance.playerHuman.animator.SetFloat("MoveSpeed", 0f);
        GameController.Instance.playerHuman.transform.position = standingPos.transform.position;
        GameController.Instance.playerHuman.transform.forward = sittingPos.transform.forward;

        GameController.Instance.playerCat.animator.SetFloat("MoveSpeed", 0f);
        GameController.Instance.playerCat.transform.position = cat1.transform.position;
        GameController.Instance.playerCat.transform.forward = cat1.transform.forward;

        nextEvent = false;
        GameHud.Instance.fadeInOut.fadeBack(() => { nextEvent = true; });
        yield return new WaitUntil(() => { return nextEvent; });

        // *** HUMAN SITTING ***
        yield return new WaitForSeconds(0.6f);
        GameController.Instance.playerHuman.animator.SetTrigger("Sitting");
        GameController.Instance.playerHuman.transform.DOMove(sittingPos.transform.position, 1f);

        yield return new WaitForSeconds(1.3f);

        // *** CAT WALK TO 2 ***
        GameController.Instance.playerCat.transform.DOMove(cat2.transform.position, 0.8f);
        GameController.Instance.playerCat.animator.SetFloat("MoveSpeed", 2f);
        yield return new WaitForSeconds(0.8f);

        GameController.Instance.playerCat.animator.SetFloat("MoveSpeed", 0f);

        yield return new WaitForSeconds(0.3f);

        // *** CAT JUMP ***
        camera.transform.position = camera2.position;
        camera.transform.LookAt(GameController.Instance.playerCat.transform);

        yield return new WaitForSeconds(0.8f);

        GameController.Instance.playerCat.transform.DOMove(cat2Jump.transform.position, 0.3f);
        GameController.Instance.playerHuman.animator.SetBool("Jump", true);

        yield return new WaitForSeconds(0.5f);

        // *** CAT ON LAP ***
        camera.transform.position = camera3.position;
        camera.transform.LookAt(GameController.Instance.playerCat.transform);

        GameController­.Instance.audioSource.loop = true;
        GameController­.Instance.audioSource.clip = audioPurr;
        GameController­.Instance.audioSource.Play();
        GameController.Instance.playerCat.transform.position = cat3.transform.position;
        GameController.Instance.playerCat.animator.SetTrigger("Eat");

        // *** CAMERA MOVE BACK ***
        camera.transform.DOMove(camera3b.position, 10f);
        bool fadeStarted = false;
        float time = 7f;
        do
        {
            camera.transform.LookAt(GameController.Instance.playerCat.transform);
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();

            if (time < 1.5f && !fadeStarted)
            {
                fadeStarted = true;
                GameHud.Instance.fadeInOut.fadeToBlack(null);
            }

        } while (time > 0f);

        yield return new WaitForSeconds(0.2f);
        textWhite.color = new Color(1f, 1f, 1f, 0f);
        textWhite.text = "Nothing matters but Coots";
        textWhite.DOColor(Color.white, 1f);

        yield return new WaitForSeconds(3f);
        textWhite.DOColor(new Color(1f, 1f, 1f, 0f), 1f);
        yield return new WaitForSeconds(1.2f);
        textWhite.text = "I would do anything for Coots";
        textWhite.DOColor(Color.white, 1f);

        yield return new WaitForSeconds(3f);
        textWhite.DOColor(new Color(1f, 1f, 1f, 0f), 1f);
        yield return new WaitForSeconds(1.2f);
        textWhite.text = "Everything is Coots";
        textWhite.DOColor(Color.white, 1f);

        yield return new WaitForSeconds(1f);

        textWhiteProduced.color = new Color(1f, 1f, 1f, 0f);
        textWhiteProduced.gameObject.SetActive(true);
        textWhiteProduced.DOColor(new Color(1f, 1f, 1f, 1f), 1f);

        yield return new WaitForSeconds(3f);
        // 
        // 

        //
#if !(UNITY_WEBGL || UNITY_WEBGL_API)
        if (Input.anyKeyDown || Input.GetMouseButton(0) ||  || Input.GetMouseButton(1))
        {
            Application.Quit();
        }
#endif
    }
}
