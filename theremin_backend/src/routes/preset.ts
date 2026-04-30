import { Router } from "express";
import Preset from "../models/Preset";
import type z from "zod";
import validate from "../middleware/validate";
import { presetSchema } from "../zod";

import {getPresets, createPreset} from "../controllers/presetController";

const router = Router();

router.get("/", getPresets);

router.post("/", validate(presetSchema), createPreset);

export default router;