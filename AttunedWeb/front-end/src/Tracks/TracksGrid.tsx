import { FunctionComponent } from "react";
import { TrackDto } from "../dtos/Dtos.ts";
import {TextCell} from "./Cells/TextCell.tsx";

interface TracksGridProps {
  tracks: TrackDto[] | undefined;
}

export const TracksGrid: FunctionComponent<TracksGridProps> = (props) => {
  const fieldsToShow: (keyof TrackDto)[] = ["Name", "Artist", "Album"];

  return (
    <table>
      <thead>
        <tr>
          {fieldsToShow.map((field) => (
            <th className="border px-2" key={field}>{field}</th>
          ))}
        </tr>
      </thead>
      <tbody>
        {props.tracks &&
          props.tracks.map((track) => (
            <tr className="dark:odd:bg-neutral-700 odd:bg-neutral-100">
              {fieldsToShow.map((field) => (
                <TextCell key={track[field]}>{track[field]}</TextCell>
              ))}
            </tr>
          ))}
      </tbody>
    </table>
  );
};
