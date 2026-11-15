import { createRoute, lazyRouteComponent } from "@tanstack/react-router";
import { rootRoute } from "./rootRoute.tsx";

export const playlistRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "playlists/$playlistId",
  component: lazyRouteComponent(
    () => import("@views/playlists/PlaylistRouteView.tsx"),
    "PlaylistRouteView",
  ),
});
