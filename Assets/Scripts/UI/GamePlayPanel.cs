using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayPanel : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO playerTrunEndEvent;

    private VisualElement rootElement;
    private Label energyAmountlabel,handcardAmountlabel,discardAmountlabel,turnlabel;
    private Button endTurnButton;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        energyAmountlabel = rootElement.Q<Label>(name: "EnergyAmount");
        handcardAmountlabel = rootElement.Q<Label>(name: "HandcardAmount");
        discardAmountlabel = rootElement.Q<Label>(name: "DiscardAmount");
        turnlabel = rootElement.Q<Label>(name: "TurnLabel");
        endTurnButton = rootElement.Q<Button>(name: "EndTurn");

        endTurnButton.clicked += OnEndTurnButtonClicked;

        handcardAmountlabel.text = "0";
        discardAmountlabel.text = "0";
        energyAmountlabel.text = "0";
        turnlabel.text = "Start";
    }

    private void OnEndTurnButtonClicked()
    {
        playerTrunEndEvent.RaisEvent(null, this);
    }

    public void UpdateHandcardChanged(int amount)
    {
        handcardAmountlabel.text=amount.ToString();
    }
    public void UpdateDiscardChanged(int amount)
    {
       discardAmountlabel.text = amount.ToString();
    }

    public void UpdateEnergyAmount(int amount)
    {
        energyAmountlabel.text = amount.ToString();
    }

    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnlabel.text = "Enemy Time";
        turnlabel.style.color = new StyleColor(Color.red);
    }
    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnlabel.text = "Player Time";
        turnlabel.style.color = new StyleColor(Color.green);
    }
}
