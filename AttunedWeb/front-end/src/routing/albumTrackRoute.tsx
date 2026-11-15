import { createRoute } from "@tanstack/react-router";
import { albumRoute } from "@root/routing/albumRoute.tsx";

// Rendering is handled by AlbumsView, which reads the trackId param.
export const albumTrackRoute = createRoute({
  getParentRoute: () => albumRoute,
  path: "$trackId",
});
