export class APIError extends Error {
  public validationErrors: Record<string, string[]> = {};
  public statusCode: number;

  constructor(statusCode: number, body: unknown) {
    super();

    this.statusCode = statusCode;

    if (hasValidationErrors(body)) {
      this.validationErrors = body.errors;
      this.message = "Validation errors found";
    } else {
      this.message = `Fetch failed with status code ${statusCode}`;
    }
  }
}

type ValidationErrorBody = {
  errors: Record<string, string[]>;
};

function hasValidationErrors(body: unknown): body is ValidationErrorBody {
  return typeof body === "object" && !!body && "errors" in body;
}
