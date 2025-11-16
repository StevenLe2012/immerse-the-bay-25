/**
 * Karaoke Backend Server - Song Library Edition
 * Manages manually uploaded songs with lyrics
 */

require('dotenv').config();
const express = require('express');
const cors = require('cors');
const multer = require('multer');
const path = require('path');
const fs = require('fs');
const songLibrary = require('./song-library');

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors());
app.use(express.json());

// Serve songs from Assets folder (where Unity has them)
const songsPath = path.join(__dirname, '..', 'Assets', 'songs');
app.use('/songs', express.static(songsPath));

console.log('Serving songs from:', songsPath);

// Configure multer for file uploads
const storage = multer.diskStorage({
    destination: function (req, file, cb) {
        let folder = 'songs/';
        
        if (file.fieldname === 'audio') {
            folder += 'audio/';
        } else if (file.fieldname === 'thumbnail') {
            folder += 'thumbnails/';
        } else if (file.fieldname === 'lyrics') {
            folder += 'lyrics/';
        }
        
        cb(null, folder);
    },
    filename: function (req, file, cb) {
        const uniqueSuffix = Date.now() + '-' + Math.round(Math.random() * 1E9);
        cb(null, file.fieldname + '-' + uniqueSuffix + path.extname(file.originalname));
    }
});

const upload = multer({
    storage: storage,
    limits: {
        fileSize: 50 * 1024 * 1024 // 50MB limit
    },
    fileFilter: function (req, file, cb) {
        if (file.fieldname === 'audio') {
            if (!file.mimetype.startsWith('audio/')) {
                return cb(new Error('Only audio files are allowed for audio field'));
            }
        } else if (file.fieldname === 'thumbnail') {
            if (!file.mimetype.startsWith('image/')) {
                return cb(new Error('Only image files are allowed for thumbnail field'));
            }
        } else if (file.fieldname === 'lyrics') {
            if (file.mimetype !== 'application/json' && !file.originalname.endsWith('.json')) {
                return cb(new Error('Only JSON files are allowed for lyrics field'));
            }
        }
        cb(null, true);
    }
});

/**
 * Root endpoint - API documentation
 */
app.get('/', (req, res) => {
    res.json({
        message: 'Karaoke API - Song Library Edition',
        version: '2.0.0',
        endpoints: {
            health: {
                method: 'GET',
                path: '/health',
                description: 'Check if API is running'
            },
            listSongs: {
                method: 'GET',
                path: '/api/songs',
                description: 'Get all songs in library'
            },
            searchSongs: {
                method: 'GET',
                path: '/api/songs/search?q=query',
                description: 'Search songs by title, author, or tags'
            },
            getSong: {
                method: 'GET',
                path: '/api/songs/:id',
                description: 'Get song details by ID'
            },
            getLyrics: {
                method: 'GET',
                path: '/api/songs/:id/lyrics',
                description: 'Get lyrics for a song'
            },
            uploadSong: {
                method: 'POST',
                path: '/api/upload',
                description: '[DISABLED] Songs must be added manually by admin. See MANUAL_SETUP_GUIDE.md'
            },
            updateSong: {
                method: 'PUT',
                path: '/api/songs/:id',
                description: 'Update song metadata'
            },
            deleteSong: {
                method: 'DELETE',
                path: '/api/songs/:id',
                description: 'Delete a song'
            },
            stats: {
                method: 'GET',
                path: '/api/stats',
                description: 'Get library statistics'
            }
        }
    });
});

/**
 * Health check
 */
app.get('/health', (req, res) => {
    res.json({
        status: 'OK',
        message: 'Karaoke API is running',
        songsCount: songLibrary.getAllSongs().length
    });
});

/**
 * Get all songs
 */
app.get('/api/songs', (req, res) => {
    try {
        const songs = songLibrary.getAllSongs();
        res.json({
            success: true,
            count: songs.length,
            songs: songs
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            error: error.message
        });
    }
});

/**
 * Search songs
 */
