import { defineConfig } from "vitest/config";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";
import { analyzer } from "vite-bundle-analyzer";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => ({
  // "npm run analyze" builds in analyze mode, which opens the bundle report
  plugins: [
    react(),
    tailwindcss(),
    ...(mode === "analyze" ? [analyzer()] : []),
  ],
  test: {
    environment: "jsdom",
  },
  resolve: {
    alias: {
      "@dtos": "/src/dtos/Dtos.ts",
      "@components": "/src/components",
      "@root": "/src",
      "@utils": "/src/utils",
      "@views": "/src/views",
    },
  },
}));
