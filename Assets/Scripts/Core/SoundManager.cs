using UnityEngine;

/// <summary>
/// Quan ly 3 tang am thanh: Music / SFX / Ambient.
/// - Doc volume da luu tu SaveManager luc khoi dong.
/// - Chinh volume -> luu lai qua SaveManager.
/// - Cung cap ham phat SFX (flap, point, hit, die), nhac nen, ambient.
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;

    [Header("Music & Ambient Clips")]
    [SerializeField] private AudioClip musicClip;     // Nhac nen (loop)
    [SerializeField] private AudioClip ambientClip;   // Tieng nen: song bien, gio (loop)

    [Header("SFX Clips")]
    [SerializeField] private AudioClip[] flapClips;   // Nhieu tieng vo canh -> random
    [SerializeField] private AudioClip[] pointClips;  // Nhieu tieng ghi diem -> random
    [SerializeField] private AudioClip hitClip;       // Tieng va cham
    [SerializeField] private AudioClip dieClip;       // Tieng chet

    protected override void Awake()
    {
        base.Awake();   // BAT BUOC: logic Singleton

        // Doc volume da luu, ap vao 3 source
        SaveManager.Instance.LoadSettings(out float music, out float sfx, out float ambient);
        musicSource.volume = music;
        sfxSource.volume = sfx;
        ambientSource.volume = ambient;
    }

    private void Start()
    {
        // Phat nhac nen + ambient ngay khi vao game
        PlayMusic();
        PlayAmbient();
    }

    // ==================== NHAC NEN & AMBIENT ====================

    public void PlayMusic()
    {
        if (musicClip == null) return;
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayAmbient()
    {
        if (ambientClip == null) return;
        ambientSource.clip = ambientClip;
        ambientSource.loop = true;
        ambientSource.Play();
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

    public void SetAmbientVolume(float value)
    {
        ambientSource.volume = value;
        SaveCurrentVolumes();
    }

    private void SaveCurrentVolumes()
    {
        SaveManager.Instance.SaveSettings(
            musicSource.volume,
            sfxSource.volume,
            ambientSource.volume
        );
    }
}