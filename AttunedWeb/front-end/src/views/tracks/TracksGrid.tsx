import { Dispatch, Fragment, FunctionComponent, SetStateAction } from "react";
import { TrackDto } from "@dtos";
import { GetCellElement } from "./GetCellElement.ts";
import { CellComponent } from "./cells/CellComponent.ts";
import { trackFieldNameDictionary } from "./trackFieldNameDictionary.ts";
import { FieldCellTypeDictionary } from "./FieldTypeDictionary.ts";
import { CellTypes } from "./CellTypes.ts";

interface SortOrder {
  field: keyof TrackDto;
  direction: "ascending" | "descending";
}

interface TracksGridProps {
  tracks: TrackDto[] | undefined;
  sortOrder: SortOrder | undefined;
  setSortOrder: Dispatch<SetStateAction<SortOrder | undefined>>;
  setTrackToShow: Dispatch<SetStateAction<TrackDto | undefined>>;
}

const RightAlignedFields: Record<CellTypes, boolean> = {
  date: false,
  loved: false,
  none: false,
  number: true,
  rating: false,
  size: true,
  string: false,
  time: true,
};

export const TracksGrid: FunctionComponent<TracksGridProps> = (props) => {
  const fieldsToShow: (keyof TrackDto)[] = [
    "Name",
    "Artist",
    "Album",
    "TotalTime",
    "Rating",
    "Loved",
    "Genre",
    "PlayCount",
    "DateAdded",
  ];

  const setSortFieldOrToggleSortDirection = (clickedField: keyof TrackDto) =>
    props.setSortOrder((value) => ({
      field: clickedField,
      direction:
        value && value.field === clickedField
          ? value.direction === "ascending"
            ? "descending"
            : "ascending"
          : "ascending",
    }));

  return (
    <table
      aria-label="Tracks"
      style={{
        gridTemplateColumns: "auto " + fieldsToShow.map(() => "auto").join(" "),
      }}
      className="grid min-w-[72rem]"
    >
      <thead className="contents">
        <tr className="contents">
          <th
            className="border flex sticky top-0 bg-background"
            aria-label="Track actions"
          ></th>
          {fieldsToShow.map((field) => (
            <th
              className="border flex sticky top-0 bg-background"
              aria-sort={
                props.sortOrder && props.sortOrder.field === field
                  ? props.sortOrder.direction
                  : undefined
              }
              key={field}
            >
              <button
                aria-label={`sort by ${trackFieldNameDictionary[field]}`}
                className="flex flex-1 px-2 gap-2"
                onClick={() => setSortFieldOrToggleSortDirection(field)}
              >
                <span
                  className={[
                    "flex flex-1",
                    RightAlignedFields[FieldCellTypeDictionary[field]] &&
                      "justify-end",
                  ]
                    .filter(Boolean)
                    .join(" ")}
                >
                  {trackFieldNameDictionary[field]}
                </span>
                <span aria-hidden>
                  {props.sortOrder &&
                    props.sortOrder.field === field &&
                    (props.sortOrder.direction === "ascending" ? "˄" : "˅")}
                </span>
              </button>
            </th>
          ))}
        </tr>
      </thead>
      <tbody className="contents">
        {props.tracks &&
          props.tracks.map((track) => (
            <tr
              key={track.Id}
              className="contents [&>td]:odd:bg-alternating-row"
            >
              <td>
                <button
                  className="px-2 text-primary font-bold"
                  aria-label="View details"
                  title="View details"
                  onClick={() => props.setTrackToShow(track)}
                >
                  🗎
                </button>
              </td>
              {fieldsToShow.map((field) => (
                <Fragment key={field}>
                  {(GetCellElement(field) as CellComponent<unknown>)({
                    value: track[field],
                  })}
                </Fragment>
              ))}
            </tr>
          ))}
      </tbody>
    </table>
  );
};
