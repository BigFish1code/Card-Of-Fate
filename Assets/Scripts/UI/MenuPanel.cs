using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton, quitGameButton, audioSettingsBtn;
    public ObjectEventSO newGameEvent;
    public GameObject audioSettingsPanel;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>(name: "NewGameButton");
        quitGameButton = rootElement.Q<Button>(name: "QuitGameButton");
        audioSettingsBtn = rootElement.Q<Button>(name: "AudioSettingsButton");

        newGameButton.clicked += OnNewGameButtonClicked;
        quitGameButton.clicked += OnQuitButtonClicked;
        audioSettingsBtn.clicked += () => audioSettingsPanel.SetActive(true);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    private void OnNewGameButtonClicked()
    {
        newGameEvent.RaisEvent(null, this);
    }
}
