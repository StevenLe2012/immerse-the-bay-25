# Karaoke App Architecture

## System Overview

```
┌─────────────────────────────────────────────────────────────┐
│                     Unity Android App                        │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │              User Interface Layer                   │    │
│  │  ┌──────────┐  ┌──────────┐  ┌──────────────┐    │    │
│  │  │ SearchUI │  │ Playback │  │    Lyrics    │    │    │
│  │  │          │  │ Controls │  │    Display   │    │    │
│  │  └──────────┘  └──────────┘  └──────────────┘    │    │
│  └────────────────────────────────────────────────────┘    │
│                          │                                   │
│  ┌────────────────────────────────────────────────────┐    │
│  │           Business Logic Layer                      │    │
│  │  ┌──────────────────────────────────────────┐     │    │
│  │  │        KaraokeManager                     │     │    │
│  │  │  - Search coordination                    │     │    │
│  │  │  - Audio management                       │     │    │
│  │  │  - Subtitle coordination                  │     │    │
│  │  └──────────────────────────────────────────┘     │    │
│  │                                                     │    │
│  │  ┌──────────────┐  ┌──────────────────────┐      │    │
│  │  │ LyricsDisplay│  │ PlaybackController   │      │    │
│  │  │ - Parse subs │  │ - Audio control      │      │    │
│  │  │ - Sync lyrics│  │ - Progress tracking  │      │    │
│  │  └──────────────┘  └──────────────────────┘      │    │
│  └────────────────────────────────────────────────────┘    │
│                          │                                   │
│  ┌────────────────────────────────────────────────────┐    │
│  │            Network Layer                            │    │
│  │  ┌──────────────────────────────────────────┐     │    │
│  │  │      UnityWebRequest                      │     │    │
│  │  │  - HTTP requests                          │     │    │
│  │  │  - JSON parsing                           │     │    │
│  │  └──────────────────────────────────────────┘     │    │
│  └────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
                          │
                          │ HTTPS
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                    External Services                         │
│                                                              │
│  ┌────────────────────┐        ┌──────────────────────┐    │
│  │  YouTube Data API  │        │   Backend Server     │    │
│  │  - Search videos   │◄───────┤   (Node.js/Express)  │    │
│  │  - Get video info  │        │   - Audio extraction │    │
│  └────────────────────┘        │   - Subtitle fetch   │    │
│                                 │   - Stream audio     │    │
│                                 └──────────────────────┘    │
│                                           │                  │
│                                           ▼                  │
│                                 ┌──────────────────────┐    │
│                                 │   YouTube (ytdl)     │    │
│                                 │   - Video download   │    │
│                                 │   - Caption scraping │    │
│                                 └──────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

## Component Interaction Flow

### 1. Search Flow

```
User Input
    │
    ▼
SearchUI.PerformSearch()
    │
    ▼
KaraokeManager.SearchKaraoke()
    │
    ▼
UnityWebRequest → YouTube Data API v3
    │
    ▼
Parse JSON Response
    │
    ▼
SearchUI.DisplayResults()
    │
    ▼
Show Results to User
```

### 2. Song Selection Flow

```
User Selects Song
    │
    ▼
ResultItemUI.OnSelectClicked()
    │
    ▼
KaraokeManager.SelectVideo()
    │
    ├─────────────────────────┐
    │                         │
    ▼                         ▼
Fetch Video Data      Show Loading State
    │
    ▼
KaraokeManager.ExtractAudioAndSubtitles()
    │
    ▼
UnityWebRequest → Backend Server /extract
    │
    ▼
Backend Server:
    ├─► ytdl-core (extract audio)
    └─► youtube-captions-scraper (get subtitles)
    │
    ▼
Return JSON:
    - audioUrl
    - subtitlesData
    - title
    - duration
    │
    ▼
KaraokeManager.ProcessExtractedData()
    │
    ├─────────────────────────┐
    │                         │
    ▼                         ▼
LoadAudio()          LyricsDisplay.LoadSubtitles()
    │                         │
    ▼                         ▼
AudioSource.clip      Parse & Store Subtitles
    │
    ▼
Ready to Play
```

### 3. Playback Flow

```
User Clicks Play
    │
    ▼
PlaybackController.OnPlayClicked()
    │
    ▼
KaraokeManager.PlayKaraoke()
    │
    ├─────────────────────────┐
    │                         │
    ▼                         ▼
