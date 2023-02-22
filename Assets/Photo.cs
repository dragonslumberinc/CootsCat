using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Photo : MonoBehaviour
{
    public RawImage photoImg;

    public TextMeshProUGUI text;


    public void showPhoto(Texture texture, string title)
    {
        //photoTransform.transform
        photoImg.texture = texture;
        text.text = title;
    }
}
