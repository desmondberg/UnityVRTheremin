import jwt from "jsonwebtoken";
import User from "../models/User";
import type { JwtPayload } from "jsonwebtoken";

interface TokenPayload extends JwtPayload {
    userId: string;
}

// export const protectRoute = async (req:any, res:any, next:any) => {
//   try {
    
//     const token = req.cookies.token;
//     const secret = process.env.JWT_SECRET;

//     if(!secret){
//         return res.status(500).json({
//         message: "JWT secret not found"
//       });
//     }
//     if (!token) {
//       return res.status(401).json({
//         message: "Not authorised"
//       });
//     }
    

//     //get decoded user id
//     const decoded = jwt.verify(
//       token,
//       secret
//     ) as TokenPayload;


//     const user = await User.findById(
//       decoded.userId
//     ).select("-password");

//     if (!user) {
//       return res.status(401).json({
//         message: "User not found"
//       });
//     }
    
//     //attach authenticated user
//     req.user = user;

//     next();

//   } catch (error) {
//     return res.status(401).json({
//       message: "Invalid token"
//     });
//   }
// };

export const protectRoute = async (req: any,res: any,next: any) => {
  try {

    const authHeader =
      req.headers.authorization;

    if (!authHeader) {
      return res.status(401).json({
        message: "No token provided"
      });
    }

    //extract token
    const token =
      authHeader.split(" ")[1];

    if (!token) {
      return res.status(401).json({
        message: "Invalid token format"
      });
    }

    const secret = process.env.JWT_SECRET;

    if (!secret) {
      return res.status(500).json({
        message: "JWT secret missing"
      });
    }

    const decoded = jwt.verify(
      token,
      secret
    ) as TokenPayload;

    const user = await User.findById(
      decoded.userId
    ).select("-password");

    if (!user) {
      return res.status(401).json({
        message: "User not found"
      });
    }

    req.user = user;

    next();

  } catch (error) {

    return res.status(401).json({
      message: "Invalid token"
    });

  }
};