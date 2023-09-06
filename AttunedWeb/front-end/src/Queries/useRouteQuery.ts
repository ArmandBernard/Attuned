import { useQuery } from "react-query";

interface useRouteQueryProps<TParams extends ValidParams, TBody> {
  url: string;
  parameters?: TParams;
  body?: TBody;
}

type ValidParams = { [key: string]: string | number | undefined } | undefined;

export function useRouteQuery<
  TResult,
  TError = unknown,
  TParams extends ValidParams = undefined,
  TBody = unknown,
>(props: useRouteQueryProps<TParams, TBody>) {
  return useQuery<TResult, TError>("repoData", async () => {
    const response = await fetch(buildUrl(props.url, props.parameters), {
      body: JSON.stringify(props.body),
      headers: []
    });

    if (!response.ok) {
      throw new Error("fetch failed");
    }
    return response.json();
  });
}

const base = "http://localhost:5280";

const buildUrl = (path: string, parameters: ValidParams | undefined) => {
  const url = new URL(path, base);

  if (parameters !== undefined) {
    Object.entries(parameters).forEach(([name, value]) => {
      if (value != undefined) {
        url.searchParams.set(name, value.toString());
      }
    });
  }
  return url;
};
