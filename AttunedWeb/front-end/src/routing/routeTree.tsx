import { rootRoute } from "./rootRoute.tsx";
import { playlistRoute } from "./playlistsRoute.tsx";
import { indexRoute } from "./indexRoute.tsx";
import { albumsRoute } from "@root/routing/albumsRoute.tsx";
import { albumRoute } from "@root/routing/albumRoute.tsx";
import { albumTrackRoute } from "@root/routing/albumTrackRoute.tsx";
import { playlistTrackRoute } from "@root/routing/playlistTrackRoute.tsx";

export const routeTree = rootRoute.addChildren([
  indexRoute,
  playlistRoute.addChildren([playlistTrackRoute]),
  albumsRoute.addChildren([albumRoute.addChildren([albumTrackRoute])]),
]);
