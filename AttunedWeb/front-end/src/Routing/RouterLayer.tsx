import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.tsx";

const router = createRouter({ routeTree });

// register the type of our router for type safety across the project
declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}

export function RouterLayer() {
  return <RouterProvider router={router} />;
}
