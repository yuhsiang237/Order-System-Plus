import { createRouter, createWebHistory } from 'vue-router'
import SignIn from '@/views/Home/SignIn.vue'
import SignUp from '@/views/Home/SignUp.vue'
import HttpClient from '@/utils/HttpClient.ts'

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



router.beforeEach(async (to, from, next) => {

  // Auth check
  // 检查 Access Token 是否存在
  const accessToken = localStorage.getItem('accessToken');
  if (!accessToken) {
    // 如果 Access Token 不存在，则跳转到登录页面
    next({ path: '/login' });
    return;
  }
  try{
    const res = await HttpClient.post(
    import.meta.env.VITE_APP_AXIOS_AUTH_VALIDATEACCESSTOKEN,{})
    next()
  }catch(ex){
    try{
      const response = await HttpClient.post(
        import.meta.env.VITE_APP_AXIOS_AUTH_REFRESHACCESSTOKEN,{});
          // 将新的 Access Token 存储在 LocalStorage
       localStorage.setItem('accessToken', response.accessToken);
   
       // 重新执行请求，检查新的 Access Token 是否有效
       const res = await HttpClient.post(
         import.meta.env.VITE_APP_AXIOS_AUTH_VALIDATEACCESSTOKEN,{})
          // 如果新的 Access Token 有效，则允许访问该页面
       next();
     }catch (refreshError) {
       // 如果刷新 Token 也失败，则跳转到登录页面
       next({ path: '/login' });
     }
  }

  // ./Auth check
});

export default router
