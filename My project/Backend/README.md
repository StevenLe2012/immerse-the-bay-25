# Karaoke Backend Server

This is the backend server for the Unity Karaoke App. It handles YouTube audio extraction and subtitle fetching since these operations cannot be done directly from Unity/Android.

## Prerequisites

- Node.js (v14 or higher)
- npm or yarn

## Installation

1. Navigate to the Backend directory:
```bash
cd Backend
```

2. Install dependencies:
```bash
npm install
```

## Running the Server

### Development Mode
```bash
npm run dev
```

### Production Mode
```bash
npm start
```

The server will run on `http://localhost:3000` by default.

## API Endpoints

### 1. Extract Audio and Subtitles
**POST** `/extract`

Request body:
```json
{
  "videoId": "dQw4w9WgXcQ"
}
```

Response:
```json
{
  "audioUrl": "https://...",
  "subtitlesData": "{\"subtitles\":[...]}",
  "title": "Video Title",
  "duration": "213"
}
```

### 2. Get Video Info
**GET** `/video-info/:videoId`

Response:
```json
{
  "title": "Video Title",
  "duration": "213",
  "thumbnail": "https://...",
  "author": "Channel Name"
}
```

### 3. Stream Audio
**GET** `/stream/:videoId`

Returns audio stream directly.

### 4. Get Available Subtitle Languages
**GET** `/subtitles/:videoId/languages`

Response:
```json
{
  "languages": [
    {
      "languageCode": "en",
      "languageName": "English"
    }
  ]
}
```

### 5. Get Subtitles in Specific Language
**GET** `/subtitles/:videoId/:lang`

Response:
```json
{
  "subtitles": [
    {
      "startTime": 0.0,
      "endTime": 2.5,
      "text": "Lyrics line 1"
    }
  ]
}
```

### 6. Health Check
**GET** `/health`

Response:
```json
{
  "status": "OK",
  "message": "Karaoke API is running"
}
```

## Deployment

### Option 1: Deploy to Heroku

1. Create a Heroku account and install Heroku CLI
2. Login to Heroku:
```bash
heroku login
```

3. Create a new Heroku app:
```bash
heroku create your-karaoke-api
```

4. Deploy:
```bash
git push heroku main
```

### Option 2: Deploy to Railway

1. Go to [Railway.app](https://railway.app)
2. Click "New Project" → "Deploy from GitHub repo"
3. Select your repository
4. Railway will automatically detect and deploy your Node.js app

### Option 3: Deploy to Render

1. Go to [Render.com](https://render.com)
2. Click "New" → "Web Service"
3. Connect your GitHub repository
4. Render will automatically deploy your app

### Option 4: Deploy to Google Cloud Run

1. Install Google Cloud SDK
2. Build and deploy:
```bash
gcloud run deploy karaoke-api --source .
```

## Environment Variables

You can set these environment variables:

- `PORT` - Server port (default: 3000)

## Important Notes

1. **YouTube Terms of Service**: Make sure your use case complies with YouTube's Terms of Service. This server is for educational purposes.

2. **Rate Limiting**: Consider implementing rate limiting to prevent abuse.

3. **Caching**: For production, implement caching to reduce YouTube API calls.

4. **HTTPS**: Use HTTPS in production for secure communication with your Unity app.

5. **CORS**: The server allows all origins by default. In production, restrict CORS to your app's domain.

## Troubleshooting

### Error: "Video unavailable"
- The video might be region-restricted or private
- Try with a different video ID

### Error: "No subtitles found"
- Not all videos have subtitles/captions
- Try requesting a different language code

### High memory usage
- Consider implementing streaming instead of loading entire audio files
- Use the `/stream/:videoId` endpoint for direct streaming

## Security Considerations

1. Add API key authentication
2. Implement rate limiting
3. Validate and sanitize all inputs
4. Use HTTPS in production
5. Restrict CORS to specific domains
6. Monitor for abuse

## License

MIT

