using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffRound;

    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    public int MaxHP { get => hp.maxValue; }

    protected Animator animator;

    public bool isDead;

    public GameObject Getbuff;
    public GameObject GetDebuff;

    //力量牌
    public float baseStrength = 1f;
    private float strengthEffect = 0.5f;

    [Header(header: "广播")]
    public ObjectEventSO characterDeadEvent;

    protected virtual void Awake()
    {
        animator= GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
        buffRound.currentValue = buffRound.maxValue;
        ResetDefense();
    }

    private void Update()
    { 
        animator.SetBool(name: "IsDead", isDead);
    }

    public virtual void TakeDamage(int damage)
    {
        var currentDamage = (damage - defense.currentValue) >= 0 ? (damage - defense.currentValue) : 0;
        var currentDefense = (damage - defense.currentValue) >= 0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);
        if (CurrentHP > currentDamage)
        {
            CurrentHP -= currentDamage;
            animator.SetTrigger("hurt");
        }
        else
        {
            CurrentHP = 0;
            // 当前人物死亡
            isDead = true;
            characterDeadEvent.RaisEvent(this, this);
        }
    }
    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
        AudioManager.Instance.PlaySFX("sfx_skill");
    }
    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        Getbuff.SetActive(true);
            AudioManager.Instance.PlaySFX("sfx_skill");
    }

    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            Getbuff.SetActive(true);
        }
        else
        {
            GetDebuff.SetActive(true);
            baseStrength = 1 - strengthEffect;
        }

        var currentRound = buffRound.currentValue + round;

        if (baseStrength == 1)
        {
            buffRound.SetValue(0);
        }
        else
            buffRound.SetValue(currentRound);
    }
    public void UpdateStrengthRound() //更新力量buff回合数
    {
        buffRound.SetValue(buffRound.currentValue-1);
        if (buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1;
        }
    }
    public void DelayedDamage(CharacterBase target, int damage, float delay)
    {
        StartCoroutine(DelayedDamageCoroutine(target, damage, delay));
    }

    private IEnumerator DelayedDamageCoroutine(CharacterBase target, int damage, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null && !target.isDead)
            target.TakeDamage(damage);
    }
}
