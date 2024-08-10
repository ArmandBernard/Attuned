import {
  Dispatch,
  Fragment,
  FunctionComponent,
  RefObject,
  SetStateAction,
  useState,
} from "react";
import { TrackDto } from "../dtos/Dtos.ts";
import { GetCellElement } from "./GetCellElement.ts";
import { CellComponent } from "./Cells/CellComponent.ts";
import { trackFieldNameDictionary } from "./trackFieldNameDictionary.ts";
import { FieldCellTypeDictionary } from "./FieldTypeDictionary.ts";
import { CellTypes } from "./CellTypes.ts";
import useObserve from "../utils/useObserve.ts";

interface SortOrder {
  field: keyof TrackDto;
  direction: "ascending" | "descending";
}

interface TracksGridProps {
  tracks: TrackDto[] | undefined;
  sortOrder: SortOrder | undefined;
  setSortOrder: Dispatch<SetStateAction<SortOrder | undefined>>;
  setTrackToShow: Dispatch<SetStateAction<TrackDto | undefined>>;
  containerRef: RefObject<Element>;
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

export const TracksGrid: FunctionComponent<TracksGridProps> = (props) => {
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
              key={field}
              className="border flex sticky top-0 bg-background"
              aria-sort={
                props.sortOrder && props.sortOrder.field === field
                  ? props.sortOrder.direction
                  : undefined
              }
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
            <RowElement
              key={track.Id}
              track={track}
              setTrackToShow={props.setTrackToShow}
              containerRef={props.containerRef}
            />
          ))}
      </tbody>
    </table>
  );
};

function RowElement({
  track,
  setTrackToShow,
  containerRef,
}: {
  track: TrackDto;
  setTrackToShow: Dispatch<SetStateAction<TrackDto | undefined>>;
  containerRef: RefObject<Element>;
}) {
  const [isVisible, setIsVisible] = useState(false);

  const callback = (isIntersecting: boolean) => {
    setIsVisible(isIntersecting);
  };

  const ref = useObserve({
    callback,
    threshold: 0.1,
    rootMargin: "1200px", // add some margin to reduce pop-in
    containerRef,
  });

  return (
    <tr className="contents [&>td]:odd:bg-alternating-row">
      <td ref={ref}>
        <button
          className="px-2 text-primary font-bold"
          aria-label="View details"
          title="View details"
          onClick={() => setTrackToShow(track)}
        >
          🗎
        </button>
      </td>
      {fieldsToShow.map((field) => {
        return isVisible ? (
          <Fragment key={field}>
            {(GetCellElement(field) as CellComponent<unknown>)({
              value: track[field],
            })}
          </Fragment>
        ) : (
          <td key={field} />
        );
      })}
    </tr>
  );
}
