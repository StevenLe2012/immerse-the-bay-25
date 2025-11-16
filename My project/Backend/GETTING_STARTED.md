# 🎵 Getting Started - Karaoke Song Library

## Your Clean Backend

You now have a **minimal, manual song library system**. No YouTube, no APIs, no complexity!

---

## 📁 What You Have (9 Files)

```
Backend/
├── songs/
│   ├── audio/          ← Put your MP3s here
│   ├── thumbnails/     ← Put images here
│   └── lyrics/         ← Put lyrics JSON here
│
├── song-library.json   ← Edit this to add songs!
│
├── server.js           ← API server (don't edit)
├── song-library.js     ← Manager (don't edit)
│
├── README.md           ← API documentation
├── MANUAL_SETUP_GUIDE.md ← Setup guide
├── song-library-example.json ← Template
└── lyrics-example.json ← Lyrics format
```

---

## 🚀 Quick Start (3 Steps)

### 1. Copy Your Files

```powershell
cd "My project\Backend"

# Audio (required)
Copy-Item "C:\your-song.mp3" -Destination "songs\audio\blue.mp3"

# Thumbnail (optional)
Copy-Item "C:\cover.jpg" -Destination "songs\thumbnails\blue.jpg"
```

### 2. Edit song-library.json

```json
[
  {
    "id": "blue",
    "title": "Blue",
    "author": "yung kai",
    "audioFile": "audio/blue.mp3",
    "thumbnailFile": "thumbnails/blue.jpg",
    "duration": 180,
    "tags": ["pop"]
  }
]
```

### 3. Start

```powershell
npm start
```

Test: `http://localhost:3000/api/songs`

---

## 📝 Lyrics Format (Optional)

**File:** `songs/lyrics/blue.json`

```json
{
  "subtitles": [
    {"startTime": 0.0, "endTime": 2.5, "text": "Line 1"},
    {"startTime": 2.5, "endTime": 5.0, "text": "Line 2"}
  ]
}
```

Then add to database:
```json
{
  "id": "blue",
  ...
  "lyricsFile": "lyrics/blue.json"
}
```

---

## 🌐 API

```
GET /api/songs              # List all
GET /api/songs/search?q=... # Search
GET /api/songs/:id/lyrics   # Get lyrics
GET /songs/audio/song.mp3   # Stream audio
```

---

## 🎨 Unity

```csharp
// Get songs
UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/api/songs");
yield return request.SendWebRequest();

// Parse JSON
string json = request.downloadHandler.text;
```

---

## 📚 Docs

- **`MANUAL_SETUP_GUIDE.md`** - Detailed setup
- **`README.md`** - API reference
- **`GETTING_STARTED.md`** - This file

---

## ✅ That's It!

Add songs → Edit JSON → Start server → Done! 🎉

