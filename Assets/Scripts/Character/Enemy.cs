using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO actionDataSO;
    public EnemyAction currentAction;
    protected Player player;

    public virtual void OnPlayerTurnBegin()
    {
        var randomIndex = Random.Range(0, actionDataSO.AIEnemy.Count);
        currentAction = actionDataSO.AIEnemy[randomIndex];
        player = GameObject.FindGameObjectWithTag(tag: "Player").GetComponent<Player>();
    }

    public virtual void OnEnemyTurnBegin()
    {
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.All:
                break;
        }
    }
    public virtual void Skill()
    {
        StartCoroutine(ProcessDelayAction("skill"));
    }
    public virtual void Attack()
    {
        StartCoroutine(ProcessDelayAction("attack"));
    }

    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f > 0.6f
        && !animator.IsInTransition(0)
        && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        if (actionName == "attack")
        {
            currentAction.effect.Execute(this, player);
        }
        else
            currentAction.effect.Execute(this, this);
    }
}
