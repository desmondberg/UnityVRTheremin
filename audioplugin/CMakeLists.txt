cmake_minimum_required(VERSION 3.22)
project(AudioPluginOscillator)
set(CMAKE_CXX_STANDARD 17)

# add_library(AudioPluginOscillator SHARED
#     Oscillator.cpp
#     NativeAudioPlugins/NativeCode/AudioPluginUtil.cpp
# )

# target_include_directories(AudioPluginOscillator PRIVATE
#     NativeAudioPlugins/NativeCode
# )


add_library(AudioPluginOscillator SHARED Oscillator.cpp NativeAudioPlugins/NativeCode/AudioPluginUtil.cpp)

target_include_directories(AudioPluginOscillator PRIVATE
     NativeAudioPlugins/NativeCode
 )

if(MSVC)
    target_compile_definitions(AudioPluginOscillator PRIVATE _USE_MATH_DEFINES)
else()
    target_link_libraries(AudioPluginOscillator m)
endif()
