import { fixupConfigRules, fixupPluginRules } from "@eslint/compat";
import react from "eslint-plugin-react";
import reactHooks from "eslint-plugin-react-hooks";
import globals from "globals";
import path from "node:path";
import { fileURLToPath } from "node:url";
import { FlatCompat } from "@eslint/eslintrc";
import eslint from "@eslint/js";
import tsEslint from "typescript-eslint";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const compat = new FlatCompat({
  baseDirectory: __dirname,
});

export default [
  eslint.configs.recommended,
  ...tsEslint.configs.recommended,
  ...fixupConfigRules(compat.extends("plugin:react-hooks/recommended")),
  {
    files: ["**/*.{js,mjs,cjs,jsx,ts,tsx}"],
    plugins: {
      react,
      "react-hooks": fixupPluginRules(reactHooks),
    },
    languageOptions: {
      globals: {
        ...globals.browser,
      },
      parserOptions: {
        ecmaFeatures: {
          jsx: true,
        },
      },
      ecmaVersion: "latest",
      sourceType: "module",
    },
    rules: {
      indent: [
        "warn",
        2,
        {
          SwitchCase: 1,
        },
      ],

      quotes: ["error", "double"],
      semi: ["error", "always"],
      eqeqeq: "error",
      "react-hooks/exhaustive-deps": ["error"],
    },
  },
  {
    files: ["src/dtos/Dtos.ts"],

    rules: {
      quotes: "off",
    },
  },
];
