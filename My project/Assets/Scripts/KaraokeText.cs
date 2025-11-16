using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Xml;
using TMPro;
// using UnityEditor.Search;
using UnityEngine;

public class Timestamp
{
    public int hours;
    public int minutes;
    public double seconds;

    // NOTE! Make sure to use GetTotalSeconds if you are comparing with AudioSource.time

    public double GetTotalSeconds()
    {
        return seconds + (60f * minutes) + (60f * 60f * hours);
    }
    public Timestamp(string text)
    {
        // Debug.Log(text);
        var split = text.Split(':');

        if (split.Length == 2)
        {
            hours = 0;
            minutes = int.Parse(split[0]);
            seconds = double.Parse(split[1]);
        }
        else if (split.Length == 3)
        {
            hours = int.Parse(split[0]);
            minutes = int.Parse(split[1]);
            seconds = double.Parse(split[2]);
        }
        else if (split.Length == 1)
        {
            hours = 0;
            minutes = 0;
            seconds = double.Parse(split[0]);
        }
    }
}
public class LyricalLine
{
    public Timestamp start;
    public Timestamp end;
    public string lyric;
}
public class KaraokeText : MonoBehaviour
{
    // audiosource, very important that it is set!!!
    [SerializeField] private AudioSource audioSource;

    // // The canvas UI gameobject.
    // public Canvas canvas;

    // An instance that you can access this class from anywhere.
    public static KaraokeText Instance;

    // MOST IMPORTANT THING!!!
    // This string should be populated with the VTT data.
    // Make sure this is the data and not the filename.
    private string vttLyrics;

    // The Text Lines that our lyrics are displayed.
    [SerializeField] private TextMeshProUGUI nextLine;
    [SerializeField] private TextMeshProUGUI currentLine;

    // The rect transform for the above text lines.
    RectTransform currentRectTransform;
    RectTransform nextRectTransform;

    // The lyrics.
    public List<LyricalLine> lyrics = new List<LyricalLine>();

    float timeElapsed = 0f;
    bool isInitialized = false;
    int currentIndex = 0;

    void Start()
    {
        // NOTE! You may not want to put this in the Start function.
        // Call this function once you recieve the information for the
        // audio and the subtitles.
        // InitKaraoke();
        Instance = this;

        //KaraokeText.Instance.str = File.ReadAllText({ insert filename});
        //KaraokeText.Instance.InitKaraoke();
        //KaraokeText.Instance.UpdateKaraoke();
    }

    public void setupKaraoke(AudioClip audioClip, string vttLyrics)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned in KaraokeText!");
            return;
        }

        if (audioClip == null)
        {
            Debug.LogError("AudioClip is null! Cannot setup karaoke.");
            return;
        }

        this.audioSource.clip = audioClip;
        this.vttLyrics = vttLyrics;
        InitKaraoke();
        audioSource.Play();
    }
    

    // Read VTT Text that is in the str variable.
    // If you need to use a different subtitle format (SRT) let me know on discord.
    private void InitKaraoke()
    {
        lyrics = new List<LyricalLine>();
        var lines = vttLyrics.Split('\n');
        bool isLyrics = false;
        foreach (var line in lines)
        {
            if (isLyrics)
            {
                lyrics[lyrics.Count - 1].lyric = line;
                isLyrics = false;
            }
            else
            {
                var timestamps = line.Split("-->");
                if (timestamps.Length > 1)
                {
                    lyrics.Add(new LyricalLine());
                    lyrics[lyrics.Count - 1].start = new Timestamp(timestamps[0]);
                    lyrics[lyrics.Count - 1].end = new Timestamp(timestamps[1]);
                    isLyrics = true;
                }
            }
        }

        // Set the rect transforms.
        currentRectTransform = currentLine.GetComponent<RectTransform>();
        nextRectTransform = currentLine.GetComponent<RectTransform>();

        // Set the current index.
        currentIndex = 0;

        // NOTE!! Use this isInitialized variable if needed.
        // Update Karaoke should only be called if isInitialized.
        isInitialized = true;

        // Reinitialize time.
        timeElapsed = 0;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // May want to add a checker for this if it works.
        if (isInitialized)
            UpdateKaraoke();
    }
    public void UpdateKaraoke()
    {
        // Check if we have finished all the lyrics or if lyrics list is empty.
        if (currentIndex >= lyrics.Count || lyrics.Count == 0) return;

        // Move to the next lyric if we have finished this lyric.
        if (lyrics[currentIndex].end.GetTotalSeconds() < GetAudioTime())
        {
            currentIndex++;
        }

        // Check again after incrementing.
        if (currentIndex >= lyrics.Count) return;

        var lyric = lyrics[currentIndex];
        var t = (GetAudioTime() - lyric.start.GetTotalSeconds()) / (lyric.end.GetTotalSeconds() - lyric.start.GetTotalSeconds());
        int middle = (int)Mathf.Round((float)t * lyric.lyric.Length);
        if (middle > 0)
        {
            currentLine.richText = true;
            currentLine.text = $"<color=#FF00FF>{lyric.lyric.Substring(0, middle)}<color=#FFFFFF>{lyric.lyric.Substring(middle)}";
        }
        else
        {
            currentLine.text = lyric.lyric;
        }

        // Check if we can set the lyric of the nextLine if it exists.
        // Else set it to nothing.
        if (currentIndex + 1 < lyrics.Count)
        {
            nextLine.text = lyrics[currentIndex + 1].lyric;
        }
        else
        {
            nextLine.text = "";
        }
    }

    // TODO! Replace this line with the AudioSource.time variable.
    // If you want you can reset timeElapsed, but AudioSource.time is more accurate.
    float GetAudioTime()
    {
        if (audioSource == null || audioSource.clip == null)
        {
            return 0f;
        }
        
        // Check if audio is actually playing, otherwise return 0
        if (!audioSource.isPlaying)
        {
            return 0f;
        }
        
        return audioSource.time;
    }
}
