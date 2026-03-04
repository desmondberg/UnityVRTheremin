#include <math.h>
#include "AudioPluginInterface.h"
#include "AudioPluginUtil.h"

enum Param
{
    P_FREQ,
    P_NUM
};

//creates an interface to control the oscillator inside unity
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
};

