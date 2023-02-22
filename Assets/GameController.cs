//#define ALL_UNLOCK

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int totalLightingBolts; // Cat
    private int totalMadness; // Human

    private const int lightningBoltThreshold1 = 45;
    private const int lightningBoltThreshold2 = 100;
    private const int lightningBoltThreshold3 = 160;

    private const int madnessThreshold1 = 120;
    private const int madnessThreshold2 = 300;
    private const int madnessThreshold3 = 1000;

    private int plantCounter = 0;
    private int plantTotalCounter = 0;
    
    private int laserFireCounter = 0;
    private int laserFireTotalCounter = 0;

    public EndGameSequence endGameSequence;

    private HumanInteractable selectedHumanInteractable;

    public Player playerCat;

    public Player playerHuman;

    public ActionAttack actionAttack;

    public ActionEyeLaser actionLaser;

    public bool catActive = true;

    public static GameController Instance;

    public PictureTaker pictureTaker;

    public Spawner spawner;

    public PossessionCamera possessionCamera;

    public CanvasScaler canvasScaler;

    private bool gameActive;

    private bool gamePaused;

    private int pictureOffset;

    private List<string> itemList = new List<string>();

    public AudioSource audioSource;
    public AudioClip audioLightning;
    public AudioClip audioMadness;

    public TextMeshProUGUI textDebug;

    private void Awake()
    {
        Instance = this;
        init();
    }

    private void Start()
    {

#if START_IMMEDIATE
        startGame();
#else
        Cursor.lockState = CursorLockMode.None;
        pictureOffset = Random.Range(0, 8);

        GameHud.Instance.mainMenu.gameObject.SetActive(true);

        playerCat.firstPersonController.readInputs = false;
        playerHuman.firstPersonController.readInputs = false;
#endif

#if ALL_UNLOCK
        playerCat.firstPersonController.canFly = true;
#endif
    }

    public void Update()
    {
        if (gameActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                pause(!gamePaused);
            }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.K))
            {
                endGame();
            }
