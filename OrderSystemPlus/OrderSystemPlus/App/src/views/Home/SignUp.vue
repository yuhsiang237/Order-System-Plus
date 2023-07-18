<template>
  <div class="container-fluid sign-up-page">
    <div class="row">
      <div
        class="col-12 col-md-6"
        style="
          background: url('/images/banner_outside.png');
          background-size: cover;
          background-repeat: no-repeat;
          background-position: center center;
          min-height: 100vh;
        "
      ></div>
      <div class="col-12 col-md-6">
        <div class="form-block">
          <h1>註冊</h1>
          <div>
            <div class="mb-2">
              <label>姓名</label>
              <input class="form-control" v-model="name" />
              <span class="error-message"></span>
            </div>
            <div class="mb-2">
              <label>帳號</label>
              <input class="form-control" v-model="account" />
              <span class="error-message"></span>
            </div>
            <div class="mb-2">
              <label>密碼</label>
              <input class="form-control" type="password" v-model="password" />
            </div>
            <div class="mb-2">
              <label>再輸入一次密碼</label>
              <input class="form-control" type="password" v-model="password2" />
            </div>
            <div class="mb-3">
              <label>信箱</label>
              <input class="form-control" v-model="email" />
            </div>
            <div>
              <input type="submit" value="註冊" class="btn btn-main-color01" @click="register" />
            </div>
          </div>
          <MessageBox></MessageBox>
          <div class="text-muted">
            <RouterLink to="/Home/SignIn">返回登入</RouterLink>
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
import MessageBox from '@/utils/MessageBox.ts'

export default defineComponent({
  name: 'sign-up',
  components: {
    RouterLink
  },
  setup() {
    const account = ref('')
    const name = ref('')
    const email = ref('')
    const password = ref('')
    const password2 = ref('')

    const register = async () => {
      const response = await HttpClient.post(import.meta.env.VITE_APP_AXIOS_USERMANAGE_CREATEUSER, {
        name: name.value,
        email: email.value,
        account: account.value,
        password: password.value
      })
      MessageBox.showSuccessMessage('註冊成功')
    }
    return {
      account,
      name,
      email,
      password,
      password2,
      register
    }
  }
})
</script>
