import { createRouter, createWebHistory } from 'vue-router'
import SignIn from '@/views/home/SignIn.vue'
import SignUp from '@/views/home/SignUp.vue'
import Dashboard from '@/views/home/Dashboard.vue'
import ProductTypeSearch from '@/views/backend/productTypeManage/ProductTypeSearch.vue'
import ProductSearch from '@/views/backend/productManage/ProductSearch.vue'
import ProductInventorySearch from '@/views/backend/productInventoryManage/ProductInventorySearch.vue'

import Backend from '@/views/Backend/Backend.vue'
import HttpClient from '@/utils/HttpClient.ts'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'default',
      component: SignIn
    },
    {
      path: '/home/signIn',
      name: 'signIn',
      component: SignIn
    },
    {
      path: '/home/signUp',
      name: 'signUp',
      component: SignUp
    },
    {
      path: '/backend',
      name: 'backend',
      component: Backend,
      meta: { requiresAuth: true },
      children: [
        {
          path: 'dashboard',
          name: 'dashboard',
          component: Dashboard
        },
        {
          path: 'productTypeManage',
          name: 'productTypeManage',
          children: [
            {
              path: 'productTypeSearch',
              name: 'productTypeSearch',
              component: ProductTypeSearch
            }
          ]
        },
        {
          path: 'productManage',
          name: 'productManage',
          children: [
            {
              path: 'productSearch',
              name: 'productSearch',
              component: ProductSearch
            }
          ]
        },
        {
          path: 'productInventoryManage',
          name: 'productInventoryManage',
          children: [
            {
              path: 'productInventorySearch',
              name: 'productInventorySearch',
              component: ProductInventorySearch
            }
          ]
        }
      ]
    }
  ]
})

router.beforeEach(async (to, from, next) => {
  // --- 登入驗證機制開始 ---
  if (to.meta.requiresAuth) {
    // 檢查 Access Token 是否存在
    const accessToken = localStorage.getItem('accessToken')
    if (!accessToken) {
      // 如果 Access Token 不存在，跳轉登入頁面
      next({ name: 'signIn' })
      return
    }
    try {
      const res = await HttpClient.post(import.meta.env.VITE_APP_AXIOS_AUTH_VALIDATEACCESSTOKEN, {})
      next()
    } catch (ex) {
      try {
        const response = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_AUTH_REFRESHACCESSTOKEN,
          {}
        )
        // 將新的 Access Token 儲存 LocalStorage
        localStorage.setItem('accessToken', response.accessToken)

        // 重新驗證 Access Token 是否有效
        const res = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_AUTH_VALIDATEACCESSTOKEN,
          {}
        )
        // 如果 Access Token 有效，允許訪問頁面
        next()
      } catch (refreshError) {
        console.error('Refresh token error.Direct to SignIn.')
        // 刷新Token失敗跳轉回登入頁面
        next({ name: 'signIn' })
      }
    }
  } else {
    next()
  }
  // --- 登入驗證機制結束 ---
})

export default router
