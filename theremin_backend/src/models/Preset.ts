import mongoose from 'mongoose';
const { Schema, model } = mongoose;

const presetSchema = new Schema({
  title: { type: String, required: true },
  description: { type: String },
  author: { type: Schema.Types.ObjectId, ref: 'User', required: true },
  tags: [{ type: String }],
  votes: [{
    user: {
      type: Schema.Types.ObjectId,
      ref: "User"
    },
    value: {
      type: Number // 1 or -1
    }
  }],
  comments: [{
    user: { type: Schema.Types.ObjectId, ref: 'User', required: true },
    content: { type: String, required: true },
  }],

  waveform: { type: Schema.Types.ObjectId, ref: 'Waveform', required: true },
  scale: { type: Schema.Types.ObjectId, ref: 'Scale' },
  pitchcurve: { type: Schema.Types.ObjectId, ref: 'Pitchcurve', required: true }



}, {
  timestamps: true
});

const Preset = model('Preset', presetSchema);
export default Preset;
