using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatsAgainstHumanity : MonoBehaviour
{
    private List<int> usedPrompts;
    private List<int> usedAnswers;

    private List<Transform> usedCards;

    private int displayPos = 0;

    private int totalBlackPrompt;
    private int totalWhiteAnswer;

    public TextMeshProUGUI textField;

    public Texture textureBlack;
    public Texture textureWhite;

    public RawImage displayedWhite;
    public RawImage displayedBlack;

    void Awake()
    {
        init();
    }

    public void init()
    {
        if(usedCards != null)
            usedCards.ForEach(x => x.gameObject.SetActive(true));

        usedCards = new List<Transform>();
        usedPrompts = new List<int>();
        usedAnswers = new List<int>();
        displayedWhite.gameObject.SetActive(false);
        displayedBlack.gameObject.SetActive(false);
        hide();
    }

    public void getCard(Transform gotCard)
    {
        Debug.Log($"getCard: { gotCard.gameObject.layer }");  // CAH Black / White
        usedCards.Add(gotCard);

        gotCard.gameObject.SetActive(false);

        int currentCards = usedPrompts.Count;
        int totalCards = totalBlackPrompt;
        Texture texture = textureBlack;
        if (gotCard.gameObject.tag == "CardBlack") // Black
        {
            getPrompt();
            currentCards = usedPrompts.Count;
        }
        else if (gotCard.gameObject.tag == "CardWhite") // White
        { 
            getAnswer();

            currentCards = usedAnswers.Count;
            totalCards = totalWhiteAnswer;
            texture = textureWhite;
        }

        GameController.Instance.spawner.spawnAt("LightningBolts", gotCard.transform.position, Vector3.up, GameController.Instance.transform);
        displayText();
        GameHud.Instance.plantCounter.addPlant(texture, currentCards, totalCards);
    }

    public void show()
    {
        this.gameObject.SetActive(true);
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void displayText()
    {
        show();
        string text = "<color=#00ffff>[blank]</color>";

        displayedBlack.gameObject.SetActive(displayPos < usedPrompts.Count);
        if (displayPos < usedPrompts.Count)
            text = getAllPrompts()[usedPrompts[displayPos]];

        displayedWhite.gameObject.SetActive(displayPos < usedAnswers.Count);
        if (displayPos < usedAnswers.Count)
            text = text.Replace("[blank]", getAllAnswers()[usedAnswers[displayPos]]);

        textField.text = text;
        if (displayPos < usedPrompts.Count && (displayPos < usedAnswers.Count))
            StartCoroutine("MoveToNextCoRoutine");
    }

    public IEnumerator MoveToNextCoRoutine()
    {
        yield return new WaitForSeconds(7f);
        displayPos++;

        if (displayPos < usedPrompts.Count || displayPos < usedAnswers.Count)
            displayText();
        else
            hide();
    }

    private string[] getAllPrompts()
    {
        return new string[]
        {
            "Help me doctor, I've got <color=#00ffff>[blank]</color> in my butt!",
            "I've got 99 problems but <color=#00ffff>[blank]</color> ain't one.",
            "I drink to forget <color=#00ffff>[blank]</color>.",
            "What's that smell? <color=#00ffff>[blank]</color>.",
            "Everything is about sex, and sex is all about <color=#00ffff>[blank]</color>.",
            "Girls just wanna have <color=#00ffff>[blank]</color>!",
            "Winter is coming, and so is <color=#00ffff>[blank]</color>.",
            "What gives me uncontrollable gas? <color=#00ffff>[blank]</color>.",
            "<color=#00ffff>[blank]</color>. That's how I want to die.",
            "How can we monetize <color=#00ffff>[blank]</color>?"
        };
    }

    private string getPrompt()
    {
        string[] allPrompts = getAllPrompts();
        int random = -1;
        do
        {
            random = Random.Range(0, allPrompts.Length);
        } while (usedPrompts.Contains(random));
        usedPrompts.Add(random);

        return allPrompts[random];
    }

    private string[] getAllAnswers()
    {
        return new string[]
        {
            "My stinky butthole",
            "Self-loathing",
            "Joe Biden",
            "Dead parents",
            "A motherfuckin' ugly dog",
            "Coots",
            "Estrogen",
            "Losing at chess boxing",
            "Three inches when hard",
            "Ludwig"
        };
    }

    private string getAnswer()
    {
        string[] allPrompts = getAllAnswers();
        int random = -1;
        do
        {
            random = Random.Range(0, allPrompts.Length);
        } while (usedAnswers.Contains(random));
        usedAnswers.Add(random);

        return allPrompts[random];
    }

    public void addTotal(Transform card)
    {
        //Debug.Log($"addTotal: { card.gameObject.tag } // b:{ totalBlackPrompt } / w:{ totalWhiteAnswer }");
        if (card.gameObject.tag == "CardBlack")
            totalBlackPrompt++;
        else if (card.gameObject.tag == "CardWhite")
            totalWhiteAnswer++;
    }

    public int getValue(string cardType)
    {
        //Debug.Log($"getValue: { cardType } // b:{ usedPrompts.Count } / w:{ usedAnswers.Count }");
        if (cardType == "CardBlack")
            return usedPrompts.Count;
        else if (cardType == "CardWhite")
            return usedAnswers.Count;
        else
            return 0;
    }

    public int getTotal(string cardType)
    {
        if (cardType == "CardBlack")
            return totalBlackPrompt;
        else if (cardType == "CardWhite")
            return totalWhiteAnswer;
        else
            return 0;
    }
}
