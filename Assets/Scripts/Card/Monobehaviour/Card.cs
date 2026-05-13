using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [Header(header: "组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText, descriptionText, typeText;

    public CardDataSO cardData;

    [Header(header: "原始数据")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;

    public bool isActivating;
    public bool isAvailiable;

    public Player player;

    [Header(header: "广播")]
    public ObjectEventSO discardCardEvent;
    public IntEventSO cardcostEvent;

    private void Start()
    {
        Init(cardData);
    }

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardpic;
        costText.text = data.cardcost.ToString();
        descriptionText.text = data.carddescription;
        typeText.text = data.cardtype.ToString();
        player = GameObject.FindWithTag(tag:"Player").GetComponent<Player>();
    }

    public void UpdatePositionRotation(Vector3 position,Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isActivating)
        {
            return;
        }
        transform.position = originalPosition + Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isActivating)
        {
            return;
        }
        ResetCardTransform();
    }
    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }
    public void ExecuteCardEffects(CharacterBase form,CharacterBase targer)
    {
        //减少体力，回收卡牌
        cardcostEvent.RaisEvent(cardData.cardcost, this);
        discardCardEvent.RaisEvent(this, this);
        foreach(var effect in cardData.effects)
        {
            effect.Execute(form, targer);
        }
    }

    public void UpdateCardState()
    {
        isAvailiable = cardData.cardcost <= player.CurrentEnergy;
        costText.color = isAvailiable ? Color.green: Color.red;
    }
}
