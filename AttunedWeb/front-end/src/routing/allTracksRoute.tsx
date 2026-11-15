import { createRoute } from "@tanstack/react-router";
import { rootRoute } from "./rootRoute.tsx";
import { PlaylistsView } from "@views/playlists/PlaylistsView.tsx";

export const allTracksRoute = createRoute({
  getParentRoute: () => rootRoute,
  id: "allTracks",
  component: () => (
    <main className="h-full min-w-0 flex-1">
      <PlaylistsView title="All tracks" playlist={undefined} />
    </main>
  ),
});
