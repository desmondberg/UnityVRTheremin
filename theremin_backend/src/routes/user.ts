import { Router } from "express";
import User from "../models/User";
import {login, signup, me} from '../controllers/userController'

const router = Router();

import bcrypt from "bcrypt";
import { protectRoute } from "../middleware/auth";

router.get("/me", protectRoute, me);
router.post("/signup", signup);
router.post("/login", login);

export default router;