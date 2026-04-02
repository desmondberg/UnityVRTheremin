using UnityEngine;

public class AudioTestScript : MonoBehaviour
{
    private double _phase;
    private int _sampleRate;
    [SerializeField, Range(0, 1)] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 440.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _phase = 0;
        _sampleRate = AudioSettings.outputSampleRate;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnAudioFilterRead(float[] data, int channels)
    {
        double phaseIncrement = frequency / _sampleRate;
        for (int sample = 0; sample < data.Length; sample += channels)
        {
            //audio generation loop
            float currentPhase = Mathf.Sin((float)_phase * 2 * Mathf.PI) * amplitude;
            _phase = (_phase + phaseIncrement) % 1;
            for (int channel = 0; channel < channels; channel++)
            {
                data[sample + channel] = currentPhase;
            }
        }
    }
}
