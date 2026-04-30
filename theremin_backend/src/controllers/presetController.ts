import mongoose from "mongoose";
import Preset from "../models/Preset";
import Waveform from "../models/Waveform";

export const getPresets = async (req: any, res: any) => {

    try {
        let presets = await Preset.find()
            .populate('author')
            .populate('waveform')
            .populate('scale')
            .exec();
        res.status(200).json({ "message": "Presets retrieved successfully", "presets": presets });
    } catch (error) {
        res.status(500).json({ "message": "Failed to get presets", "error": error });
    }
};

export const createPreset = async (req: any, res: any) => {
    const session = await mongoose.startSession();

    try {
        let createdPreset;

        await session.withTransaction(async () => {
            //CREATE OR FIND WAVEFORM

            let waveform;
            //try and find existing waveform (for sines, sawtooths, squares, triangles)
            waveform = await Waveform.findById(req.body.waveform).session(session);
            //if there isn't one, it means the waveform is custom and we create one with the given wavetable data
            if (!waveform) {
                waveform = await Waveform.create({wavetable: req.body.waveform.wavetable, type: 'custom' });
            }

            //FIND SCALE
            let scale;
            if(req.body.scale){
                scale = await mongoose.model('Scale').findOne({ name: req.body.scale }).session(session);
                if(!scale){
                    res.status(400).json({error: `Scale with name ${req.body.scale} not found` });
                }
            }

            //create final preset object
            const preset = await Preset.create(
                [
                    {
                        ...req.body,
                        waveform: waveform._id,
                        scale: scale?._id 
                    }
                ],
                { session }
            );
            
            createdPreset = preset[0];
        });

        session.endSession();

        return res.status(201).json({
            message:"Preset created successfully",
            preset: createdPreset
        });

    } catch (err: any) {
        await session.abortTransaction();
        session.endSession();

        return res.status(500).json({
            error: err.message
        });
    }
};