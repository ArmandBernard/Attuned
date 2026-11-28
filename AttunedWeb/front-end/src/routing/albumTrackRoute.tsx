import { createRoute } from "@tanstack/react-router";
import { albumsRoute } from "@root/routing/albumsRoute.tsx";

// Rendering is handled by AlbumsView, which reads the trackId param. The open album stays in the search params.
export const albumTrackRoute = createRoute({
  getParentRoute: () => albumsRoute,
  path: "$trackId",
});