AudioSource.Play()    LyricsDisplay.StartDisplay()
    │                         │
    │                         ▼
    │                   Update Loop:
    │                   - Get current time
    │                   - Find matching subtitle
    │                   - Update UI
    │                         │
    ▼                         ▼
Playback Running ◄──────────► Lyrics Synced
```

## Data Flow Diagram

```
┌─────────────┐
│   User      │
└──────┬──────┘
       │ (1) Search Query
       ▼
┌─────────────────────┐
│   SearchUI          │
└──────┬──────────────┘
       │ (2) Search Request
       ▼
┌─────────────────────┐      (3) API Call      ┌──────────────┐
│  KaraokeManager     │ ─────────────────────► │ YouTube API  │
└──────┬──────────────┘                         └──────────────┘
       │ (4) Results
       ▼
┌─────────────────────┐
│   SearchUI          │
│   Display Results   │
└──────┬──────────────┘
       │ (5) Select Video
       ▼
┌─────────────────────┐      (6) Extract Request  ┌──────────────┐
│  KaraokeManager     │ ──────────────────────►   │ Backend      │
└──────┬──────────────┘                            │ Server       │
       │                                            └──────┬───────┘
       │                                                   │
       │ (7) Audio URL + Subtitles                        │ (6a) Download
       │ ◄─────────────────────────────────────────────────┘
       │
       ├──────────────────┬─────────────────────┐
       │                  │                     │
       ▼                  ▼                     ▼
┌─────────────┐  ┌──────────────┐   ┌──────────────────┐
│ AudioSource │  │ LyricsDisplay│   │ PlaybackController│
└─────────────┘  └──────────────┘   └──────────────────┘
```

## Class Diagram

```
┌─────────────────────────────────────┐
│         KaraokeManager              │
├─────────────────────────────────────┤
│ - youtubeAPIKey: string             │
│ - audioSource: AudioSource          │
│ - lyricsDisplay: LyricsDisplay      │
│ - searchUI: SearchUI                │
│ - currentVideoId: string            │
├─────────────────────────────────────┤
│ + SearchKaraoke(query)              │
│ + SelectVideo(videoId)              │
│ + PlayKaraoke()                     │
│ + PauseKaraoke()                    │
│ + StopKaraoke()                     │
│ - SearchYouTubeVideos(query)        │
│ - ExtractAudioAndSubtitles(videoId) │
│ - LoadAudio(url)                    │
└─────────────────────────────────────┘
              │
              │ uses
              ▼
┌─────────────────────────────────────┐
│         LyricsDisplay               │
├─────────────────────────────────────┤
│ - currentLyricText: TextMeshProUGUI │
│ - subtitles: List<SubtitleEntry>    │
│ - currentSubtitleIndex: int         │
│ - isPlaying: bool                   │
├─────────────────────────────────────┤
│ + LoadSubtitles(data)               │
│ + StartDisplay()                    │
│ + Pause()                           │
│ + Stop()                            │
│ - ParseSRT(data)                    │
│ - ParseVTT(data)                    │
│ - UpdateLyricsDisplay()             │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│         SearchUI                    │
├─────────────────────────────────────┤
│ - searchInputField: TMP_InputField  │
│ - resultsContainer: Transform       │
│ - resultItemPrefab: GameObject      │
├─────────────────────────────────────┤
│ + PerformSearch(query)              │
│ + DisplayResults(results)           │
│ - CreateResultItem(result)          │
│ - ClearResults()                    │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│      PlaybackController             │
├─────────────────────────────────────┤
│ - playButton: Button                │
│ - progressSlider: Slider            │
│ - volumeSlider: Slider              │
│ - audioSource: AudioSource          │
├─────────────────────────────────────┤
│ + OnPlayClicked()                   │
│ + OnPauseClicked()                  │
│ + OnStopClicked()                   │
│ - UpdateProgressDisplay()           │
└─────────────────────────────────────┘
```

## Backend API Architecture

```
┌─────────────────────────────────────────────┐
│           Express.js Server                  │
├─────────────────────────────────────────────┤
│                                              │
│  ┌────────────────────────────────────┐    │
│  │         Middleware Layer            │    │
│  │  - CORS                             │    │
│  │  - JSON Parser                      │    │
│  │  - Error Handler                    │    │
│  └────────────────────────────────────┘    │
│                                              │
│  ┌────────────────────────────────────┐    │
│  │         Route Handlers              │    │
│  │                                     │    │
│  │  POST /extract                      │    │
│  │  GET  /video-info/:videoId          │    │
│  │  GET  /stream/:videoId              │    │
│  │  GET  /subtitles/:videoId/languages │    │
│  │  GET  /subtitles/:videoId/:lang     │    │
│  │  GET  /health                       │    │
│  └────────────────────────────────────┘    │
│                                              │
│  ┌────────────────────────────────────┐    │
│  │      Service Layer                  │    │
│  │                                     │    │
│  │  ┌──────────────────────────┐     │    │
│  │  │  ytdl-core               │     │    │
│  │  │  - getInfo()             │     │    │
│  │  │  - chooseFormat()        │     │    │
│  │  │  - download()            │     │    │
│  │  └──────────────────────────┘     │    │
│  │                                     │    │
│  │  ┌──────────────────────────┐     │    │
│  │  │  youtube-captions-scraper│     │    │
│  │  │  - getSubtitles()        │     │    │
│  │  └──────────────────────────┘     │    │
│  └────────────────────────────────────┘    │
└─────────────────────────────────────────────┘
```

## State Management

### Application States

```
┌─────────────┐
│   Initial   │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│  Searching  │ ◄──┐
└──────┬──────┘    │
       │           │
       ▼           │
