using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class BossLosePanel : MonoBehaviour
{
    private Button QuitGame;

    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("QuitGameBtn").clicked += QuitGameEvent;
    }

    private void QuitGameEvent()
    {
        Application.Quit();
    }
}
