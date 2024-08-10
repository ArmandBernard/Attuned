import { useQuery } from "@tanstack/react-query";
import { UseQueryOptions } from "@tanstack/react-query";

type useRouteQueryProps<TParams extends ValidParams, TBody, TResult, TError> = {
  url: string;
  parameters?: TParams;
  body?: TBody;
} & Omit<
  UseQueryOptions<TResult, TError, TResult>,
  "queryKey" | "queryFn" | "initialData"
>;

type ValidParams = { [key: string]: string | number | undefined } | undefined;

export function useRouteQuery<
  TResult,
  TError = unknown,
  TParams extends ValidParams = undefined,
  TBody = unknown,
>(props: useRouteQueryProps<TParams, TBody, TResult, TError>) {
  const { url, parameters, body, ...rest } = props;

  return useQuery<TResult, TError>(
    [url, parameters, body],
    async () => {
      const response = await fetch(buildUrl(url, parameters), {
        body: JSON.stringify(body),
        headers: [],
      });

      if (!response.ok) {
        throw new Error("fetch failed");
      }
      return response.json();
    },
    rest,
  );
}

const base = "http://localhost:5280";

const buildUrl = (path: string, parameters: ValidParams | undefined) => {
  const url = new URL(path, base);

  if (parameters !== undefined) {
    Object.entries(parameters).forEach(([name, value]) => {
      if (value !== undefined) {
        url.searchParams.set(name, value.toString());
      }
    });
  }
  return url;
};
