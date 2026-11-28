import { useQuery } from "@tanstack/react-query";
import { UseQueryOptions } from "@tanstack/react-query";
import { buildUrl } from "@utils/buildUrl.ts";

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

  return useQuery<TResult, TError>({
    queryKey: [url, parameters, body],
    queryFn: async () => {
      const response = await fetch(buildUrl(url, parameters), {
        body: JSON.stringify(body),
        headers: [],
      });

      if (!response.ok) {
        throw new Error("fetch failed");
      }
      return response.json();
    },
    ...rest,
  });
}
