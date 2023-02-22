using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStart : MonoBehaviour
{
    public Transform[] tansformsSetActive;

    void Awake()
    {
        Array.ForEach(tansformsSetActive, transform =>
        {
            transform.gameObject.SetActive(true);
        });
    }
}
