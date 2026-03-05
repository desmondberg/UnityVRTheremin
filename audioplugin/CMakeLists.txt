cmake_minimum_required(VERSION 3.22)

project(AudioPluginOscillator)

set(CMAKE_CXX_STANDARD 17)

add_library(AudioPluginOscillator SHARED
    Oscillator.cpp
    NativeAudioPlugins/NativeCode/AudioPluginUtil.cpp
)

target_include_directories(AudioPluginOscillator PRIVATE
    NativeAudioPlugins/NativeCode
)
