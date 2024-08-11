import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { PlaylistDetailsDto, PlaylistDto, TrackDto } from "@dtos";
import { FunctionComponent, useMemo, useState } from "react";
import { getDurationString } from "@utils/stringFormatters/getDurationString.ts";
import { TimeSpan } from "@root/dtos/TimeSpan.ts";
import { PlaylistDetails } from "@views/playlists/PlaylistDetails.tsx";
import { TrackDetails } from "@views/tracks/TrackDetails.tsx";
import { TracksGrid } from "@views/tracks/TracksGrid";

interface SortOrder {
  field: keyof TrackDto;
  direction: "ascending" | "descending";
}

export const PlaylistsView: FunctionComponent<{
  title: string;
  playlist: PlaylistDto | undefined;
}> = ({ playlist, title }) => {
  const [showPlaylistDetails, setShowPlaylistDetails] =
    useState<boolean>(false);
  const [trackToShow, setTrackToShow] = useState<TrackDto | undefined>();
  const [sortOrder, setSortOrder] = useState<SortOrder | undefined>();

  const { data: playlistDetails, isFetching: isFetchingPlaylistDetails } =
    useRouteQuery<PlaylistDetailsDto, unknown, { id: number }>({
      url: "playlist/{id}",
      enabled: playlist !== undefined,
      parameters: {
        id: playlist?.Id as number,
      },
    });

  const { data: tracks, isFetching: isFetchingTracks } = useRouteQuery<
    TrackDto[]
  >({
    url: "track",
    refetchOnWindowFocus: false,
  });

  const isFetching = isFetchingPlaylistDetails || isFetchingTracks;

  const itemSet = useMemo(
    () => new Set(playlistDetails?.Items),
    [playlistDetails?.Items],
  );

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
      {playlistDetails && (
        <PlaylistDetails
          playlist={playlistDetails}
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
