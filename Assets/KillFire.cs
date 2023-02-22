using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFire : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine("KillUpdate");
    }

    public IEnumerator KillUpdate()
    {
        yield return new WaitForSeconds(8);
        gameObject.SetActive(false);
    }
}
