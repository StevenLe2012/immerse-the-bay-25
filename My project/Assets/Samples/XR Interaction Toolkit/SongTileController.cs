using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SongData
{
    public string song;
    public string artist;
    public Sprite thumbnailSprite;

    public AudioClip audioClip;

    [TextArea]
    public string lyricsVTT;

    public string songLength;
}

public class SongTileController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI songNameText;
    [SerializeField] private TextMeshProUGUI artistNameText;
    [SerializeField] private Image thumbnailImage;

    [SerializeField] private TextMeshProUGUI songLengthText;

    private SongData songData;

    private Button button;

    private KaraokeText karaokeText;

    private CanvasSwitcher canvasSwitcher;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        // Get the KaraokeText instance (it's set in Start, so we get it here)
        karaokeText = KaraokeText.Instance;
        canvasSwitcher = CanvasSwitcher.Instance;

        
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
            karaokeText.setupKaraoke(songData.audioClip, songData.lyricsVTT);
            canvasSwitcher.SwitchToKaraokeCanvas();
        }
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

    public void SetSongLength(string songLength)
    {
        if (songLengthText != null)
        {
            songLengthText.text = songLength;
        }
        else
        {
            Debug.LogWarning("Song Length Text component is not assigned in SongListController");
        }
    }

    public void SetSongUI(SongData songData)
    {
        SetSongName(songData.song);
        SetArtistName(songData.artist);
        SetImage(songData.thumbnailSprite);
        SetSongLength(songData.songLength);
    }
}
