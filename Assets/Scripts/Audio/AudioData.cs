using UnityEngine;
[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData", order = 1)]
public class AudioData : ScriptableObject
{ 
     public AudioClip audioClip;
   [Range(0f, 1f)] public float volume = 1f;
   [Range(0.1f, 3f)] public float pitch = 1f;
 
    [Range(0f, 0.3f)] public float pitchRandomness = 0.05f;
 
    
    //   public void getAudioClip(AudioData config)
    // {
    //      audioClip = config.audioClip;

        
    //     volume = config.volume;

    //     pitch = config.pitch + Random.Range(-pitchRandomness, pitchRandomness);
    // }

}