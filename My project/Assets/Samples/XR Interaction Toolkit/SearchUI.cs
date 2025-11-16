using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the search UI for finding karaoke songs
/// </summary>
public class SearchUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private Button searchButton;
    [SerializeField] private Transform resultsContainer;
    [SerializeField] private GameObject resultItemPrefab;
    [SerializeField] private ScrollRect scrollRect;

    [Header("Loading")]
    [SerializeField] private GameObject loadingIndicator;

    private KaraokeManager karaokeManager;
    private List<GameObject> resultItems = new List<GameObject>();

    void Start()
    {
        karaokeManager = FindFirstObjectByType<KaraokeManager>();

        if (searchButton != null)
        {
            searchButton.onClick.AddListener(OnSearchButtonClicked);
        }

        if (searchInputField != null)
        {
            searchInputField.onSubmit.AddListener((text) => OnSearchButtonClicked());
        }

        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// Called when search button is clicked
    /// </summary>
    private void OnSearchButtonClicked()
    {
        if (searchInputField != null && !string.IsNullOrWhiteSpace(searchInputField.text))
        {
            PerformSearch(searchInputField.text);
        }
    }

    /// <summary>
    /// Perform search for karaoke songs
    /// </summary>
    public void PerformSearch(string query)
    {
        if (karaokeManager != null)
        {
            ClearResults();

            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(true);
            }

            karaokeManager.SearchKaraoke(query);
        }
    }

    /// <summary>
    /// Display search results
    /// </summary>
    public void DisplayResults(List<VideoSearchResult> results)
    {
        ClearResults();

        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(false);
        }

        foreach (VideoSearchResult result in results)
        {
            CreateResultItem(result);
        }

        // Reset scroll position
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    /// <summary>
    /// Create a result item in the UI
    /// </summary>
    private void CreateResultItem(VideoSearchResult result)
    {
        if (resultItemPrefab == null || resultsContainer == null) return;

        GameObject item = Instantiate(resultItemPrefab, resultsContainer);
        resultItems.Add(item);

        // Set up the result item
        ResultItemUI resultUI = item.GetComponent<ResultItemUI>();
        if (resultUI != null)
        {
            resultUI.Setup(result, OnResultSelected);
        }
    }

    /// <summary>
    /// Called when a result is selected
    /// </summary>
    private void OnResultSelected(string videoId)
    {
        if (karaokeManager != null)
        {
            karaokeManager.SelectVideo(videoId);
        }
    }

    /// <summary>
    /// Clear all search results
    /// </summary>
    private void ClearResults()
    {
        foreach (GameObject item in resultItems)
        {
            Destroy(item);
        }
        resultItems.Clear();
    }
}

/// <summary>
/// Individual result item UI component
/// </summary>
public class ResultItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI channelText;
    [SerializeField] private RawImage thumbnailImage;
    [SerializeField] private Button selectButton;

    private string videoId;
    private System.Action<string> onSelectCallback;

    /// <summary>
    /// Setup the result item with data
    /// </summary>
    public void Setup(VideoSearchResult result, System.Action<string> callback)
    {
        videoId = result.videoId;
        onSelectCallback = callback;

        if (titleText != null)
        {
            titleText.text = result.title;
        }

        if (channelText != null)
        {
            channelText.text = result.channelTitle;
        }

        if (thumbnailImage != null && !string.IsNullOrEmpty(result.thumbnailUrl))
        {
            StartCoroutine(LoadThumbnail(result.thumbnailUrl));
        }

        if (selectButton != null)
        {
            selectButton.onClick.AddListener(OnSelectClicked);
        }
    }

    /// <summary>
    /// Load thumbnail from URL
    /// </summary>
    private IEnumerator LoadThumbnail(string url)
    {
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Texture2D texture = UnityEngine.Networking.DownloadHandlerTexture.GetContent(request);
            if (thumbnailImage != null)
            {
                thumbnailImage.texture = texture;
            }
        }
    }

    /// <summary>
    /// Called when select button is clicked
    /// </summary>
    private void OnSelectClicked()
    {
        onSelectCallback?.Invoke(videoId);
    }
}

