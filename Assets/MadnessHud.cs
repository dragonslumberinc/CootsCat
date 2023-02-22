using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MadnessHud : MonoBehaviour
{
    public AudioSource audioMusic;
    public AudioSource audioMusicMadness;

    public TextMeshProUGUI mainText;

    public TextMeshProUGUI[] smallTexts;

    public RawImage imageFull;

    public Texture[] textures;
    public int textureCycle;

    private int position = 0;

    public List<string> listStrings;

    public void clear()
    {
        StopAllCoroutines();
        setLevel(0);
    }

    public void setStrings(string main, string[] sub)
    {
        mainText.text = main;
        listStrings.Clear();
        listStrings.AddRange(sub);
        
        StartCoroutine(UpdateText());
    }

    public void setLevel(int level)
    {
        if(level == 0)
        {
            imageFull.color = new Color(1f, 1f, 1f, 0.0f);
            Array.ForEach(smallTexts, x => x.gameObject.SetActive(false));
            mainText.text = "";

            audioMusic.volume = 1f;
            audioMusicMadness.volume = 0f;
        }
        else if (level == 1)
        {
            imageFull.color = new Color(1f, 1f, 1f, 0.05f);
            StartCoroutine(UpdateTextures());

            audioMusic.volume = 0.6f;
            audioMusicMadness.volume = 0.4f;
        }
        else if (level == 2)
        {
            imageFull.color = new Color(1f, 1f, 1f, 0.12f);
            StartCoroutine(UpdateTextures());

            audioMusic.volume = 0.1f;
            audioMusicMadness.volume = 0.85f;
        }
    }

    public IEnumerator UpdateText()
    {
        if (listStrings.Count > 0)
        {
            do
            {
                int pos = position;
                smallTexts[position].gameObject.SetActive(true);
                smallTexts[position].transform.localPosition = new Vector2(UnityEngine.Random.Range(-850f, 850f), UnityEngine.Random.Range(-500f, 500f));
                smallTexts[position].color = new Color(1f, 0f, 0f, 0f);
                smallTexts[position].DOColor(new Color(1f, 0f, 0f, 0.5f), 0.5f).OnComplete(() => smallTexts[pos].DOColor(new Color(1f, 0f, 0f, 0f), 0.5f).OnComplete(() => smallTexts[pos].gameObject.SetActive(false)));
                smallTexts[position].text = listStrings[UnityEngine.Random.Range(0, listStrings.Count)];
                //smallTexts[position].DOColor()
                //smallTexts
                yield return new WaitForSeconds(0.1f);
                position = (position + 1) % smallTexts.Length;
                mainText.transform.localPosition = new Vector2(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
            } while (true);
        }
    }

    public IEnumerator UpdateTextures()
    {
        Array.ForEach(smallTexts, x => x.text = "");
        do
        {
            textureCycle = (textureCycle + 1) % textures.Length;
            imageFull.texture = textures[textureCycle];
            yield return new WaitForSeconds(0.08f);
        } while (true);
    }
}
