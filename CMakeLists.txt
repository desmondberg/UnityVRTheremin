cmake_minimum_required(VERSION 3.22)
project(testlib LANGUAGES C CXX)

add_library(addlib SHARED addlib.cpp)

set_target_properties(addlib PROPERTIES
    CXX_STANDARD 17
    CXX_STANDARD_REQUIRED YES
)