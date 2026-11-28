import { HTMLAttributes } from "react";
import { APIError } from "@utils/APIError.ts";
import { RouteError } from "@utils/RouteError.ts";

export function ErrorAlert({
  error,
  ...rest
}: HTMLAttributes<HTMLDivElement> & { error: RouteError | null }) {
  if (!error) {
    return null;
  }

  if (error instanceof APIError) {
    return (
      <Alert>
        <ASPNetErrorView error={error} />
      </Alert>
    );
  }

  return (
    <div {...rest} className={`bg-error/30 rounded-sm p-4 ${rest.className}`}>
      {error.message}
    </div>
  );
}

function Alert({ children, ...rest }: HTMLAttributes<HTMLDivElement>) {
  return (
    <div {...rest} className={`bg-error/30 rounded-sm p-4 ${rest.className}`}>
      {children}
    </div>
  );
}

function ASPNetErrorView({ error }: { error: APIError }) {
  const validationErrorMessages = [
    ...new Set(
      Object.entries(error.validationErrors).flatMap(
        // oxlint-disable-next-line no-unused-vars
        ([_, messages]) => messages,
      ),
    ),
  ];

  if (validationErrorMessages.length === 0) {
    return (
      <>
        <div className="font-bold mb-1">{error.title}</div>
        <div>{error.detail}</div>
      </>
    );
  }

  return (
    <ValidationErrorView
      error={error}
      validationErrorMessages={validationErrorMessages}
    />
  );
}

function ValidationErrorView({
  error,
  validationErrorMessages,
}: {
  error: APIError;
  validationErrorMessages: string[];
}) {
  return (
    <>
      <div className="font-bold mb-1">{error.title}</div>
      <ul className="list-disc pl-4">
        {validationErrorMessages.map((message) => (
          <li key={message}>{message}</li>
        ))}
      </ul>
    </>
  );
}
