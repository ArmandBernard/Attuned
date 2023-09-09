import { TracksGrid } from "./TracksGrid";
import { useRouteQuery } from "../Queries/useRouteQuery.ts";
import { TrackDto } from "../dtos/Dtos.ts";
import { FunctionComponent, useState } from "react";

interface SortOrder {
  field: keyof TrackDto;
  direction: "ascending" | "descending";
}

export const TracksView: FunctionComponent<{
  title: string;
  idFilter: number[] | undefined;
}> = (props) => {
  const { data: tracks, isFetching } = useRouteQuery<TrackDto[]>({
    url: "track",
    refetchOnWindowFocus: false,
  });

  const [sortOrder, setSortOrder] = useState<SortOrder | undefined>();

  const filtered = tracks?.filter(x => props.idFilter ? props.idFilter.indexOf(x.Id) !== -1 : true);
  
  const sorted = sortOrder
    ? filtered?.sort((a, b) => sortComparer(a, b, sortOrder))
    : filtered;

  return (
    <div className="p-4 h-full flex flex-col gap-2">
      <div>
        <h1 className="text-2xl inline-block">{props.title}</h1>{" "}
        {isFetching && <span aria-hidden>Loading...</span>}
      </div>
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
