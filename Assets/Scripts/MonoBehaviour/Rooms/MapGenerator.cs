using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header(header:"地图配置表")]
    public MapConfigSO mapConfig;

    [Header(header:"地图布局")]
    public MapLayoutSO mapLayout;

    [Header(header: "预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;

    private float screenHeight;
    private float screenWidth;
    
    private float listsWidth;

    private Vector3 roomlocation;
    public float border;

    private List<Room> rooms=new();
    private List<LineRenderer> lines=new();

    public List<RoomDataSO> roomDataList = new();
    private Dictionary<RoomType, RoomDataSO> roomDataDict = new();

    private void Awake()
    {   
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        listsWidth = screenWidth / (mapConfig.roomBluePrints.Count + 1);//用于规范关卡在地图中生成位置

        foreach(var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }
    }

    private void OnEnable()
    {
        if (mapLayout.mapRoomDataList.Count > 0)
            LoadMap(); 
        else
            CreateMap();
    }

    public void CreateMap()
    {
        List<Room> previouslistRooms = new();

        for (int lists = 0 ; lists < mapConfig.roomBluePrints.Count; lists++)
        {
            var blueprint=mapConfig.roomBluePrints[lists];

            var amount = UnityEngine.Random.Range(blueprint.Low, blueprint.High);
            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            roomlocation = new Vector3(-screenWidth / 2 + border + listsWidth*lists, startHeight, 0);
            
            var newlocation = roomlocation;

            List<Room> currentlistRooms = new();

            var roomYJG = screenHeight / (amount + 1);

            for (int i = 0 ; i < amount ; i++)
            {
                if (lists == mapConfig.roomBluePrints.Count - 1)
                {
                    newlocation.x = screenWidth / 2 - border * 3;
                }
               else if (lists != 0)
                {
                    newlocation.x = roomlocation.x + UnityEngine.Random.Range(-border/2, border/2);//偏移大小
                }
                newlocation.y = startHeight - roomYJG * i;
                var room = Instantiate(roomPrefab,newlocation,Quaternion.identity,transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBluePrints[lists].RoomType);

                //设置第一列房间为开启，其他默认locked
                if (lists == 0)
                    room.roomState = RoomState.Attainable;
                else
                    room.roomState =RoomState.Locked; 

                room.SetupRoom(lists, i, GetRoomData(newType));
                rooms.Add(room);
                currentlistRooms.Add(room);
            }
            if(previouslistRooms.Count > 0)
            {
                RoomConnection(previouslistRooms, currentlistRooms);
            }
            previouslistRooms = currentlistRooms;
        }

        SaveMap();
    }

    private void RoomConnection(List<Room> list1, List<Room> list2)
    { 
        HashSet<Room> connectedlist2Rooms = new();

        foreach(var room in list1)
        {
            var targetRoom = ConnectToRandomRoom(room, list2,false);
            connectedlist2Rooms.Add(targetRoom);
        }
        foreach (var room in list2)
        {
            if (!connectedlist2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room, list1,true);
            }
        }
    }
    private Room ConnectToRandomRoom(Room room,List<Room> list2,bool check)
    {
        Room targetRoom;
        targetRoom = list2[UnityEngine.Random.Range(minInclusive:0, list2.Count)];

        if (check)
        {
            targetRoom.linkTo.Add(new(room.column, room.line));
        }
        else
        {
            room.linkTo.Add(new(targetRoom.column, targetRoom.line));
        }

//创建房间之间连线        
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(index:0,room.transform.position);
        line.SetPosition(index:1,targetRoom.transform.position);

        lines.Add(line);
        return targetRoom;
    }

    [ContextMenu(itemName:"MapReset")]
    public void MapReset()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }

        foreach (var item in lines)
        {
            Destroy(item.gameObject);
        }

        lines.Clear();
        rooms.Clear();//清空，防止遗漏
        CreateMap();
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');
        string randomOpotion = options[UnityEngine.Random.Range(0,options.Length)];
        RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), randomOpotion);
        return roomType;
    }
    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();
        for (int i = 0; i < rooms.Count; i++)
        {
            var room = new MapRoomData()
            {
                locX = rooms[i].transform.position.x,
                locY = rooms[i].transform.position.y,
                colum = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkTo,
            };
            mapLayout.mapRoomDataList.Add(room);
        }
        mapLayout.lineLocationList = new();
        for (int i = 0;i < lines.Count; i++)
        {
            var line = new LineLocation()
            {
                startLoc = new SerializeVector3(lines[i].GetPosition(0)),
                endLoc = new SerializeVector3(lines[i].GetPosition(1)),
            };
            mapLayout.lineLocationList.Add(line);
        }
    }
    private void LoadMap()
    {
        for(int i = 0; i < mapLayout.mapRoomDataList.Count; i++)
        {
            var newLoc = new Vector3(mapLayout.mapRoomDataList[i].locX, mapLayout.mapRoomDataList[i].locY, 0);
            var newRoom = Instantiate(roomPrefab, newLoc, Quaternion.identity, transform);
            newRoom.roomState = mapLayout.mapRoomDataList[i].roomState;
            newRoom.SetupRoom(mapLayout.mapRoomDataList[i].colum, mapLayout.mapRoomDataList[i].line, mapLayout.mapRoomDataList[i].roomData);
            newRoom.linkTo=mapLayout.mapRoomDataList[i].linkTo;
            rooms.Add(newRoom);
        }
        for(int i = 0; i < mapLayout.lineLocationList.Count; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.lineLocationList[i].startLoc.ToVector3());
            line.SetPosition(1, mapLayout.lineLocationList[i].endLoc.ToVector3());
           
            lines.Add(line);
        }
    }
}
