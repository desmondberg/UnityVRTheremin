import 'dotenv/config';
import express from 'express';
import mongoose from 'mongoose';
import {connect} from './db';
import cors from "cors";

import userRouter from './routes/user';
import presetRouter from './routes/preset';

const app = express();
const port = 3000;



app.use(cors());


app.get('/', (req, res)=>
{
     res.send('Welcome to the VR Theremin backend!');
});

app.use("/api/users", userRouter);
app.use("/api/presets", presetRouter);



 app.listen(port, ()=>{
     console.log(
`Connected successfully on port ${port}`)
});

//connect to mongodb
connect();
