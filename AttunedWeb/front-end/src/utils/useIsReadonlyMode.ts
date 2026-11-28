import { useSyncExternalStore } from "react";

export const READONLY_MODE_STORAGE_KEY = "readonlyMode";

const listeners = new Set<() => void>();

const notify = () => listeners.forEach((listener) => listener());

const subscribe = (callback: () => void) => {
  listeners.add(callback);
  // the "storage" event only fires for changes made in *other* tabs
  window.addEventListener("storage", callback);
  return () => {
    listeners.delete(callback);
    window.removeEventListener("storage", callback);
  };
};

const getReadonlyMode = (): boolean => {
  try {
    let isReadonlyMode = true;

    if (localStorage.getItem(READONLY_MODE_STORAGE_KEY) === "false") {
      isReadonlyMode = false;
    }
    return isReadonlyMode;
  } catch {
    return true;
  }
};

export const setReadonlyMode = (readonly: boolean) => {
  try {
    localStorage.setItem(READONLY_MODE_STORAGE_KEY, String(readonly));
  } catch {}
  notify();
};

/**
 * The read-only mode setting, persisted in local storage and kept in sync
 * across components and browser tabs.
 */
export function useIsReadonlyMode() {
  return useSyncExternalStore(subscribe, getReadonlyMode);
}
