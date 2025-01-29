using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // SINGLETON
    public static AudioManager Instance { get; private set; }

    [Header("--------- Audio Source -----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------- Audio Clip -----------")]
    public AudioClip background;
    public AudioClip randomNoise;


    [SerializeField] float musicVolume = 1.0f;
    [SerializeField] float SFXVolume = 1.0f;

    private void Awake()
    {
        // Singleton setup
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

    private void Start()
    {
        musicSource.volume = musicVolume;
        SFXSource.volume = SFXVolume;

        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }



    public void PlaySFXForXSeconds(AudioClip clip, float duration, float fadeDuration = 1.0f)
    {
        StartCoroutine(PlayAudioForXSeconds(clip, duration, fadeDuration));
    }

    private IEnumerator PlayAudioForXSeconds(AudioClip clip, float timeToPlay, float fadeDuration)
    {
        SFXSource.clip = clip;
        SFXSource.Play();

        yield return new WaitForSeconds(timeToPlay);

        // Fading out the audio
        float startVolume = SFXSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            SFXSource.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SFXSource.volume = 0f;
        SFXSource.Stop();

        SFXSource.volume = SFXVolume;
    }

}