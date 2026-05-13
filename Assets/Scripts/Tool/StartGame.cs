using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StartGaME : MonoBehaviour
{
    public AssetReference LongTime;
    private void Awake()
    {
        Addressables.LoadSceneAsync(LongTime);
    }
}
