import mongoose from 'mongoose';
const { Schema, model } = mongoose;

const waveformSchema = new Schema({
    type: { type: String, enum: ['sine', 'square', 'triangle', 'sawtooth', 'custom'], required: true },
    wavetable: {type:[Number], default: []}
}, {
    timestamps: true
});

const Waveform = model('Waveform', waveformSchema);
export default Waveform;