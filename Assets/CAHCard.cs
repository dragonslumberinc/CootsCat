using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAHCard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameHud.Instance.catsAgainstHumanity.addTotal(this.transform);
    }
}
