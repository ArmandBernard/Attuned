import { TrackDto } from "../dtos/Dtos.ts";
import { TextCell } from "./Cells/TextCell.tsx";
import { RatingCell } from "./Cells/RatingCell.tsx";
import { CellComponent } from "./Cells/CellComponent.ts";

type CellTypes =
  | "string"
  | "number"
  | "date"
  | "rating"
  | "loved"
  | "none"
  | "size"
  | "time";

const FieldCellTypeDictionary: Record<keyof TrackDto, CellTypes> = {
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

const CellTypeElementDictionary: Record<
  CellTypes,
  CellComponent<string> | CellComponent<number>
> = {
  date: TextCell,
  loved: TextCell,
  none: TextCell,
  number: TextCell,
  rating: RatingCell,
  size: TextCell,
  string: TextCell,
  time: TextCell,
};

export const GetCellElement = (field: keyof TrackDto) => {
  return CellTypeElementDictionary[FieldCellTypeDictionary[field]];
};
