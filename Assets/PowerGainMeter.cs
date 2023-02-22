using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PowerGainMeter : MonoBehaviour
{
    public bool isLightningBolt;

    public GameObject objectPower1;
    public GameObject objectPower2;
    public GameObject objectPower3;

    public Image imagePower1;
    public Image imagePower2;
    public Image imagePower3;

    public GameObject objectMadness1;
    public GameObject objectMadness2;
    public GameObject objectMadness3;

    public Image imageMadness1;
    public Image imageMadness2;
    public Image imageMadness3;

    public GameObject spacing;

    private Tween tween;

    private void Awake()
    {
        Vector3 position = transform.localPosition;
        position.y = 670f;
        transform.localPosition = position;
    }

    public void pauseShow(bool show)
    {
        Vector3 position = transform.localPosition;
        if (show)
            position.y = 330f;
        else
            position.y = 670f;
        transform.localPosition = position;

        objectPower1.SetActive(show);
        objectPower2.SetActive(show);
        objectPower3.SetActive(show);
        objectMadness1.SetActive(show);
        objectMadness2.SetActive(show);
        objectMadness3.SetActive(show);
        spacing.SetActive(show);

        if (GameController.Instance != null)
        {
            init(GameController.Instance.getPower(isLightningBolt), GameController.Instance.getThreshold(isLightningBolt, 1), GameController.Instance.getThreshold(isLightningBolt, 2), GameController.Instance.getThreshold(isLightningBolt, 3), imagePower1, imagePower2, imagePower3);
            init(GameController.Instance.getPower(!isLightningBolt), GameController.Instance.getThreshold(!isLightningBolt, 1), GameController.Instance.getThreshold(!isLightningBolt, 2), GameController.Instance.getThreshold(!isLightningBolt, 3), imageMadness1, imageMadness2, imageMadness3);
        }
    }

    public void gainPower(bool isLightningBolt, int value)
    {
        /*Vector3 position = transform.localPosition;
        position.y = 670f;*/

        //Debug.Log($"gainPower: { isLightningBolt } / {value }");

        gameObject.SetActive(true);

        spacing.SetActive(false);
        objectPower1.SetActive(isLightningBolt);
        objectPower2.SetActive(isLightningBolt);
        objectPower3.SetActive(isLightningBolt);
        objectMadness1.SetActive(!isLightningBolt);
        objectMadness2.SetActive(!isLightningBolt);
        objectMadness3.SetActive(!isLightningBolt);
        
        if(isLightningBolt)
            init(GameController.Instance.getPower(isLightningBolt), GameController.Instance.getThreshold(isLightningBolt, 1), GameController.Instance.getThreshold(isLightningBolt, 2), GameController.Instance.getThreshold(isLightningBolt, 3), imagePower1, imagePower2, imagePower3);
        else
            init(GameController.Instance.getPower(isLightningBolt), GameController.Instance.getThreshold(isLightningBolt, 1), GameController.Instance.getThreshold(isLightningBolt, 2), GameController.Instance.getThreshold(isLightningBolt, 3), imageMadness1, imageMadness2, imageMadness3);

        tween?.Kill(false);
        StopAllCoroutines();
        StartCoroutine("GainPower");
    }

    public IEnumerator GainPower()
    {
        if (transform.transform.localPosition.y > 330f)
            tween = transform.DOLocalMoveY(330f, 0.4f);
        yield return new WaitForSeconds(0.4f);

        //init(GameController.Instance.getPower(isLightningBolt), GameController.Instance.getThreshold(isLightningBolt, 1), GameController.Instance.getThreshold(isLightningBolt, 2), GameController.Instance.getThreshold(isLightningBolt, 3));
        yield return new WaitForSeconds(2f);

        tween = transform.DOLocalMoveY(670f, 0.4f);
        yield return new WaitForSeconds(0.4f);
    }

    public void init(int value, int marker1, int marker2, int marker3, Image img1, Image img2, Image img3)
    {
        //Debug.Log($"init: { value } / { marker1 } // { img1 }");
        if (value >= marker1)
        {
            img1.fillAmount = 1f;

            if (value >= marker2)
            {
                img2.fillAmount = 1f;
                if (value >= marker3)
                    imagePower3.fillAmount = 1f;
                else
                    img3.fillAmount = (float)(value - marker2) / (float)(marker3 - marker2);
            }
            else
            {
                img2.fillAmount = (float)(value - marker1) / (float)(marker2 - marker1);
                img3.fillAmount = 0f;
            }

        }
        else
        {
            img1.fillAmount = (float)value / (float)marker1;
            img2.fillAmount = 0f;
            img3.fillAmount = 0f;
        }
    }
}
