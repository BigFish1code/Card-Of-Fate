using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DefenseEffect",menuName ="CardEffect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        if(targetType == EffectTargetType.Self)
        {
            form.UpdateDefense(value);
        }
        if(targetType == EffectTargetType.Target)
        {
            target.UpdateDefense(value);
        }
    }

}
