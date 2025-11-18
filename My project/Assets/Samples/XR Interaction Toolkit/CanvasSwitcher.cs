using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject selectionCanvas;
    [SerializeField] private GameObject karaokeCanvas;
    public InputActionReference buttonAction; // Drag your InputAction here in the Inspector


    public static CanvasSwitcher Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchToSelectionCanvas();
    }

    private void OnEnable()
    {
        buttonAction.action.performed += OnButtonPressed;
        buttonAction.action.Enable();
    }

    private void OnDisable()
    {
        buttonAction.action.performed -= OnButtonPressed;
        buttonAction.action.Disable();
    }

    public void SwitchToSelectionCanvas()
    {
        selectionCanvas.SetActive(true);
        if (this.gameObject.GetComponent<AudioSource>() != null)
        {
            this.gameObject.GetComponent<AudioSource>().Stop();
        }
        karaokeCanvas.SetActive(false);
        Debug.Log("Switched to Selection Canvas");
    }


    public void SwitchToKaraokeCanvas()
    {
        selectionCanvas.SetActive(false);
        karaokeCanvas.SetActive(true);
        Debug.Log("Switched to Karaoke Canvas");
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        SwitchToSelectionCanvas();
    }

}