using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public CardPool cardpool;
    public List<CardDataSO> cardDataList;

    [Header(header: "卡牌库")]
    public CardLibrarySO NormalLibrary;
    public CardLibrarySO currentLibrary;

    private int previousIndex;

    private void Awake()
    {
        InitializeCardDataList();
        foreach(var item in NormalLibrary.CardLibraryList)
        {
            currentLibrary.CardLibraryList.Add(item);
        }
    }

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>(key:"CardData",callback: null).Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError(message: "No CardData Found!");
        }
    }

    private void OnDisable()
    {
        currentLibrary.CardLibraryList.Clear();
    }

    public GameObject GetCardObject()
    {
        var cardObj = cardpool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;

        return cardObj; 
    }
    public void DiscardCard(GameObject cardobj)
    {
        cardpool.ReturnObjectToPool(cardobj);
    }
    public CardDataSO GetNewCardData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = Random.Range(0,cardDataList.Count);
        }
        while(previousIndex == randomIndex);

        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    //游戏胜利添加卡牌
    public void AddWinCard(CardDataSO newCardData)
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1,
        };
        if (currentLibrary.CardLibraryList.Contains(newCard))
        {
            var target =currentLibrary.CardLibraryList.Find(t =>t.cardData == newCardData);
            target.amount++;
        }
        else
        {
            currentLibrary.CardLibraryList.Add(newCard);
        }
    }
}
