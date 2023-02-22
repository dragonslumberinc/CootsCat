using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    public enum TYPE { SWIPE, POSSESS, EYE_LASER, FLYING }
    private bool complete = false;
    public TYPE type;
    private float fCount = 1f;

    void Update()
    {
        switch(type)
        {
            case TYPE.POSSESS:
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    complete = true;
                    hide();
                }
                break;

            case TYPE.EYE_LASER:
                if (Input.GetMouseButton(1))
                {
                    fCount -= Time.deltaTime;
                    complete = fCount <= 0;
                    if(complete)
                        hide();
                }
                break;

            case TYPE.SWIPE:
                if (Input.GetMouseButtonDown(0))
                {
                    fCount = 1f;
                }
                if (Input.GetMouseButton(0))
                {
                    fCount -= Time.deltaTime;
                    complete = fCount <= 0;
                    if (complete)
                        hide();
                }
                break;

            case TYPE.FLYING:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    fCount -= 0.35f;
                    complete = fCount <= 0;
                    if (complete)
                        hide();
                }
                break;
        }
    }

    public void updateDisplay()
    {
        if(!complete)
        {
            show();
        }
        else
        {
            hide();
        }
    }

    private void show()
    {
        gameObject.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }
}
