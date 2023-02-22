using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PowerGainDisplay : MonoBehaviour
{
    public List<string> gainComplete;
    
    public TextMeshProUGUI textTitle;

    public TextMeshProUGUI textSub;

    public CanvasGroup canvasGroup;

    void Awake()
    {
        gainComplete = new List<string>();
    }

    public void gainPower(bool power, int value)
    {
        if(power)
        {
            if(value == 1 && !gainComplete.Contains("power1"))
            {
                GameHud.Instance.questManager.addQuest(true, "questcatbreakstuff");
                textTitle.text = "Gained Super Strength";
                textSub.text = "Hold LMB to charge your attack";
                gainComplete.Add("power1");
                gameObject.SetActive(true);
                StartCoroutine("FadeInOut");
            }
            else if (value == 2 && !gainComplete.Contains("power2"))
            {
                GameHud.Instance.questManager.addQuest(true, "questcateyelasers");
                textTitle.text = "Gained Eye Lasers";
                textSub.text = "Hold RMB to watch the world burn";
                gainComplete.Add("power2");
                gameObject.SetActive(true);
                StartCoroutine("FadeInOut");
            }
            else if (value == 3 && !gainComplete.Contains("power3"))
            {
                GameHud.Instance.questManager.addQuest(true, "questcatflight");
                GameHud.Instance.questManager.addQuest(true, "questcatlower");
                textTitle.text = "Gained Flight";
                textSub.text = "Hold Space to move up\nHold C to move down";
                gainComplete.Add("power3");
                gameObject.SetActive(true);
                StartCoroutine("FadeInOut");
            }
        }
        else
        {
            if (value == 1 && !gainComplete.Contains("madness1"))
            {
                GameHud.Instance.questManager.addQuest(true, "questmadness1");
                textTitle.text = "Gained Possession Powers";
                textSub.text = "Press Q and the human will be in your control";
                gainComplete.Add("madness1");
                gameObject.SetActive(true);
                StartCoroutine("FadeInOut");
            }
            else if (value == 2 && !gainComplete.Contains("madness2"))
            {
                GameHud.Instance.questManager.addQuest(true, "questcatmadness");
                textTitle.text = "Gained Full Possession Powers";
                textSub.text = "Get him to the couch and get your cuddles";
                gainComplete.Add("madness2");
                gameObject.SetActive(true);
                StartCoroutine("FadeInOut");
            }
        }
    }

    public IEnumerator FadeInOut()
    {
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, 0.5f);
        yield return new WaitForSeconds(4f);


        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
