using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Resources/Sounds 폴더에 있는 사운드 파일 이름 목록
    private string[] soundNames = {
        "기본 bgm",
        "보스전 bgm",
        "총 사운드",
        "수류탄",
        "칼 사운드",
        "글러치 사운드"
    };

    void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Add AudioSource components
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Load all audio clips from Resources
        foreach (string name in soundNames)
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
            if (clip != null)
            {
                audioClips.Add(name, clip);
            }
            else
            {
                Debug.LogError("SoundManager: Failed to load sound: " + name);
            }
        }
    }

    public void PlayBgm(string bgmName)
    {
        if (audioClips.TryGetValue(bgmName, out AudioClip clip))
        {
            if (bgmSource.clip == clip && bgmSource.isPlaying) return; // Don't restart if already playing

            bgmSource.clip = clip;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("SoundManager: BGM not found: " + bgmName);
        }
    }

    public void StopBgm()
    {
        bgmSource.Stop();
    }

    public void PlaySfx(string sfxName)
    {
        if (audioClips.TryGetValue(sfxName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SoundManager: SFX not found: " + sfxName);
        }
    }
}
