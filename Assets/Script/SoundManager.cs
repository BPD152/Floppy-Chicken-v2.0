using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float MusicVolume;
    public float SFXVolume;
    public float AmbientVolume;
    public static SoundManager Instance;

    [Header("Music")]
    public AudioSource MusicSource;
    public AudioClip BackgroundMusic;


    [Header("SFX")]
    public AudioSource SfxSource;
    public AudioClip[] PointClip;
    public AudioClip[] FlapClip;


    [Header("WaveSound")]
    public AudioSource AmbientSource;
    public AudioClip[] WaveSound;
    public float WaveInterval = 5f;
    private float WaveTimer;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        if (BackgroundMusic != null)
        {
            MusicSource.clip = BackgroundMusic;
            MusicSource.volume = MusicVolume;
            MusicSource.loop = true;
            MusicSource.Play();
            SfxSource.volume = SFXVolume;
            AmbientSource.volume = AmbientVolume;

        }

        WaveTimer = WaveInterval;
    }


    void Update()
    {
        WaveTimer = WaveTimer - Time.deltaTime;

        if (WaveTimer <= 0f)
        {
            PlayRandomWave();
            WaveTimer = WaveInterval;
            AmbientSource.volume = AmbientVolume;
        }
    }


    public void PlayRandomFlap()
    {
        if (FlapClip.Length > 0)
        {
            AudioClip clip = FlapClip[Random.Range(0, FlapClip.Length)];
            SfxSource.PlayOneShot(clip);
        }
    }


    public void PlayRandomPoint()
    {
        if (PointClip.Length > 0)
        {
            AudioClip clip = PointClip[Random.Range(0, PointClip.Length)];
            SfxSource.PlayOneShot(clip);
        }
    }


    private void PlayRandomWave()
    {
        if (WaveSound.Length > 0)
        {
            AudioClip clip = WaveSound[Random.Range(0, WaveSound.Length)];
            AmbientSource.PlayOneShot(clip);
        }
    }

}
