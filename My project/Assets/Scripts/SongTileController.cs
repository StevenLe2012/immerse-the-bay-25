using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SongData
{
    public string song;
    public string artist;
    public Sprite thumbnailSprite;
}

public class SongTileController : MonoBehaviour
{
[Header("UI Components")]
    [SerializeField] private TextMeshProUGUI songNameText;
    [SerializeField] private TextMeshProUGUI artistNameText;
    [SerializeField] private Image thumbnailImage;

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
