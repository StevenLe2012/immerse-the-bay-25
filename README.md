# Karaoke App 🎤🎵

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

## 🎥 Submission Requirements (Immerse the Bay 2025)

To submit this project for the hackathon:

### ✅ GitHub Repository Checklist
- [x] **MIT License** - Added to repository root
- [x] **Comprehensive README** - Includes all packages, assets, and AI tools used
- [x] **Latest Code** - Ensure all code is pushed to GitHub
- [x] **Documentation** - Setup guides and API documentation included

### 📹 Devpost Video Demo
Upload a **~30-second vertical video** demonstrating:
1. **App Launch** - Show the app starting on device
2. **Song Search** - Demonstrate searching for a song
3. **Karaoke Play** - Show synchronized lyrics with audio playback
4. **Key Features** - Highlight unique AR/XR features if applicable

**Video Specifications:**
- Duration: ~30 seconds
- Orientation: Vertical (9:16 aspect ratio)
- Format: MP4 recommended
- Quality: Clear audio and visuals

### 📋 Submission Steps
1. Push all code to this GitHub repository
2. Record vertical demo video on device
3. Upload video to Devpost project page
4. Verify README has MIT License reference
5. Double-check all packages and tools are listed

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

## 📦 Packages, Assets, and Tools Used

### Unity Packages
- **AR/XR Packages:**
  - `com.unity.xr.arcore` (6.2.0) - ARCore support for Android
  - `com.unity.xr.arfoundation` (6.2.1) - AR Foundation framework
  - `com.unity.xr.arkit` (6.2.0) - ARKit support for iOS
  - `com.unity.xr.hands` (1.7.1) - Hand tracking
  - `com.unity.xr.interaction.toolkit` (3.2.2) - XR interaction system
  - `com.unity.xr.management` (4.5.3) - XR plugin management
  - `com.xreal.xr` - XREAL AR glasses integration

- **Rendering & UI:**
  - `com.unity.render-pipelines.universal` (17.0.4) - Universal Render Pipeline
  - TextMesh Pro - Advanced text rendering
  - `com.unity.2d.sprite` (1.0.0) - 2D sprite system

- **Development Tools:**
  - `com.unity.inputsystem` (1.14.2) - New Input System
  - `com.unity.mobile.android-logcat` (1.4.6) - Android debugging
  - `com.unity.timeline` (1.8.9) - Timeline for sequencing
  - `com.unity.learn.iet-framework` (4.0.4) - Learning framework

### Backend Packages (Node.js)
- **Core Dependencies:**
  - `express` (^4.18.2) - Web server framework
  - `cors` (^2.8.5) - Cross-origin resource sharing
  - `multer` (^2.0.2) - Multipart form data handling
  - `dotenv` (^17.2.3) - Environment variable management

- **Development Dependencies:**
  - `nodemon` (^3.0.1) - Auto-restart development server

### Assets & Resources

- **Fonts:**
  - LTKaraoke Font Family (Bold, Light, Medium, Regular, SemiBold)
  - VendSans Variable Font

- **3D Models:**
  - Drum Kit Models (cymbal, drums, snare) - Created with Blender
  - Drum by Poly by Google [[CC-BY](https://creativecommons.org/licenses/by/3.0/)] via [Poly Pizza](https://poly.pizza/m/c3RNKJflc4O)

- **Audio Files:**
  - Multiple karaoke tracks in MP3 format (user-provided song library)
  - Songs include: "Blue", "Die With A Smile", "Golden", "Heather", "Luther", "Sugar", "You Belong With Me", and more

- **Lyrics Files:**
  - WebVTT (.vtt) format subtitle files with synchronized timestamps
  - Custom JSON format for lyrics data

- **Images:**
  - Song thumbnails and cover art (JPG/PNG format)
  - UI assets and icons


### External APIs
- **YouTube Data API v3** - For searching karaoke videos (optional feature)
- **Google Cloud Services** - API key management

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

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
