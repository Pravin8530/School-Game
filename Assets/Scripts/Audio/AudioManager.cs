using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
 
     [SerializeField] private AudioSource musicSource;
     [SerializeField] private AudioSource uiSource;
     [SerializeField] private AudioSource sfxSound3d;

     [SerializeField] private AudioClip  ambiantMusic;
     [SerializeField] private AudioClip chaseClip; 
     [SerializeField] private AudioClip  uiClick;


    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    public void PlayAmbientMusic () => SwapMusic(ambiantMusic);
    public void PlayUiClip () => uiSource.PlayOneShot(uiClick);
    public void PlayChaseClip () => SwapMusic(chaseClip); 
 
    private void SwapMusic(AudioClip clip)
    {
         if(musicSource.clip == clip && musicSource.isPlaying) return;
         musicSource.clip =clip;
         musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip =null;
    } 
   

    public void FadeOutSound(float fadeOutTime)
    {
        StartCoroutine(FadeOutRoutine(fadeOutTime));
    }
 
    private IEnumerator FadeOutRoutine(float fadeDuriation)
    { 
        float startVolume = musicSource.volume;
         float elaspedTime =0f;

        while(elaspedTime < fadeDuriation)
        {
            elaspedTime += Time.deltaTime;
           musicSource.volume = Mathf.Lerp(startVolume,0,elaspedTime/fadeDuriation);

          yield return null; 
        }
        musicSource.volume =0f;
        musicSource.Stop();
        musicSource.clip =null;
        musicSource.volume = startVolume;

    }

}
