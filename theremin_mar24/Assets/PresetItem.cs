using System;
using TMPro;
using UnityEngine;

public class PresetItem : MonoBehaviour
{
    private Preset preset;
    public TMP_Text titleText;
    public TMP_Text authorText;
    public TMP_Text scoreText;

    private ViewPresetHandler handler;
    public void Setup(Preset preset, ViewPresetHandler handler)
    {

        this.preset = preset;
        this.handler = handler;

        titleText.text = preset.title;
        authorText.text = preset.author.username;
        int score = 0;

        foreach (Vote vote in preset.votes)
        {
            score += vote.value;
        }

        scoreText.text = score.ToString();
    }

    public void viewPreset()
    {
        handler.viewPreset(preset);
    }

}
