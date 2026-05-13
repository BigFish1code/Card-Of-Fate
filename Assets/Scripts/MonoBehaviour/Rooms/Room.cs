using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;//纵变量
    public int line;//横变量

    private SpriteRenderer spriteRenderer;

    public RoomDataSO roomData;

    public RoomState roomState;

    public List<Vector2Int> linkTo = new();

    [Header(header: "广播")]
    public ObjectEventSO loadRoomEvent;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();//创建房间数据类型
    }
    private void OnMouseDown()//点击触发切换场景
    {
        if (roomState == RoomState.Attainable)
            loadRoomEvent.RaisEvent(this,this);
    }

    public void SetupRoom(int column,int line,RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;

        spriteRenderer.sprite = roomData.roomIcon;
        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1f),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 0.5f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        };
    }//调用房间时配置对应的数据
}
