import { Router } from "express";
import Preset from "../models/Preset";
import type z from "zod";
import validate from "../middleware/validate";
import { presetSchema } from "../zod";

import {getPresets, createPreset, ratePreset, commentOnPreset, getPresetById} from "../controllers/presetController";
import { protectRoute } from "../middleware/auth";

const router = Router();

router.get("/", protectRoute, getPresets);
router.get("/:id", protectRoute, getPresetById);

router.post("/", protectRoute, createPreset);
router.post("/rate", protectRoute, ratePreset);
router.post("/comment", protectRoute, commentOnPreset);

export default router;