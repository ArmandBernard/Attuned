import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
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
