import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.tsx";

const router = createRouter({ routeTree });

export function RouterLayer() {
  return <RouterProvider router={router} />;
}
