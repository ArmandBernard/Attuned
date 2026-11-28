import { rootRoute } from "./rootRoute.tsx";
import { playlistRoute } from "./playlistsRoute.tsx";
import { indexRoute } from "./indexRoute.tsx";
import { albumsRoute } from "@root/routing/albumsRoute.tsx";
import { albumTrackRoute } from "@root/routing/albumTrackRoute.tsx";
import { playlistTrackRoute } from "@root/routing/playlistTrackRoute.tsx";
import { allTracksRoute } from "@root/routing/allTracksRoute.tsx";
import { allTracksTrackRoute } from "@root/routing/allTracksTrackRoute.tsx";

export const routeTree = rootRoute.addChildren([
  allTracksRoute.addChildren([indexRoute, allTracksTrackRoute]),
  playlistRoute.addChildren([playlistTrackRoute]),
  albumsRoute.addChildren([albumTrackRoute]),
]);
