using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InstrumentCollisionSound : MonoBehaviour
{
    [Header("Playback")]
    [SerializeField] private float minPlayInterval = 0.4f;

    private AudioSource audioSource;
    private float lastPlayTime = Mathf.NegativeInfinity;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastPlayTime < minPlayInterval) return;
        PlaySound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastPlayTime < minPlayInterval) return;
        PlaySound();
    }

    private void PlaySound()
    {
        Debug.Log("Playing sound for " + gameObject.name);

        if (audioSource == null || audioSource.clip == null) return;
        audioSource.Stop();
        audioSource.Play();
        lastPlayTime = Time.time;
    }
}

