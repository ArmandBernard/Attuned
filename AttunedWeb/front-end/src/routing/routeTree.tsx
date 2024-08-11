import { rootRoute } from "./rootRoute.tsx";
import { playlistRoute } from "./playlistsRoute.tsx";
import { indexRoute } from "./indexRoute.tsx";

export const routeTree = rootRoute.addChildren([indexRoute, playlistRoute]);
