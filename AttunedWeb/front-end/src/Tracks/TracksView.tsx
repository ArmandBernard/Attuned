import { TracksGrid } from "./TracksGrid";
import { useRouteQuery } from "../Queries/useRouteQuery.ts";
import { TrackDto } from "../dtos/Dtos.ts";

export const TracksView = () => {
  const { data, isFetching } = useRouteQuery<TrackDto[]>({
    url: "track",
    refetchOnWindowFocus: false,
  });

  return (
    <>
      {isFetching && <>Loading...</>}
      <TracksGrid tracks={data} />
    </>
  );
};