app.get('/api/songs/search', (req, res) => {
    try {
        const query = req.query.q;
        
        if (!query) {
            return res.status(400).json({
                success: false,
                error: 'Search query (q) is required'
            });
        }

        const results = songLibrary.searchSongs(query);
        
        res.json({
            success: true,
            query: query,
            count: results.length,
            songs: results
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            error: error.message
        });
    }
});

/**
 * Get song by ID
 */
app.get('/api/songs/:id', (req, res) => {
    try {
        const song = songLibrary.getSongById(req.params.id);
        
        if (!song) {
            return res.status(404).json({
                success: false,
                error: 'Song not found'
            });
        }

        res.json({
            success: true,
            song: song
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            error: error.message
        });
    }
});

/**
 * Get song lyrics
 */
app.get('/api/songs/:id/lyrics', (req, res) => {
    try {
        const lyrics = songLibrary.getLyrics(req.params.id);
        
        if (!lyrics) {
            return res.status(404).json({
                success: false,
                error: 'Lyrics not found'
            });
        }

        res.json({
            success: true,
            lyrics: lyrics
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            error: error.message
        });
    }
});

/**
 * Upload new song - DISABLED (Admin only - manual setup)
 * To add songs, manually edit song-library.json
 * See MANUAL_SETUP_GUIDE.md for instructions
 */
app.post('/api/upload', (req, res) => {
    res.status(403).json({
        success: false,
        error: 'Upload is disabled. Songs must be added manually by admin.',
        message: 'See MANUAL_SETUP_GUIDE.md for instructions on adding songs.'
    });
});

/**
 * Update song metadata
 */
app.put('/api/songs/:id', (req, res) => {
    try {
        const { title, author, tags, duration } = req.body;
        
        const updates = {};
        if (title) updates.title = title;
        if (author) updates.author = author;
        if (tags) updates.tags = tags.split(',').map(t => t.trim());
        if (duration) updates.duration = parseFloat(duration);

        const song = songLibrary.updateSong(req.params.id, updates);
        
        if (!song) {
            return res.status(404).json({
                success: false,
                error: 'Song not found'
            });
        }

        res.json({
            success: true,
            message: 'Song updated successfully',
            song: song
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            error: error.message
        });
    }
});

/**
 * Delete song
 */
app.delete('/api/songs/:id', (req, res) => {
    try {
        const deleted = songLibrary.deleteSong(req.params.id);
        
        if (!deleted) {
            return res.status(404).json({
                success: false,
                error: 'Song not found'
            });
        }

        res.json({
            success: true,
            message: 'Song deleted successfully'
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            error: error.message
        });
    }
});

/**
 * Get library statistics
 */
app.get('/api/stats', (req, res) => {
    try {
        const stats = songLibrary.getStats();
        
        res.json({
            success: true,
            stats: {
                totalSongs: stats.totalSongs,
                totalSize: `${(stats.totalSize / 1024 / 1024).toFixed(2)} MB`,
                uniqueAuthors: stats.uniqueAuthors
            }
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            error: error.message
        });
    }
});

// Error handling middleware
app.use((error, req, res, next) => {
    if (error instanceof multer.MulterError) {
        if (error.code === 'LIMIT_FILE_SIZE') {
            return res.status(400).json({
                success: false,
                error: 'File too large. Maximum size is 50MB'
            });
        }
    }
    
    res.status(500).json({
        success: false,
        error: error.message
    });
});

// Start server
app.listen(PORT, () => {
    console.log('\n' + '═'.repeat(60));
    console.log('  🎵 Karaoke API - Song Library Edition');
    console.log('═'.repeat(60));
    console.log(`\n✓ Server running on port ${PORT}`);
    console.log(`✓ Songs in library: ${songLibrary.getAllSongs().length}`);
    console.log(`\n📋 API Documentation: http://localhost:${PORT}/`);
    console.log(`🏥 Health check: http://localhost:${PORT}/health`);
    console.log(`🎵 List songs: http://localhost:${PORT}/api/songs`);
    console.log('\n' + '═'.repeat(60) + '\n');
});

