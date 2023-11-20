import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import path from 'path'
import dotenv from 'dotenv'

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const envFile = '.env';
    dotenv.config({ path: envFile });
    return {
        plugins: [react()],
        build: {
            outDir: 'build',
            assetsDir: 'assets',
            emptyOutDir: true,
        },
        resolve: {
            alias: {
                '@': path.resolve(__dirname, './src'),
            },
        },
        server: {
            // watch: {
            //     usePolling: true,
            //   },
            // proxy: {
            //     '/api': {
            //         target: 'http://hos/Tele',
            //         changeOrigin: true,
            //         rewrite: (path) => path.replace(/^\/api/, ''),
            //     },
            // },
            //host: true, //needed for the Docker Container port mapping to work
            strictPort: true,
            port: 8081
        },
    };
})
