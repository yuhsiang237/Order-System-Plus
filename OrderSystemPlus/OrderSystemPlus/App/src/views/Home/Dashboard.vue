<template>
  <div class="container-fluid sign-in-page">
    Dashboard
    <div class="mb-3">
      <button class="btn btn-main-color01" @click="signOut">登出</button>
      <button class="btn btn-main-color01" @click="getUserList">fetch</button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import { useRouter } from 'vue-router'
import HttpClient from '@/utils/HttpClient.ts'

export default defineComponent({
  name: 'dashboard',
  components: {},
  setup() {
    const router = useRouter()

    const signOut = async () => {
      var res = await HttpClient.postWithCredentials(
        import.meta.env.VITE_APP_AXIOS_AUTH_SIGNOUT,
        {}
      )
      localStorage.removeItem('accessToken')
      console.log(res)
      // login direct
      router.push({ name: 'signIn' })
    }

    const getUserList = async () => {
      var res = await HttpClient.post(import.meta.env.VITE_APP_AXIOS_USERMANAGE_GETUSERLIST, {})
      console.log(res)
    }
    return { signOut, getUserList }
  }
})
</script>
