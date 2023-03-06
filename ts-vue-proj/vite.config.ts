import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import AutoImport from 'unplugin-auto-import/vite'
import Components from 'unplugin-vue-components/vite'
import { ElementPlusResolver } from 'unplugin-vue-components/resolvers'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    AutoImport({
      imports: ['vue'],
      resolvers: [ElementPlusResolver()],
    }),
    Components({
      resolvers: [ElementPlusResolver()],
    }),
  ],
  server: {
    open: true,
    host: '127.0.0.1',
    port: 5174,
    proxy: {
      '/api': {
        target: "https://localhost:7166",
        //target: loadEnv("",process.cwd()).VITE_API_URL,//vite not support  //import.meta.env.VITE_API_URL,
        changeOrigin: true,
        secure: false,
        rewrite(path) {
          return path.replace(/^\/api/, '');
        }
      }
    }
  }
})
