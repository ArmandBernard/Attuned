import { createRootRoute, Outlet } from "@tanstack/react-router";
import { Navigation } from "@root/structure/Navigation.tsx";

export const rootRoute = createRootRoute({
  component: () => (
    <div className="flex h-full">
      <div className="sm:w-80 max-sm:w-8">
        <Navigation />
      </div>
      <Outlet />
    </div>
  ),
});
