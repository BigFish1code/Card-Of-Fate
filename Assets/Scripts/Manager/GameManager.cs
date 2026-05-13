using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header(header: "地图布局")]
    public MapLayoutSO mapLayout;

    public List<Enemy> GameResultsList;
    [Header(header: "游戏结果事件")]
    public ObjectEventSO GameWinEvent;
    public ObjectEventSO GameOverEvent;
    public ObjectEventSO BossLoseEvent;

    public void UpdateMapLayoutData(object value)
    {
        var roomVector = (Vector2Int)value;
        if (mapLayout.mapRoomDataList.Count == 0)
            return;
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.colum == roomVector.x && r.line == roomVector.y);
        currentRoom.roomState = RoomState.Visited;
        //更新房间数据

        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.colum == currentRoom.colum);
        foreach (var room in sameColumnRooms)
        {
            if (room.line != roomVector.y)
                room.roomState = RoomState.Locked;
        }
        foreach (var link in currentRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r => r.colum == link.x && r.line == link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }
        GameResultsList.Clear();
    }

    public void OnRoomLoadedEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            GameResultsList.Add(enemy);
        }
    }

    public void OnCharacterDeadEvent(object character)
    {
        if (character is Player)
        {
            //lose
            StartCoroutine(EventDelayAction(GameOverEvent));
        }
        else if (character is Boss)
        {
            StartCoroutine(EventDelayAction(BossLoseEvent));
        }
        else if (character is Enemy)
        {
            GameResultsList.Remove(character as Enemy);
            if (GameResultsList.Count == 0)
            {
                //win
                StartCoroutine(EventDelayAction(GameWinEvent));
            }
       
       
        }
    }
    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(seconds: 2f);
        eventSO.RaisEvent(null, this);
    }

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.lineLocationList.Clear();
    }

    private void Start()
    {
        /*var conn = DBManager.GetConnection();
        conn.Close();*/
    }
}