# 🎵 Manual Song Library Setup Guide

## Overview

You'll manually create your song database by:
1. Adding MP3 files and thumbnails to folders
2. Creating/editing a JSON database file
3. Users can only search and play (no uploads)

---

## 📁 Folder Structure

```
My project/Backend/
├── songs/
│   ├── audio/
│   │   ├── song-1.mp3
│   │   ├── song-2.mp3
│   │   └── song-3.mp3
│   ├── thumbnails/
│   │   ├── thumb-1.jpg
│   │   ├── thumb-2.jpg
│   │   └── thumb-3.jpg
│   └── lyrics/
│       ├── lyrics-1.json
│       ├── lyrics-2.json
│       └── lyrics-3.json
└── song-library.json  ← Your database
```

---

## 📝 Step-by-Step Setup

### Step 1: Add Your Files

Copy your files into these folders:

```powershell
# Audio files
Copy-Item "C:\path\to\your\song.mp3" -Destination "My project\Backend\songs\audio\song-1.mp3"

# Thumbnails
Copy-Item "C:\path\to\your\cover.jpg" -Destination "My project\Backend\songs\thumbnails\thumb-1.jpg"

# Lyrics (if you have them)
Copy-Item "C:\path\to\your\lyrics.json" -Destination "My project\Backend\songs\lyrics\lyrics-1.json"
```

### Step 2: Create song-library.json

Create or edit `song-library.json` in the Backend folder:

```json
[
  {
    "id": "song1",
    "title": "Blue",
    "author": "yung kai",
    "audioFile": "audio/song-1.mp3",
    "thumbnailFile": "thumbnails/thumb-1.jpg",
    "lyricsFile": "lyrics/lyrics-1.json",
    "duration": 180,
    "uploadDate": "2025-11-16T12:00:00.000Z",
    "tags": ["pop", "karaoke", "2024"]
  },
  {
    "id": "song2",
    "title": "Another Song",
    "author": "Artist Name",
    "audioFile": "audio/song-2.mp3",
    "thumbnailFile": "thumbnails/thumb-2.jpg",
    "lyricsFile": "lyrics/lyrics-2.json",
    "duration": 210,
    "uploadDate": "2025-11-16T12:00:00.000Z",
    "tags": ["rock", "karaoke"]
  }
]
```

**That's it!** The server will load this file automatically.

---

## 📋 JSON Template

Copy this template for each song:

```json
{
  "id": "unique-id-here",
  "title": "Song Title",
  "author": "Artist Name",
  "audioFile": "audio/filename.mp3",
  "thumbnailFile": "thumbnails/filename.jpg",
  "lyricsFile": "lyrics/filename.json",
  "duration": 180,
  "uploadDate": "2025-11-16T12:00:00.000Z",
  "tags": ["tag1", "tag2", "tag3"]
}
```

### Field Descriptions:

| Field | Required | Description | Example |
|-------|----------|-------------|---------|
| `id` | ✅ Yes | Unique identifier | `"song1"` or `"blue-yungkai"` |
| `title` | ✅ Yes | Song name | `"Blue"` |
| `author` | ✅ Yes | Artist name | `"yung kai"` |
| `audioFile` | ✅ Yes | Path to MP3 | `"audio/song-1.mp3"` |
| `thumbnailFile` | ❌ No | Path to image | `"thumbnails/thumb-1.jpg"` |
| `lyricsFile` | ❌ No | Path to lyrics | `"lyrics/lyrics-1.json"` |
| `duration` | ❌ No | Length in seconds | `180` |
| `uploadDate` | ❌ No | ISO date string | `"2025-11-16T12:00:00.000Z"` |
| `tags` | ❌ No | Search tags | `["pop", "karaoke"]` |

---

## 📝 Lyrics File Format

If you have lyrics, save them as JSON:

**File:** `songs/lyrics/lyrics-1.json`

```json
{
  "subtitles": [
    {
      "startTime": 0.0,
      "endTime": 2.5,
      "text": "First line of lyrics"
    },
    {
      "startTime": 2.5,
      "endTime": 5.0,
      "text": "Second line"
    },
    {
      "startTime": 5.0,
      "endTime": 8.0,
      "text": "Third line"
    }
  ]
}
```

---

## 🎯 Quick Example

Let's say you have these files:
- `blue.mp3` (your audio)
- `blue-cover.jpg` (cover art)
- `blue-lyrics.json` (lyrics)

### 1. Copy files:
```powershell
cd "My project\Backend"

# Audio
Copy-Item "C:\Music\blue.mp3" -Destination "songs\audio\blue.mp3"

# Thumbnail
Copy-Item "C:\Images\blue-cover.jpg" -Destination "songs\thumbnails\blue.jpg"

# Lyrics
Copy-Item "C:\Lyrics\blue-lyrics.json" -Destination "songs\lyrics\blue.json"
```

