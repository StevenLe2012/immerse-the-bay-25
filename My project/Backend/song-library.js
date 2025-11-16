/**
 * Song Library Management System
 * Manages uploaded songs with metadata
 */

const fs = require('fs');
const path = require('path');

const LIBRARY_FILE = path.join(__dirname, 'song-library.json');
const SONGS_DIR = path.join(__dirname, '..', 'Assets', 'songs');

class SongLibrary {
    constructor() {
        this.songs = [];
        this.loadLibrary();
    }

    /**
     * Load song library from file
     */
    loadLibrary() {
        try {
            if (fs.existsSync(LIBRARY_FILE)) {
                const data = fs.readFileSync(LIBRARY_FILE, 'utf8');
                this.songs = JSON.parse(data);
                console.log(`✓ Loaded ${this.songs.length} songs from library`);
            } else {
                this.songs = [];
                this.saveLibrary();
            }
        } catch (error) {
            console.error('Error loading library:', error.message);
            this.songs = [];
        }
    }

    /**
     * Save song library to file
     */
    saveLibrary() {
        try {
            fs.writeFileSync(LIBRARY_FILE, JSON.stringify(this.songs, null, 2));
        } catch (error) {
            console.error('Error saving library:', error.message);
        }
    }

    /**
     * Add a new song to the library
     */
    addSong(songData) {
        const song = {
            id: this.generateId(),
            title: songData.title,
            author: songData.author,
            audioFile: songData.audioFile,
            thumbnailFile: songData.thumbnailFile,
            lyricsFile: songData.lyricsFile,
            duration: songData.duration || null,
            uploadDate: new Date().toISOString(),
            tags: songData.tags || []
        };

        this.songs.push(song);
        this.saveLibrary();
        
        return song;
    }

    /**
     * Get song by ID
     */
    getSongById(id) {
        return this.songs.find(song => song.id === id);
    }

    /**
     * Get all songs
     */
    getAllSongs() {
        return this.songs;
    }

    /**
     * Search songs by title or author
     */
    searchSongs(query) {
        const lowerQuery = query.toLowerCase();
        return this.songs.filter(song =>
            song.title.toLowerCase().includes(lowerQuery) ||
            song.author.toLowerCase().includes(lowerQuery) ||
            (song.tags && song.tags.some(tag => tag.toLowerCase().includes(lowerQuery)))
        );
    }

    /**
     * Delete song
     */
    deleteSong(id) {
        const index = this.songs.findIndex(song => song.id === id);
        if (index === -1) {
            return false;
        }

        const song = this.songs[index];
        
        // Delete associated files
        try {
            if (song.audioFile && fs.existsSync(path.join(SONGS_DIR, song.audioFile))) {
                fs.unlinkSync(path.join(SONGS_DIR, song.audioFile));
            }
            if (song.thumbnailFile && fs.existsSync(path.join(SONGS_DIR, song.thumbnailFile))) {
                fs.unlinkSync(path.join(SONGS_DIR, song.thumbnailFile));
            }
            if (song.lyricsFile && fs.existsSync(path.join(SONGS_DIR, song.lyricsFile))) {
                fs.unlinkSync(path.join(SONGS_DIR, song.lyricsFile));
            }
        } catch (error) {
            console.error('Error deleting files:', error.message);
        }

        this.songs.splice(index, 1);
        this.saveLibrary();
        
        return true;
    }

    /**
     * Update song metadata
     */
    updateSong(id, updates) {
        const song = this.getSongById(id);
        if (!song) {
            return null;
        }

        Object.assign(song, updates);
        this.saveLibrary();
        
        return song;
    }

    /**
     * Get lyrics for a song
     */
    getLyrics(id) {
        const song = this.getSongById(id);
        if (!song || !song.lyricsFile) {
            return null;
        }

        try {
            const lyricsPath = path.join(SONGS_DIR, song.lyricsFile);
            if (fs.existsSync(lyricsPath)) {
                const data = fs.readFileSync(lyricsPath, 'utf8');
                return JSON.parse(data);
            }
        } catch (error) {
            console.error('Error reading lyrics:', error.message);
        }

        return null;
    }

    /**
     * Generate unique ID
     */
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substring(2);
    }

    /**
     * Get library statistics
     */
    getStats() {
        return {
            totalSongs: this.songs.length,
            totalSize: this.calculateTotalSize(),
            uniqueAuthors: [...new Set(this.songs.map(s => s.author))].length
        };
    }

    /**
     * Calculate total storage size
     */
    calculateTotalSize() {
        let totalSize = 0;
        
        this.songs.forEach(song => {
            try {
                if (song.audioFile) {
                    const audioPath = path.join(SONGS_DIR, song.audioFile);
                    if (fs.existsSync(audioPath)) {
                        totalSize += fs.statSync(audioPath).size;
                    }
                }
                if (song.thumbnailFile) {
                    const thumbPath = path.join(SONGS_DIR, song.thumbnailFile);
                    if (fs.existsSync(thumbPath)) {
                        totalSize += fs.statSync(thumbPath).size;
                    }
                }
            } catch (error) {
                // Ignore file errors
            }
        });

        return totalSize;
    }
}

module.exports = new SongLibrary();

