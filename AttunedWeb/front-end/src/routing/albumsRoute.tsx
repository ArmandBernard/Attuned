import { createRoute } from "@tanstack/react-router";
import { rootRoute } from "@root/routing/rootRoute.tsx";
import { AlbumsView } from "@views/albums/AlbumsView.tsx";

export const albumsRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/albums",
  component: AlbumsView,
});
