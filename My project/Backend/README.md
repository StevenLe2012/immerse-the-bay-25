# 🎵 Karaoke Backend - Song Library API

## Overview

A simple, curated song library system for your karaoke app. You manually manage songs by editing a JSON file - users search and play from your collection.

---

## 🚀 Quick Start

### 1. Add Your Song Files

```powershell
cd "My project\Backend"

# Copy MP3 to audio folder
Copy-Item "C:\path\to\song.mp3" -Destination "songs\audio\blue.mp3"

# Copy thumbnail (optional)
Copy-Item "C:\path\to\cover.jpg" -Destination "songs\thumbnails\blue.jpg"

# Copy lyrics (optional)
Copy-Item "C:\path\to\lyrics.json" -Destination "songs\lyrics\blue.json"
```

### 2. Edit song-library.json

```json
[
  {
    "id": "blue-yungkai",
    "title": "Blue",
    "author": "yung kai",
    "audioFile": "audio/blue.mp3",
    "thumbnailFile": "thumbnails/blue.jpg",
    "lyricsFile": "lyrics/blue.json",
    "duration": 180,
    "tags": ["pop", "karaoke"]
  }
]
```

### 3. Start Server

```bash
npm start
```

### 4. Test

```
http://localhost:3000/api/songs
```

---

## 📁 Folder Structure

```
Backend/
├── songs/
│   ├── audio/          ← Your MP3 files
│   ├── thumbnails/     ← Cover images
│   └── lyrics/         ← Lyrics JSON files
│
├── song-library.json   ← YOUR DATABASE (edit this!)
├── server.js           ← API server
└── song-library.js     ← Library manager
```

---

## 🌐 API Endpoints

### List All Songs
```
GET /api/songs
```

Response:
```json
{
  "success": true,
  "count": 10,
  "songs": [...]
}
```

### Search Songs
```
GET /api/songs/search?q=blue
```

### Get Song Details
```
GET /api/songs/:id
```

### Get Lyrics
```
GET /api/songs/:id/lyrics
```

Response:
```json
{
  "success": true,
  "lyrics": {
    "subtitles": [
      {
        "startTime": 0.0,
        "endTime": 2.5,
        "text": "First line"
      }
    ]
  }
}
```

### Stream Audio
```
GET /songs/audio/song.mp3
```

### Get Thumbnail
```
GET /songs/thumbnails/cover.jpg
```

### Get Statistics
```
GET /api/stats
```

---

## 📝 Database Format

**File:** `song-library.json`

```json
[
  {
    "id": "unique-id",
    "title": "Song Title",
    "author": "Artist Name",
    "audioFile": "audio/song.mp3",
    "thumbnailFile": "thumbnails/cover.jpg",
    "lyricsFile": "lyrics/lyrics.json",
    "duration": 180,
    "tags": ["pop", "karaoke"]
  }
]
```

**Fields:**
- `id` (required) - Unique identifier
- `title` (required) - Song name
- `author` (required) - Artist name
- `audioFile` (required) - Path to MP3 file
- `thumbnailFile` (optional) - Path to image
- `lyricsFile` (optional) - Path to lyrics JSON
- `duration` (optional) - Length in seconds
- `tags` (optional) - Array of search tags

---

## 📋 Lyrics Format

**File:** `songs/lyrics/song.json`

```json
{
  "subtitles": [
    {
      "startTime": 0.0,
      "endTime": 2.5,
      "text": "First line"
    },
    {
      "startTime": 2.5,
      "endTime": 5.0,
      "text": "Second line"
    }
  ]
}
```

---

## 🎨 Unity Integration

### Get All Songs

```csharp
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class Song {
    public string id;
    public string title;
    public string author;
    public string audioFile;
    public string thumbnailFile;
    public string lyricsFile;
}

[System.Serializable]
public class SongsResponse {
    public bool success;
    public Song[] songs;
}

IEnumerator GetSongs() {
    UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/api/songs");
    yield return request.SendWebRequest();
    
    if (request.result == UnityWebRequest.Result.Success) {
        SongsResponse response = JsonUtility.FromJson<SongsResponse>(request.downloadHandler.text);
        Debug.Log($"Loaded {response.songs.Length} songs");
    }
}
```

### Get Lyrics

```csharp
[System.Serializable]
public class Subtitle {
    public float startTime;
    public float endTime;
    public string text;
}

[System.Serializable]
public class LyricsData {
    public Subtitle[] subtitles;
}

[System.Serializable]
public class LyricsResponse {
    public bool success;
    public LyricsData lyrics;
}

IEnumerator GetLyrics(string songId) {
    UnityWebRequest request = UnityWebRequest.Get($"http://localhost:3000/api/songs/{songId}/lyrics");
    yield return request.SendWebRequest();
    
    if (request.result == UnityWebRequest.Result.Success) {
        LyricsResponse response = JsonUtility.FromJson<LyricsResponse>(request.downloadHandler.text);
        
        foreach (var subtitle in response.lyrics.subtitles) {
            Debug.Log($"[{subtitle.startTime}s] {subtitle.text}");
        }
    }
}
```

### Play Audio

```csharp
IEnumerator PlaySong(Song song) {
    string audioUrl = $"http://localhost:3000/songs/{song.audioFile}";
    
    using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.MPEG)) {
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success) {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
```

---

## 📦 Dependencies

```json
{
  "express": "^4.18.2",  // Web server
  "cors": "^2.8.5",      // Cross-origin requests
  "multer": "^2.0.2",    // File handling
  "dotenv": "^17.2.3"    // Environment variables
}
```

**That's all you need!** No YouTube packages, no complex authentication.

---

## 📚 Documentation

| File | Purpose |
|------|---------|
| **`MANUAL_SETUP_GUIDE.md`** | Complete setup instructions |
| `README.md` | This file - API reference |
| `song-library-example.json` | Database template |
| `lyrics-example.json` | Lyrics format template |

---

## 🔧 Adding More Songs

Just edit `song-library.json` and add more objects:

```json
[
  { "id": "song1", "title": "First Song", ... },
  { "id": "song2", "title": "Second Song", ... },
  { "id": "song3", "title": "Third Song", ... }
]
```

---

## ✅ Features

- ✅ List all songs
- ✅ Search by title/author/tags
- ✅ Stream audio files
- ✅ Serve thumbnails
- ✅ Return lyrics with timestamps
- ✅ Library statistics
- ❌ No user uploads (admin-curated only)

---

## 🆘 Support

### Common Issues

**"Song not found"**
- Check file paths in JSON match actual files

**"JSON parse error"**
- Validate at https://jsonlint.com
- Check for missing commas

**"Server won't start"**
- Check port 3000: `netstat -ano | findstr :3000`

---

## 🚀 Commands

```bash
npm start              # Start server
npm run dev            # Start with auto-reload
```

---

## 🎯 Next Steps

1. Add your MP3 files to `songs/audio/`
2. Edit `song-library.json` with song info
3. Start server with `npm start`
4. Integrate with Unity app

---

**Your backend is clean and ready!** 🎵

**See `MANUAL_SETUP_GUIDE.md` for detailed setup instructions.**
