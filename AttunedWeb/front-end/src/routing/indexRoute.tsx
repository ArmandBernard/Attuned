import { createRoute } from "@tanstack/react-router";
import { rootRoute } from "./rootRoute.tsx";
import { PlaylistsView } from "@views/playlists/PlaylistsView.tsx";

export const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/",
  component: () => (
    <main className="h-full min-w-0 flex-1">
      <PlaylistsView title="All tracks" playlist={undefined} />
    </main>
  ),
});
