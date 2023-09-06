import {useCallback, useEffect, useRef} from "react";
import {useSystemPreferredTheme} from "./useSystemPreferredTheme.ts";

export default function App() {
  const htmlRef = useRef(document.documentElement);
  
  const systemPreferredTheme = useSystemPreferredTheme();
  
  // when the themePreference changes, also update the app theme
  useEffect(() => {
    setTheme(
        systemPreferredTheme === "dark"
    );
  }, [systemPreferredTheme]);
  
  // set the site theme using a class on the main html element
  const setTheme = useCallback((dark: boolean) => {
    if (dark) {
      htmlRef.current.classList.add("dark");
    } else {
      htmlRef.current.classList.remove("dark");
    }
  }, []);
  
  return <></>;
}
