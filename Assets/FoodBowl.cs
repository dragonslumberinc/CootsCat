using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBowl : HumanInteractable
{
    public Transform foodObject;

    public float foodEmptyY = -0.719f;
    public float foodFullY = -0.719f;
    private float quantity = 0f;

    private void Awake()
    {
        quantity = 0f;
        updateFoodHeight();
    }

    public override void use()
    {
        if (GameController.Instance.hasItem("FoodBag"))
        {
            GameHud.Instance.questManager.completeQuest("questhumanbowl");
            GameController.Instance.loseItem("FoodBag");
            fill();
        }
    }

    public void startEat()
    {
        if(quantity > 0)
        {
            StartCoroutine("UpdateEat");
            audioPlay.Play();
        }
    }

    public IEnumerator UpdateEat()
    {
        do
        {
            quantity -= Time.deltaTime * 0.34f;
            updateFoodHeight();
            yield return new WaitForEndOfFrame();
        } while(quantity > 0);
        GameHud.Instance.questManager.completeQuest("questcathumanfeed");
        GameHud.Instance.questManager.completeQuest("questcatbowltoeat");
        GameHud.Instance.questManager.addQuest(true, "questbreakstuff2");
        GameController.Instance.spawner.spawnAt("LightningBolts", this.transform.position, Vector3.up, GameController.Instance.transform);
        endEat();
    }

    public void endEat()
    {
        audioPlay.Pause();
        updateFoodHeight();
        StopAllCoroutines();
    }

    public void fill()
    {
        quantity = 1f;
        updateFoodHeight();
    }

    private void updateFoodHeight()
    {
        Vector3 position = foodObject.transform.localPosition;
        position.y = ((foodFullY - foodEmptyY) * quantity) + foodEmptyY;
        foodObject.transform.localPosition = position;
    }
}
