using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button Select;
    private Button BackMap;
    [Header(header: "事件广播")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO SelectCardEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        Select = rootElement.Q<Button>(name: "Select");
        BackMap = rootElement.Q<Button>(name: "BackMap");


        BackMap.clicked += OnBackMapButtonClicked;
        Select.clicked += OnSelectCardEvent;
    }

    private void OnSelectCardEvent()
    {
        SelectCardEvent.RaisEvent(null, this);
    }

    private void OnBackMapButtonClicked()
    {
        loadMapEvent.RaisEvent(null, this);
    }

    public void OnCloseSelectEvent()
    {
        Select.style.display = DisplayStyle.None;
    }
}
