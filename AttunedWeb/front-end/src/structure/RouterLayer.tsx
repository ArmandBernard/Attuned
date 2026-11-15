import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "@root/routing/routeTree.tsx";
import { FullScreenLoading } from "@components/FullScreenLoading.tsx";

const router = createRouter({
  routeTree,
  // shown while a lazy route chunk loads
  defaultPendingComponent: FullScreenLoading,
  defaultPendingMs: 200,
});

// register the type of our router for type safety across the project
declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}

export function RouterLayer() {
  return <RouterProvider router={router} />;
}
