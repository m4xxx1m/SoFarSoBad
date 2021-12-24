using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] public AudioClip shootClip;
    [SerializeField] public AudioClip deathClip;
    [SerializeField] public AudioClip damageClip;
    [SerializeField] public AudioClip gearPickUpClip;
    [SerializeField] public AudioClip powerUpClip;

    private static SoundManager instance;

    private AudioSource musicAudioSource;

    private void Awake()
    {
        instance = this;
        musicAudioSource = GetComponent<AudioSource>();
    }

    public static SoundManager getInstance()
    {
        return instance;
    }

    public void PlaySound(AudioClip clipToPlay, float volume)
    {
        musicAudioSource.clip = clipToPlay;
        musicAudioSource.volume = volume;
        musicAudioSource.loop = false;
        musicAudioSource.Play();
    }
}
