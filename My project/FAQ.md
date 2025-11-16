# Frequently Asked Questions (FAQ)

Common questions and answers about the Unity Karaoke App.

---

## 🎯 General Questions

### Q: What is this app?
**A:** A Unity-based karaoke application for Android that integrates with YouTube to search for songs, extract audio, and display synchronized lyrics.

### Q: Is this app free to use?
**A:** Yes, the code is free to use for educational purposes. However, you'll need:
- YouTube API key (free tier: 10,000 units/day)
- Backend hosting (free tiers available on Railway/Render)

### Q: Can I use this for commercial purposes?
**A:** Not without proper licensing. You must comply with:
- YouTube Terms of Service
- Copyright laws for music content
- Proper licensing for commercial karaoke use

### Q: What platforms does it support?
**A:** Currently Android only (API 24+). iOS support would require additional work.

---

## 🔧 Setup & Installation

### Q: What do I need to get started?
**A:** You need:
- Unity 2021.3 LTS or newer (with Android Build Support)
- Node.js v14 or higher
- Google Cloud account (for YouTube API)
- Android device or emulator

### Q: How long does setup take?
**A:** 
- Quick setup: 15-30 minutes (using QUICK_START.md)
- Complete setup: 1-2 hours (with full customization)

### Q: Do I need coding experience?
**A:** Basic understanding helps, but the guides are beginner-friendly. You should know:
- How to navigate Unity Editor
- Basic command line usage
- How to follow step-by-step instructions

### Q: Can I skip the backend server?
**A:** No, the backend is required because:
- YouTube doesn't allow direct audio extraction from clients
- It protects your API keys
- It handles complex processing

---

## 🔑 YouTube API

### Q: How do I get a YouTube API key?
**A:** 
1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project
3. Enable YouTube Data API v3
4. Create credentials → API Key
5. Copy the key

See [QUICK_START.md](QUICK_START.md) for detailed steps.

### Q: Is the YouTube API free?
**A:** Yes, with limits:
- Free quota: 10,000 units/day
- Search cost: 100 units per request
- This allows ~100 searches per day

### Q: What happens if I exceed the quota?
**A:** 
- API requests will fail with 403 error
- Quota resets at midnight Pacific Time
- You can request quota increase or upgrade to paid tier

### Q: How do I protect my API key?
**A:** 
- Don't commit it to public repositories
- Use environment variables in production
- Restrict key usage in Google Cloud Console
- Consider using a backend proxy

### Q: Can I use the same API key for multiple apps?
**A:** Yes, but they'll share the same quota. For production, use separate keys per app.

---

## 🖥️ Backend Server

### Q: Why do I need a backend server?
**A:** Because:
- YouTube doesn't allow direct video downloads from browsers/apps
- Audio extraction requires server-side processing
- It protects your API credentials
- It provides better control and security

### Q: Where should I host the backend?
**A:** Recommended options:
- **Railway** (easiest, free tier)
- **Render** (good free tier)
- **Google Cloud Run** (scalable, pay-per-use)
- **Heroku** (paid, but reliable)

### Q: Can I run the backend on my computer?
**A:** Yes, for development only:
```bash
cd Backend
npm install
npm run dev
```
But for production/testing on real devices, you need cloud hosting.

### Q: How much does backend hosting cost?
**A:** 
- **Free tier:** Railway (500 hrs/month), Render (750 hrs/month)
- **Paid tier:** $5-7/month for always-on service
- **Pay-per-use:** Google Cloud Run (often free for low traffic)

### Q: Can multiple users use the same backend?
**A:** Yes, the backend can handle multiple concurrent users. Consider:
- Adding rate limiting
- Implementing caching
- Monitoring usage
- Scaling if needed

---

## 📱 Android Development

### Q: What Android version is required?
**A:** Minimum: Android 7.0 (API 24) or higher

### Q: How do I build for Android?
**A:** 
1. File → Build Settings
2. Select Android → Switch Platform
3. Configure Player Settings
4. Click Build or Build And Run

See [KARAOKE_SETUP_GUIDE.md](KARAOKE_SETUP_GUIDE.md) for details.

### Q: Can I test without an Android device?
**A:** Yes, use:
- Unity Editor (limited testing)
- Android Emulator (full testing)
- Unity Remote app (on actual device)

### Q: Why isn't my app installing on Android?
**A:** Common causes:
- USB debugging not enabled
- Wrong API level
- Insufficient storage
- Security settings blocking installation

### Q: How do I enable USB debugging?
**A:** 
1. Settings → About Phone
2. Tap "Build Number" 7 times
3. Settings → Developer Options
4. Enable "USB Debugging"

---

## 🎵 Features & Functionality

### Q: How does song search work?
**A:** 
1. User enters search query
2. App calls YouTube Data API v3
3. API returns matching videos
4. Results displayed with thumbnails

### Q: Can I search for non-karaoke songs?
**A:** Yes, but the app automatically adds "karaoke" to searches for better results.

### Q: Why are there no lyrics for some songs?
**A:** Because:
- Not all videos have captions/subtitles
- Some captions are auto-generated and poor quality
- Some videos have captions disabled

### Q: Can I upload my own lyrics?
**A:** Not currently, but you could add this feature by:
- Creating a lyrics database
- Implementing manual lyric entry
- Syncing with timestamps

### Q: Does it support multiple languages?
**A:** Yes, if the YouTube video has subtitles in that language. You can request specific languages via the backend API.

### Q: Can I record my singing?
**A:** Not yet, but this is a planned feature. You would need to:
- Add microphone input
- Mix with backing track
- Save recording

