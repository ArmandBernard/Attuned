/** @type {import('tailwindcss').Config} */
export default {
  content: ["./src/**/*.{html,ts,tsx}"],
  theme: {
    extend: {
      colors: {
        background: "var(--background)",
        "alternating-row": "var(--alternating-row)",
        "selected-row": "var(--selected-row)",
        "text-color": "var(--text-color)",
        primary: "var(--primary)",
        love: "var(--love)",
        "paper-1": "var(--paper-1)",
        "paper-2": "var(--paper-2)",
      },
    },
  },
  plugins: [],
};
