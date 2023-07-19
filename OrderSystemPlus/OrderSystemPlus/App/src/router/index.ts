import { createRouter, createWebHistory } from 'vue-router'
import SignIn from '@/views/Home/SignIn.vue'
import SignUp from '@/views/Home/SignUp.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Default',
      component: SignIn
    },
    {
      path: '/Home/SignIn',
      name: 'SignIn',
      component: SignIn
    },
    {
      path: '/Home/SignUp',
      name: 'SignUp',
      component: SignUp
    }
  ]
})

export default router
