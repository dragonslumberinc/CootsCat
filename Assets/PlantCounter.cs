using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantCounter : MonoBehaviour
{
    public RawImage img;

    public TextMeshProUGUI txtText;

    public Texture texturePlant;

    private float timerRemaining;

    public void Start()
    {
        //Debug.Log($"PlantCounter: { this.transform.localPosition }");
        this.gameObject.SetActive(false);
    }

    public void addPlant(Texture texture, int count, int total)
    {
        if (texture == null)
            texture = texturePlant;
        img.texture = texture;
        this.gameObject.SetActive(true);
        txtText.text = $"{ count } / { total }";
        timerRemaining = 2;
        StartCoroutine("CoRoutinePlant");
    }

    public IEnumerator CoRoutinePlant()
    {
        float totalTime = 0.4f;
        Vector3 inPos = new Vector3(722f, 322f, 0f);
        Vector3 outPos = new Vector3(1185f, 322f, 0f);
        this.transform.localPosition = outPos;

        Vector3 moveSpeed = (inPos - this.transform.localPosition) / totalTime;

        do
        {
            this.transform.localPosition += moveSpeed * Time.deltaTime;
            totalTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (totalTime > 0f);

        yield return new WaitForSeconds(3f);

        totalTime = 0.4f;
        moveSpeed = (outPos - this.transform.localPosition) / totalTime;
        do
        {
            this.transform.localPosition += moveSpeed * Time.deltaTime;
            totalTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (totalTime > 0f);

        this.gameObject.SetActive(false);
    }
}
