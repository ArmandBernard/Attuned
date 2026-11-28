import { HTMLAttributes } from "react";

export function AlertTitle({
  children,
  ...rest
}: HTMLAttributes<HTMLDivElement>) {
  return (
    <div {...rest} className={`font-bold mb-1 ${rest.className}`}>
      {children}
    </div>
  );
}
