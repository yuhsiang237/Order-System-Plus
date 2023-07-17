<template>
  <div class="container-fluid sign-in-page">
    <div class="row">
      <div
        class="col-12 col-md-6 watermark-block"
        style="
          background: url('/images/banner_outside.png');
          background-size: cover;
          background-repeat: no-repeat;
          background-position: center center;
        "
      ></div>
      <div class="col-12 col-md-6">
        <div class="form-block">
          <h1>登入</h1>
          <div>
            <div class="mb-3">
              <label>帳號</label>
              <input class="form-control" v-model="account" />
            </div>
            <div class="mb-3">
              <label>密碼</label>
              <input class="form-control" type="password" v-model="password" />
            </div>
            <div class="mb-3">
              <input type="submit" class="btn btn-main-color01" @click="signIn" v value="登入" />
            </div>

            <div class="text-muted">
              還沒有帳號嗎? <RouterLink to="/Home/SignUp">註冊帳號</RouterLink>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import { RouterLink } from 'vue-router'
import HttpClient from '@/utils/HttpClient.ts'
import { encrypt } from '@/utils/Encryption.ts'

export default defineComponent({
  name: 'sign-in',
  components: {
    RouterLink
  },
  setup() {
    const account = ref('')
    const password = ref('')

    const signIn = async () => {
      try {
        const response = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_USERMANAGE_SIGNINUSER,
          {
            account: account.value,
            password: password.value
          }
        )
        localStorage.setItem('token', encrypt(response.token))
        alert('success')
      } catch (error) {
        alert(error?.response?.data?.message)
      }
    }
    return {
      account,
      password,
      signIn
    }
  }
})
</script>
