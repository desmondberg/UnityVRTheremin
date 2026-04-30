using UnityEngine;

[CreateAssetMenu(fileName = "Scale", menuName = "Scriptable Objects/Scale")]
//public class Scale : ScriptableObject
//{
//    static int[] MAJOR = { 0, 2, 4, 5, 7, 9, 11 };
//    static int[] MINOR = { 0, 2, 3, 5, 7, 8, 10 };

//    static int[] MAJOR_PENT = { 0, 2, 4, 7, 9};
//    static int[] MINOR_PENT = {0, 3, 5, 7, 10};

//    static int[] DORIAN = { 0, 2, 3, 5, 7, 9, 10 };
//    static int[] PHRYGIAN = { 0, 1, 3, 5, 7, 8, 10 };
//    static int[] LYDIAN = { 0, 2, 4, 6, 7, 9, 11 };
//    static int[] MIXOLYDIAN = { 0, 2, 4, 5, 7, 9, 10 };
//    static int[] LOCRIAN = { 0, 1, 3, 5, 6, 8, 10 };

//    static int[] HARMONIC_MINOR = { 0, 2, 3, 5, 7, 8, 11 };
//    static int[] MELODIC_MINOR = { 0, 2, 3, 5, 7, 9, 11 };
//    static int[] BLUES = { 0, 3, 5, 6, 7, 10 };
//    static int[] CHROMATIC = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

//    public static int[][] scales = { MAJOR, MINOR, MAJOR_PENT, MINOR_PENT, DORIAN, PHRYGIAN, LYDIAN, MIXOLYDIAN, LOCRIAN, HARMONIC_MINOR, MELODIC_MINOR, BLUES, CHROMATIC };
//}
public class Scale : ScriptableObject
{
    public string scaleName;
    public int rootNote = 0;
    public int[] intervals;

    public Scale(string name, int[] intervals)
    {
        scaleName = name;
        rootNote = 0;
        this.intervals = intervals;
    }

    public Scale(string name, int root, int[] intervals)
    {
        scaleName = name;
        if (root >= 0 && root <= 11) {
            rootNote = root;
        }
        else
        {
            root = 0;
        }
        this.intervals = intervals;
    }
}