#endif

            if (!gamePaused)
            {
                if (catActive && playerCat.firstPersonController.readInputs)
                {
                    if (canEyeLaser())
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            actionLaser.startLaser();
                        }
                        else if (Input.GetMouseButtonUp(1))
                        {
                            actionLaser.endLaser();
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit hitDown = GameHud.Instance.getLastRaycast();
                        if (hitDown.transform != null && hitDown.transform.tag == "FoodBowl")
                            hitDown.transform.GetComponent<FoodBowl>().startEat();
                        else
                            actionAttack.startAttack(playerCat);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        RaycastHit hitUp = GameHud.Instance.getLastRaycast();
                        if (hitUp.transform != null && hitUp.transform.tag == "FoodBowl")
                            hitUp.transform.GetComponent<FoodBowl>().endEat();
                        else
                            actionAttack.endAttack(playerCat);
                    }

                    if (Input.GetKeyDown(KeyCode.Q) && canPossess())
                    {
                        GameHud.Instance.questManager.completeQuest("questcatpossess");
                        possessionCamera.possess(playerCat.transform, playerHuman.transform, !catActive);
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        playerCat.talk();
                        GameHud.Instance.questManager.completeQuest("questcattalk");
                    }
                }
                else if (!catActive && playerHuman.firstPersonController.readInputs)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log($"Distance: { Vector3.Distance(selectedHumanInteractable.transform.position, playerHuman.transform.position) }");
                        if (selectedHumanInteractable != null)
                        {
                            if (Vector3.Distance(selectedHumanInteractable.transform.position, playerHuman.transform.position) < 2.8f)
                                selectedHumanInteractable.use();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Q) && canPossess())
                    {
                        GameHud.Instance.questManager.completeQuest("questhumanrecoots");
                        possessionCamera.possess(playerHuman.transform, playerCat.transform, !catActive);
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        playerHuman.talk();
                        GameHud.Instance.questManager.completeQuest("questhumantalk");
                    }
                }
            }
        }
    }

    public void gainItem(string item)
    {
        itemList.Add(item);
    }

    public bool hasItem(string item)
    {
        return itemList.Contains(item);
    }

    public void loseItem(string item)
    {
        itemList.Remove(item);
    }

    public void startGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameActive = true;

        init();

        playerCat.firstPersonController.readInputs = true;
        playerHuman.firstPersonController.readInputs = false;

        GameHud.Instance.questManager.addQuest(true, "questcatstart");
    }

    public void endGame()
    {
        playerCat.firstPersonController.readInputs = false;
        playerHuman.firstPersonController.readInputs = false;
        endGameSequence.startEndGame();
    }

    public void pause(bool _gamePaused)
    {
        gamePaused = _gamePaused;
        GameHud.Instance.pauseMenu.gameObject.SetActive(gamePaused);

        if (gamePaused)
        {
            Cursor.lockState = CursorLockMode.None;

            playerCat.firstPersonController.readInputs = false;
            playerHuman.firstPersonController.readInputs = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (catActive)
                playerCat.firstPersonController.readInputs = true;
            else
                playerHuman.firstPersonController.readInputs = true;
        }
    }

    public void init()
    {
        setCatActive(true);

        plantCounter = 0;
        laserFireCounter = 0;
        totalLightingBolts = 0;
        totalMadness = 0;
    }

    public void setCatActive(bool _catActive)
    {
        catActive = _catActive;
        playerCat.setActive(catActive);
        playerHuman.setActive(!catActive);
        GameHud.Instance.setHud(_catActive);
    }

    public void setActiveInteractable(HumanInteractable _humanInteractable)
    {
        selectedHumanInteractable = _humanInteractable;
    }

    public void setInactiveInteractable(HumanInteractable _humanInteractable)
    {
        if (selectedHumanInteractable == _humanInteractable)
            selectedHumanInteractable = null;
    }

    public bool canPossess()
    {
#if ALL_UNLOCK
        return true;
#else
        return totalMadness >= madnessThreshold1;
#endif
    }

    public bool canEyeLaser()
    {
#if ALL_UNLOCK
        return true;
#else
        return totalLightingBolts >= lightningBoltThreshold2;
#endif
    }

    public bool canFly()
    {
#if ALL_UNLOCK
        return true;
#else
        return totalLightingBolts >= lightningBoltThreshold3;
#endif
    }

    public bool canStrongPower()
    {
#if ALL_UNLOCK
        return true;
#else
        return totalLightingBolts >= lightningBoltThreshold1;
#endif 
    }

    public void gainPower(bool lightningbolt, int quantity)
    {
        if (lightningbolt)
        {
            if (totalLightingBolts < lightningBoltThreshold1 && totalLightingBolts + quantity >= lightningBoltThreshold1)
                GameHud.Instance.powerGainDisplay.gainPower(lightningbolt, 1);
            else if (totalLightingBolts < lightningBoltThreshold2 && totalLightingBolts + quantity >= lightningBoltThreshold2)
                GameHud.Instance.powerGainDisplay.gainPower(lightningbolt, 2);
            else if (totalLightingBolts < lightningBoltThreshold3 && totalLightingBolts + quantity >= lightningBoltThreshold3)
            {
                GameController.Instance.playerCat.firstPersonController.canFly = true;
                GameHud.Instance.powerGainDisplay.gainPower(lightningbolt, 3);
            }

            audioSource.PlayOneShot(audioLightning);
            totalLightingBolts += quantity;
            GameHud.Instance.powerGainMeter.gainPower(lightningbolt, totalLightingBolts);

        }
        else
        {
            if (totalMadness < madnessThreshold1 && totalMadness + quantity >= madnessThreshold1)
                GameHud.Instance.powerGainDisplay.gainPower(lightningbolt, 1);
            else if (totalMadness < madnessThreshold2 && totalMadness + quantity >= madnessThreshold2)
                GameHud.Instance.powerGainDisplay.gainPower(lightningbolt, 2);

            audioSource.PlayOneShot(audioMadness);
            totalMadness += quantity;
            GameHud.Instance.powerGainMeter.gainPower(lightningbolt, totalMadness);
        }

        //Debug.Log($"gainPower: { totalLightingBolts }");

        playerCat.firstPersonController.canFly = canFly();
        GameHud.Instance.setHud(catActive);
    }

    public void addTotalPlantCounter()
    {
        plantTotalCounter++;
    }

    public void addPlantCounter()
    {
        plantCounter++;
        //GameHud.Instance.plantCounter.addPlant(null, plantCounter, plantTotalCounter);
    }

    public int getPlantBreak()
    {
        return plantCounter;
    }

    public int getPlantBreakTotal()
    {
        return plantTotalCounter;
    }

    public int getLaserFires()
    {
        return laserFireCounter;
    }

    public int getLaserFiresTotal()
    {
        return laserFireTotalCounter;
    }

    public void addTotalLaserFiresCounter()
    {
        laserFireTotalCounter++;
    }

    public void addLaserFiresCounter()
    {
        laserFireCounter++;
        //GameHud.Instance.plantCounter.addPlant(null, plantCounter, plantTotalCounter);
    }

    public int getPower(bool lightningbolt)
    {
        if (lightningbolt)
            return totalLightingBolts;
        else
            return totalMadness;
    }

    public int getThreshold(bool isLightningBolt, int position)
    {
        if (isLightningBolt)
        {
            if (position == 1)
                return lightningBoltThreshold1;
            else if (position == 2)
                return lightningBoltThreshold2;
            else
                return lightningBoltThreshold3;
        }
        else
        {
            if (position == 1)
                return madnessThreshold1;
            else if (position == 2)
                return madnessThreshold2;
            else
                return madnessThreshold3;
        }
    }

    public void backOneFireOffset()
    {
        pictureOffset--;
    }

    public string pictureFireTitle()
    {
        string[] titleList = new string[]
        {
            "Burn baby!",
            "You're fired",
            "That's lit",
            "Just a warm up",
            "Holy smoke",
            "My version of tinder",
            "And boom goes the dynamite.",
            "Open the gates of hell",
            "Disco Inferno!",
            "Bringing the heat"
        };

        pictureOffset += titleList.Length + 1;
        pictureOffset = pictureOffset % titleList.Length;

        return titleList[pictureOffset];
    }
}
