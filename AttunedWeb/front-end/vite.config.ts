import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      "@dtos": "/src/dtos/Dtos.ts",
      "@components": "/src/components",
      "@root": "/src",
      "@utils": "/src/utils",
      "@views": "/src/views",
    },
  },
});
