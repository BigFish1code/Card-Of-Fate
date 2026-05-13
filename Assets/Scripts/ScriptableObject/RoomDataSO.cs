using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "RoomDataSO",menuName = "Map/RoomDataSO")]
public class RoomDataSO : ScriptableObject 
{
    public Sprite roomIcon;

    public RoomType roomType;

    public AssetReference sceneToLoad;//用Addressable管理场景加载
}
