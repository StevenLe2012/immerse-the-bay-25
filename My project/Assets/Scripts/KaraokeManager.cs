using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Main manager for the Karaoke app - handles YouTube search, audio extraction, and lyrics
/// </summary>
public class KaraokeManager : MonoBehaviour
{
    [Header("API Configuration")]
    [SerializeField] private string youtubeAPIKey = "AIzaSyBlP3iaZdC5ChdflHqjo_zga1F_QeNJ7Us";
    
    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private LyricsDisplay lyricsDisplay;
    [SerializeField] private SearchUI searchUI;
    
    private const string YOUTUBE_SEARCH_API = "https://www.googleapis.com/youtube/v3/search";
    private const string YOUTUBE_VIDEO_API = "https://www.googleapis.com/youtube/v3/videos";
    
    private List<VideoSearchResult> searchResults = new List<VideoSearchResult>();
    private string currentVideoId;
    
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        SearchKaraoke("Bohemian Rhapsody");
    }
    
    /// <summary>
    /// Search for karaoke videos on YouTube
    /// </summary>
    public void SearchKaraoke(string searchQuery)
    {
        StartCoroutine(SearchYouTubeVideos(searchQuery + " karaoke"));
    }
    
    /// <summary>
    /// Coroutine to search YouTube using YouTube Data API v3
    /// </summary>
    private IEnumerator SearchYouTubeVideos(string query)
    {
        string url = $"{YOUTUBE_SEARCH_API}?part=snippet&maxResults=10&q={UnityWebRequest.EscapeURL(query)}&type=video&key={youtubeAPIKey}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ParseSearchResults(jsonResponse);
                
                if (searchUI != null)
                {
                    searchUI.DisplayResults(searchResults);
                }
            }
            else
            {
                Debug.LogError($"YouTube Search Error: {request.error}");
                Debug.LogError($"Response: {request.downloadHandler.text}");
            }
        }
    }
    
    /// <summary>
    /// Parse YouTube search results JSON
    /// </summary>
    private void ParseSearchResults(string json)
    {
        searchResults.Clear();
        
        try
        {
            YouTubeSearchResponse response = JsonUtility.FromJson<YouTubeSearchResponse>(json);
            
            if (response != null && response.items != null)
            {
                foreach (var item in response.items)
                {
                    VideoSearchResult result = new VideoSearchResult
                    {
                        videoId = item.id.videoId,
                        title = item.snippet.title,
                        channelTitle = item.snippet.channelTitle,
                        thumbnailUrl = item.snippet.thumbnails.medium.url
                    };
                    searchResults.Add(result);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing search results: {e.Message}");
        }
    }
    
    /// <summary>
    /// Select a video to play and fetch its audio and subtitles
    /// </summary>
    public void SelectVideo(string videoId)
    {
        currentVideoId = videoId;
        StartCoroutine(FetchVideoData(videoId));
    }
    
    /// <summary>
    /// Fetch video data including captions availability
    /// </summary>
    private IEnumerator FetchVideoData(string videoId)
    {
        string url = $"{YOUTUBE_VIDEO_API}?part=snippet,contentDetails&id={videoId}&key={youtubeAPIKey}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Video data fetched successfully");
                // Now we need to extract audio and subtitles
                StartCoroutine(ExtractAudioAndSubtitles(videoId));
            }
            else
            {
                Debug.LogError($"Error fetching video data: {request.error}");
            }
        }
    }
    
    /// <summary>
    /// Extract audio and subtitles from YouTube video
    /// This requires a backend service as direct extraction isn't possible from Unity
    /// </summary>
    private IEnumerator ExtractAudioAndSubtitles(string videoId)
    {
        // IMPORTANT: You need to set up a backend server for this
        // YouTube doesn't allow direct audio extraction from client-side
        // You'll need to use yt-dlp or similar on a server
        
        string backendUrl = "http://localhost:3000/extract";
        string postData = $"{{\"videoId\":\"{videoId}\"}}";
        
        using (UnityWebRequest request = new UnityWebRequest(backendUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ProcessExtractedData(jsonResponse);
            }
            else
            {
                Debug.LogError($"Error extracting audio/subtitles: {request.error}");
            }
        }
    }
    
    /// <summary>
    /// Process the extracted audio and subtitle data
    /// </summary>
    private void ProcessExtractedData(string json)
    {
        try
        {
            ExtractedData data = JsonUtility.FromJson<ExtractedData>(json);
            
            if (!string.IsNullOrEmpty(data.audioUrl))
            {
                StartCoroutine(LoadAudio(data.audioUrl));
            }
            
            if (!string.IsNullOrEmpty(data.subtitlesData))
            {
                if (lyricsDisplay != null)
                {
                    lyricsDisplay.LoadSubtitles(data.subtitlesData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error processing extracted data: {e.Message}");
        }
    }
    
    /// <summary>
    /// Load audio from URL
    /// </summary>
    private IEnumerator LoadAudio(string audioUrl)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.MPEG))
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                audioSource.clip = clip;
                Debug.Log("Audio loaded successfully");
            }
            else
            {
                Debug.LogError($"Error loading audio: {request.error}");
            }
        }
    }
    
    /// <summary>
    /// Play the loaded karaoke track
    /// </summary>
    public void PlayKaraoke()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            if (lyricsDisplay != null)
            {
                lyricsDisplay.StartDisplay();
            }
        }
    }
    
    /// <summary>
    /// Pause the karaoke playback
    /// </summary>
    public void PauseKaraoke()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
            if (lyricsDisplay != null)
            {
                lyricsDisplay.Pause();
            }
        }
    }
    
    /// <summary>
    /// Stop the karaoke playback
    /// </summary>
    public void StopKaraoke()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            if (lyricsDisplay != null)
            {
                lyricsDisplay.Stop();
            }
        }
    }
    
    /// <summary>
    /// Get current playback time
    /// </summary>
    public float GetCurrentTime()
    {
        return audioSource != null ? audioSource.time : 0f;
    }
}

// Data classes for JSON parsing
[Serializable]
public class VideoSearchResult
{
    public string videoId;
    public string title;
    public string channelTitle;
    public string thumbnailUrl;
}

[Serializable]
public class YouTubeSearchResponse
{
    public SearchItem[] items;
}

[Serializable]
public class SearchItem
{
    public VideoId id;
    public Snippet snippet;
}

[Serializable]
public class VideoId
{
    public string videoId;
}

[Serializable]
public class Snippet
{
    public string title;
    public string channelTitle;
    public Thumbnails thumbnails;
}

[Serializable]
public class Thumbnails
{
    public Thumbnail medium;
    public Thumbnail high;
}

[Serializable]
public class Thumbnail
{
    public string url;
}

[Serializable]
public class ExtractedData
{
    public string audioUrl;
    public string subtitlesData;
}

