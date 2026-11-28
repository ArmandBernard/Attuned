import { HTMLAttributes } from "react";

export function ErrorAlert({
  error,
  ...rest
}: HTMLAttributes<HTMLDivElement> & { error: Error | null }) {
  if (!error) {
    return null;
  }

  return (
    <div {...rest} className={`bg-error/30 rounded-sm p-4 ${rest.className}`}>
      {error.message}
    </div>
  );
}
