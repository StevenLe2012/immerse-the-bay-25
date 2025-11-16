# Unity Karaoke App for Android 🎤🎵

A fully-featured karaoke application built with Unity for Android that integrates with YouTube to search for songs, extract audio, and display synchronized lyrics.

## ✨ Features

- 🔍 **YouTube Search Integration** - Search for karaoke songs directly from YouTube
- 🎵 **Audio Extraction** - Automatically extract audio from YouTube videos
- 📝 **Synchronized Lyrics** - Display lyrics in sync with the music
- 🎮 **Playback Controls** - Play, pause, stop, seek, and volume control
- 📱 **Android Optimized** - Built specifically for Android devices
- 🎨 **Modern UI** - Clean and intuitive user interface
- 🌐 **Multiple Subtitle Formats** - Supports SRT, VTT, and JSON subtitle formats

## 🚀 Quick Start

### Prerequisites
- Unity 2021.3 LTS or newer (with Android Build Support)
- Node.js v14 or higher
- Google Cloud account (for YouTube API)
- Android device or emulator

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/immerse-the-bay-25.git
cd immerse-the-bay-25
```

2. **Set up YouTube API**
   - Follow the guide in [QUICK_START.md](My%20project/QUICK_START.md)
   - Get your API key from Google Cloud Console

3. **Install and run backend server**
```bash
cd "My project/Backend"
npm install
npm run dev
```

4. **Open Unity project**
   - Open Unity Hub
   - Add project from `My project` folder
   - Open the project

5. **Configure the app**
   - Add your YouTube API key to `KaraokeManager` component
   - Ensure all script references are assigned

6. **Build and run**
   - File → Build Settings → Android
   - Build and Run

## 📚 Documentation

- **[Quick Start Guide](My%20project/QUICK_START.md)** - Get up and running in 15 minutes
- **[Complete Setup Guide](My%20project/KARAOKE_SETUP_GUIDE.md)** - Detailed setup instructions
- **[Backend Documentation](My%20project/Backend/README.md)** - Backend server setup and API reference

## 🏗️ Architecture

### Unity Components

- **KaraokeManager.cs** - Main controller for search and playback
- **LyricsDisplay.cs** - Handles synchronized lyrics display
- **SearchUI.cs** - Manages search interface and results
- **PlaybackController.cs** - Controls audio playback
- **AndroidPermissions.cs** - Handles Android runtime permissions

### Backend Server

- **Node.js/Express** server for YouTube integration
- **ytdl-core** for audio extraction
- **youtube-captions-scraper** for subtitle fetching
- RESTful API endpoints for Unity client

## 🔧 Configuration

### Unity Configuration

In `KaraokeManager.cs`:
```csharp
[SerializeField] private string youtubeAPIKey = "YOUR_API_KEY_HERE";
```

In `ExtractAudioAndSubtitles()` method:
```csharp
string backendUrl = "YOUR_BACKEND_URL/extract";
```

### Backend Configuration

Create `.env` file in Backend directory:
```env
PORT=3000
ALLOWED_ORIGINS=*
```

## 🌐 Deployment

### Backend Deployment Options

1. **Railway** (Recommended)
   - Free tier available
   - Automatic deployments from GitHub
   - [Deploy Guide](My%20project/Backend/README.md#deployment)

2. **Render**
   - Free tier available
   - Easy setup

3. **Heroku**
   - Simple deployment with CLI

4. **Google Cloud Run**
   - Serverless option
   - Pay per use

See [Backend README](My%20project/Backend/README.md) for detailed deployment instructions.

## 📱 Android Build

### Requirements
- Minimum API Level: Android 7.0 (API 24)
- Target API Level: Android 13+ (API 33+)
- Permissions: Internet, Network State, External Storage

### Build Steps
1. File → Build Settings
2. Select Android platform
3. Configure Player Settings:
   - Package name: `com.yourcompany.karaokeapp`
   - Minimum API: 24
   - Internet Access: Require
4. Build APK

## 🎯 Usage

1. **Search for a song**
   - Enter song name + "karaoke" in search box
   - Browse results with thumbnails

2. **Select a song**
   - Tap on a result to load audio and lyrics

3. **Play karaoke**
   - Use playback controls
   - Follow synchronized lyrics
   - Adjust volume as needed

## 🔐 API Keys and Security

⚠️ **Important Security Notes:**

- Never commit API keys to public repositories
- Use environment variables for production
- Implement rate limiting on backend
- Use HTTPS in production
- Restrict API keys in Google Cloud Console

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| No search results | Verify API key and internet connection |
| Audio not playing | Check backend server is running |
| No subtitles | Not all videos have captions |
| Build errors | Ensure Android SDK is properly installed |

See [Troubleshooting Guide](My%20project/KARAOKE_SETUP_GUIDE.md#troubleshooting) for more details.

## 🚧 Roadmap

- [ ] User authentication and profiles
- [ ] Offline mode with cached songs
- [ ] Recording and playback of user performances
- [ ] Social sharing features
- [ ] Pitch detection and scoring
- [ ] Multiple language support
- [ ] Playlist management
- [ ] Voice effects (reverb, echo)

## 📄 License

This project is provided as-is for educational purposes.

## ⚖️ Legal

This application is for educational purposes only. Ensure compliance with:
- YouTube Terms of Service
- Copyright laws
- Content licensing requirements

Do not use for commercial purposes without proper licensing.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📧 Support

For issues and questions:
- Check the [Setup Guide](My%20project/KARAOKE_SETUP_GUIDE.md)
- Review [Backend Documentation](My%20project/Backend/README.md)
- Open an issue on GitHub

## 🙏 Acknowledgments

- YouTube Data API v3
- ytdl-core library
- Unity Technologies
- TextMesh Pro

---

**Built with ❤️ for Immerse the Bay 2025 - Vibers Team**
