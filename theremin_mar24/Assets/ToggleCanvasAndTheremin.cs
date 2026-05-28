using TMPro;
using UnityEngine;

public class ToggleCanvasAndTheremin : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject backUi;
    [SerializeField] private OscillatorScript theremin;

    private bool canvasIsActive;

    public TMP_Text debugText;

    public void Toggle()
    {
        canvasIsActive = !canvasIsActive;

        debugText.text = "Toggle pressed!";

        canvas.SetActive(canvasIsActive);
        backUi.SetActive(!canvasIsActive);
        theremin.enabled = !canvasIsActive;
    }
}