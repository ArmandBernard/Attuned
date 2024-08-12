import { rootRoute } from "./rootRoute.tsx";
import { playlistRoute } from "./playlistsRoute.tsx";
import { indexRoute } from "./indexRoute.tsx";
import { albumsRoute } from "@root/routing/albumsRoute.tsx";

export const routeTree = rootRoute.addChildren([
  indexRoute,
  playlistRoute,
  albumsRoute,
]);
