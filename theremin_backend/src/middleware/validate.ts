import { z } from "zod";

const validate = (schema:z.ZodObject<any>) => (req:any, res:any, next:any) => {
  const result = schema.safeParse(req.body);

  if (!result.success) {
    return res.status(400).json(result.error);
  }

  req.body = result.data;
  next();
};

export default validate;