import { createRoute } from "@tanstack/react-router";
import { albumsRoute } from "@root/routing/albumsRoute.tsx";

// Rendering is handled by AlbumsView, which reads the params. The route exists
// so that /albums/<albumArtist>/<albumName> is a valid, deep-linkable URL.
export const albumRoute = createRoute({
  getParentRoute: () => albumsRoute,
  path: "$albumArtist/$album",
});
