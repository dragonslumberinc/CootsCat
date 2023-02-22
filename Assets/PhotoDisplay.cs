using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotoDisplay : MonoBehaviour
{
    private string displayTitle;

    public Transform photoTransform;

    public Photo photo;

    private void Awake()
    {
        photoTransform.transform.localPosition = new Vector3(-521, 49, 0f);
        photoTransform.transform.localEulerAngles = new Vector3(0, 0, 40f);
    }

    public void showPhoto(Texture texture, string title)
    {
        //photoTransform.transform
        photo.photoImg.texture = texture;
        displayTitle = title;
        StopAllCoroutines();
        StartCoroutine($"PhotoRoutine");
    }

    public IEnumerator PhotoRoutine()
    {
        float totalTime = 0.4f;
        Vector3 inPos = new Vector3(38, -72, 0f);
        Vector3 outPos = new Vector3(-521, 49, 0f);

        photoTransform.gameObject.SetActive(true);
        photoTransform.localPosition = outPos;
        photoTransform.localEulerAngles = new Vector3(0, 0, 40f);
        photo.text.text = "";

        yield return new WaitForSeconds(1f);

        Vector3 moveSpeed = (inPos - photoTransform.localPosition) / totalTime;
        Vector3 rotateSpeed = (new Vector3(0, 0, 10f) - photoTransform.localEulerAngles) / totalTime;
        do
        {
            photoTransform.localPosition += moveSpeed * Time.deltaTime;
            photoTransform.localEulerAngles += rotateSpeed * Time.deltaTime;
            totalTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (totalTime > 0f);

        yield return new WaitForSeconds(0.2f);
        do
        {
            photo.text.text += displayTitle.Substring(0, 1);
            displayTitle = displayTitle.Substring(1);
            yield return new WaitForSeconds(0.02f);
        } while(displayTitle.Length > 0);


        yield return new WaitForSeconds(3f);

        photoTransform.gameObject.SetActive(false);
        photoTransform.localPosition = inPos;
        photoTransform.localEulerAngles = new Vector3(0, 0, 40f);
    }
}
