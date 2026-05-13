 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//泛型——基础监听
public class BaseListener<T> : MonoBehaviour
{
    public BaseSO<T> eventSO;
    public UnityEvent<T> response;
    private void OnEnable()
    {
        if(eventSO!= null)
        {
            eventSO.OnEventRaised += OnEventRaised;
        }
    }
    private void OnDisable()
    {
        if (eventSO!= null)
        {
            eventSO.OnEventRaised -= OnEventRaised;
        }
    }
    private void OnEventRaised(T value)
    {
        response.Invoke(value);
    }
}
