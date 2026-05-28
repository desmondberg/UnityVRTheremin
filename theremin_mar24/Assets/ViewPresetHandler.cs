using System;
using UnityEngine;

public class ViewPresetHandler : MonoBehaviour
{
    [SerializeField] private AuthManager auth;
    [SerializeField] private DatabaseHandler handler;

    public Preset currentPreset;
    [SerializeField] private GameObject viewPresetPanel;
    [SerializeField] private GameObject commentPanel;


    [SerializeField] private ScaleManager scaleManager;
    [SerializeField] private OscillatorScript oscillator;

    public ToggleCanvasAndTheremin toggler;

    public void viewPreset(Preset preset)
    {
        currentPreset = preset;
        toggler.Toggle();
        viewPresetPanel.SetActive(true);
    }
    public void commentOnPreset()
    {
        toggler.Toggle();
        commentPanel.SetActive(true);
    }
    public void usePreset()
    {
        if (currentPreset.scale != null)
        {
            Scale scale = ScaleLib.CreateScale(currentPreset.scale.type, currentPreset.scale.rootNote);
            scaleManager.SetScale(scale);
        }
        oscillator.SetWaveform(currentPreset.waveform.type.ToUpper());
        toggler.Toggle();
    }
}
