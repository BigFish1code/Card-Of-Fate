using UnityEngine;
using UnityEngine.UIElements;

public class AudioSettingsPanel : MonoBehaviour
{
    private Slider masterSlider;
    private Slider bgmSlider;
    private Slider sfxSlider;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        masterSlider = root.Q<Slider>("MasterSlider");
        bgmSlider = root.Q<Slider>("BGMSlider");
        sfxSlider = root.Q<Slider>("SFXSlider");

        if (AudioManager.Instance != null)
        {
            // 初始值
            masterSlider.value = AudioManager.Instance.MasterVolume;
            bgmSlider.value = AudioManager.Instance.BGMVolume;
            sfxSlider.value = AudioManager.Instance.SFXVolume;

            // 注册变化回调
            masterSlider.RegisterValueChangedCallback(evt => {
                Debug.Log($"MasterSlider value changed: {evt.newValue}");
                AudioManager.Instance.MasterVolume = evt.newValue;
            });
            bgmSlider.RegisterValueChangedCallback(evt => {
                Debug.Log($"BGMSlider value changed: {evt.newValue}");
                AudioManager.Instance.BGMVolume = evt.newValue;
            });
            sfxSlider.RegisterValueChangedCallback(evt => {
                Debug.Log($"SFXSlider value changed: {evt.newValue}");
                AudioManager.Instance.SFXVolume = evt.newValue;
            });
        }
        root.Q<Button>("CloseBtn").clicked += () => gameObject.SetActive(false);
    }
}