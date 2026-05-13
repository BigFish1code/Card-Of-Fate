
using System;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackEffect", menuName = "CardEffect/AttackEffect")]
public class AttackEffect : Effect
{
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        if (target == null)
            return;
        switch (targetType)
        {
            case EffectTargetType.Target:
                var damage = (int)math.round(value * form.baseStrength);
                target.DelayedDamage(target, damage, 0.45f);
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
        AudioManager.Instance.PlaySFX("sfx_attack", volume: 0.9f, pitch: UnityEngine.Random.Range(0.9f, 1.2f));
    }
}
