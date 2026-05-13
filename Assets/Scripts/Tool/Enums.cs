using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum RoomType
{
    NormalEnemy = 1,
    EliteEnemy = 2,
    Shop = 4,
    Treasure = 8,
    RestRoom = 16,
    Boss = 32
}

public enum RoomState //房间状态
{
    Locked,     //锁定
    
    Visited,    //已访问

    Attainable  //可访问
}

public enum CardType
{
    Attack,
    
    Defense,
    
    Abilities

}

public enum EffectTargetType
{
    Self,
    Target,
    All,
}