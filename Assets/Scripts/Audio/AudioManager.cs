using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource sfxAudioSource;
    public AudioSource ambientAudioSource;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetupAudioSources();
    }


   
    private void SetupAudioSources()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        if (audioSources.Length > 0 && audioSources[0] != null)
        {
            sfxAudioSource = audioSources[0];
        }
        if (audioSources.Length > 1 && audioSources[1] != null)
        {
            ambientAudioSource = audioSources[1];
        }

    }

    public void PlayAudioClip(AudioClip clip)
    {
        if (sfxAudioSource != null && clip != null)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayAudioClipAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }

    public void PlayAmbientAudio(AudioClip clip)
    {

        if (ambientAudioSource != null && clip != null)
        {
            ambientAudioSource.clip = clip;
            ambientAudioSource.pitch = 1f; // Reset pitch to normal
            ambientAudioSource.volume = 1f; // Reset volume to normal
            ambientAudioSource.loop = true;
            ambientAudioSource.Play();
        }
    }

    public void StopAmbientAudio()
    {
        if (ambientAudioSource != null)
        {
            ambientAudioSource.Stop();
        }
    }
}
