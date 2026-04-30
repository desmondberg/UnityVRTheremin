

export type User = {
    username: String;
    email: String;
    password: String;
    presets?: String[];
}

export type Preset = {
    title: String,
    description?:String,
    author: User,
    tags?: String[],
    upvotes: Number,
    downvotes: Number,
    comments?: {
        user: User,
        content: String,
    }[],
    waveform: String[],
    scale?: String[]
}