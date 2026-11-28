import { HTMLAttributes } from "react";

export function Alert({ children, ...rest }: HTMLAttributes<HTMLDivElement>) {
  return (
    <div
      {...rest}
      className={`bg-error/30 rounded-sm py-2 px-3 ${rest.className}`}
    >
      {children}
    </div>
  );
}
