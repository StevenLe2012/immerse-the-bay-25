using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles displaying synchronized lyrics for karaoke
/// </summary>
public class LyricsDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI currentLyricText;
    [SerializeField] private TextMeshProUGUI nextLyricText;
    [SerializeField] private TextMeshProUGUI previousLyricText;
    
    [Header("Settings")]
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private float fadeSpeed = 2f;
    
    private List<SubtitleEntry> subtitles = new List<SubtitleEntry>();
    private int currentSubtitleIndex = 0;
    private bool isPlaying = false;
    private KaraokeManager karaokeManager;
    
    void Start()
    {
        karaokeManager = GetComponent<KaraokeManager>();
    }
    
    void Update()
    {
        if (isPlaying && subtitles.Count > 0)
        {
            UpdateLyricsDisplay();
        }
    }
    
    /// <summary>
    /// Load subtitles from various formats (SRT, VTT, or JSON)
    /// </summary>
    public void LoadSubtitles(string subtitleData)
    {
        subtitles.Clear();
        currentSubtitleIndex = 0;
        
        // Try to detect format and parse accordingly
        if (subtitleData.Contains("WEBVTT"))
        {
            ParseVTT(subtitleData);
        }
        else if (subtitleData.Contains("-->"))
        {
            ParseSRT(subtitleData);
        }
        else
        {
            // Assume JSON format
            ParseJSON(subtitleData);
        }
        
        Debug.Log($"Loaded {subtitles.Count} subtitle entries");
    }
    
    /// <summary>
    /// Parse SRT subtitle format
    /// </summary>
    private void ParseSRT(string srtData)
    {
        string[] blocks = srtData.Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string block in blocks)
        {
            string[] lines = block.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (lines.Length >= 3)
            {
                // Line 0: Index
                // Line 1: Timestamp
                // Line 2+: Text
                
                string timeLine = lines[1];
                if (timeLine.Contains("-->"))
                {
                    string[] times = timeLine.Split(new string[] { " --> " }, StringSplitOptions.None);
                    
                    float startTime = ParseSRTTime(times[0].Trim());
                    float endTime = ParseSRTTime(times[1].Trim());
                    
                    string text = "";
                    for (int i = 2; i < lines.Length; i++)
                    {
                        text += lines[i] + " ";
                    }
                    
                    subtitles.Add(new SubtitleEntry
                    {
                        startTime = startTime,
                        endTime = endTime,
                        text = text.Trim()
                    });
                }
            }
        }
    }
    
    /// <summary>
    /// Parse VTT subtitle format
    /// </summary>
    private void ParseVTT(string vttData)
    {
        string[] lines = vttData.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("-->"))
            {
                string[] times = lines[i].Split(new string[] { " --> " }, StringSplitOptions.None);
                
                float startTime = ParseVTTTime(times[0].Trim());
                float endTime = ParseVTTTime(times[1].Trim());
                
                string text = "";
                i++;
                while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]) && !lines[i].Contains("-->"))
                {
                    text += lines[i] + " ";
                    i++;
                }
                
                subtitles.Add(new SubtitleEntry
                {
                    startTime = startTime,
                    endTime = endTime,
                    text = text.Trim()
                });
            }
        }
    }
    
    /// <summary>
    /// Parse JSON subtitle format
    /// </summary>
    private void ParseJSON(string jsonData)
    {
        try
        {
            SubtitleList list = JsonUtility.FromJson<SubtitleList>(jsonData);
            if (list != null && list.subtitles != null)
            {
                subtitles.AddRange(list.subtitles);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing JSON subtitles: {e.Message}");
        }
    }
    
    /// <summary>
    /// Parse SRT time format (00:00:00,000)
    /// </summary>
    private float ParseSRTTime(string timeString)
    {
        try
        {
            timeString = timeString.Replace(',', '.');
            string[] parts = timeString.Split(':');
            
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);
            float seconds = float.Parse(parts[2]);
            
            return hours * 3600 + minutes * 60 + seconds;
        }
        catch
        {
            return 0f;
        }
    }
    
    /// <summary>
    /// Parse VTT time format (00:00:00.000)
    /// </summary>
    private float ParseVTTTime(string timeString)
    {
        try
        {
            string[] parts = timeString.Split(':');
            
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);
            float seconds = float.Parse(parts[2]);
            
            return hours * 3600 + minutes * 60 + seconds;
        }
        catch
        {
            return 0f;
        }
    }
    
    /// <summary>
    /// Update the lyrics display based on current playback time
    /// </summary>
    private void UpdateLyricsDisplay()
    {
        if (karaokeManager == null) return;
        
        float currentTime = karaokeManager.GetCurrentTime();
        
        // Find the current subtitle
        for (int i = 0; i < subtitles.Count; i++)
        {
            if (currentTime >= subtitles[i].startTime && currentTime <= subtitles[i].endTime)
            {
                if (currentSubtitleIndex != i)
                {
                    currentSubtitleIndex = i;
                    UpdateTextDisplay();
                }
                break;
            }
        }
    }
    
    /// <summary>
    /// Update the text display with current, previous, and next lyrics
    /// </summary>
    private void UpdateTextDisplay()
    {
        if (currentLyricText != null && currentSubtitleIndex < subtitles.Count)
        {
            currentLyricText.text = subtitles[currentSubtitleIndex].text;
            currentLyricText.color = highlightColor;
        }
        
        if (previousLyricText != null && currentSubtitleIndex > 0)
        {
            previousLyricText.text = subtitles[currentSubtitleIndex - 1].text;
            previousLyricText.color = normalColor;
        }
        else if (previousLyricText != null)
        {
            previousLyricText.text = "";
        }
        
        if (nextLyricText != null && currentSubtitleIndex < subtitles.Count - 1)
        {
            nextLyricText.text = subtitles[currentSubtitleIndex + 1].text;
            nextLyricText.color = normalColor;
        }
        else if (nextLyricText != null)
        {
            nextLyricText.text = "";
        }
    }
    
    /// <summary>
    /// Start displaying lyrics
    /// </summary>
    public void StartDisplay()
    {
        isPlaying = true;
        currentSubtitleIndex = 0;
    }
    
    /// <summary>
    /// Pause lyrics display
    /// </summary>
    public void Pause()
    {
        isPlaying = false;
    }
    
    /// <summary>
    /// Stop and reset lyrics display
    /// </summary>
    public void Stop()
    {
        isPlaying = false;
        currentSubtitleIndex = 0;
        
        if (currentLyricText != null) currentLyricText.text = "";
        if (previousLyricText != null) previousLyricText.text = "";
        if (nextLyricText != null) nextLyricText.text = "";
    }
}

[Serializable]
public class SubtitleEntry
{
    public float startTime;
    public float endTime;
    public string text;
}

[Serializable]
public class SubtitleList
{
    public SubtitleEntry[] subtitles;
}

