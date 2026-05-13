using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenTreasureBox : MonoBehaviour, IPointerDownHandler
{
    public ObjectEventSO GameWinEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        GameWinEvent.RaisEvent(null, this);
    }
}
