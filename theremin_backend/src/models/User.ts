import mongoose from 'mongoose';
const { Schema, model } = mongoose;

const userSchema = new Schema({
    username: String,
    email: String,
    password: String,
    presets: [{ type: Schema.Types.ObjectId, ref: 'Preset' }]
}, {
    timestamps: true
});

const User = model('User', userSchema);
export default User;