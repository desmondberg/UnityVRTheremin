import { Router } from "express";
import User from "../models/User";

const router = Router();

import bcrypt from "bcrypt";

router.post("/signup", async (req, res) => {
  const { username, email, password } = req.body;

  try {
    const existingUser = await User.findOne({ email });
    if (existingUser) {
      return res.status(400).json({ message: "User already exists" });
    }

    //hash password
    const hashedPassword = await bcrypt.hash(password, 10);

    const user = new User({
      username,
      email,
      password: hashedPassword
    });

    await user.save();

    res.status(201).json({ message: "User created" });

  } catch (err) {
    res.status(500).json({ message: "Signup failed", error: err });
  }
});
export default router;