import { TracksGrid } from "./TracksGrid";
import { useRouteQuery } from "../Queries/useRouteQuery.ts";
import { TrackDto } from "../dtos/Dtos.ts";

export const TracksView = () => {
  const { data } = useRouteQuery<TrackDto[]>({ url: "track" });

  return <TracksGrid tracks={data} />;
};
