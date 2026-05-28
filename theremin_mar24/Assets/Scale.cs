using System;

[Serializable]
public class Scale
{
    public string scaleName;
    public int rootNote;
    public int[] intervals;

    public Scale(string name, int root, int[] intervals)
    {
        scaleName = name;
        rootNote = (root >= 0 && root <= 11) ? root : 0;
        this.intervals = intervals;
    }
}