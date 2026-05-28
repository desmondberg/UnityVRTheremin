import mongoose from 'mongoose';
const { Schema, model } = mongoose;

const pitchcurveSchema = new Schema({
    type: { type: String, enum: ['linear', 'ease_in', 'ease_out', 'ease_in_out'], required: true }
}, {
    timestamps: true
});

const Pitchcurve = model('Pitchcurve', pitchcurveSchema);
export default Pitchcurve;