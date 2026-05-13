using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectCardPanel : MonoBehaviour
{
    public CardManager cardManager; 

    private VisualElement rootElement;
    public VisualTreeAsset CardChange;
    private VisualElement CardContainer;
    private CardDataSO currentCardData;

    private Button containerButton;

    [Header("广播")]
    public ObjectEventSO CloseSelectEvent;

    private List<Button> cardButtons = new();

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        CardContainer = rootElement.Q<VisualElement>(name: "Container");
        containerButton = rootElement.Q<Button>(name: "ContainerButton");
        containerButton.clicked += OnContainerButtonClicked;

        for(int i = 0;i<3; i++)
        {
            var card = CardChange.Instantiate();
            var data = cardManager.GetNewCardData();
//初始化
            InitCard(card, data);
            var cardButton = card.Q<Button>(name: "Card"); ;
            CardContainer.Add(card);
            cardButtons.Add(cardButton);

            cardButton.clicked += () => OnCardClicked(cardButton,data);
         }
    }

    private void OnContainerButtonClicked()
    {
        cardManager.AddWinCard(currentCardData);
        CloseSelectEvent.RaisEvent(null, this);
    }

    private void OnCardClicked(Button cardButton,CardDataSO data)
    {
        currentCardData = data;
        for (int i = 0; i < cardButtons.Count; i++)
        {
            if (cardButtons[i] == cardButton)
            {
                cardButtons[i].SetEnabled(false);
            }
            else
            {
                cardButtons[i].SetEnabled(true);
            }
        }
    }

    public void InitCard(VisualElement card,CardDataSO cardData)
    {
        var cardSprite = card.Q<VisualElement>(name: "CardSprite");
        var cardCost = card.Q<Label>(name: "EnergyCost");
        var cardDescription = card.Q<Label>(name: "CardDescription");
        var cardType = card.Q<Label>(name: "CardType");
        var cardName = card.Q<Label>(name: "CardName");

        cardSprite.style.backgroundImage = new StyleBackground(cardData.cardpic);
        cardName.text = cardData.cardname;
        cardCost.text = cardData.cardcost.ToString();
        cardDescription.text = cardData.carddescription.ToString();
        cardType.text = cardData.cardtype.ToString(); 
    }
}
