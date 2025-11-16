using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls playback of karaoke audio with UI controls
/// </summary>
public class PlaybackController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private KaraokeManager karaokeManager;
    
    [Header("UI Controls")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI totalTimeText;
    
    [Header("Volume Control")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource audioSource;
    
    private bool isDraggingSlider = false;
    
    void Start()
    {
        if (karaokeManager == null)
        {
            karaokeManager = FindObjectOfType<KaraokeManager>();
        }
        
        SetupButtons();
        SetupSliders();
    }
    
    void Update()
    {
        UpdateProgressDisplay();
    }
    
    /// <summary>
    /// Setup button listeners
    /// </summary>
    private void SetupButtons()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayClicked);
        }
        
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnPauseClicked);
        }
        
        if (stopButton != null)
        {
            stopButton.onClick.AddListener(OnStopClicked);
        }
    }
    
    /// <summary>
    /// Setup slider listeners
    /// </summary>
    private void SetupSliders()
    {
        if (progressSlider != null)
        {
            progressSlider.onValueChanged.AddListener(OnProgressSliderChanged);
        }
        
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            volumeSlider.value = audioSource != null ? audioSource.volume : 1f;
        }
    }
    
    /// <summary>
    /// Play button clicked
    /// </summary>
    private void OnPlayClicked()
    {
        if (karaokeManager != null)
        {
            karaokeManager.PlayKaraoke();
        }
    }
    
    /// <summary>
    /// Pause button clicked
    /// </summary>
    private void OnPauseClicked()
    {
        if (karaokeManager != null)
        {
            karaokeManager.PauseKaraoke();
        }
    }
    
    /// <summary>
    /// Stop button clicked
    /// </summary>
    private void OnStopClicked()
    {
        if (karaokeManager != null)
        {
            karaokeManager.StopKaraoke();
        }
    }
    
    /// <summary>
    /// Progress slider value changed
    /// </summary>
    private void OnProgressSliderChanged(float value)
    {
        if (isDraggingSlider && audioSource != null && audioSource.clip != null)
        {
            audioSource.time = value * audioSource.clip.length;
        }
    }
    
    /// <summary>
    /// Volume slider changed
    /// </summary>
    private void OnVolumeChanged(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
        }
    }
    
    /// <summary>
    /// Update progress display
    /// </summary>
    private void UpdateProgressDisplay()
    {
        if (audioSource == null || audioSource.clip == null) return;
        
        float currentTime = audioSource.time;
        float totalTime = audioSource.clip.length;
        
        // Update slider
        if (progressSlider != null && !isDraggingSlider)
        {
            progressSlider.value = totalTime > 0 ? currentTime / totalTime : 0;
        }
        
        // Update time text
        if (currentTimeText != null)
        {
            currentTimeText.text = FormatTime(currentTime);
        }
        
        if (totalTimeText != null)
        {
            totalTimeText.text = FormatTime(totalTime);
        }
    }
    
    /// <summary>
    /// Format time in MM:SS format
    /// </summary>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    /// <summary>
    /// Called when user starts dragging the progress slider
    /// </summary>
    public void OnSliderDragStart()
    {
        isDraggingSlider = true;
    }
    
    /// <summary>
    /// Called when user stops dragging the progress slider
    /// </summary>
    public void OnSliderDragEnd()
    {
        isDraggingSlider = false;
    }
}

