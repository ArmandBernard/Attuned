import { HTMLAttributes } from "react";

type Severity = "error" | "warning";

export function Alert({
  children,
  severity,
  ...rest
}: HTMLAttributes<HTMLDivElement> & { severity: Severity }) {
  let colorClassNames = "";

  switch (severity) {
    case "error":
      colorClassNames = "bg-error/30";
      break;
    case "warning":
      colorClassNames = "bg-warning/30";
      break;
  }

  return (
    <div
      {...rest}
      className={`${colorClassNames} text-sm rounded-sm py-2 px-3 ${rest.className}`}
    >
      {children}
    </div>
  );
}
