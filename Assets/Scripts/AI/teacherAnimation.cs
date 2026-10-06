using UnityEngine;

public class teacherAnimation : MonoBehaviour
{
    
    
    [Header("Animation Settings")]
    public Animator animator;

    [Header("Animation Clips")]
    public AnimationClip patrolClip;
    public AnimationClip supriseClip;
    public AnimationClip searchClip;
    public AnimationClip chaseClip;
    public AnimationClip stunClip;
    public AnimationClip catchClip;



    public void PlayAnimation(AnimationClip clip, float crossfadeTime = 0.15f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Animation clip is null. Cannot play animation.");
            return;
        }

        if (animator != null)
        {
            if (!animator.gameObject.activeInHierarchy)
            {
                animator.gameObject.SetActive(true);
            }

            if (!animator.enabled)
            {
                animator.enabled = true;
            }

            animator.CrossFade(clip.name, crossfadeTime);
            Debug.Log("Playing animation: " + clip.name);
        }
    }


    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found in children.");
            }
        }
    }
}
