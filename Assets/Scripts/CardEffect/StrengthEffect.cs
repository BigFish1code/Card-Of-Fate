using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "CardEffect/StrengthEffect")]
public class StrenghtEffect : Effect
{
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                form.SetupStrength(value,true);
                break;
            case EffectTargetType.Target:
                target.SetupStrength(value,false);
                break;
            case EffectTargetType.All:
                break;
        }
    }
}
