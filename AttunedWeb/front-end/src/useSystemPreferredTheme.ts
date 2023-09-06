import { useCallback, useEffect, useState } from "react";

type ThemePreference = "dark" | "light" | null;

export const useSystemPreferredTheme = () => {
  const [preferredTheme, setPreferredTheme] = useState<ThemePreference>(null);

  // check for both dark, light and unset theme preference
  const handleThemeChange = useCallback(() => {
    let preference: ThemePreference = null;
    if (window.matchMedia("(prefers-color-scheme: dark)").matches) {
      preference = "dark";
    } else if (window.matchMedia("(prefers-color-scheme: light)").matches) {
      preference = "light";
    }

    setPreferredTheme(preference);
  }, []);

  useEffect(() => {
    // add event listener for theme changes on mount
    const query = window.matchMedia("(prefers-color-scheme: dark)");
    query.addEventListener("change", handleThemeChange);

    // call once on mount to get accurate value
    handleThemeChange();

    return () => {
      // cleanup event listener on unmount
      query.removeEventListener("change", handleThemeChange);
    };
  }, [handleThemeChange]);

  return preferredTheme;
};