┌─────────────┐    │
│   Results   │ ───┘
│   Displayed │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Loading   │
│   Song      │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│   Ready     │
│   to Play   │
└──────┬──────┘
       │
       ├──────┬──────┬──────┐
       │      │      │      │
       ▼      ▼      ▼      ▼
   ┌────┐ ┌────┐ ┌────┐ ┌────┐
   │Play│ │Pause│ │Stop│ │Seek│
   └────┘ └────┘ └────┘ └────┘
```

## Threading Model

```
┌─────────────────────────────────────┐
│          Main Thread                 │
│  - UI Updates                        │
│  - User Input                        │
│  - Audio Playback                    │
│  - Lyrics Synchronization            │
└─────────────────────────────────────┘
              │
              │ Coroutines
              ▼
┌─────────────────────────────────────┐
│      Asynchronous Operations         │
│  - Network Requests                  │
│  - Audio Loading                     │
│  - Thumbnail Loading                 │
│  - JSON Parsing                      │
└─────────────────────────────────────┘
```

## Security Architecture

```
┌─────────────────────────────────────┐
│         Unity App (Client)           │
│  - API Key (should be secured)       │
│  - HTTPS only in production          │
└──────────────┬──────────────────────┘
               │ HTTPS
               ▼
┌─────────────────────────────────────┐
│      Backend Server (Proxy)          │
│  - CORS validation                   │
│  - Rate limiting                     │
│  - Input validation                  │
│  - Error handling                    │
└──────────────┬──────────────────────┘
               │ HTTPS
               ▼
┌─────────────────────────────────────┐
│      External APIs                   │
│  - YouTube Data API                  │
│  - YouTube Video Service             │
└─────────────────────────────────────┘
```

## Deployment Architecture

```
┌─────────────────────────────────────┐
│      Android Device                  │
│  ┌─────────────────────────────┐   │
│  │   Karaoke App (APK)          │   │
│  │   - Unity Runtime            │   │
│  │   - Mono/.NET                │   │
│  └─────────────────────────────┘   │
└──────────────┬──────────────────────┘
               │ Internet
               ▼
┌─────────────────────────────────────┐
│    Cloud Platform (Railway/Render)   │
│  ┌─────────────────────────────┐   │
│  │   Backend Server Container   │   │
│  │   - Node.js Runtime          │   │
│  │   - Express Server           │   │
│  │   - ytdl-core                │   │
│  └─────────────────────────────┘   │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│      Google Cloud Platform           │
│  - YouTube Data API v3               │
│  - API Key Management                │
│  - Quota Management                  │
└─────────────────────────────────────┘
```

## Performance Considerations

### Optimization Points

1. **Network Layer**
   - Connection pooling
   - Request caching
   - Thumbnail caching
   - Progressive loading

2. **Memory Management**
   - Audio clip disposal
   - Texture cleanup
   - Object pooling for results

3. **UI Rendering**
   - Lazy loading of results
   - Virtual scrolling
   - Texture atlasing

4. **Audio Playback**
   - Streaming vs downloading
   - Buffer management
   - Format optimization

---

This architecture provides a scalable, maintainable foundation for the karaoke app with clear separation of concerns and well-defined component interactions.

