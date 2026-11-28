import { useQuery } from "@tanstack/react-query";
import { UseQueryOptions } from "@tanstack/react-query";
import { buildUrl } from "@utils/buildUrl.ts";
import { APIError } from "@utils/APIError.ts";
import { RouteError } from "@utils/RouteError.ts";

type UseRouteQueryProps<TParams extends ValidParams, TBody, TResult> = {
  url: string;
  parameters?: TParams;
  body?: TBody;
} & Omit<
  UseQueryOptions<TResult, RouteError, TResult>,
  "queryKey" | "queryFn" | "initialData"
>;

type ValidParams = { [key: string]: string | number | undefined } | undefined;

export function useRouteQuery<
  TResult,
  TParams extends ValidParams = undefined,
  TBody = unknown,
>(props: UseRouteQueryProps<TParams, TBody, TResult>) {
  const { url, parameters, body, ...rest } = props;

  return useQuery<TResult, RouteError>({
    queryKey: [url, parameters, body],
    queryFn: async () => {
      const response = await fetch(buildUrl(url, parameters), {
        body: JSON.stringify(body),
        headers: [],
      });

      if (!response.ok) {
        throw new APIError(response.status, await response.json());
      }
      return await response.json();
    },
    ...rest,
  });
}
