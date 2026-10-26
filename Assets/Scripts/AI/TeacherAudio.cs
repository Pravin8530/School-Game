using UnityEngine;

public class TeacherAudio : MonoBehaviour
{
   // public AudioClip nationalAnthem;

    [SerializeField] private AudioData audioData;
   //AudioSource audioSource;
    void Start()
    {
        // audioSource = GetComponent<AudioSource>();

        // if(audioSource == null)
        // {
        //     Debug.Log("audioSorceis Nullheheh");

        // }
    }

    void Update()
    {
        
    }

    public void PlaySoundClip()
    {
        // if(audioSource == null)
        // {
        //     Debug.Log("audio source is null");
        //     return;
        // }
        AudioManager.Instance.PlayAudioClip(audioData.audioClip);


    }

}
