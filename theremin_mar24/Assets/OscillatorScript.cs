
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OscillatorScript : MonoBehaviour
{
    [SerializeField] private Transform tBase;
    [SerializeField] private Transform tAntenna;
    private CapsuleCollider baseCol;

    [SerializeField] private Slider volumeSlider;
    private void OnVolumeChanged(float value)
    {
        amplitude = value;
    }



    private double _phase;
    private int _sampleRate;
    [SerializeField, Range(0, 1)] private float amplitude = 0.5f;
    private float frequency = 440.0f;
    private float currentFrequency;

    private volatile bool playing;



    private double _vibratoPhase;

    [Header("Pitch Controller")]
    public Transform pitchController;

    [Header("Pitch Controller Mapping")]
    public float pMinY = 0.5f; 
    public float pMaxY = 1.5f; 

    [Header("Mod Controller")]
    public Transform modController;

    [Header("Mod Controller Mapping")]
    public float mMinPosY = 1f; 
    public float mMaxPosY = 1.5f; 
    public float minRoll = -60f; 
    public float maxRoll = 60f;

    [Header("Pitch Distance Mapping")]
    public float minDistance = 0.04f;
    public float maxDistance = 0.4f;


    [Range(-12f, 24f)] public float pitch; 
    [Range(0f, 1f)] public float vibratoDepth; 
    [Range(0f, 10f)] public float vibratoRate;


    public TMP_Text pitchDebugText;
    public TMP_Text scaleManagerDebugText;
    public TMP_Text handPositionDebugText;


    //wavetable variables
    //[SerializeField] private int wavetable_length = 256;
    [SerializeField] private int wavetable_length = 2048;
    private float[] wavetable;

    public enum WaveType
    {
        Sine,
        Saw,
        Square,
        Triangle
    }
    public WaveType waveType;
    WaveType currentWaveType;

    public AnimationCurve pitchCurve;

    private GlobalScaleManager scaleManager => GlobalScaleManager.Instance;

    public void SetWavetable(float[] newTable)
    {
        //prevent table from mismatched length from being copied
        if (newTable.Length != wavetable_length)
        {
            return;
        }

        System.Array.Copy(newTable, wavetable, newTable.Length);
    }
    public void SetWaveform(int type)
    {
        switch (type)
        {
            case 0:
                currentWaveType=WaveType.Sine;
                break;
            case 1:
                currentWaveType = WaveType.Saw;
                break;
            case 2:
                currentWaveType = WaveType.Square;
                break;
            case 3:
                currentWaveType = WaveType.Triangle;
                break;
            default:
                currentWaveType = WaveType.Sine;
                break;
        }
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
        baseCol = tBase.GetComponent<CapsuleCollider>();
        _sampleRate = AudioSettings.outputSampleRate;

        wavetable = new float[wavetable_length];

        currentWaveType = waveType;
        GenerateWavetable(waveType);

        //set volume
        if (volumeSlider != null)
        {
            amplitude = volumeSlider.value;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (GlobalScaleManager.Instance == null)
        {
            Debug.LogWarning("GlobalScaleManager not ready yet");
        }


    }

    // Update is called once per frame
    void Update()
    {
        //set scale manager status
        try
        {
            scaleManagerDebugText.text = $"Scale Snapping: {(scaleManager.snapIsActive ? "On" : "Off")} \n Current Scale: {scaleManager.rootValueToNote[scaleManager.currentScale.rootNote]} {scaleManager.currentScale.scaleName}";
        }
        catch (Exception ex)
        {
            scaleManagerDebugText.text = ex.ToSafeString();
        }


        if (waveType != currentWaveType)
        {
            currentWaveType = waveType;
            GenerateWavetable(waveType);
        }
        bool pitchControllerActive = false;
        bool modControllerActive = false;

        if (pitchController != null)
        {
            Vector3 controllerPos = pitchController.position;
            Vector3 antennaPos = tAntenna.position;


            controllerPos.y = antennaPos.y;

            float distance = Vector3.Distance(controllerPos, antennaPos);
            distance = Mathf.Max(distance, minDistance);

            //-0.1f to give some space at the end where the user can play the lowest pitch without it cutting off
            float t = Mathf.InverseLerp(minDistance, maxDistance-0.1f, distance);
            //t = 1f - t;

            pitch = Mathf.Lerp(-24f, 12f, pitchCurve.Evaluate(t));

            //calculation with animation curve
            //float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
            //t = 1f - t;

            //float curved = pitchCurve.Evaluate(t);

            //pitch = Mathf.Lerp(-12f, 24f, curved);

            float calculatedFreq = frequency * Mathf.Pow(2f, pitch / 12f);


            currentFrequency = calculatedFreq;
            if (Vector3.Distance(controllerPos, antennaPos) < maxDistance)
            {
                pitchControllerActive = true;
            }
        }
        if (modController != null)
        {
            Vector3 controllerPos = modController.position;
            Bounds baseBounds = baseCol.bounds;
            //check if pos of controller is within x and z of base
            bool modControllerWithinBounds = controllerPos.x >= baseBounds.min.x && controllerPos.x <= baseBounds.max.x &&
        controllerPos.z >= baseBounds.min.z && controllerPos.z <= baseBounds.max.z;
            if (modControllerWithinBounds)
            {
                //get roll
                float controllerRoll = modController.eulerAngles.z;
                if (controllerRoll > 180f) controllerRoll -= 360f;


                float height = modController.position.y - tBase.position.y;

                vibratoDepth = Mathf.InverseLerp(0f, 0.5f, height);
                vibratoDepth = Mathf.Clamp01(vibratoDepth);

                vibratoRate = Mathf.InverseLerp(minRoll, maxRoll, controllerRoll);
                vibratoRate = Mathf.Lerp(0.5f, 12f, vibratoRate);

                modControllerActive = true;
            }


        }
        //set pitch info status
        pitchDebugText.text = $"Pitch:{pitch}, current freq: {currentFrequency} \n Vib rate:{vibratoRate}, Vib depth:{vibratoDepth}";
        //set hand position status
        handPositionDebugText.text = $"Pitch Controller active? {(pitchControllerActive ? "Yup" : "Nope")} \n Mod Controller active? {(modControllerActive ? "Yup" : "Nope")}";
        
        if (!pitchControllerActive || !modControllerActive)
        {
            playing = false;
        }
        if (pitchControllerActive && modControllerActive)
        {
            playing = true;
        }
    }

    void snapFrequencyToScale(float frequency)
    {

    }




    //private bool IsInsideTrigger(Transform controller) { 
    //    Vector3 controllerPos = controller.position; 
    //    Vector3 closest = detectionArea.ClosestPoint(controllerPos); 
    //    return closest == controllerPos; 
    //}
    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (playing)
        {
            //audio generation loop
            for (int sample = 0; sample < data.Length; sample += channels)
            {


                //vibrato generation
                double vibratoIncrement = vibratoRate / _sampleRate;
                _vibratoPhase = (_vibratoPhase + vibratoIncrement) % 1;
                float vibrato = Mathf.Sin((float)_vibratoPhase * 2 * Mathf.PI);

                float vibratoSteps = vibrato * vibratoDepth * 2f;
               
                double freqWithVibrato = currentFrequency * Mathf.Pow(2f, vibratoSteps / 12f);

                double phaseIncrement = freqWithVibrato / _sampleRate;
                _phase = (_phase + phaseIncrement) % 1;

                //wavetable lookup
                float index = (float)_phase * (wavetable_length - 1);

                ////pick two indexes, and interpolate to get the final phase
                int i1 = (int)index;
                int i2 = (i1 + 1) % wavetable_length;

                float indexFrac = index - i1;
                float sampleVal = Mathf.Lerp(wavetable[i1], wavetable[i2], indexFrac);


                float currentPhase = sampleVal * amplitude;

                for (int channel = 0; channel < channels; channel++)
                {
                    data[sample + channel] = currentPhase;
                }
            }
        }
    }

}
