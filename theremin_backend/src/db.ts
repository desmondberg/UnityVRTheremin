import mongoose from "mongoose";

const uri = process.env.MONGODB_URI;


export function connect() {
    if (!uri) {
        throw new Error("MONGODB_URI is not defined");
    }

    mongoose.connect(uri);
}