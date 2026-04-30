import mongoose from 'mongoose';
const { Schema, model } = mongoose;

const scaleSchema = new Schema({
    type: { type: String, enum: ['Major', 'Minor', 'Major Pentatonic', 'Minor Pentatonic', 'Dorian', 'Lydian', 'Mixolydian', 'Phrygian', 'Harmonic Minor', 'Melodic Minor', 'Blues', 'Chromatic'] },
    rootNote: Number,
    intervals: [Number]
}, {
    timestamps: true
});

const Scale = model('Scale', scaleSchema);
export default Scale;