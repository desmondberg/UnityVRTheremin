import numpy as np
from scipy.io.wavfile import write

sample_rate = 48000

freq = 440

t = np.arange(sample_rate) / sample_rate

sine_wave = np.sin(2 * np.pi * freq * t)
saw_wave = 2 * ((freq * t) - np.floor(0.5 + freq * t))
square_wave = np.sign(sine_wave)

write('sine_wave.wav', sample_rate, sine_wave.astype(np.float32))
write('saw_wave.wav', sample_rate, saw_wave.astype(np.float32))
write('square_wave.wav', sample_rate, square_wave.astype(np.float32))