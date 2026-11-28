import { HTMLAttributes } from "react";
import { APIError } from "@utils/APIError.ts";

export function ErrorAlert({
  error,
  ...rest
}: HTMLAttributes<HTMLDivElement> & { error: APIError | null }) {
  if (!error) {
    return null;
  }

  return (
    <div {...rest} className={`bg-error/30 rounded-sm p-4 ${rest.className}`}>
      {error.message}
      <ValidationErrorList error={error} />
    </div>
  );
}

function ValidationErrorList({ error }: { error: APIError }) {
  const entries = [
    ...new Set(
      Object.entries(error.validationErrors).flatMap(
        // oxlint-disable-next-line no-unused-vars
        ([_, messages]) => messages,
      ),
    ),
  ];

  if (entries.length === 0) {
    return null;
  }

  return (
    <ul className="list-disc pl-4">
      {entries.map((message) => (
        <li key={message}>{message}</li>
      ))}
    </ul>
  );
}
