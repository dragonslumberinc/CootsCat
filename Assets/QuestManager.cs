using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestPrefab questPrefab;

    public List<QuestPrefab> questsActive = new List<QuestPrefab>();

    public List<QuestPrefab> questsInactive = new List<QuestPrefab>();

    public List<string> questsComplete = new List<string>();

    void Awake()
    {
        questPrefab.gameObject.SetActive(false);
        questsComplete = new List<string>();
    }

    public void displayQuests(bool isCat)
    {
        questsActive.ForEach(quest => quest.gameObject.SetActive(quest.isCat == isCat));
    }

    public void addQuest(bool isCat, string questId)
    {
        //GameHud.Instance.questManager.addQuest(true, "questcathouse");
        if (!questsComplete.Contains(questId))
        {
            Debug.Log($"addQuest: { isCat} // { questId }");
            string text = "";
            switch (questId)
            {
                case "questcatstart":
                    addQuest(true, "questcatmove");
                    addQuest(true, "questcatstrike");
                    addQuest(true, "questcatjump");
                    addQuest(true, "questcattalk");
                    break;

                case "questcatmove":
                    text = "WASD to Move";
                    break;

                case "questcatstrike":
                    text = "Hold LMB to Paw strike";
                    break;

                case "questcatjump":
                    text = "Space to Jump";
                    break;

                case "questcathouse":
                    text = "Explore the house";
                    break;

                case "questcatbreakstuff":
                    text = "Break stuff";
                    GameHud.Instance.questManager.completeQuest("questcathouse");
                    break;

                case "questbreakstuff2":
                    text = "Break stuff";
                    break;

                case "questcattalk":
                    text = "E to Talk";
                    break;

                /****/
                case "questmadness1":
                    addQuest(true, "questcathumanfeed");
                    addQuest(true, "questcatpossess");
                    addQuest(false, "questhumanfeed");
                    addQuest(false, "questhumanopendoors");
                    addQuest(false, "questhumanrecoots");
                    addQuest(false, "questhumantalk");
                    break;

                case "questhumantalk":
                    text = "E to Talk";
                    break;


                case "questcathumanfeed":
                    text = "You're hungry";
                    GameHud.Instance.questManager.completeQuest("questcathouse");
                    GameHud.Instance.questManager.completeQuest("questcatbreakstuff");
                    break;

                case "questcatpossess":
                    text = "Q to Possess";
                    break;

                case "questhumanfeed":
                    text = "There's food in the fridge";
                    break;

                case "questhumanbowl":
                    text = "Put food in the bowl";
                    break;

                case "questhumanopendoors":
                    text = "LMB to open doors";
                    break;

                case "questhumanrecoots":
                    text = "Q to re-Coots";
                    break;

                case "questcateyelasers":
                    text = "RMB to Shoot lasers";
                    break;

                case "questcatbowltoeat":
                    text = "LMB on bowl to eat";
                    break;

                // human;

                case "questcatflight":
                    text = "Space to Fly";
                    break;

                case "questcatlower":
                    text = "C to Land";
                    break;

                case "questcatmadness":
                    completeQuest("questbreakstuff2");
                    text = "Get your human to the couch";
                    addQuest(false, "questhumansit");
                    break;

                case "questhumansit":
                    text = "LMB on couch to sit";
                    break;
            }

            if (!string.IsNullOrEmpty(text))
            {
                QuestPrefab newQuest = Instantiate<QuestPrefab>(questPrefab, this.transform);
                newQuest.gameObject.SetActive(GameController.Instance.catActive == isCat);
                newQuest.id = questId;
                newQuest.isCat = isCat;
                newQuest.text.text = text;
                questsActive.Add(newQuest);
            }
        }
    }

    public void completeQuest(string questId)
    {
        if (!questsComplete.Contains(questId))
        {
            questsComplete.Add(questId);
            Debug.Log($"completeQuest: { questId }");
            int pos = questsActive.FindIndex(x => x.id == questId);
            if (pos >= 0)
            {
                questsInactive.Add(questsActive[pos]);
                questsActive[pos].gameObject.SetActive(false);
                questsActive.RemoveAt(pos);
            }

            if ((questId == "questcatmove" || questId == "questcatstrike" || questId == "questcatjump") &&
                (questsComplete.Contains("questcatmove") && questsComplete.Contains("questcatstrike") && questsComplete.Contains("questcatjump")))
                addQuest(true, "questcathouse");
            else if (questId == "questhumanfeed")
                GameHud.Instance.setHud(GameController.Instance.catActive);
            else if (questId == "questhumanbowl")
            {
                addQuest(true, "questcatbowltoeat");
                GameHud.Instance.setHud(GameController.Instance.catActive);
            }
        }
    }
}
