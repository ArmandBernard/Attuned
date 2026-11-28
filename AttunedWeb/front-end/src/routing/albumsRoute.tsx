import { createRoute, lazyRouteComponent } from "@tanstack/react-router";
import { rootRoute } from "@root/routing/rootRoute.tsx";

// An album has no id, so the open album is identified by its name and album artist in the search params, which
// (unlike path params) can be left out when a value is missing.
export interface AlbumsSearch {
  album?: string;
  albumArtist?: string;
}

export const albumsRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/albums",
  validateSearch: (search: Record<string, unknown>): AlbumsSearch => ({
    album: typeof search.album === "string" ? search.album : undefined,
    albumArtist:
      typeof search.albumArtist === "string" ? search.albumArtist : undefined,
  }),
  component: lazyRouteComponent(
    () => import("@views/albums/AlbumsView.tsx"),
    "AlbumsView",
  ),
});
