import { z } from "zod";

export const presetSchema = z.object({
  title: z.string(),
  description: z.string(),
  author: z.string(),
  tags: z.array(z.string()).optional(),
  upvotes: z.number().optional(),
  downvotes: z.number().optional(),
  comments: z.array(
    z.object({
      user: z.string(),
      content: z.string()
    })
  ).optional(),
  waveform: z.string().optional(),
  scale: z.string().optional()
});