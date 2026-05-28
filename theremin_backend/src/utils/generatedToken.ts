import jwt from "jsonwebtoken";

export const generateToken = (userId: any) => {
    const secret = process.env.JWT_SECRET;
    if(!secret){
        return {error:"No JWT string"}
    }

    return jwt.sign(
        { userId },
        secret,
        {
            expiresIn: "7d"
        }
    );
};