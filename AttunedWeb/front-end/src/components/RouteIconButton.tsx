import { ComponentProps } from "react";
import { createLink } from "@tanstack/react-router";

// createLink keeps Link's generics, so `to` and `params` stay type-checked
const RouteIconButton = createLink(function ({
  className,
  ...props
}: ComponentProps<"a">) {
  return (
    <a
      {...props}
      className={
        "cursor-pointer hover:bg-hover h-8 w-8 p-1 rounded-full flex justify-center items-center fill-text-color " +
        (className ?? "")
      }
    />
  );
});

export default RouteIconButton;
