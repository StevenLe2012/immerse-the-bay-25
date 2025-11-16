/**
 * Backend Server for Karaoke App
 * This Node.js server handles YouTube audio extraction and subtitle fetching
 * 
 * IMPORTANT: You need to install the following packages:
 * npm install express cors ytdl-core @distube/ytdl-core youtube-captions-scraper
 */

const express = require('express');
const cors = require('cors');
const ytdl = require('@distube/ytdl-core');
const { getSubtitles } = require('youtube-captions-scraper');

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors());
app.use(express.json());

/**
 * Extract audio and subtitles from YouTube video
 */
app.post('/extract', async (req, res) => {
    try {
        const { videoId } = req.body;
        
        if (!videoId) {
            return res.status(400).json({ error: 'Video ID is required' });
        }
        
        const videoUrl = `https://www.youtube.com/watch?v=${videoId}`;
        
        // Get video info
        const info = await ytdl.getInfo(videoUrl);
        
        // Get audio format (best audio quality)
        const audioFormat = ytdl.chooseFormat(info.formats, { 
            quality: 'highestaudio',
            filter: 'audioonly'
        });
        
        // Get subtitles/captions
        let subtitlesData = null;
        try {
            const subtitles = await getSubtitles({
                videoID: videoId,
                lang: 'en' // You can make this configurable
            });
            
            // Convert to our format
            if (subtitles && subtitles.length > 0) {
                const formattedSubtitles = subtitles.map(sub => ({
                    startTime: parseFloat(sub.start),
                    endTime: parseFloat(sub.start) + parseFloat(sub.dur),
                    text: sub.text
                }));
                
                subtitlesData = JSON.stringify({ subtitles: formattedSubtitles });
            }
        } catch (subError) {
            console.error('Error fetching subtitles:', subError);
            // Continue without subtitles
        }
        
        res.json({
            audioUrl: audioFormat.url,
            subtitlesData: subtitlesData,
            title: info.videoDetails.title,
            duration: info.videoDetails.lengthSeconds
        });
        
    } catch (error) {
        console.error('Error extracting video data:', error);
        res.status(500).json({ 
            error: 'Failed to extract video data',
            message: error.message 
        });
    }
});

/**
 * Get video information
 */
app.get('/video-info/:videoId', async (req, res) => {
    try {
        const { videoId } = req.params;
        const videoUrl = `https://www.youtube.com/watch?v=${videoId}`;
        
        const info = await ytdl.getInfo(videoUrl);
        
        res.json({
            title: info.videoDetails.title,
            duration: info.videoDetails.lengthSeconds,
            thumbnail: info.videoDetails.thumbnails[0].url,
            author: info.videoDetails.author.name
        });
        
    } catch (error) {
        console.error('Error getting video info:', error);
        res.status(500).json({ 
            error: 'Failed to get video info',
            message: error.message 
        });
    }
});

/**
 * Stream audio directly
 */
app.get('/stream/:videoId', async (req, res) => {
    try {
        const { videoId } = req.params;
        const videoUrl = `https://www.youtube.com/watch?v=${videoId}`;
        
        res.header('Content-Type', 'audio/mpeg');
        
        ytdl(videoUrl, {
            filter: 'audioonly',
            quality: 'highestaudio'
        }).pipe(res);
        
    } catch (error) {
        console.error('Error streaming audio:', error);
        res.status(500).json({ 
            error: 'Failed to stream audio',
            message: error.message 
        });
    }
});

/**
 * Get available subtitle languages
 */
app.get('/subtitles/:videoId/languages', async (req, res) => {
    try {
        const { videoId } = req.params;
        const videoUrl = `https://www.youtube.com/watch?v=${videoId}`;
        
        const info = await ytdl.getInfo(videoUrl);
        const captionTracks = info.player_response?.captions?.playerCaptionsTracklistRenderer?.captionTracks || [];
        
        const languages = captionTracks.map(track => ({
            languageCode: track.languageCode,
            languageName: track.name.simpleText
        }));
        
        res.json({ languages });
        
    } catch (error) {
        console.error('Error getting subtitle languages:', error);
        res.status(500).json({ 
            error: 'Failed to get subtitle languages',
            message: error.message 
        });
    }
});

/**
 * Get subtitles in specific language
 */
app.get('/subtitles/:videoId/:lang', async (req, res) => {
    try {
        const { videoId, lang } = req.params;
        
        const subtitles = await getSubtitles({
            videoID: videoId,
            lang: lang
        });
        
        if (subtitles && subtitles.length > 0) {
            const formattedSubtitles = subtitles.map(sub => ({
                startTime: parseFloat(sub.start),
                endTime: parseFloat(sub.start) + parseFloat(sub.dur),
                text: sub.text
            }));
            
            res.json({ subtitles: formattedSubtitles });
        } else {
            res.status(404).json({ error: 'No subtitles found for this language' });
        }
        
    } catch (error) {
        console.error('Error getting subtitles:', error);
        res.status(500).json({ 
            error: 'Failed to get subtitles',
            message: error.message 
        });
    }
});

// Health check endpoint
app.get('/health', (req, res) => {
    res.json({ status: 'OK', message: 'Karaoke API is running' });
});

// Start server
app.listen(PORT, () => {
    console.log(`Karaoke API server running on port ${PORT}`);
    console.log(`Health check: http://localhost:${PORT}/health`);
});

