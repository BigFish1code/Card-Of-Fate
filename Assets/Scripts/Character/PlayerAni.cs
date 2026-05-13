using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    private Player player;
    private Animator animator;
    
    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        animator.Play(stateName: "Sleep");
        animator.SetBool(name: "IsSleep", value: true);
    }

    public void PlayerTurnBeginAnimation()
    {
        animator.SetBool(name:"IsSleep",value :false);
        animator.SetBool(name: "IsDefense", value: false);
    }
    public void PlayerTurnEndAnimation()
    {
        if (player.defense.currentValue > 0)
        {
            animator.SetBool("IsDefense", true);
        }
        else
        {
            animator.SetBool("IsSleep", true);
            animator.SetBool("IsDefense", false);
        }
    }

    public void OnPlayerCardEvent(object obj)
    {
        Card card = obj as Card;
        switch (card.cardData.cardtype)
        {
            case CardType.Attack:
                animator.SetTrigger(name: "attack");
                break;
            case CardType.Defense:
            case CardType.Abilities:
                animator.SetTrigger(name: "skill");
                break;
        }
    }
    public void SetSleepAni()
    {
        animator.Play("death");
    }
}
