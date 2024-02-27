/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  theme: {
    extend: {
      spacing: {},
      colors: {
        "struct-000": "rgba(255, 255, 255, 1)",
        "struct-200": "rgba(224, 227, 231, 1)",
        "struct-300": "rgba(194, 199, 207, 1)",
        "struct-700": "rgba(81, 96, 116, 1)",
        "struct-600": "rgba(107, 118, 136, 1)",
        "struct-900-sievo": "rgba(27, 50, 75, 1)",
        "primary-500": "rgba(67, 110, 188, 1)",
      },
    },
  },
  plugins: [],
};
