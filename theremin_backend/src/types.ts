import type { Types } from "mongoose";

export type Vote = {
    user: Types.ObjectId;
    value: 1 | -1;
};