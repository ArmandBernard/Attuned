import { Rating } from "@root/dtos/Rating.ts";

export const getRatingString = (value: Rating) =>
  `${Array(value).fill("★").join("")}${Array(5 - value)
    .fill("☆")
    .join("")}`;
