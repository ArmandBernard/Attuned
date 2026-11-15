import { ComponentPropsWithoutRef, forwardRef } from "react";
import { createLink } from "@tanstack/react-router";

const StyledAnchor = forwardRef<
  HTMLAnchorElement,
  ComponentPropsWithoutRef<"a">
>(({ className, ...props }, ref) => (
  <a
    ref={ref}
    {...props}
    className={
      "cursor-pointer hover:bg-hover h-8 w-8 p-1 rounded-full flex justify-center items-center fill-text-color " +
      (className ?? "")
    }
  />
));

const RouteIconButton = createLink(StyledAnchor);

export default RouteIconButton;
