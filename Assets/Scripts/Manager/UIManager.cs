using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header(header: "面板")]
    public GameObject GamePlayPanel;
    public GameObject GameWinPanel;
    public GameObject GameOverPanel;
    public GameObject SelectCardPanel;
    public GameObject RestRoomPanel;
    public GameObject BossLosePanel;
    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = (Room)data;
        switch (currentRoom.roomData.roomType)
        {
            case RoomType.NormalEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                GamePlayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                RestRoomPanel.SetActive(true);
                break;
        }
    }

    public void HideAllPanels()
    {
        GamePlayPanel.SetActive(false);
        GameWinPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        RestRoomPanel.SetActive(false) ;
        BossLosePanel.SetActive(false);
    }
    public void OnGameWinEvent()
    {
        GamePlayPanel.SetActive(false);
        GameWinPanel.SetActive(true);
    }
    public void OnGameOverEvent()
    {
        GamePlayPanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }
    public void OnSelectCardEvent()
    {
        SelectCardPanel.SetActive(true);
    }

    public void OnCloseSelectEvent()
    {
        SelectCardPanel.SetActive(false);
    }
    public void OnBossLosePanelEvent()
    {
        GamePlayPanel.SetActive(false);
        BossLosePanel.SetActive(true);
    }
}
