using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light[] lights; // Assign your three Point Lights here
    public AudioSource audioSource; // Assign your Audio Source here
    public bool enableFlicker = true; // Toggle for flickering
    public float flickerProbability = 0.3f; // Probability (0 to 1) of flickering each interval
    public float flickerInterval = 5.0f; // Interval in seconds to check for a flicker event
    public float flickerDurationMin = 0.5f; // Minimum duration of a flicker event
    public float flickerDurationMax = 1.5f; // Maximum duration of a flicker event
    public float flickerIntensityMin = 0.5f; // Minimum intensity for flicker effect
    public float flickerIntensityMax = 1.0f; // Maximum intensity for flicker effect
    public float flickerSpeedMin = 0.05f; // Minimum speed between flickers within an event
    public float flickerSpeedMax = 0.15f; // Maximum speed between flickers within an event

    private float originalIntensity;
    private float intervalTimer;

    void Start()
    {
        if (lights.Length > 0)
        {
            originalIntensity = lights[0].intensity;
        }

        intervalTimer = flickerInterval; // Start countdown for the first flicker check
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            if (enableFlicker)
            {
                intervalTimer -= Time.deltaTime;

                // Check if it's time to consider a flicker event
                if (intervalTimer <= 0)
                {
                    intervalTimer = flickerInterval; // Reset the interval timer

                    // Randomly decide if a flicker event should occur based on probability
                    if (Random.value <= flickerProbability)
                    {
                        // Trigger a flicker event with a random duration
                        float flickerDuration = Random.Range(flickerDurationMin, flickerDurationMax);
                        StartFlickerSound(flickerDuration); // Play the flicker sound
                        yield return StartCoroutine(FlickerEvent(flickerDuration));
                    }
                }
            }
            else
            {
                // If flickering is disabled, keep lights stable
                foreach (Light light in lights)
                {
                    light.intensity = originalIntensity;
                    light.enabled = true;
                }
                yield return null; // Wait until the next frame
            }

            yield return null; // Continue looping every frame
        }
    }

    IEnumerator FlickerEvent(float duration)
    {
        float flickerEndTime = Time.time + duration;

        while (Time.time < flickerEndTime)
        {
            // Randomly adjust the intensity within the specified range
            float randomIntensity = Random.Range(flickerIntensityMin, flickerIntensityMax);
            foreach (Light light in lights)
            {
                light.intensity = randomIntensity;
            }

            // Randomly wait for a short period to simulate flicker speed
            float waitTime = Random.Range(flickerSpeedMin, flickerSpeedMax);
            yield return new WaitForSeconds(waitTime);

            // Turn lights off momentarily for realistic flickering
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
            yield return new WaitForSeconds(0.05f); // Small off duration

            foreach (Light light in lights)
            {
                light.enabled = true;
            }
        }

        // Reset lights to original intensity after flicker event
        foreach (Light light in lights)
        {
            light.intensity = originalIntensity;
            light.enabled = true;
        }
    }

    void StartFlickerSound(float duration)
    {

        // Log the audio clip length and flicker duration
        Debug.Log("Playing flicker sound for " + duration + " seconds.");


        if (audioSource != null && audioSource.clip != null)
        {
            // Calculate a random start time within the audio clip length minus the flicker duration
            float maxStartTime = Mathf.Max(0, audioSource.clip.length - duration);
            float startTime = Random.Range(0, maxStartTime);

            // Set the AudioSource to start from the random start time
            audioSource.time = startTime;

            // Play the audio clip
            audioSource.Play();

            // Stop the audio after the flicker duration
            StartCoroutine(StopAudioAfterDuration(duration));
        }
    }

    IEnumerator StopAudioAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}