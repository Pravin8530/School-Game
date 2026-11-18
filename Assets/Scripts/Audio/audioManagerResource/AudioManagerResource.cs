using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManagerResource : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SfxSource;

    public static AudioManagerResource Instance { get; set; }

    

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
            return;
        }
        ;
        Instance = this;
        DontDestroyOnLoad(gameObject);


    }

    public void PlayMusic(string clipPath)
    {

        AudioClip musicClip = Resources.Load<AudioClip>(clipPath);
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlayClip(string clipPath)
    {
        AudioClip sfxClip = Resources.Load<AudioClip>(clipPath);
        SfxSource.clip = sfxClip;
        SfxSource.PlayOneShot(sfxClip);
    }


    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;

    }
 
    



    private IEnumerator FadeOutMusicRoutine(float duriation)
    {
        float elaspedTime = 0f;
        float startVolume = musicSource.volume;
        while (elaspedTime < duriation)
        {
            elaspedTime += Time.deltaTime;

            musicSource.volume = Mathf.Lerp(startVolume, 0, elaspedTime / duriation);
            yield return null;
        }
         
         musicSource.volume =0f;
         musicSource.Stop();

    }

    public void FadeOutMusic(float fadeDuriation)
    {
        StartCoroutine(FadeOutMusicRoutine(fadeDuriation));
    }






}
