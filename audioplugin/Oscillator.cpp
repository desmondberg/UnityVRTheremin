#include <math.h>
#include "AudioPluginInterface.h"
#include "AudioPluginUtil.h"

#define _USE_MATH_DEFINES
#include <cmath>



enum Param
{
    P_FREQ,
    P_NUM
};
//creates an interface to control the oscillator inside unity
extern "C" UNITY_AUDIODSP_EXPORT_API
int InternalRegisterEffectDefinition(UnityAudioEffectDefinition& definition)
{
    int numparams = P_NUM;
    definition.paramdefs = new UnityAudioParameterDefinition [numparams];
    AudioPluginUtil::RegisterParameter(definition, "Frequency", "Hz",
        0.0f, AudioPluginUtil::kMaxSampleRate, 1000.0f,
        1.0f, 3.0f,
        P_FREQ);
    return numparams;
}
struct OscData{
    //number of samples per second
    float sample_rate;
    //position in waveform, 0 - start of cycle, 0.5 - apex, 1 - back to the start
    float phase;
    //current frequency
    float freq;
};

//initialise oscillator
extern "C" UNITY_AUDIODSP_EXPORT_API
UNITY_AUDIODSP_RESULT UNITY_AUDIODSP_CALLBACK CreateCallback(UnityAudioEffectState* state){
    OscData* data = new OscData();
    data->sample_rate = state->samplerate; //48000hz 
    data->phase = 0.0f;

    state->effectdata = data;

    return UNITY_AUDIODSP_OK;
}

extern "C" UNITY_AUDIODSP_EXPORT_API
UNITY_AUDIODSP_RESULT UNITY_AUDIODSP_CALLBACK ProcessCallback(
    UnityAudioEffectState* state,
    //incoming audio, can be ignored as the oscillator only produces audio
    float* inbuffer,
    //outgoing audio
    float* outbuffer,
    //no. of frames in the buffer
    unsigned int length,
    //input channels, can be ignored
    int inchannels,
    //output channels
    int outchannels)
{
    OscData* data = state->GetEffectData<OscData>();

    float phase = data->phase;
    float srate = state->samplerate;  
    float freq = data->freq;

    //how much phase to add per sample increment
    float phaseIncrement = freq / srate;

    //for the length of the buffer, calculate the current sample to send, increment phase, and if phase goes over 1, reset it to 0
    for (unsigned int n = 0; n < length; ++n)
    {
        float current_sample = sinf(phase * 2.0f * M_PI);

        phase += phaseIncrement;
        if (phase >= 1.0f)
            phase -= 1.0f;

        for (int ch = 0; ch < outchannels; ++ch)
            outbuffer[n * outchannels + ch] = current_sample;
    }

    data->phase = phase;

    return UNITY_AUDIODSP_OK;
}

//clean-up
extern "C" UNITY_AUDIODSP_EXPORT_API
UNITY_AUDIODSP_RESULT UNITY_AUDIODSP_CALLBACK ReleaseCallback(UnityAudioEffectState* state){
    OscData* data = state->GetEffectData<OscData>();
    delete data;
    return UNITY_AUDIODSP_OK;
}

