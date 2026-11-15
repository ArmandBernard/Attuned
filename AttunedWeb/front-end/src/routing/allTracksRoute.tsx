import { createRoute, lazyRouteComponent } from "@tanstack/react-router";
import { rootRoute } from "./rootRoute.tsx";

export const allTracksRoute = createRoute({
  getParentRoute: () => rootRoute,
  id: "allTracks",
  component: lazyRouteComponent(
    () => import("@views/playlists/AllTracksRouteView.tsx"),
    "AllTracksRouteView",
  ),
});
