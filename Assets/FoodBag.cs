using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBag : HumanInteractable
{

    public override void use()
    {
        if (gameObject.activeSelf)
        {
            GameController.Instance.gainItem("FoodBag");
            GameHud.Instance.questManager.completeQuest("questhumanfeed");
            GameHud.Instance.questManager.addQuest(false, "questhumanbowl");
            gameObject.SetActive(false);
        }
    }
}
