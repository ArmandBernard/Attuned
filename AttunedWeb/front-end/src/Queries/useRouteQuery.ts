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

  if (parameters === undefined) {
    return url;
  }

  let filledUrlString = url.toString();

  // %7B is "{", %7D is "}"
  const found = filledUrlString.matchAll(/%7B(\w+)%7D/g);

  const handled: string[] = [];

  for (const regExpExecArray of found) {
    const name = regExpExecArray[1];

    const value = parameters[name]?.toString();

    if (value === undefined) {
      throw new Error(`parameter ${name} not found in fetch ${url}`);
    }

    filledUrlString = filledUrlString.replace(`%7B${name}%7D`, value);

    handled.push(name);
  }

  const filledUrl = new URL(filledUrlString);

  Object.entries(parameters).forEach(([name, value]) => {
    if (value !== undefined && !handled.includes(name)) {
      filledUrl.searchParams.set(name, value.toString());
    }
  });

  return filledUrl;
};
