using UnityEngine;

public class WaveformButtonsController : MonoBehaviour
{
    public OscillatorScript osc;
    public void HandleButtonClick(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                Debug.Log("Wave type set to Sine");
                osc.waveType = OscillatorScript.WaveType.SINE;
                break;
            case 1:
                Debug.Log("Wave type set to Saw");
                osc.waveType = OscillatorScript.WaveType.SAW;
                break;
            case 2:
                Debug.Log("Wave type set to Square");
                osc.waveType = OscillatorScript.WaveType.SQUARE;
                break;
            case 3:
                Debug.Log("Wave type set to Triangle");
                osc.waveType = OscillatorScript.WaveType.TRIANGLE;
                break;
        }
    }
}
