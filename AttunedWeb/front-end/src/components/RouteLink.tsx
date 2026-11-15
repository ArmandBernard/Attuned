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
      "text-primary underline decoration-current/50 hover:decoration-current cursor-pointer " +
      (className ?? "")
    }
  />
));

// createLink keeps Link's generics, so `to` and `params` stay type-checked
const RouteLink = createLink(StyledAnchor);

export default RouteLink;
