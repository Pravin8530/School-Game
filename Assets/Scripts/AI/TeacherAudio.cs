using UnityEngine;

public class TeacherAudio : MonoBehaviour
{
    public AudioClip nationalAnthem;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            Debug.Log("audioSorceis Nullheheh");

        }
    }

    void Update()
    {
        
    }

    public void PlaySoundClip(AudioClip audioClip)
    {
        if(audioClip == null)
        {
            Debug.Log("audio clip is null");
            return;
        }
        audioSource.PlayOneShot(audioClip);


    }

}
