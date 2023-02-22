using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameInteractable : HumanInteractable
{

    public override void use()
    {
        if (GameController.Instance.getPower(false) >= GameController.Instance.getThreshold(false, 2))
            GameController.Instance.endGame();
        else
            GameController.Instance.playerHuman.sayNoCouch();
    }
}
