using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SongData
{
    public string song;
    public string artist;
    public Sprite thumbnailSprite;

    public string lyricsVTT;

    
}

public class SongTileController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI songNameText;
    [SerializeField] private TextMeshProUGUI artistNameText;
    [SerializeField] private Image thumbnailImage;

    private SongData songData;

    private Button button;

    private KaraokeText karaokeText;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        // Get the KaraokeText instance (it's set in Start, so we get it here)
        karaokeText = KaraokeText.Instance;
        
        if (karaokeText == null)
        {
            Debug.LogWarning("KaraokeText.Instance is null. Make sure KaraokeText is initialized before using SongTileController.");
        }

        button.onClick.AddListener(OnButtonClicked);
    }

    public void SetSongData(SongData songData)
    {
        this.songData = songData;
    }

    private void OnButtonClicked()
    {
        if (songData == null)
        {
            Debug.LogWarning("SongData is null. Cannot handle button click.");
            return;
        }

        Debug.Log("Button clicked: " + songData.song);

        // Use KaraokeText.Instance directly or the cached reference
        if (karaokeText == null)
        {
            karaokeText = KaraokeText.Instance;
        }

        if (karaokeText != null && !string.IsNullOrEmpty(songData.lyricsVTT))
        {
            // Set the VTT lyrics data and initialize karaoke
            karaokeText.str = songData.lyricsVTT;
            karaokeText.InitKaraoke();
        }
        else
        {
            Debug.LogWarning("KaraokeText instance is null or lyricsVTT is empty.");
        }
    }

    public void SetSongData(string song, string artist, Sprite thumbnailSprite, string lyricsVTT)
    {
        if (songData == null)
        {
            songData = new SongData();
        }
        
        songData.song = song;
        songData.artist = artist;
        songData.thumbnailSprite = thumbnailSprite;
        songData.lyricsVTT = lyricsVTT;
    }



    /// <summary>
    /// Changes the artist name text component
    /// </summary>
    /// <param name="artistName">The name of the artist to display</param>
    public void SetArtistName(string artistName)
    {
        if (artistNameText != null)
        {
            artistNameText.text = artistName;
        }
        else
        {
            Debug.LogWarning("Artist Name Text component is not assigned in SongListController");
        }
    }

    /// <summary>
    /// Changes the song name text component
    /// </summary>
    /// <param name="songName">The name of the song to display</param>
    public void SetSongName(string songName)
    {
        if (songNameText != null)
        {
            songNameText.text = songName;
        }
        else
        {
            Debug.LogWarning("Song Name Text component is not assigned in SongListController");
        }
    }

    /// <summary>
    /// Changes the image component sprite
    /// </summary>
    /// <param name="sprite">The sprite to display as the thumbnail</param>
    public void SetImage(Sprite sprite)
    {
        if (thumbnailImage != null)
        {
            thumbnailImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Thumbnail Image component is not assigned in SongListController");
        }
    }

    /// <summary>
    /// Changes all three components at once: song name, artist name, and image
    /// </summary>
    /// <param name="songName">The name of the song to display</param>
    /// <param name="artistName">The name of the artist to display</param>
    /// <param name="sprite">The sprite to display as the thumbnail</param>
    public void SetSongUI(string songName, string artistName, Sprite sprite)
    {
        SetSongName(songName);
        SetArtistName(artistName);
        SetImage(sprite);
    }
}
