using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealEffect", menuName = "CardEffect/HealEffect")]

public class HealEffect : Effect
{
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            form.HealHealth(value);
        }
        if (targetType == EffectTargetType.Target)
        {
            target.HealHealth(value);
        }
    }
}
