using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InstrumentCollisionSound : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private LayerMask handLayers;
    [SerializeField] private string[] handTags = new string[0];

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
        TryPlay(collision.collider.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryPlay(other.gameObject);
    }

    private void TryPlay(GameObject source)
    {
        if (!CanPlay(source)) return;
        PlaySound();
    }

    private bool CanPlay(GameObject source)
    {
        if (source == null || Time.time - lastPlayTime < minPlayInterval) return false;
        if (handLayers.value != 0 && (handLayers.value & (1 << source.layer)) == 0) return false;
        if (handTags.Length == 0) return true;

        foreach (var tag in handTags)
        {
            if (string.IsNullOrEmpty(tag)) continue;
            if (source.CompareTag(tag)) return true;
        }

        return false;
    }

    private void PlaySound()
    {
        if (audioSource == null || audioSource.clip == null) return;
        audioSource.Play();
        lastPlayTime = Time.time;
    }
}

