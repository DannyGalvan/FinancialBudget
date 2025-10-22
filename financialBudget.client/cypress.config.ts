import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    baseUrl: "http://93.127.139.74:83",
    specPattern: "cypress/e2e/**/*.cy.{js,jsx,ts,tsx}",
    setupNodeEvents(on, config) {
      // implementa listeners de eventos aquí

      return config;
    },
  },
  component: {
    devServer: {
      framework: "react",
      bundler: "vite", // o 'webpack'
    },
  },
});
