using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="MapLayoutSO",menuName="Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDataList = new();
    public List<LineLocation> lineLocationList = new();
}

[System.Serializable]
public class MapRoomData
{
    public float locX, locY;
    public int colum, line;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkTo;
}
[System.Serializable]
public class LineLocation
{
    public SerializeVector3 startLoc, endLoc;
}