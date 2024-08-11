import {
  createContext,
  FunctionComponent,
  ReactNode,
  useCallback,
  useContext,
  useEffect,
  useRef,
} from "react";
import { useSystemPreferredTheme } from "@utils/useSystemPreferredTheme.ts";

const ThemeContext = createContext<"light" | "dark">("light");

export const useTheme = () => useContext(ThemeContext);

export const ThemeLayer: FunctionComponent<{ children?: ReactNode }> = ({
  children,
}) => {
  const htmlRef = useRef(document.documentElement);

  const systemPreferredTheme = useSystemPreferredTheme();

  // set the site theme using a class on the main html element
  const setTheme = useCallback((dark: boolean) => {
    if (dark) {
      htmlRef.current.classList.add("dark");
    } else {
      htmlRef.current.classList.remove("dark");
    }
  }, []);

  // when the themePreference changes, also update the app theme
  useEffect(() => {
    setTheme(systemPreferredTheme === "dark");
  }, [setTheme, systemPreferredTheme]);

  return (
    <ThemeContext.Provider value={systemPreferredTheme || "light"}>
      {children}
    </ThemeContext.Provider>
  );
};
