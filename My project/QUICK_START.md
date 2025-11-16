# Karaoke App - Quick Start Guide

Get your Unity Karaoke App running in 15 minutes!

## 🚀 Quick Setup (Development)

### 1. Get YouTube API Key (5 minutes)

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project
3. Enable "YouTube Data API v3"
4. Create credentials → API Key
5. Copy the API key

### 2. Setup Backend Server (3 minutes)

```bash
# Navigate to backend directory
cd "My project/Backend"

# Install dependencies
npm install

# Start the server
npm run dev
```

Server will run at `http://localhost:3000`

### 3. Configure Unity (5 minutes)

1. Open the Unity project
2. Open the main scene
3. Select the `KaraokeManager` GameObject
4. In the Inspector:
   - Paste your YouTube API key in `Youtube API Key` field
   - Ensure all references are assigned

### 4. Test in Unity Editor (2 minutes)

1. Press Play
2. Search for "Bohemian Rhapsody karaoke"
3. Select a result
4. Click Play when audio loads

## 📱 Build for Android

### Quick Build

1. File → Build Settings
2. Select Android → Switch Platform
3. Player Settings:
   - Set Package Name: `com.yourcompany.karaokeapp`
   - Set Minimum API Level: Android 7.0 (API 24)
   - Set Internet Access: Require
4. Build And Run

## 🎯 What You Get

- ✅ YouTube search for karaoke songs
- ✅ Audio extraction and playback
- ✅ Synchronized lyrics display
- ✅ Playback controls (play, pause, stop)
- ✅ Progress slider and volume control
- ✅ Android support

## 📁 Project Structure

```
My project/
├── Assets/
│   └── Scripts/
│       ├── KaraokeManager.cs       # Main manager
│       ├── LyricsDisplay.cs        # Lyrics synchronization
│       ├── SearchUI.cs             # Search interface
│       ├── PlaybackController.cs   # Playback controls
│       └── AndroidPermissions.cs   # Android permissions
│
├── Backend/
│   ├── server.js                   # Node.js backend
│   ├── package.json                # Dependencies
│   └── README.md                   # Backend docs
│
├── KARAOKE_SETUP_GUIDE.md         # Detailed setup guide
└── QUICK_START.md                  # This file
```

## 🔧 Key Configuration Points

### KaraokeManager.cs
```csharp
// Line 10: Add your YouTube API key
[SerializeField] private string youtubeAPIKey = "YOUR_YOUTUBE_API_KEY_HERE";

// Line 62: Update with your deployed backend URL
string backendUrl = "YOUR_BACKEND_SERVER_URL/extract";
```

### Backend server.js
- Runs on port 3000 by default
- Handles audio extraction and subtitles
- Must be running for the app to work

## 🐛 Quick Troubleshooting

| Issue | Solution |
|-------|----------|
| No search results | Check API key and internet connection |
| No audio playing | Ensure backend server is running |
| No subtitles | Not all videos have captions, try another |
| App crashes | Check Android Logcat for errors |

## 📚 Next Steps

1. Read the full [KARAOKE_SETUP_GUIDE.md](KARAOKE_SETUP_GUIDE.md) for detailed instructions
2. Deploy your backend to Railway/Render for production use
3. Customize the UI to match your design
4. Add features like favorites, playlists, recording

## 🆘 Need Help?

- Check the detailed setup guide: `KARAOKE_SETUP_GUIDE.md`
- Backend documentation: `Backend/README.md`
- YouTube API docs: https://developers.google.com/youtube/v3

## ⚠️ Important Notes

- **Development**: Backend runs on localhost (http://localhost:3000)
- **Production**: Deploy backend to a cloud service (Railway, Render, Heroku)
- **API Limits**: YouTube API has a free quota of 10,000 units/day
- **Legal**: Ensure compliance with YouTube Terms of Service

## 🎉 You're Ready!

Your karaoke app is now set up and ready to use. Search for songs, sing along with synchronized lyrics, and have fun!

For production deployment and advanced features, refer to the complete setup guide.

