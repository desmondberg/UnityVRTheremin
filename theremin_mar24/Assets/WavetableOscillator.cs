using UnityEngine;

public class WavetableOscillator : MonoBehaviour
{
    float phase;
    float sample_rate;
    static int wavetable_length = 512;

    float[] wavetable = new float[wavetable_length];
    float stepSize;

    [Range(138.81f, 523.25f)]
    public float frequency;

    public enum WaveType
    {
        Sine,
        Saw,
        Square,
        Triangle
    }
    public WaveType waveType;
    WaveType currentWaveType;

    public void SetWavetable(float[] newTable)
    {
        //prevent table from mismatched length from being copied
        if (newTable.Length != wavetable_length)
        {
            return;
        }

        System.Array.Copy(newTable, wavetable, newTable.Length);
    }
    void GenerateWavetable(WaveType type)
    {
        for (int n = 0; n < wavetable_length; n++)
        {
            float sample;

            switch (type)
            {
                case WaveType.Saw:
                    sample = 2f * (n / (float)wavetable_length) - 1f;
                    break;

                case WaveType.Square:
                    sample = n < (wavetable_length / 2) ? 1f : -1f;
                    break;
                case WaveType.Triangle:
                {
                    float x = n / (float)wavetable_length; 
                    sample = 1f - 4f * Mathf.Abs(x - 0.5f);
                    break;
                }
                case WaveType.Sine:
                default:
                    sample = Mathf.Sin(2 * Mathf.PI * n / wavetable_length);
                    break;
            }

            wavetable[n] = sample;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sample_rate = AudioSettings.outputSampleRate;
        phase = 0f;

        currentWaveType = waveType;
        GenerateWavetable(waveType);
    }

    // Update is called once per frame
    void Update()
    {

        if (waveType != currentWaveType)
        {
            currentWaveType = waveType;
            GenerateWavetable(waveType);
        }
    }
    private void OnAudioFilterRead(float[] data, int channels)
    {
        for(int i =0; i<data.Length; i+= channels)
        {
            //calculate step size
            stepSize = frequency / sample_rate;

            //get index, and corresponding sample value in the wavetable
            //int index = (int)phase * wavetable_length;
            int index = (int)(phase * wavetable_length);
            index = index % wavetable_length;

            float sample = wavetable[index];

            //write to both channels
            for (int ch = 0; ch< channels; ch++)
            {
                data[i + ch] = sample;
            }

            //increment phase
            phase += stepSize;

            //handle phase looping over
            if (phase >= 1f) phase -= 1f;
        }
    }
}
