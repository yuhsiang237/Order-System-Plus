import './assets/css/style.css'
import 'vue-loading-overlay/dist/css/index.css'
import 'vue-multiselect/dist/vue-multiselect.css'

import { createApp } from 'vue'

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(router)

app.mount('#app')
