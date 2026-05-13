using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerEnergy;
    public int maxEnergy;

    public int CurrentEnergy 
    {
        get => playerEnergy.currentValue; 
        set => playerEnergy.SetValue(value);
    }

    private void OnEnable()
    {
        playerEnergy.maxValue = maxEnergy;
        CurrentEnergy = playerEnergy.maxValue;//初始能力值
    }
    public void NewTurn()
    {
        CurrentEnergy = maxEnergy;
    }
    public void UpdateEnergy(int cost)
    {
        CurrentEnergy -= cost;
        if (CurrentEnergy <= 0)
        {
            CurrentEnergy = 0;
        }
    }
    public void NewGame()
    {
        CurrentHP = MaxHP;
        isDead = false;
        buffRound.currentValue = buffRound.maxValue;
        NewTurn();
    }
}