---

## 🐛 Troubleshooting

### Q: "No search results found" - What's wrong?
**A:** Check:
- Internet connection
- YouTube API key is correct
- API quota not exceeded
- Search query is valid

### Q: "Audio not playing" - How to fix?
**A:** Check:
- Backend server is running
- Backend URL is correct in KaraokeManager.cs
- Device volume is up
- Audio permissions granted

### Q: "Backend connection failed" - What to do?
**A:** Verify:
- Backend server is deployed and running
- URL is correct (including https://)
- CORS is enabled on backend
- Firewall not blocking requests

### Q: App crashes on startup - Why?
**A:** Common causes:
- Missing script references
- Null reference exceptions
- Android permissions not granted
- Incompatible Android version

Check Android Logcat for specific errors:
```bash
adb logcat -s Unity
```

### Q: Lyrics not syncing properly - How to fix?
**A:** 
- Check subtitle format is supported
- Verify audio is playing
- Check GetCurrentTime() returns correct value
- Ensure subtitle timestamps are correct

### Q: High memory usage - How to optimize?
**A:** 
- Dispose audio clips when done
- Clear unused textures
- Implement object pooling
- Use lower quality audio

---

## 🔐 Security & Privacy

### Q: Is my data collected?
**A:** No, the app doesn't collect user data currently. If you add user features, implement a privacy policy.

### Q: Is it safe to use?
**A:** Yes, for educational/personal use. For production:
- Implement proper security
- Use HTTPS only
- Validate all inputs
- Add rate limiting

### Q: Can others see what I'm searching?
**A:** No, searches are private. However:
- Your backend server logs may contain queries
- YouTube may track API requests

### Q: How do I secure my API key?
**A:** 
- Use environment variables
- Restrict key in Google Cloud Console
- Use backend proxy
- Never commit to public repos

---

## 💰 Costs & Limits

### Q: How much does it cost to run?
**A:** 
- **Development:** Free (local backend)
- **Production (low traffic):** Free (using free tiers)
- **Production (high traffic):** $5-20/month

### Q: What are the API limits?
**A:** 
- YouTube API: 10,000 units/day (free)
- Backend: Depends on hosting plan
- Audio extraction: Limited by backend resources

### Q: Can I monetize this app?
**A:** Only with proper licensing:
- YouTube API Terms of Service compliance
- Music licensing for commercial karaoke
- Copyright clearance for songs

---

## 🚀 Advanced Features

### Q: Can I add user accounts?
**A:** Yes, you would need to:
- Implement authentication (Firebase, Auth0)
- Add database for user data
- Create user profile system

### Q: Can I add offline mode?
**A:** Yes, by:
- Caching downloaded songs
- Storing lyrics locally
- Implementing local database

### Q: Can I add pitch detection?
**A:** Yes, using:
- Unity audio analysis
- External pitch detection libraries
- Scoring algorithm

### Q: Can I add duet mode?
**A:** Yes, by:
- Splitting lyrics by singer
- Implementing multiplayer
- Adding separate audio tracks

### Q: Can I add voice effects?
**A:** Yes, using:
- Unity Audio Mixer
- Audio effects (reverb, echo, pitch shift)
- Real-time audio processing

---

## 📚 Learning & Support

### Q: Where can I learn more about Unity?
**A:** 
- [Unity Learn](https://learn.unity.com/)
- [Unity Documentation](https://docs.unity3d.com/)
- [Unity Forums](https://forum.unity.com/)

### Q: Where can I get help?
**A:** 
- Read the documentation in this project
- Check Unity forums
- Stack Overflow
- GitHub Issues

### Q: Can I contribute to this project?
**A:** Yes! Contributions are welcome:
- Fork the repository
- Make improvements
- Submit pull requests

### Q: Where can I report bugs?
**A:** 
- GitHub Issues
- Include error messages
- Describe steps to reproduce
- Provide system information

---

## 🎓 Educational Use

### Q: Can I use this for my school project?
**A:** Yes, this is perfect for:
- Computer science projects
- Mobile app development courses
- Unity learning
- API integration practice

### Q: Can I modify the code?
**A:** Yes, feel free to:
- Customize features
- Change UI design
- Add new functionality
- Learn from the code

### Q: Can I share this with others?
**A:** Yes, but:
- Credit the original source
- Don't remove license information
- Share knowledge, not API keys

---

## 🔄 Updates & Maintenance

### Q: Will this project be updated?
**A:** The project is provided as-is for educational purposes. You're encouraged to maintain and improve it yourself.

### Q: What if YouTube API changes?
**A:** 
- Monitor YouTube API changelog
- Update backend code as needed
- Test regularly
- Have fallback options

### Q: How do I update dependencies?
**A:** 
```bash
# Backend
cd Backend
npm update

# Unity
# Use Package Manager to update packages
```

---

## 📞 Still Have Questions?

If your question isn't answered here:

1. Check the documentation:
   - [QUICK_START.md](QUICK_START.md)
   - [KARAOKE_SETUP_GUIDE.md](KARAOKE_SETUP_GUIDE.md)
   - [Backend/README.md](Backend/README.md)

2. Review the code comments in:
   - KaraokeManager.cs
   - LyricsDisplay.cs
   - SearchUI.cs
   - PlaybackController.cs

3. Check online resources:
   - Unity Documentation
   - YouTube API Documentation
   - Stack Overflow

4. Open a GitHub issue with:
   - Clear description
   - Error messages
   - Steps to reproduce
   - System information

---

**Happy Karaoke! 🎤🎵**

