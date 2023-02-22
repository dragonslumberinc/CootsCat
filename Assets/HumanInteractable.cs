using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInteractable : MonoBehaviour
{
    public Animator animator;
    private bool bInitState = true;
    public string initState;
    public string otherState;
    public AudioSource audioPlay;

    public virtual void use()
    {
        GameHud.Instance.questManager.completeQuest("questhumanopendoors");
        if (bInitState)
            animator.Play(otherState);
        else
            animator.Play(initState);
        bInitState = !bInitState;
        if(audioPlay != null)
            audioPlay.Play();
    }

    void OnMouseEnter()
    {
        if(GameController.Instance != null)
            GameController.Instance.setActiveInteractable(this);
    }

    void OnMouseExit()
    {
        if (GameController.Instance != null)
            GameController.Instance.setInactiveInteractable(this);
    }
}
