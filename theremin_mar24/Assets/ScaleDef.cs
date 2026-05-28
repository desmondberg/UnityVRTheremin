using System;

using System.Collections.Generic;

public static class ScaleLib
{
    public static readonly Dictionary<string, int[]>
        ScaleIntervals = new Dictionary<string, int[]>{
        { "Chromatic", new[] {0,1,2,3,4,5,6,7,8,9,10,11} },
        { "Major", new[] {0,2,4,5,7,9,11} },
        { "Minor", new[] {0,2,3,5,7,8,10} },
        { "Major Pentatonic", new[] {0,2,4,7,9} },
        { "Minor Pentatonic", new[] {0,3,5,7,10} },
        { "Dorian", new[] {0,2,3,5,7,9,10} },
        { "Phrygian", new[] {0,1,3,5,7,8,10} },
        { "Lydian", new[] {0,2,4,6,7,9,11} },
        { "Mixolydian", new[] {0,2,4,5,7,9,10} },
        { "Locrian", new[] {0,1,3,5,6,8,10} },
        { "Harmonic Minor", new[] {0,2,3,5,7,8,11} },
        { "Melodic Minor", new[] {0,2,3,5,7,9,11} },
        { "Blues", new[] {0,3,5,6,7,10} }
    };

    public static Scale CreateScale(string scaleName,int root)
    {
        if (!ScaleIntervals.ContainsKey(scaleName))
        {
            return null;
        }

        return new Scale(scaleName, root,ScaleIntervals[scaleName]);
    }
}

[Serializable]
public class ScaleDef
{
    public string type;
    public int[] intervals;

    public ScaleDef(string type,int[] intervals)
    {
        this.type = type;
        this.intervals = intervals;
    }
}