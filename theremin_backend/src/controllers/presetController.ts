import mongoose from "mongoose";
import Preset from "../models/Preset";
import Waveform from "../models/Waveform";
import Pitchcurve from "../models/Pitchcurve";
import Scale from "../models/Scale";


export const getPresets = async (req: any, res: any) => {

  try {
    let presets = await Preset.find()
      .populate('author', "-password")
      .populate('waveform')
      .populate('scale')
      .populate('pitchcurve')
      .exec();
    res.status(200).json({ "message": "Presets retrieved successfully", "presets": presets });
  } catch (error) {
    res.status(500).json({ "message": "Failed to get presets", "error": error });
  }
};

export const getPresetById = async (req: any, res: any) => {
  if (!req.params.id) {
    res.status(500).json({ "error": "No preset id provided" });
  }
  try {
    let preset = await Preset.findById(req.params.id).populate('author')
      .populate('waveform')
      .populate('scale')
      .populate('pitchcurve')
      .exec();
    res.status(200).json({ "message": "Preset retrieved successfully", "preset": preset });
  } catch (error) {
    res.status(500).json({ "message": "Failed to get preset", "error": error });
  }
}

export const createPreset = async (req: any, res: any) => {
  const session = await mongoose.startSession();

  try {
    let createdPreset;

    await session.withTransaction(async () => {
      //CREATE OR FIND WAVEFORM

      let waveform;
      //try and find existing waveform (for sines, sawtooths, squares, triangles)
      // //if there isn't one, it means the waveform is custom and we create one with the given wavetable data
      waveform = await Waveform.findOne({type: req.body.waveform}).session(session);

      if (!waveform) {
        waveform = (await Waveform.create([{ type: req.body.waveform }],{ session }))[0];
      }


      let pitchcurve;
      pitchcurve = await Pitchcurve.findOne({type: req.body.pitchcurve}).session(session);

      if (!pitchcurve) {
        pitchcurve = (await Pitchcurve.create([{ type: req.body.pitchcurve }],{ session }))[0];
      }


      //FIND SCALE
      let scale;

      if (req.body.scale) {
        scale =await Scale.findOne({
            type: req.body.scale.type,
            rootNote: req.body.scale.root,
            intervals: req.body.scale.intervals
          }).session(session);

        if (!scale) {
          scale = (await Scale.create([{type: req.body.scale.type, rootNote:req.body.scale.root, intervals:req.body.scale.intervals}],{session}))[0];
          //return res.status(400).json({error:`Scale ${req.body.scale.root} ${req.body.scale.type} not found`});
        }
      }

      let author = req.user._id;

      //create final preset object
      const preset = await Preset.create(
        [
          {
            ...req.body,
            author,
            waveform: waveform?._id,
            scale: scale?._id,
            pitchcurve: pitchcurve?._id
          }
        ],
        { session }
      );

      createdPreset = preset[0];
    });

    session.endSession();

    return res.status(201).json({
      message: "Preset created successfully",
      preset: createdPreset
    });

  } catch (err: any) {
    return res.status(500).json({
      error: err.message,
      req: req.body
    });
  }
};


export const ratePreset = async (req: any, res: any) => {
  try {
    const { presetId, rating } = req.body;

    if (!presetId) {
      return res.status(400).json({
        message: "preset id required"
      });
    }

    if (rating !== "upvote" &&rating !== "downvote") {
      return res.status(400).json({
        message: "invalid rating"
      });
    }

    const preset =await Preset.findById(presetId);
    if (!preset) {
      return res.status(404).json({
        message: "preset not found"
      });
    }

    //find existing vote
    let voteByUser = preset.votes.find((vote: any) =>vote.user.toString() ===req.user._id.toString());
    const voteValue = rating === "upvote" ? 1 : -1;
    //if user hasn't voted on this preset
    if (!voteByUser) {
      preset.votes.push({user: req.user._id, value: voteValue});
    } else {

      //toggle vote if same option is selected again
      if (voteByUser.value === voteValue) {
        voteByUser.value = -voteValue;
      } else {
        //change vote
        voteByUser.value = voteValue;
      }
    }

    await preset.save();

    //upvote, downvotes and total score to display on the frontend
    const upvotes = preset.votes.filter((vote: any) => vote.value === 1).length;
    const downvotes = preset.votes.filter((vote: any) => vote.value === -1).length;
    const score = preset.votes.reduce((sum: number, vote: any) => sum + vote.value, 0);

    return res.status(200).json({
      message: "rating successful",
      upvotes,
      downvotes,
      score
    });

  } catch (err: any) {

    return res.status(500).json({
      message: err.message
    });

  }
};

export const commentOnPreset = async (req: any, res: any) => {
  try {
    const { presetId, content } = req.body;

    if (!presetId) {
      return res.status(400).json({
        message: "preset id required"
      });
    }

    if (!content?.trim()) {
      return res.status(400).json({
        message: "comment is empty"
      });
    }

    const preset =
      await Preset.findById(presetId);

    if (!preset) {
      return res.status(404).json({
        message: "Preset not found"
      });
    }

    preset.comments.push({
      user: req.user._id,
      content
    });

    await preset.save();

    await preset.populate(
      "comments.user",
      "username email"
    );

    return res.status(201).json({
      message: "comment added",
      comments: preset.comments
    });
  } catch (err: any) {
    return res.status(500).json({
      message: err.message
    });
  }
};