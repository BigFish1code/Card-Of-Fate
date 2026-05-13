using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer References")]
    public AudioMixer mixer;
    public AudioMixerSnapshot normalSnapshot;
    public AudioMixerSnapshot battleSnapshot;
    public AudioMixerSnapshot bossSnapshot;

    [Header("SFX Pool Settings")]
    public int sfxPoolSize = 15;
    public float defaultSfxVolume = 0.8f;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    [Header("BGM Settings")]
    public float bgmFadeTime = 1.5f;

    // 缓存已加载的 AudioClip
    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>();

    // 对象池
    private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
    private List<AudioSource> activeSfx = new List<AudioSource>();

    private AudioSource bgmSource;
    private string currentBgmKey;

    [Header("Volume Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float bgmVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;

    private Dictionary<AudioSource, float> sfxBaseVolumes = new Dictionary<AudioSource, float>();
    public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = Mathf.Clamp01(value); ApplyVolumes(); SaveVolumes();
        }
    }
    public float BGMVolume
    {
        get => bgmVolume;
        set
        {
            bgmVolume = Mathf.Clamp01(value); ApplyVolumes(); SaveVolumes();
        }
    }
    public float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = Mathf.Clamp01(value); ApplyVolumes(); SaveVolumes();
        }
    }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 创建音效池
        GameObject poolRoot = new GameObject("SFX_Pool");
        poolRoot.transform.parent = transform;
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var go = new GameObject("SFX_Source_" + i);
            go.transform.parent = poolRoot.transform;
            var source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            source.spatialBlend = 0f; // 2D音效
            go.SetActive(false);
            sfxPool.Enqueue(source);
        }

        // 创建背景音乐专用音源
        GameObject bgmGo = new GameObject("BGM_Source");
        bgmGo.transform.parent = transform;
        bgmSource = bgmGo.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];

        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        Debug.Log($"ApplyVolumes - Master:{masterVolume} BGM:{bgmVolume} SFX:{sfxVolume}");

        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume * masterVolume;
            Debug.Log($"Set BGM volume = {bgmSource.volume}");
        }

        foreach (var kvp in sfxBaseVolumes)
        {
            kvp.Key.volume = kvp.Value * sfxVolume * masterVolume;
            Debug.Log($"Set SFX volume for {kvp.Key.name} = {kvp.Key.volume}");
        }
    }
    private void SaveVolumes()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    // ================== 公共接口 ==================

    /// <summary>
    /// 播放一个音效（通过 Addressables Key 异步加载）
    /// </summary>
    public void PlaySFX(string key, float volume = -1f, float pitch = 0f)
    {
        if (volume < 0f) volume = defaultSfxVolume;
        if (pitch == 0f) pitch = Random.Range(minPitch, maxPitch);
        StartCoroutine(PlaySFXCoroutine(key, volume, pitch));
    }

    /// <summary>
    /// 切换背景音乐（带淡入淡出）
    /// </summary>
    public void PlayBGM(string key, float fadeTime = -1f)
    {
        if (fadeTime < 0f) fadeTime = bgmFadeTime;
        StartCoroutine(PlayBGMCoroutine(key, fadeTime));
    }

    /// <summary>
    /// 切换到指定快照（如进入战斗、BOSS战）
    /// </summary>
    public void SetMixerSnapshot(AudioMixerSnapshot snapshot, float transitionTime = 0.5f)
    {
        if (snapshot != null)
            snapshot.TransitionTo(transitionTime);
    }

    /// <summary>
    /// 预加载一组音效（场景加载时调用，避免首次播放卡顿）
    /// </summary>
    public void PreloadSFX(string[] keys)
    {
        foreach (var key in keys)
        {
            if (!sfxClips.ContainsKey(key))
                StartCoroutine(PreloadSFXCoroutine(key));
        }
    }

    // ================== 内部实现 ==================

    private IEnumerator PlaySFXCoroutine(string key, float volume, float pitch)
    {
        AudioClip clip = null;
        if (sfxClips.TryGetValue(key, out clip))
        {
            PlayFromPool(clip, volume, pitch);
            yield break;
        }

        var handle = Addressables.LoadAssetAsync<AudioClip>(key);
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            clip = handle.Result;
            sfxClips[key] = clip;
            PlayFromPool(clip, volume, pitch);
        }
        else
        {
            Debug.LogWarning($"Failed to load SFX: {key}");
        }
    }

    private void PlayFromPool(AudioClip clip, float volume, float pitch)
    {
        AudioSource source = null;
        if (sfxPool.Count > 0)
        {
            source = sfxPool.Dequeue();
        }
        else
        {
            // 池子耗尽，临时创建
            var go = new GameObject("SFX_Extra");
            go.transform.parent = transform;
            source = go.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            source.spatialBlend = 0f;
            activeSfx.Add(source);
        }

        source.gameObject.SetActive(true);
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        sfxBaseVolumes[source] = volume;
        source.volume = volume * sfxVolume * masterVolume;
        activeSfx.Add(source);

        // 根据实际播放时长延时回收
        StartCoroutine(ReturnToPool(source, clip.length / pitch + 0.1f));
    }

    private IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Stop();
        source.clip = null;
        source.pitch = 1f;
        source.gameObject.SetActive(false);
        activeSfx.Remove(source);
        sfxBaseVolumes.Remove(source);  // 清理字典
        if (sfxPool.Count < sfxPoolSize)
            sfxPool.Enqueue(source);
        else
            Destroy(source.gameObject);
    }

    private IEnumerator PlayBGMCoroutine(string key, float fadeTime)
    {
        if (currentBgmKey == key) yield break;

        // 淡出当前音乐
        if (bgmSource.isPlaying)
        {
            float startVol = bgmSource.volume;
            for (float t = 0; t < fadeTime; t += Time.deltaTime)
            {
                bgmSource.volume = Mathf.Lerp(startVol, 0f, t / fadeTime);
                yield return null;
            }
            bgmSource.Stop();
        }

        AudioClip newClip = null;
        if (bgmClips.TryGetValue(key, out newClip))
        {
            StartNewBGM(newClip, fadeTime);
            currentBgmKey = key;
            yield break;
        }

        var handle = Addressables.LoadAssetAsync<AudioClip>(key);
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            newClip = handle.Result;
            bgmClips[key] = newClip;
            StartNewBGM(newClip, fadeTime);
            currentBgmKey = key;
        }
        else
        {
            Debug.LogWarning($"Failed to load BGM: {key}");
        }
    }

    private void StartNewBGM(AudioClip clip, float fadeTime)
    {
        bgmSource.clip = clip;
        bgmSource.volume = 0f;
        bgmSource.Play();
        StartCoroutine(FadeInBGM(fadeTime));
    }

    private IEnumerator FadeInBGM(float fadeTime)
    {
        float startVolume = 0f;
        bgmSource.volume = startVolume;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            // 实时读取当前全局音量
            float targetVolume = bgmVolume * masterVolume;
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, t / fadeTime);
            yield return null;
        }
        // 最终确保正确
        bgmSource.volume = bgmVolume * masterVolume;
    }

    private IEnumerator PreloadSFXCoroutine(string key)
    {
        if (sfxClips.ContainsKey(key)) yield break;
        var handle = Addressables.LoadAssetAsync<AudioClip>(key);
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
            sfxClips[key] = handle.Result;
    }
}