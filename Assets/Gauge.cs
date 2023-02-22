using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    public Image image;

    public void setValue(float value, float max)
    {
        image.fillAmount = value / max;
    }
}
