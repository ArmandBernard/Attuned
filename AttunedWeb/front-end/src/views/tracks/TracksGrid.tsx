import { Dispatch, memo, SetStateAction, useMemo, useRef } from "react";
import { useVirtualizer } from "@tanstack/react-virtual";
import { TrackDto } from "@dtos";
import { GetCellElement } from "./GetCellElement.ts";
import { CellComponent } from "./cells/CellComponent.ts";
import { trackFieldNameDictionary } from "./trackFieldNameDictionary.ts";
import { FieldCellTypeDictionary } from "./FieldTypeDictionary.ts";
import { CellTypes } from "./CellTypes.ts";
import ChevronUpIcon from "@components/icons/ChevronUpIcon.tsx";
import ChevronDownIcon from "@components/icons/ChevronDownIcon.tsx";
import { TextCell } from "@views/tracks/cells/TextCell.tsx";
import { useNavigate, useParams } from "@tanstack/react-router";
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

// Rows are virtualized, so heights and column widths need to be fixed to
// avoid shifting.
const ROW_HEIGHT = 28;
const HEADER_HEIGHT = 32;
// The columns to show, in order, with their width as a CSS grid track size.
const columnWidths = {
  Name: "minmax(200px, 2fr)",
  Artist: "minmax(150px, 1fr)",
  Album: "minmax(150px, 1.5fr)",
  TotalTime: "4rem",
  Rating: "7.25rem",
  Loved: "4rem",
  Genre: "minmax(50px, 1fr)",
  PlayCount: "5rem",
  DateAdded: "11rem",
} as const satisfies Partial<Record<keyof TrackDto, string>>;

const fieldsToShow = Object.keys(columnWidths) as (keyof typeof columnWidths)[];

const gridTemplateColumns = Object.values(columnWidths).join(" ");

const TrackRow = memo(function TrackRow({
  track,
  index,
  start,
  playlistId,
}: {
  track: TrackDto;
  index: number;
  start: number;
  // undefined on the "All tracks" view
  playlistId: string | undefined;
}) {
  return (
    <tr
      aria-rowindex={index + 2} // 1-based, after the header row
      className={
        "absolute left-0 top-0 w-full grid items-center " +
        (index % 2 === 1 ? "[&>td]:bg-alternating-row bg-alternating-row" : "")
      }
      style={{
        gridTemplateColumns,
        height: ROW_HEIGHT,
        transform: `translateY(${start}px)`,
      }}
    >
      {fieldsToShow.map((field) => {
        if (field === "Name") {
          return (
            <TextCell
              key={field}
              value={
                playlistId !== undefined ? (
                  <RouteLink
                    className="max-w-full truncate"
                    to="/playlists/$playlistId/$trackId"
                    params={{ playlistId, trackId: String(track.Id) }}
                  >
                    {track[field]}
                  </RouteLink>
                ) : (
                  <RouteLink
                    className="max-w-full truncate"
                    to="/tracks/$trackId"
                    params={{ trackId: String(track.Id) }}
                  >
                    {track[field]}
                  </RouteLink>
                )
              }
            />
          );
        }
        const Cell = GetCellElement(field) as CellComponent<unknown>;
        return <Cell key={field} value={track[field]} />;
      })}
    </tr>
  );
});

export function TracksGrid({
  tracks,
  sortOrder,
  setSortOrder,
}: TracksGridProps) {
  const navigate = useNavigate();

  const playlistParams = useParams({
    from: "/playlists/$playlistId",
    shouldThrow: false,
  });
  const playlistTrackParams = useParams({
    from: "/playlists/$playlistId/$trackId",
    shouldThrow: false,
  });
  const allTracksTrackParams = useParams({
    from: "/allTracks/tracks/$trackId",
    shouldThrow: false,
  });

  const params = playlistTrackParams ?? allTracksTrackParams ?? playlistParams;

  const trackId =
    params && "trackId" in params ? parseInt(params.trackId) : undefined;

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

  const scrollRef = useRef<HTMLDivElement>(null);

  // Safe as we don't pass these values to memoized components
  // eslint-disable-next-line react-hooks/incompatible-library
  const virtualizer = useVirtualizer({
    count: tracks?.length ?? 0,
    getScrollElement: () => scrollRef.current,
    estimateSize: () => ROW_HEIGHT,
    getItemKey: (index) => tracks?.[index]?.Id ?? index,
    // the header is above the rows inside the same scroll element
    scrollMargin: HEADER_HEIGHT,
    overscan: 20, // good balance of not too much flashing when rapidly scrolling and not too many items loaded at once.
  });

  const playlistId =
    params && "playlistId" in params ? params.playlistId : undefined;

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
      <div ref={scrollRef} className="h-full overflow-scroll">
        <table
          aria-label="Tracks"
          aria-rowcount={(tracks?.length ?? 0) + 1}
          className="grid min-w-[80rem]"
        >
          <thead className="sticky top-0 z-10 grid">
            <tr
              className="grid"
              style={{ gridTemplateColumns, height: HEADER_HEIGHT }}
            >
              {fieldsToShow.map((field) => (
                <th
                  className="border flex bg-background box-border"
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
          <tbody
            className="grid relative"
            style={{
              height:
                virtualizer.getTotalSize() - virtualizer.options.scrollMargin,
            }}
          >
            {tracks &&
              virtualizer
                .getVirtualItems()
                .map((item) => (
                  <TrackRow
                    key={item.key}
                    track={tracks[item.index]}
                    index={item.index}
                    start={item.start - virtualizer.options.scrollMargin}
                    playlistId={playlistId}
                  />
                ))}
          </tbody>
        </table>
      </div>
      <TrackDetails
        previousTrack={previousTrack}
        nextTrack={nextTrack}
        track={currentTrack}
        onClose={() =>
          void navigate({
            // "/tracks/$trackId" has no "/tracks" parent route to go ".." to
            to: params && "playlistId" in params ? ".." : "/",
          })
        }
      />
    </>
  );
}
