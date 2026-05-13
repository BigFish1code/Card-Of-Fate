using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restButton;
    private Button restRoomToMapButton;

    public Effect restEffect;
    public ObjectEventSO loadMapEvent;
    
    private CharacterBase player;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        restButton = rootElement.Q<Button>(name:"RestButton");
        restRoomToMapButton = rootElement.Q<Button>(name: "RestRoomToMap");

        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);

        restButton.clicked += OnrRestButtonClicked;
        restRoomToMapButton.clicked += OnRestRoomToMapClicked;
    }

    private void OnRestRoomToMapClicked()
    {
        loadMapEvent.RaisEvent(null, this);
    }

    private void OnrRestButtonClicked()
    {
        restEffect.Execute(player, null);
        restButton.SetEnabled(false);
    }
}
