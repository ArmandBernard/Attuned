import { act, renderHook } from "@testing-library/react";
import { afterEach, beforeEach, describe, expect, it, vi } from "vitest";
import {
  READONLY_MODE_STORAGE_KEY,
  setReadonlyMode,
  useIsReadonlyMode,
} from "./useIsReadonlyMode.ts";

describe("setReadonlyMode", () => {
  beforeEach(() => localStorage.clear());
  afterEach(() => vi.restoreAllMocks());

  it("persists the value to local storage", () => {
    setReadonlyMode(false);
    expect(localStorage.getItem(READONLY_MODE_STORAGE_KEY)).toBe("false");

    setReadonlyMode(true);
    expect(localStorage.getItem(READONLY_MODE_STORAGE_KEY)).toBe("true");
  });

  it("does not throw when writing storage throws, and stays read-only", () => {
    vi.spyOn(Storage.prototype, "setItem").mockImplementation(() => {
      throw new Error("quota");
    });
    expect(() => setReadonlyMode(false)).not.toThrow();
    expect(localStorage.getItem(READONLY_MODE_STORAGE_KEY)).not.toBe("true");
  });
});

describe("useIsReadonlyMode", () => {
  beforeEach(() => localStorage.clear());

  it("returns true by default", () => {
    const { result } = renderHook(() => useIsReadonlyMode());
    expect(result.current).toBe(true);
  });

  it("reads the initial value from local storage", () => {
    localStorage.setItem(READONLY_MODE_STORAGE_KEY, "false");
    const { result } = renderHook(() => useIsReadonlyMode());
    expect(result.current).toBe(false);
  });

  it("updates when the setting changes", () => {
    const { result } = renderHook(() => useIsReadonlyMode());

    act(() => setReadonlyMode(false));
    expect(result.current).toBe(false);

    act(() => setReadonlyMode(true));
    expect(result.current).toBe(true);
  });

  it("keeps every consumer in sync", () => {
    const first = renderHook(() => useIsReadonlyMode());
    const second = renderHook(() => useIsReadonlyMode());

    act(() => setReadonlyMode(false));

    expect(first.result.current).toBe(false);
    expect(second.result.current).toBe(false);
  });

  it("updates when another tab changes the setting", () => {
    const { result } = renderHook(() => useIsReadonlyMode());

    act(() => {
      // another tab writes directly to storage, which fires a "storage" event here
      localStorage.setItem(READONLY_MODE_STORAGE_KEY, "false");
      window.dispatchEvent(new StorageEvent("storage"));
    });

    expect(result.current).toBe(false);
  });

  it("stops listening after unmount", () => {
    const remove = vi.spyOn(window, "removeEventListener");
    const { unmount } = renderHook(() => useIsReadonlyMode());

    unmount();

    expect(remove).toHaveBeenCalledWith("storage", expect.any(Function));
    remove.mockRestore();
  });
});
