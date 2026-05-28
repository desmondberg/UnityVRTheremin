using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalScaleManager : MonoBehaviour
{
    static int[] CHROMATIC = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    public String[] rootValueToNote = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    //singleton instance
    public static GlobalScaleManager Instance;

    public bool snapIsActive;

    public Scale currentScale;
    public event Action<Scale> onScaleChanged;

    private void Awake()
    {
        snapIsActive = false;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Instance = this;
        currentScale = new Scale("Chromatic", 0, CHROMATIC);
    }

    public void SetScale(Scale scale)
    {
        currentScale = scale;
        onScaleChanged?.Invoke(scale);
    }
}
