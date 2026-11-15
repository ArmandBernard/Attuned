import { Dispatch, SetStateAction, useMemo } from "react";
import { TrackDto } from "@dtos";
import { GetCellElement } from "./GetCellElement.ts";
import { CellComponent } from "./cells/CellComponent.ts";
import { trackFieldNameDictionary } from "./trackFieldNameDictionary.ts";
import { FieldCellTypeDictionary } from "./FieldTypeDictionary.ts";
import { CellTypes } from "./CellTypes.ts";
import ChevronUpIcon from "@components/icons/ChevronUpIcon.tsx";
import ChevronDownIcon from "@components/icons/ChevronDownIcon.tsx";
import { TextCell } from "@views/tracks/cells/TextCell.tsx";
import { useParams } from "@tanstack/react-router";
import RouteLink from "@components/RouteLink.tsx";
import { TrackDetails } from "@views/tracks/TrackDetails.tsx";

interface SortOrder {
  field: keyof TrackDto;
  direction: "ascending" | "descending";
}

interface TracksGridProps {
  tracks: TrackDto[] | undefined;
  sortOrder: SortOrder | undefined;
  setSortOrder: Dispatch<SetStateAction<SortOrder | undefined>>;
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

export function TracksGrid({
  tracks,
  sortOrder,
  setSortOrder,
}: TracksGridProps) {
  const { playlistId } = useParams({
    from: "/playlists/$playlistId",
  });
  const trackParams = useParams({
    from: "/playlists/$playlistId/$trackId",
    shouldThrow: false,
  });

  const trackId = trackParams ? parseInt(trackParams.trackId) : undefined;

  const trackIndex = useMemo(
    () =>
      trackId === undefined
        ? undefined
        : tracks?.findIndex((x) => x.Id === trackId),
    [tracks, trackId],
  );

  const currentTrack =
    tracks && trackIndex !== undefined && trackIndex >= 0
      ? tracks[trackIndex]
      : undefined;
  const previousTrack =
    tracks && trackIndex !== undefined && trackIndex > 0
      ? tracks[trackIndex - 1]
      : undefined;
  const nextTrack =
    tracks && trackIndex !== undefined && trackIndex < tracks.length - 1
      ? tracks[trackIndex + 1]
      : undefined;

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
    setSortOrder((value) => ({
      field: clickedField,
      direction:
        value && value.field === clickedField
          ? value.direction === "ascending"
            ? "descending"
            : "ascending"
          : "ascending",
    }));

  return (
    <>
      <table
        aria-label="Tracks"
        style={{
          gridTemplateColumns: fieldsToShow.map(() => "auto").join(" "),
        }}
        className="grid min-w-[72rem]"
      >
        <thead className="contents">
          <tr className="contents">
            {fieldsToShow.map((field) => (
              <th
                className="border flex sticky top-0 bg-background"
                aria-sort={
                  sortOrder && sortOrder.field === field
                    ? sortOrder.direction
                    : undefined
                }
                key={field}
              >
                <button
                  aria-label={`sort by ${trackFieldNameDictionary[field]}`}
                  className="flex flex-1 items-center pl-2 gap-2"
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
                  <span
                    className="fill-text-color h-6 w-6 py-1 px-1"
                    aria-hidden
                  >
                    {sortOrder &&
                      sortOrder.field === field &&
                      (sortOrder.direction === "ascending" ? (
                        <ChevronUpIcon />
                      ) : (
                        <ChevronDownIcon />
                      ))}
                  </span>
                </button>
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="contents">
          {tracks?.map((track) => (
            <tr
              key={track.Id}
              className="contents [&>td]:odd:bg-alternating-row"
            >
              {fieldsToShow.map((field) => {
                if (field === "Name") {
                  return (
                    <TextCell
                      key={field}
                      value={
                        <RouteLink
                          className="max-w-full truncate"
                          to="/playlists/$playlistId/$trackId"
                          params={{ playlistId, trackId: String(track.Id) }}
                        >
                          {track[field]}
                        </RouteLink>
                      }
                    />
                  );
                }
                const Cell = GetCellElement(field) as CellComponent<unknown>;
                return <Cell key={field} value={track[field]} />;
              })}
            </tr>
          ))}
        </tbody>
      </table>
      <TrackDetails
        previousTrack={previousTrack}
        nextTrack={nextTrack}
        track={currentTrack}
      />
    </>
  );
}
