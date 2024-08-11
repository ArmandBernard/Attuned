import { createRoute } from "@tanstack/react-router";
import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { PlaylistDto } from "@dtos";
import { PlaylistsView } from "@views/playlists/PlaylistsView.tsx";
import { FullScreenLoading } from "@components/FullScreenLoading.tsx";
import { rootRoute } from "./rootRoute.tsx";

export const playlistRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "playlists/$playlistId",
  component: PlaylistsComponent,
});

function PlaylistsComponent() {
  const { playlistId } = playlistRoute.useParams();

  const { data, isLoading } = useRouteQuery<
    PlaylistDto,
    unknown,
    { id: number }
  >({
    url: "playlist/{id}",
    parameters: {
      id: parseInt(playlistId),
    },
    refetchOnWindowFocus: false,
  });

  return (
    <main className="h-full min-w-0 flex-1">
      {playlistId && data && (
        <PlaylistsView title={data.Name} playlist={data} />
      )}
      {!isLoading && !data && (
        <div className="flex-1 h-full flex items-center justify-center">
          Playlist not found
        </div>
      )}
      {isLoading && <FullScreenLoading />}
    </main>
  );
}
