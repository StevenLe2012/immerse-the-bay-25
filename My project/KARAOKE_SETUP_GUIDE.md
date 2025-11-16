# Unity Karaoke App - Complete Setup Guide

This guide will walk you through setting up your Unity Karaoke App for Android with YouTube integration.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [YouTube API Setup](#youtube-api-setup)
3. [Backend Server Setup](#backend-server-setup)
4. [Unity Project Setup](#unity-project-setup)
5. [Android Build Configuration](#android-build-configuration)
6. [Testing](#testing)
7. [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Required Software
- **Unity 2021.3 LTS or newer** (with Android Build Support)
- **Node.js** (v14 or higher) for backend server
- **Android SDK** (installed via Unity Hub)
- **Google Account** (for YouTube API access)

### Required Unity Packages
- TextMesh Pro (should be installed by default)
- Android Build Support module

---

## YouTube API Setup

### Step 1: Create a Google Cloud Project

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Click "Select a project" → "New Project"
3. Enter project name: "Karaoke App" (or your preferred name)
4. Click "Create"

### Step 2: Enable YouTube Data API v3

1. In the Google Cloud Console, go to "APIs & Services" → "Library"
2. Search for "YouTube Data API v3"
3. Click on it and press "Enable"

### Step 3: Create API Credentials

1. Go to "APIs & Services" → "Credentials"
2. Click "Create Credentials" → "API Key"
3. Copy the generated API key
4. (Optional but recommended) Click "Restrict Key":
   - Under "API restrictions", select "Restrict key"
   - Choose "YouTube Data API v3"
   - Under "Application restrictions", you can restrict by IP or HTTP referrer
5. Click "Save"

### Step 4: Configure API Key in Unity

1. Open your Unity project
2. Find the `KaraokeManager.cs` script in the Inspector
3. Paste your API key in the `Youtube API Key` field

**⚠️ IMPORTANT SECURITY NOTE:**
- Never commit your API key to public repositories
- For production, use environment variables or secure key management
- Consider implementing a backend proxy to hide your API key

---

## Backend Server Setup

The backend server is required because YouTube doesn't allow direct audio extraction from client applications. The server handles:
- Audio extraction from YouTube videos
- Subtitle/caption fetching
- Streaming audio to your Unity app

### Step 1: Install Node.js

Download and install Node.js from [nodejs.org](https://nodejs.org/)

Verify installation:
```bash
node --version
npm --version
```

### Step 2: Install Backend Dependencies

Navigate to the Backend directory:
```bash
cd "My project/Backend"
npm install
```

This will install:
- `express` - Web server framework
- `cors` - Cross-origin resource sharing
- `@distube/ytdl-core` - YouTube downloader
- `youtube-captions-scraper` - Subtitle fetcher

### Step 3: Run the Server Locally

For development:
```bash
npm run dev
```

For production:
```bash
npm start
```

The server will run on `http://localhost:3000`

Test it by visiting: `http://localhost:3000/health`

### Step 4: Deploy the Backend (Production)

For production use, you need to deploy the backend to a cloud service. See options below:

#### Option A: Deploy to Railway (Recommended - Free Tier Available)

1. Create account at [Railway.app](https://railway.app)
2. Click "New Project" → "Deploy from GitHub repo"
3. Select your repository
4. Railway will auto-detect and deploy your Node.js app
5. Copy the generated URL (e.g., `https://your-app.railway.app`)

#### Option B: Deploy to Render (Free Tier Available)

1. Create account at [Render.com](https://render.com)
2. Click "New" → "Web Service"
3. Connect your GitHub repository
4. Render will auto-deploy
5. Copy the generated URL

#### Option C: Deploy to Heroku

1. Install Heroku CLI
2. Login: `heroku login`
3. Create app: `heroku create your-karaoke-api`
4. Deploy: `git push heroku main`
5. Copy the app URL

### Step 5: Update Unity with Backend URL

1. Open `KaraokeManager.cs` in Unity
2. Find the line: `string backendUrl = "YOUR_BACKEND_SERVER_URL/extract";`
3. Replace with your deployed URL: `string backendUrl = "https://your-app.railway.app/extract";`

---

## Unity Project Setup

### Step 1: Import TextMesh Pro

1. In Unity, go to Window → TextMesh Pro → Import TMP Essential Resources
2. Click "Import"

### Step 2: Create the Scene Structure

Create a new scene or modify your existing scene with the following structure:

```
Canvas (UI)
├── SearchPanel
│   ├── SearchInputField (TMP_InputField)
│   ├── SearchButton (Button)
│   ├── ResultsScrollView (ScrollRect)
│   │   └── ResultsContainer (Vertical Layout Group)
│   └── LoadingIndicator (Image/GameObject)
│
├── PlaybackPanel
│   ├── LyricsDisplay
│   │   ├── PreviousLyricText (TextMeshProUGUI)
│   │   ├── CurrentLyricText (TextMeshProUGUI)
│   │   └── NextLyricText (TextMeshProUGUI)
│   │
│   └── Controls
│       ├── PlayButton (Button)
│       ├── PauseButton (Button)
│       ├── StopButton (Button)
│       ├── ProgressSlider (Slider)
│       ├── VolumeSlider (Slider)
│       ├── CurrentTimeText (TextMeshProUGUI)
│       └── TotalTimeText (TextMeshProUGUI)
│
└── KaraokeManager (Empty GameObject)
```

### Step 3: Create Result Item Prefab

1. Create a new UI Panel: Right-click in Hierarchy → UI → Panel
2. Name it "ResultItemPrefab"
3. Add the following children:
   - ThumbnailImage (RawImage)
   - TitleText (TextMeshProUGUI)
   - ChannelText (TextMeshProUGUI)
   - SelectButton (Button)
4. Add the `ResultItemUI` component to the prefab
5. Assign the references in the Inspector
6. Drag the prefab to your Assets/Prefabs folder
7. Delete the instance from the Hierarchy

### Step 4: Assign Script References

1. Select the KaraokeManager GameObject
2. Add the `KaraokeManager` component
3. Add the `LyricsDisplay` component
4. Add the `PlaybackController` component
5. Assign all references in the Inspector:
   - Audio Source
   - Lyrics Display texts
   - Search UI elements
   - Playback controls

### Step 5: Configure the Scripts

In the Inspector for KaraokeManager:
- **Youtube API Key**: Paste your YouTube API key
- **Audio Source**: Assign the AudioSource component
- **Lyrics Display**: Assign the LyricsDisplay component
- **Search UI**: Assign the SearchUI component

---

## Android Build Configuration

### Step 1: Switch to Android Platform

1. Go to File → Build Settings
2. Select "Android"
3. Click "Switch Platform"

### Step 2: Configure Player Settings

1. In Build Settings, click "Player Settings"
2. Configure the following:

**Company Name & Product Name:**
- Company Name: Your company name
- Product Name: Karaoke App

**Package Name:**
- com.yourcompany.karaokeapp

**Minimum API Level:**
- Android 7.0 'Nougat' (API level 24) or higher

**Target API Level:**
- Latest available (Android 13 or higher recommended)

**Internet Access:**
- Set to "Require"

### Step 3: Add Android Permissions

1. In Player Settings → Android → Other Settings
2. Ensure the following permissions are enabled:
   - Internet
   - Network State
   - Write External Storage (if saving audio)

### Step 4: Configure Android Manifest (if needed)

Create or modify `Assets/Plugins/Android/AndroidManifest.xml`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
    <uses-permission android:name="android.permission.INTERNET" />
    <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
    <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" 
                     android:maxSdkVersion="28" />
    
    <application android:usesCleartextTraffic="true">
        <!-- This allows HTTP traffic for development -->
        <!-- Remove or set to false for production with HTTPS -->
    </application>
</manifest>
```

### Step 5: Build the APK

1. Go to File → Build Settings
2. Click "Add Open Scenes" to add your scene
3. Click "Build" or "Build And Run"
4. Choose a location to save the APK
5. Wait for the build to complete

---

## Testing

### Test Locally (Unity Editor)

1. Make sure your backend server is running (`npm run dev`)
2. Press Play in Unity Editor
3. Search for a karaoke song (e.g., "Bohemian Rhapsody karaoke")
4. Select a result
5. Wait for audio and lyrics to load
6. Click Play

### Test on Android Device

1. Enable Developer Options on your Android device:
   - Go to Settings → About Phone
   - Tap "Build Number" 7 times
2. Enable USB Debugging:
   - Go to Settings → Developer Options
   - Enable "USB Debugging"
3. Connect your device via USB
4. In Unity, go to File → Build Settings
5. Select your device from the "Run Device" dropdown
6. Click "Build And Run"

### Test Backend Server

Test individual endpoints using a browser or Postman:

```bash
# Health check
http://localhost:3000/health

# Get video info
http://localhost:3000/video-info/dQw4w9WgXcQ

# Get available subtitle languages
http://localhost:3000/subtitles/dQw4w9WgXcQ/languages
```

---

## Troubleshooting

### Common Issues

#### 1. "YouTube API Error: 403 Forbidden"
**Solution:**
- Check if YouTube Data API v3 is enabled in Google Cloud Console
- Verify your API key is correct
- Check if you've exceeded your daily quota (10,000 units/day free)

#### 2. "Backend Connection Failed"
**Solution:**
- Verify backend server is running
- Check the backend URL in `KaraokeManager.cs`
- Ensure CORS is enabled on the backend
- Check Android manifest allows cleartext traffic (for HTTP)

#### 3. "No Audio Playing"
**Solution:**
- Check if AudioSource component is assigned
- Verify audio URL is valid
- Check device volume
- Look for errors in Unity Console

#### 4. "No Subtitles Found"
**Solution:**
- Not all videos have subtitles
- Try a different video
- Check if the video has captions on YouTube
- Try requesting different language codes (en, es, fr, etc.)

#### 5. "App Crashes on Android"
**Solution:**
- Check Android Logcat for errors
- Ensure minimum API level is set correctly
- Verify all permissions are granted
- Check for null reference exceptions

#### 6. "Video Unavailable"
**Solution:**
- Video might be region-restricted
- Video might be private or deleted
- Try with a different video ID

### Debug Mode

Enable debug logging in Unity:

```csharp
// In KaraokeManager.cs, add at the top of methods:
Debug.Log($"Searching for: {query}");
Debug.Log($"API Response: {jsonResponse}");
```

View Android logs:
```bash
adb logcat -s Unity
```

---

## Performance Optimization

### For Better Performance:

1. **Cache Thumbnails**: Store downloaded thumbnails to avoid re-downloading
2. **Preload Audio**: Start loading audio while user is browsing results
3. **Compress Audio**: Use lower bitrate audio for mobile data
4. **Lazy Load Results**: Load search results in batches
5. **Background Loading**: Use async/await for all network operations

### Memory Management:

```csharp
// Unload audio when done
if (audioSource.clip != null)
{
    Destroy(audioSource.clip);
    audioSource.clip = null;
}

// Clear unused textures
Resources.UnloadUnusedAssets();
```

---

## Next Steps

### Enhancements to Consider:

1. **User Authentication**: Save favorite songs per user
2. **Offline Mode**: Cache downloaded songs
3. **Recording**: Allow users to record their karaoke sessions
4. **Social Features**: Share scores and recordings
5. **Multiple Languages**: Support for international songs
6. **Pitch Detection**: Score users based on pitch accuracy
7. **Playlists**: Create and manage song playlists
8. **Voice Effects**: Add reverb, echo, etc.

---

## Support & Resources

- **YouTube Data API Documentation**: https://developers.google.com/youtube/v3
- **Unity Android Documentation**: https://docs.unity3d.com/Manual/android.html
- **Node.js Documentation**: https://nodejs.org/docs/

---

## Legal & Compliance

⚠️ **IMPORTANT**: 
- Ensure your app complies with YouTube's Terms of Service
- This app is for educational purposes
- Do not use for commercial purposes without proper licensing
- Respect copyright and intellectual property rights
- Consider implementing content ID checking

---

## License

This project is provided as-is for educational purposes. Use at your own risk.