### 2. Edit song-library.json:
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
    "uploadDate": "2025-11-16T12:00:00.000Z",
    "tags": ["pop", "sad", "karaoke", "2024"]
  }
]
```

### 3. Start server:
```powershell
npm start
```

### 4. Test:
```
http://localhost:3000/api/songs
```

**Done!** Your song is now available! 🎉

---

## 🔧 Adding More Songs

### Method 1: Edit JSON Directly

Just add more objects to the array in `song-library.json`:

```json
[
  {
    "id": "song1",
    "title": "First Song",
    ...
  },
  {
    "id": "song2",
    "title": "Second Song",
    ...
  },
  {
    "id": "song3",
    "title": "Third Song",
    ...
  }
]
```

**Remember:** Add a comma between songs!

### Method 2: Batch Import Script

I can create a script to help you add multiple songs at once if you need it.

---

## 📊 File Naming Tips

### Good Naming:
```
audio/
  ├── blue-yungkai.mp3
  ├── never-gonna-give-you-up-rickastley.mp3
  └── shape-of-you-edsheeran.mp3

thumbnails/
  ├── blue-yungkai.jpg
  ├── never-gonna-give-you-up-rickastley.jpg
  └── shape-of-you-edsheeran.jpg
```

### Avoid:
- Special characters: `#`, `@`, `%`, `&`
- Spaces (use hyphens or underscores)
- Very long names

---

## 🎨 Thumbnail Guidelines

**Recommended:**
- Size: 1280x720 or 1920x1080
- Format: JPG or PNG
- File size: Under 1MB
- Quality: High quality, clear text

**Where to get:**
- YouTube video thumbnail
- Album cover art
- Custom designs

---

## 🔍 Search Tips

Users can search by:
- Song title
- Artist name
- Tags

**Example searches:**
- `"blue"` → Finds "Blue" by yung kai
- `"yung kai"` → Finds all songs by yung kai
- `"pop"` → Finds all songs tagged with "pop"

---

## ✅ Validation Checklist

Before starting server, check:

- [ ] All MP3 files are in `songs/audio/`
- [ ] All thumbnails are in `songs/thumbnails/`
- [ ] All lyrics are in `songs/lyrics/`
- [ ] `song-library.json` exists in Backend folder
- [ ] JSON is valid (no syntax errors)
- [ ] File paths in JSON match actual files
- [ ] Each song has a unique ID

**Validate JSON:** https://jsonlint.com/

---

## 🆘 Troubleshooting

### "Song not found"
**Check:**
1. File exists in correct folder
2. Path in JSON matches actual filename
3. No typos in JSON

### "JSON parse error"
**Check:**
1. Valid JSON syntax
2. Commas between objects
3. All quotes are proper `"`
4. Use JSON validator

### "Audio won't play"
**Check:**
1. File is valid MP3/WAV
2. Not corrupted
3. File size reasonable (under 50MB)

---

## 📦 Backup Your Library

```powershell
# Backup everything
Copy-Item -Path "songs" -Destination "songs-backup-$(Get-Date -Format 'yyyy-MM-dd')" -Recurse
Copy-Item "song-library.json" "song-library-backup.json"
```

---

## 🚀 Starting Fresh

If you want to start with a clean library:

```powershell
# Create empty library
echo "[]" > song-library.json

# Or with one example song
echo '[{"id":"example","title":"Example Song","author":"Artist","audioFile":"audio/example.mp3"}]' > song-library.json
```

---

## 💡 Pro Tips

### 1. Consistent Naming
Use the same pattern for all files:
- `{title}-{artist}.mp3`
- `{title}-{artist}.jpg`
- `{title}-{artist}.json`

### 2. Organize by Genre
```
audio/
  ├── pop/
  ├── rock/
  └── karaoke/
```
(Update paths in JSON accordingly)

### 3. Batch Processing
If you have many songs, I can help create a script to auto-generate the JSON from your file list.

### 4. Version Control
Keep backups of `song-library.json` as you add songs.

---

## 📝 Example song-library.json

Complete example with 3 songs:

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
    "uploadDate": "2025-11-16T12:00:00.000Z",
    "tags": ["pop", "sad", "karaoke"]
  },
  {
    "id": "never-gonna-give-you-up",
    "title": "Never Gonna Give You Up",
    "author": "Rick Astley",
    "audioFile": "audio/rickroll.mp3",
    "thumbnailFile": "thumbnails/rickroll.jpg",
    "lyricsFile": "lyrics/rickroll.json",
    "duration": 213,
    "uploadDate": "2025-11-16T12:00:00.000Z",
    "tags": ["80s", "pop", "meme", "karaoke"]
  },
  {
    "id": "shape-of-you",
    "title": "Shape of You",
    "author": "Ed Sheeran",
    "audioFile": "audio/shape-of-you.mp3",
    "thumbnailFile": "thumbnails/shape-of-you.jpg",
    "lyricsFile": "lyrics/shape-of-you.json",
    "duration": 234,
    "uploadDate": "2025-11-16T12:00:00.000Z",
    "tags": ["pop", "dance", "karaoke", "2017"]
  }
]
```

---

## 🎯 Quick Start Commands

```powershell
# 1. Create folder structure (if not exists)
cd "My project\Backend"
mkdir songs\audio, songs\thumbnails, songs\lyrics -Force

# 2. Create empty library
echo "[]" > song-library.json

# 3. Copy your files
# (manually or use Copy-Item commands)

# 4. Edit song-library.json
notepad song-library.json

# 5. Start server
npm start

# 6. Test
start http://localhost:3000/api/songs
```

---

## ✅ You're Ready!

Your setup is **admin-curated, user-friendly**:
- ✅ You manually add songs
- ✅ Users search and play
- ✅ No upload functionality for users
- ✅ Full control over content

**Next:** Add your first song using the steps above! 🎵

