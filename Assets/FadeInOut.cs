using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class FadeInOut : MonoBehaviour
{
    public RawImage imgBlack;
    // Start is called before the first frame update
    void Start()
    {
        imgBlack.color = new Color(0f, 0f, 0f, 0f);
    }

    public void fadeToBlack(UnityAction action)
    {
        imgBlack.DOColor(Color.black, 1f).OnComplete(() => { action?.Invoke(); });
    }

    public void fadeBack(UnityAction action)
    {
        imgBlack.DOColor(new Color(0f, 0f, 0f, 0f), 1f).OnComplete(() => { action?.Invoke(); });
    }
}
