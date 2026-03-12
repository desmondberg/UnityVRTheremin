using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using TMPro;

public class OscillatorScript : MonoBehaviour
{
    public EventReference oscEvent;
    public EventInstance oscInstance;



    [Range(-12f, 24f)]
    public float pitch;

    [Range(0f, 1f)]
    public float vibratoDepth;
    [Range(0f, 1f)]
    public float vibratoRate;

    [Header("Pitch Controller")]
    public Transform pitchControllerTransform;

    [Header("Pitch Controller Mapping")]
    public float minY = 0.8f;      
    public float maxY = 1.3f;      
    public float minPitch = -12f; 
    public float maxPitch = 24f;

    [Header("Modulation Controller")]
    public Transform modControllerTransform;

    [Header("Mod Controller Mapping")]
    public float minPosY = 0.8f;
    public float maxPosY = 1.3f;

    //rotation on the Z axis, tracking from -60deg to +60deg
    public float minRoll = -60f;
    public float maxRoll = 60f;


    //reference starting positions
    Vector3 pitchControllerStartPos;
    Vector3 modControllerStartPos;

    public TMP_Text debugText;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oscInstance = RuntimeManager.CreateInstance(oscEvent);
        RuntimeManager.AttachInstanceToGameObject(oscInstance, gameObject);
        oscInstance.start();

        //enable vibrato by default
        oscInstance.setParameterByName("vibrato", 1);

        //record starting positions
        pitchControllerStartPos = pitchControllerTransform.localPosition;
        modControllerStartPos = modControllerTransform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if (pitchControllerTransform != null)
        {
            float controllerY = pitchControllerTransform.localPosition.y - pitchControllerStartPos.y;

            pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.InverseLerp(minY, maxY, controllerY));
            oscInstance.setParameterByName("pitch", pitch);
        }
        if (modControllerTransform != null)
        {
            float controllerY = modControllerTransform.localPosition.y - modControllerStartPos.y;
            float controllerRoll = modControllerTransform.localEulerAngles.z;
            //shift the value to lie between -180deg and +180deg
            if (controllerRoll > 180f) controllerRoll -= 360f;

            //calculate Y position of controller relative to min and max pos Y as percentage
            vibratoDepth = Mathf.InverseLerp(minPosY, maxPosY, controllerY);
            vibratoDepth = Mathf.Clamp01(vibratoDepth);

            //calculate roll of controller relative to min and max roll as percentage
            vibratoRate = Mathf.InverseLerp(minRoll, maxRoll, controllerRoll);
            vibratoRate = Mathf.Clamp01(vibratoRate);

            oscInstance.setParameterByName("vibrato_depth", vibratoDepth);
            oscInstance.setParameterByName("vibrato_rate", vibratoRate);
        }
        //set debug text
        if (debugText != null)
        {
            debugText.text = $"Pitch: {pitch:F2}, Vib depth: {vibratoDepth:F2}, Vib rate :{vibratoRate:F2}";
        }
    }

    private void OnDestroy()
    {
        oscInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        oscInstance.release();
    }
}
