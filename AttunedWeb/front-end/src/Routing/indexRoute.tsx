import { createRoute } from "@tanstack/react-router";
import { rootRoute } from "./rootRoute.tsx";
import { TracksView } from "../Tracks/TracksView.tsx";

export const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: "/",
  component: () => (
    <main className="h-full min-w-0 flex-1">
      <TracksView title="All tracks" playlist={undefined} />
    </main>
  ),
});
