using System;
using TMPro;
using UnityEngine;


public class ScaleDropdownController : MonoBehaviour
{
    private GlobalScaleManager scaleManager => GlobalScaleManager.Instance;

    //public event Action<bool> onToggleChanged;

    public TMP_Dropdown scaleDropdown;
    public TMP_Dropdown rootDropdown;

    //scale intervals
    static int[] MAJOR = { 0, 2, 4, 5, 7, 9, 11 };
    static int[] MINOR = { 0, 2, 3, 5, 7, 8, 10 };

    static int[] MAJOR_PENT = { 0, 2, 4, 7, 9 };
    static int[] MINOR_PENT = { 0, 3, 5, 7, 10 };

    static int[] DORIAN = { 0, 2, 3, 5, 7, 9, 10 };
    static int[] PHRYGIAN = { 0, 1, 3, 5, 7, 8, 10 };
    static int[] LYDIAN = { 0, 2, 4, 6, 7, 9, 11 };
    static int[] MIXOLYDIAN = { 0, 2, 4, 5, 7, 9, 10 };
    static int[] LOCRIAN = { 0, 1, 3, 5, 6, 8, 10 };

    static int[] HARMONIC_MINOR = { 0, 2, 3, 5, 7, 8, 11 };
    static int[] MELODIC_MINOR = { 0, 2, 3, 5, 7, 9, 11 };
    static int[] BLUES = { 0, 3, 5, 6, 7, 10 };
    static int[] CHROMATIC = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

    public void HandleToggle(bool value)
    {
        scaleDropdown.interactable = value;
        rootDropdown.interactable = value;


        if (GlobalScaleManager.Instance == null)
        {
            Debug.LogWarning("GlobalScaleManager not ready yet");
            return;
        }

        scaleManager.snapIsActive = value;
    }

    public void HandleSetScale(TMP_Dropdown dropdown)
    {
        int root = scaleManager.currentScale.rootNote;
        int scale = dropdown.value;
        switch (scale)
        {
            //CHROMATIC
            case 0:
                scaleManager.SetScale(new Scale("Chromatic", root, CHROMATIC));
                break;
            //MAJOR
            case 1:
                scaleManager.SetScale(new Scale("Major", root, MAJOR));
                break;
            //MINOR
            case 2:
                scaleManager.SetScale(new Scale("Minor", root, MINOR));
                break;
            //MAJOR PENT
            case 3:
                scaleManager.SetScale(new Scale("Major Pentatonic", root, MAJOR_PENT));
                break;
            //MINOR PENT
            case 4:
                scaleManager.SetScale(new Scale("Minor Pentatonic", root, MINOR_PENT));
                break;
            //DORIAN
            case 5:
                scaleManager.SetScale(new Scale("Dorian", root, DORIAN));
                break;
            //PHRYGIAN
            case 6:
                scaleManager.SetScale(new Scale("Phrygian", root, PHRYGIAN));
                break;
            //LYDIAN
            case 7:
                scaleManager.SetScale(new Scale("Lydian", root, LYDIAN));
                break;
            //MIXOLYDIAN
            case 8:
                scaleManager.SetScale(new Scale("Mixolydian", root, MIXOLYDIAN));
                break;
            //LOCRIAN
            case 9:
                scaleManager.SetScale(new Scale("Locrian", root, LOCRIAN));
                break;
            //HARMONIC MINOR
            case 10:
                scaleManager.SetScale(new Scale("Harmonic Minor", root, HARMONIC_MINOR));
                break;
            //MELODIC MINOR
            case 11:
                scaleManager.SetScale(new Scale("Melodic Minor", root, MELODIC_MINOR));
                break;
            //BLUES
            case 12:
                scaleManager.SetScale(new Scale("Blues", root, BLUES));
                break;
        }
    }

    public void HandleSetRoot(TMP_Dropdown dropdown)
    {
        int root = dropdown.value;
        scaleManager.SetScale(
            new Scale(
                scaleManager.currentScale.scaleName,
                root,
                scaleManager.currentScale.intervals
            )
        );
    }






}
