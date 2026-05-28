using TMPro;
using UnityEngine;

public class ViewPresetPanel : MonoBehaviour
{
    public ViewPresetHandler handler;
    private Preset preset;

    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text scoreText;
    void Start()
    {
        preset = handler.currentPreset;
        titleText.text = preset.title;
        descriptionText.text = preset.description;
    }
}
