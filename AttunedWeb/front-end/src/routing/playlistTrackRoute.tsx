import { createRoute } from "@tanstack/react-router";
import { playlistRoute } from "@root/routing/playlistsRoute.tsx";

// Rendering is handled by the playlist route's component, which reads the
// trackId param.
export const playlistTrackRoute = createRoute({
  getParentRoute: () => playlistRoute,
  path: "$trackId",
});
