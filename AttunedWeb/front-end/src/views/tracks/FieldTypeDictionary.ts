import { TrackDto } from "@dtos";
import { CellTypes } from "./CellTypes.ts";

export const FieldCellTypeDictionary: Record<keyof TrackDto, CellTypes> = {
  Album: "string",
  Artist: "string",
  BitRate: "number",
  Bpm: "number",
  Composer: "string",
  DateAdded: "date",
  DateModified: "date",
  DiscCount: "number",
  DiscNumber: "number",
  Genre: "string",
  Id: "number",
  Location: "none",
  Loved: "loved",
  Media: "none",
  Name: "string",
  PlayCount: "number",
  PlayDate: "date",
  Rating: "rating",
  SampleRate: "none",
  Size: "size",
  SkipCount: "number",
  TotalTime: "time",
  TrackCount: "number",
  TrackNumber: "number",
  Year: "number",
};
