import { TrackDto } from "../dtos/Dtos.ts";
import { TextCell } from "./Cells/TextCell.tsx";
import { RatingCell } from "./Cells/RatingCell.tsx";
import { CellComponent } from "./Cells/CellComponent.ts";
import { TimeSpanCell } from "./Cells/TimeSpanCell.tsx";
import { TimeSpan } from "../dtos/TimeSpan.ts";
import { NumberCell } from "./Cells/NumberCell.tsx";
import { DateTimeCell } from "./Cells/DateTimeCell.tsx";
import { CellTypes } from "./CellTypes.ts";
import { FieldCellTypeDictionary } from "./FieldTypeDictionary.ts";
import { Rating } from "../dtos/Rating.ts";
import { LoveCell } from "./Cells/LoveCell.tsx";

const CellTypeElementDictionary: Record<
  CellTypes,
  | CellComponent<string | undefined>
  | CellComponent<Rating>
  | CellComponent<number | undefined>
  | CellComponent<boolean | undefined>
  | CellComponent<TimeSpan | undefined>
> = {
  date: DateTimeCell,
  loved: LoveCell,
  none: TextCell,
  number: NumberCell,
  rating: RatingCell,
  size: TextCell,
  string: TextCell,
  time: TimeSpanCell,
};

export const GetCellElement = (field: keyof TrackDto) => {
  return CellTypeElementDictionary[FieldCellTypeDictionary[field]];
};
