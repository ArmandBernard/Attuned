import { createRoute } from "@tanstack/react-router";
import { allTracksRoute } from "./allTracksRoute.tsx";

export const allTracksTrackRoute = createRoute({
  getParentRoute: () => allTracksRoute,
  path: "tracks/$trackId",
});
