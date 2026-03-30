using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DungTran31.GamePlay.Player;

public class VignetteOnHit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VolumeProfile volumeProfile;
    private Volume volume;

    [Header("Vignette Settings")]
    private Vignette vignette;
    private Color freezeColor;
    [SerializeField] private float vignetteIntensity = 0.4f;

    [Header("Freeze Settings")]
    [SerializeField] private float freezeDuration = 5f;
    [SerializeField] private float slowPercentage = 50f; // Slow down by percentage

    private void Start()
    {
        // Find references make sure to only have 1 volume in the scene
        volume = FindObjectOfType<Volume>();
        volume.profile = volumeProfile;

        volumeProfile.TryGet(out vignette);

        // Set the freeze color
        if (ColorUtility.TryParseHtmlString("#0200FF", out freezeColor))
            vignette.color.value = freezeColor;
        vignette.intensity.value = 0;
        // Start the coroutine to trigger the vignette effect every 5 seconds
        StartCoroutine(TriggerVignetteEffect());
    }

    private IEnumerator TriggerVignetteEffect()
    {
        while (true)
        {
            // Show freeze vignette effect
            vignette.intensity.value = vignetteIntensity;
            vignette.color.value = freezeColor;

            // Slow down the player
            if (TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                float originalSpeed = playerMovement.moveSpeed;
                playerMovement.moveSpeed *= (1 - slowPercentage / 100f);
                yield return new WaitForSeconds(freezeDuration);
                playerMovement.moveSpeed = originalSpeed;
            }

            // Wait for 5 seconds before triggering the effect again
            yield return new WaitForSeconds(5f);
            // Reset vignette to normal
            vignette.intensity.value = 0; // Assuming 0 is the normal intensity
            vignette.color.value = Color.black; // Assuming black is the normal color

            yield return new WaitForSeconds(5f);
        }
    }
}
