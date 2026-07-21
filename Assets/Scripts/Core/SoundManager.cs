using UnityEngine;

/// <summary>
/// Quan ly 2 tang am thanh: Music / SFX.
/// - Music: nhac nen (loop).
/// - SFX: tat ca am thanh con lai (flap, point, hit, die).
/// - Doc volume da luu tu SaveManager luc khoi dong.
/// - Chinh volume -> luu lai qua SaveManager.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clip")]
    [SerializeField] private AudioClip musicClip;     // Nhac nen (loop)

    [Header("SFX Clips")]
    [SerializeField] private AudioClip[] flapClips;   // Nhieu tieng vo canh -> random
    [SerializeField] private AudioClip[] pointClips;  // Nhieu tieng ghi diem -> random
    [SerializeField] private AudioClip hitClip;       // Tieng va cham
    [SerializeField] private AudioClip dieClip;       // Tieng chet

    protected override void Awake()
    {
        base.Awake();   // BAT BUOC: logic Singleton

        // Doc volume da luu, ap vao 2 source
        SaveManager.Instance.LoadSettings(out float music, out float sfx);
        musicSource.volume = music;
        sfxSource.volume = sfx;
    }

    private void Start()
    {
        // Phat nhac nen ngay khi vao game
        PlayMusic();
    }

    // ==================== NHAC NEN ====================

    public void PlayMusic()
    {
        if (musicClip == null) return;
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // ==================== SFX ====================

    /// <summary> Vo canh: phat 1 tieng ngau nhien trong flapClips. </summary>
    public void PlayRandomFlap()
    {
        PlayRandom(flapClips);
    }

    /// <summary> Ghi diem: phat 1 tieng ngau nhien trong pointClips. </summary>
    public void PlayRandomPoint()
    {
        PlayRandom(pointClips);
    }

    public void PlayHit()
    {
        if (hitClip != null) sfxSource.PlayOneShot(hitClip);
    }

    public void PlayDie()
    {
        if (dieClip != null) sfxSource.PlayOneShot(dieClip);
    }

    // Chon ngau nhien 1 clip trong mang roi phat.
    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        int i = Random.Range(0, clips.Length);   // 0 -> length-1
        sfxSource.PlayOneShot(clips[i]);
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