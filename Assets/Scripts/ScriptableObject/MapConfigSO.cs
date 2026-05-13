using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName="MapConfigSO",menuName ="Map/MapConfig")]
public class MapConfigSO : ScriptableObject
{
    public List<RoomBluePrint> roomBluePrints;
}
[System.Serializable]
public class RoomBluePrint
{
    public int Low, High;
    public RoomType RoomType;
}
