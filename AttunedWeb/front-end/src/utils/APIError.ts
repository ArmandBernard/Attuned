export class APIError extends Error {
  public validationErrors: Record<string, string[]> = {};
  public statusCode: number;
  public title: string | undefined;
  public detail: string | undefined;

  constructor(statusCode: number, body: unknown) {
    super();

    this.statusCode = statusCode;

    if (isASPNetBody(body)) {
      this.title = body.title;
      this.detail = body.detail;
      if (hasValidationErrors(body)) {
        this.validationErrors = body.errors;
        this.message = "Validation errors found";
      }
    } else {
      this.message = `Fetch failed with status code ${statusCode}`;
    }
  }
}

type ASPNetErrorBody = {
  title: string;
  detail: string | undefined;
  errors?: Record<string, string[]>;
};

type ValidationErrorBody = ASPNetErrorBody & {
  errors: Record<string, string[]>;
};

function isASPNetBody(body: unknown): body is ASPNetErrorBody {
  return typeof body === "object" && !!body && "title" in body;
}

function hasValidationErrors(
  body: ASPNetErrorBody,
): body is ValidationErrorBody {
  return "errors" in body;
}
