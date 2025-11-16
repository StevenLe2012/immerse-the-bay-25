using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SongListController : MonoBehaviour
{
    [Header("Prefab and Container")]
    [SerializeField] private GameObject songTilePrefab;
    [SerializeField] private Transform contentContainer;

    [Header("Song Data")]
    [SerializeField] private List<SongData> songDataList = new List<SongData>();

    private List<GameObject> spawnedTiles = new List<GameObject>();

    private void Start()
    {
        PopulateSongList();
        // AddSongTile("Song 1", "Artist 1", null);
        // AddSongTile("Song 2", "Artist 2", null);
        // AddSongTile("Song 3", "Artist 3", null);
    }

    /// <summary>
    /// Populates the list with song tiles based on the songDataList
    /// </summary>
    public void PopulateSongList()
    {
        ClearSongList();
        
        if (songTilePrefab == null)
        {
            Debug.LogError("Song Tile Prefab is not assigned in SongListController!");
            return;
        }

        if (contentContainer == null)
        {
            Debug.LogError("Content Container is not assigned in SongListController!");
            return;
        }

        foreach (SongData songData in songDataList)
        {
            CreateSongTile(songData);
        }
    }

    /// <summary>
    /// Populates the list with song tiles from a provided list of SongData
    /// </summary>
    /// <param name="songDataList">List of song data to populate</param>
    public void PopulateSongList(List<SongData> songDataList)
    {
        this.songDataList = songDataList;
        PopulateSongList();
    }

    /// <summary>
    /// Populates the list using separate lists for song names, artist names, sprites, and tile types
    /// </summary>
    /// <param name="songNames">List of song names</param>
    /// <param name="artistNames">List of artist names</param>
    /// <param name="sprites">List of thumbnail sprites</param>
    public void PopulateSongList(List<string> songNames, List<string> artistNames, List<Sprite> sprites)
    {
        ClearSongList();
        
        if (songTilePrefab == null || contentContainer == null)
        {
            Debug.LogError("Prefab or Container not assigned in SongListController!");
            return;
        }

        int maxCount = Mathf.Max(songNames?.Count ?? 0, artistNames?.Count ?? 0, sprites?.Count ?? 0);

        for (int i = 0; i < maxCount; i++)
        {
            SongData songData = new SongData
            {
                song = i < songNames.Count ? songNames[i] : "",
                artist = i < artistNames.Count ? artistNames[i] : "",
                thumbnailSprite = i < sprites.Count ? sprites[i] : null,
            };

            CreateSongTile(songData);
        }
    }

    /// <summary>
    /// Creates a single song tile from SongData
    /// </summary>
    /// <param name="songData">Data for the song tile</param>
    private void CreateSongTile(SongData songData)
    {
        GameObject tileObject = Instantiate(songTilePrefab, contentContainer);
        spawnedTiles.Add(tileObject);

        SongTileController tileController = tileObject.GetComponent<SongTileController>();
        if (tileController != null)
        {
            print("SongTileController component found on prefab: " + songTilePrefab.name);
            print("Song: " + songData.song);
            print("Artist: " + songData.artist);
            print("Thumbnail Sprite: " + songData.thumbnailSprite);
            tileController.SetSongUI(songData.song, songData.artist, songData.thumbnailSprite);
        }
        else
        {
            Debug.LogWarning($"SongTileController component not found on prefab: {songTilePrefab.name}");
        }
    }

    /// <summary>
    /// Clears all spawned song tiles from the list
    /// </summary>
    public void ClearSongList()
    {
        foreach (GameObject tile in spawnedTiles)
        {
            if (tile != null)
            {
                Destroy(tile);
            }
        }
        spawnedTiles.Clear();
    }

    /// <summary>
    /// Adds a single song tile to the list
    /// </summary>
    /// <param name="songName">Name of the song</param>
    /// <param name="artistName">Name of the artist</param>
    /// <param name="sprite">Thumbnail sprite</param>
    public void AddSongTile(string songName, string artistName, Sprite sprite)
    {
        SongData songData = new SongData
        {
            song = songName,
            artist = artistName,
            thumbnailSprite = sprite,
        };

        CreateSongTile(songData);
        songDataList.Add(songData);
    }

    /// <summary>
    /// Gets the number of spawned tiles
    /// </summary>
    /// <returns>Number of tiles currently in the list</returns>
    public int GetTileCount()
    {
        return spawnedTiles.Count;
    }
}
