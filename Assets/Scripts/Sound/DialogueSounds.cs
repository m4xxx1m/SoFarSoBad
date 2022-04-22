using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] public AudioClip VrudniSpeech;
    [SerializeField] public AudioClip RatSpeech;
    [SerializeField] public AudioClip TronedSpeech;
    [SerializeField] public AudioClip RatSpeech2;

    //private static DialogueSounds instance;

    private AudioSource musicAudioSource;

    private void Awake()
    {
        //instance = this;
        musicAudioSource = GetComponent<AudioSource>();
    }

    /*public static DialogueSounds getInstance()
    {
        return instance;
    }*/

    public void PlaySound(AudioClip clipToPlay, float volume)
    {
        musicAudioSource.clip = clipToPlay;
        musicAudioSource.volume = volume;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void StopSound()
    {
        musicAudioSource.Stop();
    }
}
