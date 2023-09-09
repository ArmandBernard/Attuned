import { FunctionComponent } from "react";
import { TrackDto } from "../dtos/Dtos.ts";
import { GetCellElement } from "./GetCellElement.ts";
import { CellComponent } from "./Cells/CellComponent.ts";

interface TracksGridProps {
  tracks: TrackDto[] | undefined;
}

export const TracksGrid: FunctionComponent<TracksGridProps> = (props) => {
  const fieldsToShow: (keyof TrackDto)[] = [
    "Name",
    "Artist",
    "Album",
    "Rating",
  ];

  return (
    <table
      style={{ gridTemplateColumns: fieldsToShow.map(() => "auto").join(" ") }}
      className="grid"
    >
      <thead className="contents">
        <tr className="contents">
          {fieldsToShow.map((field) => (
            <th className="border px-2" key={field}>
              {field}
            </th>
          ))}
        </tr>
      </thead>
      <tbody className="contents">
        {props.tracks &&
          props.tracks.map((track) => (
            <tr
              key={track.Id}
              className="contents [&>td]:dark:odd:bg-neutral-700 [&>td]:odd:bg-neutral-100"
            >
              {fieldsToShow.map((field) => {
                return (GetCellElement(field) as CellComponent<unknown>)({
                  key: field,
                  value: track[field],
                });
              })}
            </tr>
          ))}
      </tbody>
    </table>
  );
};
