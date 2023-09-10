import { TracksGrid } from "./TracksGrid";
import { useRouteQuery } from "../Queries/useRouteQuery.ts";
import { PlaylistDto, TrackDto } from "../dtos/Dtos.ts";
import { FunctionComponent, useState } from "react";
import { getDurationString } from "../getDurationString.ts";
import { TimeSpan } from "../dtos/TimeSpan.ts";
import { PlaylistDetails } from "../PlaylistDetails.tsx";

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
  const [sortOrder, setSortOrder] = useState<SortOrder | undefined>();

  const { data: tracks, isFetching } = useRouteQuery<TrackDto[]>({
    url: "track",
    refetchOnWindowFocus: false,
  });

  const filtered = tracks?.filter((x) =>
    playlist?.Items ? playlist.Items.indexOf(x.Id) !== -1 : true,
  );

  const sorted = sortOrder
    ? filtered?.sort((a, b) => sortComparer(a, b, sortOrder))
    : filtered;

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
        />
      </div>
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
