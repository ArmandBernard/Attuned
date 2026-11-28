import { HTMLAttributes } from "react";
import { APIError } from "@utils/APIError.ts";
import { RouteError } from "@utils/RouteError.ts";
import { Alert } from "@components/Alert.tsx";
import { AlertTitle } from "@components/AlertTitle.tsx";

export function ErrorAlert({
  error,
  ...rest
}: HTMLAttributes<HTMLDivElement> & { error: RouteError | null }) {
  if (!error) {
    return null;
  }

  if (error instanceof APIError) {
    return (
      <Alert {...rest}>
        <ASPNetErrorView error={error} />
      </Alert>
    );
  }

  return <Alert {...rest}>{error.message}</Alert>;
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
        <AlertTitle>{error.title}</AlertTitle>
        {error.detail}
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
      <AlertTitle>{error.title}</AlertTitle>
      <ul className="list-disc pl-4">
        {validationErrorMessages.map((message) => (
          <li key={message}>{message}</li>
        ))}
      </ul>
    </>
  );
}
