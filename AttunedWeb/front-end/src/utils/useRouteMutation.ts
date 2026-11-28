import { useMutation, UseMutationOptions } from "@tanstack/react-query";
import { useCallback } from "react";
import { buildUrl, ValidParams } from "@utils/buildUrl.ts";
import { useIsReadonlyMode } from "@utils/useIsReadonlyMode.ts";
import { APIError } from "@utils/APIError.ts";
import { RouteError } from "@utils/RouteError.ts";

type Method = "PATCH" | "POST";

type UseRouteMutationProps<
  TData,
  TParams extends ValidParams = undefined,
  TBody = unknown,
> = { url: string; method: Method } & ReactMutationOptions<
  TData,
  MutationRequestOptions<TParams, TBody>
>;

type ReactMutationOptions<TData, TVariables> = Omit<
  UseMutationOptions<TData, RouteError, TVariables>,
  "mutationKey" | "mutationFn"
>;

type MutationRequestOptions<TParams extends ValidParams, TBody> = {
  parameters?: TParams;
  body?: TBody;
};

export function useRouteMutation<
  TData,
  TParams extends ValidParams = undefined,
  TBody = unknown,
>(props: UseRouteMutationProps<TData, TParams, TBody>) {
  const { url, method, ...rest } = props;
  const isReadonlyMode = useIsReadonlyMode();

  const mutateFn = useCallback(
    async ({ parameters, body }: MutationRequestOptions<TParams, TBody>) => {
      const headers: Record<string, string> = {
        "Content-Type": "application/json",
        "X-Readonly-Mode": isReadonlyMode.toString(),
      };

      const response = await fetch(buildUrl(url, parameters), {
        method: method,
        body: JSON.stringify(body),
        headers: headers,
      });

      if (!response.ok) {
        throw new APIError(response.status, await response.json());
      }
      return await response.json();
    },
    [url, method, isReadonlyMode],
  );

  return useMutation<TData, RouteError, MutationRequestOptions<TParams, TBody>>(
    {
      ...rest,
      mutationFn: mutateFn,
    },
  );
}
