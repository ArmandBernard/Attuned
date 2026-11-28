import { useMutation, UseMutationOptions } from "@tanstack/react-query";
import { useCallback } from "react";
import { buildUrl, ValidParams } from "@utils/buildUrl.ts";
import { useIsReadonlyMode } from "@utils/useIsReadonlyMode.ts";

type Method = "PATCH" | "POST";

type UseRouteMutationProps<
  TData,
  TError = unknown,
  TParams extends ValidParams = undefined,
  TBody = unknown,
> = { url: string; method: Method } & ReactMutationOptions<
  TData,
  TError,
  MutationRequestOptions<TParams, TBody>
>;

type ReactMutationOptions<TData, TError, TVariables> = Omit<
  UseMutationOptions<TData, TError, TVariables>,
  "mutationKey" | "mutationFn"
>;

type MutationRequestOptions<TParams extends ValidParams, TBody> = {
  parameters?: TParams;
  body?: TBody;
};

export function useRouteMutation<
  TData,
  TError = unknown,
  TParams extends ValidParams = undefined,
  TBody = unknown,
>(props: UseRouteMutationProps<TData, TError, TParams, TBody>) {
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
        throw new Error("fetch failed");
      }
      return response.json();
    },
    [url, method, isReadonlyMode],
  );

  return useMutation<TData, TError, MutationRequestOptions<TParams, TBody>>({
    ...rest,
    mutationFn: mutateFn,
  });
}
