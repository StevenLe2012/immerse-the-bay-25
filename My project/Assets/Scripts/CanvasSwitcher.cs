using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject selectionCanvas;
    [SerializeField] private GameObject karaokeCanvas;

    public static CanvasSwitcher Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchToSelectionCanvas();
    }

    public void SwitchToSelectionCanvas()
    {
        selectionCanvas.SetActive(true);
        karaokeCanvas.SetActive(false);
    }


    public void SwitchToKaraokeCanvas()
    {
        selectionCanvas.SetActive(false);
        karaokeCanvas.SetActive(true);
    }
    
}