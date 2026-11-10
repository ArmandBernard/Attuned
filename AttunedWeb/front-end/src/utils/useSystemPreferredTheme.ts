import { useSyncExternalStore } from "react";

type ThemePreference = "dark" | "light" | null;

const darkQuery = "(prefers-color-scheme: dark)";
const lightQuery = "(prefers-color-scheme: light)";

const subscribe = (callback: () => void) => {
  const query = window.matchMedia(darkQuery);
  query.addEventListener("change", callback);
  return () => query.removeEventListener("change", callback);
};

// returns a primitive string/null, so snapshots compare equal and avoid re-render loops
const getSnapshot = (): ThemePreference => {
  if (window.matchMedia(darkQuery).matches) return "dark";
  if (window.matchMedia(lightQuery).matches) return "light";
  return null;
};

export const useSystemPreferredTheme = (): ThemePreference =>
  useSyncExternalStore(subscribe, getSnapshot);
