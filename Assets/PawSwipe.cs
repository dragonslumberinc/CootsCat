using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PawSwipe : MonoBehaviour
{
    public RawImage pawImage;

    public void clearSwipe()
    {
        pawImage.gameObject.SetActive(false);
    }

    public void startSwipe()
    {
        pawImage.transform.localPosition = Vector2.zero;
        StartCoroutine("ChargeSwipe");
    }

    public IEnumerator ChargeSwipe()
    {
        yield return new WaitForSeconds(0.15f);
        pawImage.gameObject.SetActive(true);
        float time = 0;
        do
        {
            pawImage.rectTransform.sizeDelta = getSize(time);
            pawImage.transform.localPosition = getPosition(time);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        } while (true);
    }

    public Vector2 getSize(float time)
    {
        float addSize = 50 * Mathf.Pow(time, 0.7f);
        return new Vector2(75 + addSize, 75 + addSize);
    }

    public Vector2 getPosition(float time)
    {
        Vector2 pos = new Vector2(Random.Range(-15f, 15f), Random.Range(-15f, 15f));
        return pos * Mathf.Pow(time, 0.5f);
    }

    public void endSwipe()
    {
        StopAllCoroutines();
        StartCoroutine("EndChargeSwipe");
    }

    public IEnumerator EndChargeSwipe()
    {
        pawImage.transform.localPosition = new Vector2(450, 300);
        pawImage.transform.DOLocalMove(new Vector2(-450, -300), 0.18f);
           yield return new WaitForSeconds(0.25f);
        pawImage.gameObject.SetActive(false);
    }
}
