using System;
using UnityEngine;
using static Oculus.Interaction.Context;

[CreateAssetMenu(fileName = "ScaleManager", menuName = "Scriptable Objects/ScaleManager")]

public class ScaleManager : ScriptableObject
{
    static int[] CHROMATIC = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    public String[] rootValueToNote = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    public bool snapIsActive;
    public Scale currentScale;

    public event Action<Scale> OnScaleChanged;

    private void Awake()
    {
        snapIsActive = false;
        currentScale = new Scale("Chromatic", 0, CHROMATIC);
    }

    public void SetScale(Scale scale)
    {
        currentScale = scale;
        OnScaleChanged?.Invoke(scale);
    }
}
