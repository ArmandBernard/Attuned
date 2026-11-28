export type ValidParams =
  | { [key: string]: string | number | undefined }
  | undefined;

export const apiBase = "http://localhost:5280";

export const buildUrl = (path: string, parameters: ValidParams | undefined) => {
  const url = new URL(path, apiBase);

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
