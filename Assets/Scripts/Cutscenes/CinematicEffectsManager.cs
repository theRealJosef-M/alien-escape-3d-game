using UnityEngine;
using System.Collections;

public class CinematicEffectsManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup screenBlurCanvasGroup;
    [SerializeField] private Image screenBlurImage;
    [SerializeField] private float blurIntensity = 1f;
    [SerializeField] private AudioClip knockoutSound;
    [SerializeField] private AudioSource audioSource;

    private Material blurMaterial;

    private void Start()
    {
        if (screenBlurImage != null)
        {
            blurMaterial = new Material(screenBlurImage.material);
            screenBlurImage.material = blurMaterial;
        }
    }

    public void PlayKnockoutEffect()
    {
        StartCoroutine(KnockoutEffectCoroutine());
    }

    private IEnumerator KnockoutEffectCoroutine()
    {
        // Play knockout sound
        if (audioSource != null && knockoutSound != null)
        {
            audioSource.PlayOneShot(knockoutSound);
        }

        // Blur effect
        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            if (screenBlurCanvasGroup != null)
            {
                screenBlurCanvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            }

            if (blurMaterial != null)
            {
                blurMaterial.SetFloat("_BlurIntensity", Mathf.Lerp(0, blurIntensity, progress));
            }

            yield return null;
        }

        // Hold blur
        yield return new WaitForSeconds(1f);

        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / 1f;

            if (screenBlurCanvasGroup != null)
            {
                screenBlurCanvasGroup.alpha = Mathf.Lerp(1, 0, progress);
            }

            yield return null;
        }
    }

    public IEnumerator PlayWakeUpSequence()
    {
        // Fade in from black
        if (screenBlurCanvasGroup != null)
        {
            screenBlurCanvasGroup.alpha = 1;
        }

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            if (screenBlurCanvasGroup != null)
            {
                screenBlurCanvasGroup.alpha = Mathf.Lerp(1, 0, progress);
            }

            yield return null;
        }
    }
}
