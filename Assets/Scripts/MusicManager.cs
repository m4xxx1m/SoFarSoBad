using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] public AudioClip musicClip;

    private AudioSource musicAudioSource;
    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    private void PlayMusic()
    {
        musicAudioSource.loop = true;
        musicAudioSource.clip = musicClip;
        musicAudioSource.volume = 0.5f;
        musicAudioSource.Play();
    }
}
