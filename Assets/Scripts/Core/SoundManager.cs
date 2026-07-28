using UnityEngine;

/// <summary>
/// Quan ly am thanh: Music / SFX (SFX gom ca flap, point, hit, die va tieng song).
/// - Music: nhac nen (loop) - source rieng.
/// - SFX: tat ca am thanh con lai + tieng song ambient - dung chung 1 source.
/// - Doc volume da luu tu SaveManager luc khoi dong, chinh volume -> luu lai.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX (gom ca Ambient)")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] flapClips;
    [SerializeField] private AudioClip[] pointClips;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip dieClip;

    [Header("Ambient (Wave)")]
    [SerializeField] private AudioClip[] waveClips;
    [SerializeField] private float waveInterval = 5f;
    private float waveTimer;

    protected override void Awake()
    {
        base.Awake();   // BAT BUOC: logic Singleton

        // Doc volume da luu, ap vao 2 source
        SaveManager.Instance.LoadSettings(out float music, out float sfx);
        musicSource.volume = music;
        sfxSource.volume   = sfx;
    }

    private void Start()
    {
        PlayMusic();
        waveTimer = waveInterval;
    }

    private void Update()
    {
        // Dem nguoc de phat tieng song ngau nhien
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0f)
        {
            PlayRandomWave();
            waveTimer = waveInterval;
        }
    }

    // ==================== NHAC NEN ====================

    public void PlayMusic()
    {
        if (backgroundMusic == null) return;
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // ==================== SFX ====================

    public void PlayRandomFlap()  => PlayRandom(flapClips);
    public void PlayRandomPoint() => PlayRandom(pointClips);

    public void PlayHit()
    {
        if (hitClip != null) sfxSource.PlayOneShot(hitClip);
    }

    public void PlayDie()
    {
        if (dieClip != null) sfxSource.PlayOneShot(dieClip);
    }

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        int i = Random.Range(0, clips.Length);
        sfxSource.PlayOneShot(clips[i]);
    }

    // ==================== AMBIENT (WAVE) - dung chung sfxSource ====================

    private void PlayRandomWave()
    {
        if (waveClips == null || waveClips.Length == 0) return;
        int i = Random.Range(0, waveClips.Length);
        sfxSource.PlayOneShot(waveClips[i]);
    }

    // ==================== SETTER VOLUME ====================

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        SaveCurrentVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        SaveCurrentVolumes();
    }

    private void SaveCurrentVolumes()
    {
        SaveManager.Instance.SaveSettings(
            musicSource.volume,
            sfxSource.volume
        );
    }
}