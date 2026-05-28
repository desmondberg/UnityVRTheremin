using System;
using TMPro;
using UnityEngine;


public class ScaleDropdownController : MonoBehaviour
{
    //private GlobalScaleManager scaleManager => GlobalScaleManager.Instance;

    [SerializeField] private ScaleManager scaleManager;

    //public event Action<bool> onToggleChanged;

    public TMP_Dropdown scaleDropdown;
    public TMP_Dropdown rootDropdown;

    public TMP_Text globalDebug;

    //scale intervals
    //static int[] MAJOR = { 0, 2, 4, 5, 7, 9, 11 };
    //static int[] MINOR = { 0, 2, 3, 5, 7, 8, 10 };

    //static int[] MAJOR_PENT = { 0, 2, 4, 7, 9 };
    //static int[] MINOR_PENT = { 0, 3, 5, 7, 10 };

    //static int[] DORIAN = { 0, 2, 3, 5, 7, 9, 10 };
    //static int[] PHRYGIAN = { 0, 1, 3, 5, 7, 8, 10 };
    //static int[] LYDIAN = { 0, 2, 4, 6, 7, 9, 11 };
    //static int[] MIXOLYDIAN = { 0, 2, 4, 5, 7, 9, 10 };
    //static int[] LOCRIAN = { 0, 1, 3, 5, 6, 8, 10 };

    //static int[] HARMONIC_MINOR = { 0, 2, 3, 5, 7, 8, 11 };
    //static int[] MELODIC_MINOR = { 0, 2, 3, 5, 7, 9, 11 };
    //static int[] BLUES = { 0, 3, 5, 6, 7, 10 };
    //static int[] CHROMATIC = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

    public void HandleToggle(bool value)
    {
        scaleDropdown.interactable = value;
        rootDropdown.interactable = value;


        scaleManager.snapIsActive = value;
    }

    public void HandleSetScale(int selected)
    {
        string scaleType =scaleDropdown.options[selected].text;

        int root =scaleManager.currentScale.rootNote;

        Scale scale = ScaleLib.CreateScale(scaleType,root);

        scaleManager.SetScale(scale);
    }

    public void HandleSetRoot(int selected)
    {
        int root = selected;
        globalDebug.text = $"Setting scale to: {root} {scaleManager.currentScale.scaleName}";
        scaleManager.SetScale(
            new Scale(
                scaleManager.currentScale.scaleName,
                root,
                scaleManager.currentScale.intervals
            )
        );
    }






}
