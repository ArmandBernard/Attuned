import { useMutation, UseMutationOptions } from "@tanstack/react-query";
import { useCallback } from "react";
import { buildUrl, ValidParams } from "@utils/buildUrl.ts";

type UseRouteMutationProps<
  TData,
  TError = unknown,
  TParams extends ValidParams = undefined,
  TBody = unknown,
> = { url: string } & ReactMutationOptions<
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
  const { url, ...rest } = props;

  const mutateFn = useCallback(
    async ({ parameters, body }: MutationRequestOptions<TParams, TBody>) => {
      const response = await fetch(buildUrl(url, parameters), {
        body: JSON.stringify(body),
        headers: [],
      });

      if (!response.ok) {
        throw new Error("fetch failed");
      }
      return response.json();
    },
    [url],
  );

  return useMutation<TData, TError, MutationRequestOptions<TParams, TBody>>({
    ...rest,
    mutationFn: mutateFn,
  });
}
