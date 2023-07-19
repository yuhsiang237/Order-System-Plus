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
              <error-message :errors="errors.name" />
            </div>
            <div class="mb-2">
              <label>帳號</label>
              <input class="form-control" v-model="account" />
              <error-message :errors="errors.account" />
            </div>
            <div class="mb-2">
              <label>密碼</label>
              <input class="form-control" type="password" v-model="password" />
              <error-message :errors="errors.password" />
            </div>
            <div class="mb-2">
              <label>再輸入一次密碼</label>
              <input class="form-control" type="password" v-model="password2" />
              <error-message :errors="errors.password" />
              <error-message :errors="errors.password2" />
            </div>
            <div class="mb-3">
              <label>信箱</label>
              <input class="form-control" v-model="email" />
              <error-message :errors="errors.email" />
            </div>
            <div>
              <input type="submit" value="註冊" class="btn btn-main-color01" @click="register" />
            </div>
          </div>
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
import ErrorMessage from '@/components/commons/ErrorMessage.vue'

export default defineComponent({
  name: 'sign-up',
  components: {
    RouterLink,
    ErrorMessage
  },
  setup() {
    const account = ref('')
    const name = ref('')
    const email = ref('')
    const password = ref('')
    const password2 = ref('')
    const errors = ref({})

    const validate = (): boolean => {
      errors.value = {}
      if (password.value !== password2.value) errors.value.password2 = ['輸入的密碼不相符']

      return Object.keys(errors.value).length > 0 ? false : true
    }

    const register = async () => {
      try {
        const validateResult = await validate()
        if (!validateResult) return

        const response = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_USERMANAGE_CREATEUSER,
          {
            name: name.value,
            email: email.value,
            account: account.value,
            password: password.value
          }
        )
        const isConfirm = (await MessageBox.showSuccessMessage('註冊成功!將導向登入頁面'))
          ?.isConfirmed
        if (isConfirm) window.location.href = '/Home/SignIn'
      } catch (ex) {
        errors.value = ex?.response?.data?.errors ?? {}
        console.log(errors.value)
      }
    }
    return {
      account,
      name,
      email,
      password,
      password2,
      register,
      errors
    }
  }
})
</script>
