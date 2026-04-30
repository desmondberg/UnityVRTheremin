using UnityEngine;

public class CommitWaveform : MonoBehaviour
{
    [SerializeField] private DrawWaveform drawWaveform;
    [SerializeField] private WavetableOscillator osc;

    public void onButtonPressed()
    {
        float[] table = drawWaveform.GetWavetable();
        osc.SetWavetable(table);
    }
}
