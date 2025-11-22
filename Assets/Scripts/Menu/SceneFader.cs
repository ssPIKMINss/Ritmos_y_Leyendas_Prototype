using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public Animator animator;
    public AudioSource fadeSound;

    public void FadeToBlack()
    {
        if (fadeSound != null)
            fadeSound.Play();

        animator.SetTrigger("FadeOut"); // ← IMPORTANTÍSIMO
    }
}

