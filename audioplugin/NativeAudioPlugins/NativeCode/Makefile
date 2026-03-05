SRCS=AudioPluginUtil.cpp \
Plugin_ConvolutionReverb.cpp \
Plugin_Granulator.cpp \
Plugin_PitchDetector.cpp \
Plugin_RingModulator.cpp \
Plugin_Synthesizer.cpp \
hrtftable.cpp
OBJS=$(SRCS:.cpp=.o)
OUTPUT=libAudioPluginDemo.so
CXXFLAGS=-I. -O2 -fPIC -fvisibility=hidden -Wno-attributes
LDFLAGS=-shared -rdynamic -fPIC
CXX=g++

all: $(OUTPUT)
clean:
	rm -f $(OUTPUT) $(OBJS)

$(OUTPUT): $(OBJS)
	$(CXX) $(LDFLAGS) -o $(OUTPUT) $(OBJS)

.cpp.o:
	$(CXX) $(CXXFLAGS) -c $<
