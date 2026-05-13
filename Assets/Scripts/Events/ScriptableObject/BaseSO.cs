using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//泛型事件架构——基础事件
public class BaseSO<T> : ScriptableObject
{
    [TextArea]
    public string description;

    public UnityAction<T> OnEventRaised;

    public string lastSender;
    public void RaisEvent(T value,object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }
}