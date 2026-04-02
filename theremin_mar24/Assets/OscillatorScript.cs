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
    [SerializeField] private float frequency = 440.0f;
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
    public float minDistance = 0.1f;
    public float maxDistance = 0.6f;


    [Range(-12f, 24f)] public float pitch; 
    [Range(0f, 1f)] public float vibratoDepth; 
    [Range(0f, 10f)] public float vibratoRate;


    public TMP_Text debugText;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseCol = tBase.GetComponent<CapsuleCollider>();
        _sampleRate = AudioSettings.outputSampleRate;

        if (volumeSlider != null)
        {
            amplitude = volumeSlider.value;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool pitchControllerActive = false;
        bool modControllerActive = false;

        if (pitchController != null)
        {
            Vector3 controllerPos = pitchController.position;
            Vector3 antennaPos = tAntenna.position;


            controllerPos.y = antennaPos.y;

            float distance = Vector3.Distance(controllerPos, antennaPos);
            distance = Mathf.Max(distance, minDistance);

            float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
            t = 1f - t;

            pitch = Mathf.Lerp(-12f, 24f, t);

            currentFrequency = frequency * Mathf.Pow(2f, pitch / 12f);

            if(Vector3.Distance(controllerPos, antennaPos) < maxDistance)
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
        setDebugText(pitchControllerActive, modControllerActive);
        if (!pitchControllerActive || !modControllerActive)
        {
            playing = false;
        }
        if (pitchControllerActive && modControllerActive)
        {
            playing = true;
        }
    }

    void setDebugText(bool pitchControllerActive, bool modControllerActive)
    {
        debugText.text = $"Pitch controller active?: {(pitchControllerActive ? "Yep" : "Nope")}, Mod controller active?: {(modControllerActive ? "Yep" : "Nope")}, Pitch:{pitch:F2}, Vib Depth:{vibratoDepth:F2}, Vib Rate:{vibratoRate:F2} Amplitude: {amplitude:F2}";
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
                //pitch calculated logarithmically
                double freqWithVibrato = currentFrequency * Mathf.Pow(2f, vibratoSteps / 12f);

                double phaseIncrement = freqWithVibrato / _sampleRate;
                _phase = (_phase + phaseIncrement) % 1;
                float currentPhase = Mathf.Sin((float)_phase * 2 * Mathf.PI) * amplitude;

                for (int channel = 0; channel < channels; channel++)
                {
                    data[sample + channel] = currentPhase;
                }
            }
        }
    }
}
