import mongoose from "mongoose";
import User from "../models/User";
import bcrypt from "bcryptjs";
import { generateToken } from "../utils/generatedToken";


export const me = async (req: any, res: any) => {
    try {
        return res.status(200).json(req.user);
    } catch (error) {
        console.error(error);

        return res.status(500).json({
            message: "Server error"
        });
    }
};

export const login = async (req: any, res: any) => {
    try {
        const { password, email } = req.body;


        //find user
        const user = await User.findOne({ email });

        //no user = invalid credentials
        if (!user) {
            return res.status(401).json({
                message: "Invalid credentials"
            });
        }

        //compare against hash
        const match = await bcrypt.compare(password, user.password);
        if (!match) {
            return res.status(401).json({
                message: "Invalid credentials"
            });
        }
        //generate token
        const token = generateToken(user._id);

        //send cookie
        // res.cookie("token", token, {
        //     httpOnly: true,
        //     secure: process.env.NODE_ENV === "production",
        //     sameSite: "strict",
        //     //1 week
        //     maxAge: 7 * 24 * 60 * 60 * 1000
        // });

        res.json({
            message: "Successfully logged in",
            token:token
        });
    }
    catch (error) {
        console.error(error);

        return res.status(500).json({
            message: "Server error"
        });
    }
}
export const signup = async (req: any, res: any) => {
    try {
        const { username, password, email } = req.body;

        //check for existing username and email
        //let existingUser = await User.findOne({username:username}) || await User.findOne({email:email});
        const existingUser = await User.findOne({
            $or: [
                { username },
                { email }
            ]
        });

        if (existingUser) {
            return res.status(403).json({
                message: "Duplicate user"
            });
        }

        //generate password hash    
        const salt = await bcrypt.genSalt();
        const hashedPassword = await bcrypt.hash(password, salt);

        const newUser = await User.create({ username: username, email: email, password: hashedPassword });

        //generate token
        const token = generateToken(newUser._id);

        //send cookie
        res.cookie("token", token, {
            httpOnly: true,
            secure: process.env.NODE_ENV === "production",
            sameSite: "strict",
            //1 week
            maxAge: 7 * 24 * 60 * 60 * 1000
        });

        res.json({
            message: "Successfully signed up"
        });
    }
    catch (error) {
        console.error(error);

        return res.status(500).json({
            message: "Server error"
        });
    }



}