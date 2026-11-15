import { createRoute } from "@tanstack/react-router";
import { allTracksRoute } from "./allTracksRoute.tsx";

// Rendering is handled by the allTracksRoute layout.
export const indexRoute = createRoute({
  getParentRoute: () => allTracksRoute,
  path: "/",
});
