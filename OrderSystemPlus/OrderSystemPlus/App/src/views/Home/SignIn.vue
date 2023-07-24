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
              <error-message :errors="errors.account" />
            </div>
            <div class="mb-3">
              <label>密碼</label>
              <input class="form-control" type="password" v-model="password" />
              <error-message :errors="errors.password" />
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
import MessageBox from '@/utils/MessageBox.ts'
import ErrorMessage from '@/components/commons/ErrorMessage.vue'
import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'

export default defineComponent({
  name: 'sign-in',
  components: {
    RouterLink,
    ErrorMessage
  },
  setup() {
    const account = ref('')
    const password = ref('')
    const errors = ref({})

    const signIn = async () => {
      try {
        const axiosInstance = axios.create({
          withCredentials: true
        })
        var response = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_USERMANAGE_SIGNINUSER,{
            account: account.value,
            password: password.value
          })

        await MessageBox.showSuccessMessage('登入成功!', false, 1500)

        const accessToken = response.accessToken;
        
        // Access token 儲存於localStorage
        localStorage.setItem('accessToken', accessToken);

        // TODO
        console.log('導向')
        // 測refresh token
        var res = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_USERMANAGE_REFRESHACCESSTOKEN,{})

        console.log(res)
      } catch (ex) {
        errors.value = ex?.response?.data?.errors ?? {}
        console.log(errors.value)
      }
    }
    return {
      account,
      password,
      signIn,
      errors
    }
  }
})
</script>
