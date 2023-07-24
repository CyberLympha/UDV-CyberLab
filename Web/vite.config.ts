import {defineConfig} from "vite"
import react from "@vitejs/plugin-react"
import eslint from "vite-plugin-eslint"
// https://vitejs.dev/config/
// eslint-disable-next-line import/no-default-export
export default defineConfig({
    plugins: [react(), eslint()],
    resolve: {
        mainFields: [],
    },
})
