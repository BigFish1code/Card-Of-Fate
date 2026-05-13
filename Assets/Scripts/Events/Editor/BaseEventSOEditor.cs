using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseSO<T> baseSO;
    private void OnEnable()
    {
        if(baseSO == null)
        {
            baseSO = target as BaseSO<T>;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        foreach(var listener in Getlistener())
        {
            EditorGUILayout.LabelField("订阅数量:" + Getlistener().Count);
        }
    }
    
    private List<MonoBehaviour> Getlistener()
    {
        List<MonoBehaviour> listener = new();
        if(baseSO==null || baseSO.OnEventRaised == null)
        {
            return listener;
        }
        var subscribers = baseSO.OnEventRaised.GetInvocationList();//获得所有subscriber
        foreach(var subscriber in subscribers)
        {
            var obj = subscriber.Target as MonoBehaviour;
            if (!listener.Contains(obj))
            {
                listener.Add(obj);
            }
        }
        return listener;
    }
}
