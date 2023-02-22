using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICollectibleBox : MonoBehaviour
{
    public enum TYPE { LIGHTNING, MADNESS, BROKEN, WHITE_CARD, BLACK_CARD, FIRE }
    
    public TextMeshProUGUI text;

    public TYPE type;

    public void OnEnable()
    {
        if (GameHud.Instance != null && GameHud.Instance.catsAgainstHumanity != null)
        {
            switch (type)
            {
                case TYPE.WHITE_CARD:
                    setValue(GameHud.Instance.catsAgainstHumanity.getValue("CardWhite"), GameHud.Instance.catsAgainstHumanity.getTotal("CardWhite"));
                    break;

                case TYPE.BLACK_CARD:
                    setValue(GameHud.Instance.catsAgainstHumanity.getValue("CardBlack"), GameHud.Instance.catsAgainstHumanity.getTotal("CardBlack"));
                    break;

                case TYPE.BROKEN:
                    setValue(GameController.Instance.getPlantBreak(), GameController.Instance.getPlantBreakTotal());
                    break;

                case TYPE.FIRE:
                    setValue(GameController.Instance.getLaserFires(), GameController.Instance.getLaserFiresTotal());
                    break;
            }
        }
    }

    public void setValue(int value, int max)
    {
        text.text = $"{ value.ToString() } / { max.ToString() }";
    }
}
