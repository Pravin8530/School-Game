using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations; 

public class AddressablesAudioManager : MonoBehaviour
{
   [SerializeField] private AudioSource musicSource;
   [SerializeField] private AudioSource sfxSource;

 public static AddressablesAudioManager Instance{ get; private set;}
 
 
    private Dictionary<string, AsyncOperationHandle<AudioClip>> lodedAudioHandles =  new Dictionary<string, AsyncOperationHandle<AudioClip>>();
    
    private HashSet<string> currentlyLoading = new HashSet<string>();
    void Awake()
    {
        if(Instance != null && Instance != this){Destroy(gameObject);return;}
        Instance= this;
        DontDestroyOnLoad(gameObject);
    }    

    public void PlayMusic(string clipName)
    {
        // if clip handle inside the dict
        // if yes play that
        if (lodedAudioHandles.ContainsKey(clipName))
        {      
           Playclip(lodedAudioHandles[clipName].Result);
           return;

        }
        if(currentlyLoading.Contains(clipName)) return;

         currentlyLoading.Add(clipName);


        Addressables.LoadAssetAsync<AudioClip>(clipName).Completed += handle =>
        {  
             currentlyLoading.Remove(clipName);
             AudioClip loadedClip = handle.Result;

            if (!lodedAudioHandles.ContainsKey(clipName))
            {
                lodedAudioHandles.Add(clipName, handle);
            
            }

            
             musicSource.clip = loadedClip;
             musicSource.Play();
        };  

    }
 

    public void PlaySfx(string clipName)
    {
         if(currentlyLoading.Contains(clipName)) return;

         currentlyLoading.Add(clipName);

        Addressables.LoadAssetAsync<AudioClip>(clipName).Completed += handle =>
        { 
            currentlyLoading.Remove(clipName);

             AudioClip sfxClip = handle.Result;
             sfxSource.PlayOneShot(sfxClip);

             Addressables.Release(handle);

        };

    }

    private void Playclip(AudioClip clip)
    {
       musicSource.clip=clip;
       musicSource.Play();     

    }
   
  


     public void FadeMusic(float fadeduriation)
    {
        
      StartCoroutine(FadeMusicRoutine(fadeduriation));

    }

    private IEnumerator FadeMusicRoutine(float duriation)
    {
      float elaspedTime =0f;
     float startVolume = musicSource.volume;
        while (elaspedTime < duriation)
        {
            elaspedTime +=Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume,0,elaspedTime/duriation);

                yield return null;

        }
        
        musicSource.volume=0f;
        musicSource.clip= null;
        musicSource.volume = startVolume; 

    }


}
