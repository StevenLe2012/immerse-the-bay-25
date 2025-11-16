using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


[RequireComponent(typeof(AudioSource))]
public class InstrumentCollisionSound : MonoBehaviour
{
    [Header("Playback")]
    [SerializeField] private float minPlayInterval = 0.4f;

    private AudioSource audioSource;
    private XRGrabInteractable interactable;
    private float lastPlayTime = Mathf.NegativeInfinity;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        interactable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (interactable == null) return;
        interactable.selectEntered.AddListener(HandleSelectEntered);
    }

    private void OnDisable()
    {
        if (interactable == null) return;
        interactable.selectEntered.RemoveListener(HandleSelectEntered);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlaySound();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlaySound();
    }

    private void HandleSelectEntered(SelectEnterEventArgs args)
    {
        PlaySound();
    }

    public void PlaySound()
    {
        Debug.Log("Playing sound for " + gameObject.name);

        if (audioSource == null || audioSource.clip == null) return;
        if (Time.time - lastPlayTime < minPlayInterval) return;
        audioSource.Stop();
        audioSource.Play();
        lastPlayTime = Time.time;
    }
}

