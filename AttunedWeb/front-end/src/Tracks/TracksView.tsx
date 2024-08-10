import { TracksGrid } from "./TracksGrid";
import { useRouteQuery } from "../Queries/useRouteQuery.ts";
import { PlaylistDto, TrackDto } from "../dtos/Dtos.ts";
import { FunctionComponent, useMemo, useState } from "react";
import { getDurationString } from "../StringFormatters/getDurationString.ts";
import { TimeSpan } from "../dtos/TimeSpan.ts";
import { PlaylistDetails } from "../Playlists/PlaylistDetails.tsx";
import { TrackDetails } from "./TrackDetails.tsx";

interface SortOrder {
  field: keyof TrackDto;
  direction: "ascending" | "descending";
}

export const TracksView: FunctionComponent<{
  title: string;
  playlist: PlaylistDto | undefined;
}> = ({ playlist, title }) => {
  const [showPlaylistDetails, setShowPlaylistDetails] =
    useState<boolean>(false);
  const [trackToShow, setTrackToShow] = useState<TrackDto | undefined>();
  const [sortOrder, setSortOrder] = useState<SortOrder | undefined>();

  const { data: tracks, isFetching } = useRouteQuery<TrackDto[]>({
    url: "track",
    refetchOnWindowFocus: false,
  });

  const itemSet = useMemo(() => new Set(playlist?.Items), [playlist]);

  let filtered: TrackDto[] | undefined;

  if (playlist === undefined) {
    filtered = tracks;
  } else {
    filtered = tracks?.filter((x) => itemSet.has(x.Id));
  }

  const sorted = useMemo(
    () =>
      sortOrder
        ? filtered?.sort((a, b) => sortComparer(a, b, sortOrder))
        : filtered,
    [filtered, sortOrder],
  );

  const trackIndex = useMemo(
    () => trackToShow && sorted?.map((x) => x.Id).indexOf(trackToShow.Id),
    [sorted, trackToShow],
  );

  const navigatePrevious =
    sorted && trackIndex !== undefined && trackIndex > 0
      ? () => setTrackToShow(sorted[trackIndex - 1])
      : undefined;

  const navigateNext =
    sorted && trackIndex !== undefined && trackIndex < sorted.length - 1
      ? () => setTrackToShow(sorted[trackIndex + 1])
      : undefined;

  return (
    <div className="p-4 h-full flex flex-col gap-2">
      {playlist && (
        <PlaylistDetails
          playlist={playlist}
          show={showPlaylistDetails}
          onClose={() => setShowPlaylistDetails(false)}
        />
      )}
      <div>
        <h1 className="text-2xl inline-block">{title}</h1>{" "}
        {isFetching && <span aria-hidden>Loading...</span>}
      </div>
      {filtered && (
        <div className="flex gap-4">
          <span>
            {filtered.length} tracks •{" "}
            {getDurationString(
              filtered
                .map((x) => x.TotalTime)
                .reduce((agg, curr) => agg + (curr ?? 0), 0) as TimeSpan,
            )}
          </span>
          {playlist?.IsSmart && (
            <button
              className="inline-block text-primary"
              onClick={() => setShowPlaylistDetails(true)}
            >
              View rules
            </button>
          )}
        </div>
      )}
      <div
        className="flex-1 overflow-scroll"
        aria-busy={isFetching}
        aria-live="polite"
      >
        <TracksGrid
          tracks={sorted}
          sortOrder={sortOrder}
          setSortOrder={(field) => setSortOrder(field)}
          setTrackToShow={setTrackToShow}
        />
      </div>
      <TrackDetails
        show={trackToShow !== undefined}
        onPreviousTrack={navigatePrevious}
        onNextTrack={navigateNext}
        onClose={() => setTrackToShow(undefined)}
        track={trackToShow}
      />
    </div>
  );
};

const sortComparer = (
  trackA: TrackDto,
  trackB: TrackDto,
  order: SortOrder,
): number => {
  let swap: number;

  const propertyA = trackA[order.field];

  const propertyB = trackB[order.field];

  if (propertyA === undefined && propertyB === undefined) {
    swap = 0;
  } else if (propertyA === undefined) {
    swap = -1;
  } else if (propertyB === undefined) {
    swap = 1;
  } else if (typeof propertyA === "string" && typeof propertyB === "string") {
    swap = propertyA.localeCompare(propertyB);
  } else {
    swap = propertyA > propertyB ? 1 : propertyA < propertyB ? -1 : 0;
  }

  return order.direction === "ascending" ? swap : -1 * swap;
};
