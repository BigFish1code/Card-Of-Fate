using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="CardDataSO",menuName ="Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public string cardname;
    public Sprite cardpic;
    public int cardcost;
    public CardType cardtype;

    [TextArea]
    
    public string carddescription;
    public List<Effect> effects;
}
