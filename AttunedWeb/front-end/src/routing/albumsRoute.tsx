import { createRoute, lazyRouteComponent } from "@tanstack/react-router";
import { rootRoute } from "@root/routing/rootRoute.tsx";

export const albumsRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/albums",
  component: lazyRouteComponent(
    () => import("@views/albums/AlbumsView.tsx"),
    "AlbumsView",
  ),
});
