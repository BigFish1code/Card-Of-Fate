using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DrawCardEffect",menuName ="CardEffect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        drawCardEvent?.RaisEvent(value, this);
    }
}
