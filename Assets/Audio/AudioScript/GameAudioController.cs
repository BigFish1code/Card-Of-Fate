using UnityEngine;

public class GameAudioController : MonoBehaviour
{
    public enum SceneType { MainMenu, Battle, BossWin, VictoryOrDefeat }

    [Header("Scene Audio Settings")]
    public SceneType sceneType;
    public string bgmKey;          // 对应 Addressables Key
    public string eventSFXKey;     // 场景进入立即播放的特殊音效（如胜利音效）
    public float sfxDelay = 0f;

    void Start()
    {
        if (AudioManager.Instance == null) return;

        // 切换 BGM
        if (!string.IsNullOrEmpty(bgmKey))
            AudioManager.Instance.PlayBGM(bgmKey);

        // 切换 Mixer 快照
        switch (sceneType)
        {
            case SceneType.MainMenu:
                AudioManager.Instance.SetMixerSnapshot(AudioManager.Instance.normalSnapshot, 0.3f);
                break;
            case SceneType.Battle:
                AudioManager.Instance.SetMixerSnapshot(AudioManager.Instance.battleSnapshot, 0.5f);
                break;
            case SceneType.BossWin:
                AudioManager.Instance.SetMixerSnapshot(AudioManager.Instance.bossSnapshot, 0.8f);
                break;
            case SceneType.VictoryOrDefeat:
                AudioManager.Instance.SetMixerSnapshot(AudioManager.Instance.normalSnapshot, 0.3f);
                break;
        }

        // 如果有进入音效（如胜利欢呼）
        if (!string.IsNullOrEmpty(eventSFXKey))
            Invoke(nameof(PlayEventSFX), sfxDelay);
    }

    private void PlayEventSFX()
    {
        AudioManager.Instance.PlaySFX(eventSFXKey);
    }
}