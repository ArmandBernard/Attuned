import { TrackDto } from "@dtos";
import { TextCell } from "./cells/TextCell.tsx";
import { RatingCell } from "./cells/RatingCell.tsx";
import { CellComponent } from "./cells/CellComponent.ts";
import { TimeSpanCell } from "./cells/TimeSpanCell.tsx";
import { TimeSpan } from "@root/dtos/TimeSpan.ts";
import { NumberCell } from "./cells/NumberCell.tsx";
import { DateTimeCell } from "./cells/DateTimeCell.tsx";
import { LoveCell } from "./cells/LoveCell.tsx";
import { CellTypes } from "./CellTypes.ts";
import { FieldCellTypeDictionary } from "./FieldTypeDictionary.ts";
import { Rating } from "@root/dtos/Rating.ts";

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
