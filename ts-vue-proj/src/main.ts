import { createApp, ref } from 'vue'
import { defineStore, acceptHMRUpdate, createPinia } from 'pinia'
import './style.scss'
import App from './App.vue'
import router from './router/index'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

const app = createApp(App)
const pinia = createPinia()
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(key, component)
}
app.use(router).use(pinia).mount('#app')
