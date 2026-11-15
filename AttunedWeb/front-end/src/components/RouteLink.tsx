import { ComponentProps } from "react";
import { createLink } from "@tanstack/react-router";

// createLink keeps Link's generics, so `to` and `params` stay type-checked
const RouteLink = createLink(function({ className, ...props }: ComponentProps<"a">) {
  return (
    <a
      {...props}
      className={
        "text-primary underline decoration-current/50 hover:decoration-current cursor-pointer " +
        (className ?? "")
      }
    />
  );
});

export default RouteLink;
