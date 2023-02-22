using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public AudioMixerGroup audioMusic;

    public AudioMixerGroup audioSFX;

    public AudioMixerGroup audioMaster;

    public PausePhotoDisplay pausePhotoDisplay;

    public PowerGainMeter powerGainMeter;

    public GameObject catHud;
    public GameObject gameHud;

    public GameObject creditsMenu;
    public GameObject creditsCloseThis;
    public GameObject creditsCloseNotThis;

    public TextMeshProUGUI textAudio;

    void OnEnable()
    {
        pausePhotoDisplay?.setPhotos();
        powerGainMeter?.pauseShow(true);
        if(catHud != null)
            catHud.gameObject.SetActive(false);
        if (creditsMenu != null)
            creditsMenu.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        powerGainMeter?.pauseShow(false);
    }

    public void startCredits()
    {
        if (gameHud != null)
            gameHud.SetActive(false);
        if (creditsMenu != null)
        {
            creditsMenu.gameObject.SetActive(true);
        }

        creditsCloseThis.SetActive(true);
        creditsCloseNotThis.SetActive(false);
    }

    public void endCredits()
    {
        gameHud.SetActive(true);
        this.gameObject.SetActive(true);
        if (creditsMenu != null)
        {
            creditsMenu?.gameObject.SetActive(false);
        }
    }

    public void startGame()
    {
        GameController.Instance.startGame();
        gameHud.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void resumeGame()
    {
        GameController.Instance.pause(false);
    }

    public void changeVolumeSFX()
    {
        float volume;
        audioMaster.audioMixer.GetFloat("VolumeMaster", out volume);
        if (volume > -20)
        {
            volume = -80;
            textAudio.text = "Audio off";
        }
        else
        {
            volume = 0;
            textAudio.text = "Audio on";
        }
        
        audioMaster.audioMixer.SetFloat("VolumeMaster", volume);
    }

    public void changeVolumeMusic()
    {
        changeVolumeSFX();
    }

    public void clickQuit()
    {

    }

    public void clickConfirm()
    {

    }

    public void clickCancel()
    {

    }
}
