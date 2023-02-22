using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHud : MonoBehaviour
{
    public static GameHud Instance;

    public PlantCounter plantCounter;

    public CatsAgainstHumanity catsAgainstHumanity;

    public GameObject catHud;

    public FadeInOut fadeInOut;

    public PowerGainMeter powerGainMeter;

    public PowerGainDisplay powerGainDisplay;

    public GameObject humanHud;

    private bool bCat;

    public Menu mainMenu;

    public Menu pauseMenu;

    public Menu confirmMenu;

    public LayerMask layerMaskCat;

    public LayerMask layerMaskHuman;

    public TutorialBox tutorialPossess;
    public TutorialBox tutorialSwipe;
    public TutorialBox tutorialEyeLaser;
    public TutorialBox tutorialFlight;

    public QuestManager questManager;

    public MadnessHud madnessHud;

    public PawSwipe pawSwipe;

    private Ray lastRay;

    private RaycastHit lastRaycast;
    private void Awake()
    {
        Instance = this;

        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        confirmMenu.gameObject.SetActive(false);
        pawSwipe.clearSwipe();
        madnessHud.clear();

        tutorialPossess.hide();
        tutorialSwipe.hide();
        tutorialEyeLaser.hide();
        tutorialFlight.hide();
    }

    // Update is called once per frame
    void Update()
    {
        lastRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(bCat)
            Physics.Raycast(lastRay, out lastRaycast, 100f, layerMaskCat);
        else
            Physics.Raycast(lastRay, out lastRaycast, 100f, layerMaskHuman);

        //Debug.Log($"GameHud Update: { bCat } // { lastRaycast.transform }");
    }

    public void setHud(bool _bCat)
    {
        bCat = _bCat;
        catHud?.gameObject.SetActive(_bCat);
        humanHud?.gameObject.SetActive(!_bCat);
        GameHud.Instance.questManager.displayQuests(_bCat);

        if (_bCat)
        {
            madnessHud.clear();
            tutorialPossess.updateDisplay();
            tutorialSwipe.updateDisplay();
            tutorialEyeLaser.updateDisplay();
            tutorialFlight.updateDisplay();
        }
        else
        {
            int level = 0;
            madnessHud.clear();
            if (GameController.Instance.getPower(false) >= GameController.Instance.getThreshold(false, 2))
            {
                level = 2;
                madnessHud.setStrings("Take care of Coots", new string[] { "Living room couch", "Living room couch", "Living room couch", "Coots" });
            }
            else if (GameController.Instance.getPower(false) >= GameController.Instance.getThreshold(false, 1))
            {
                level = 1;
                if(!GameHud.Instance.questManager.questsComplete.Contains("questhumanfeed"))
                    madnessHud.setStrings("Feed Coots", new string[] { "Fridge", "Fridge", "Fridge", "Coots" });
                else if (!GameHud.Instance.questManager.questsComplete.Contains("questhumanbowl"))
                    madnessHud.setStrings("Feed Coots", new string[] { "Bowl", "Bowl", "Bowl", "Bowl" });
            }
            madnessHud.setLevel(level);
        }
    }

    public Ray getLastRay()
    {
        return lastRay;
    }

    public RaycastHit getLastRaycast()
    {
        return lastRaycast;
    }
}
