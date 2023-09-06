/** @type {import('tailwindcss').Config} */
export default {
  content: ["./src/**/*.{html,ts,tsx}"],
  theme: {
    extend: {
      colors: {
        background: "var(--background)",
        "text-color": "var(--text-color)",
        primary: "var(--primary)",
      },
    },
  },
  plugins: [],
};
