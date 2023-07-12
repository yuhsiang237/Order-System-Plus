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
          <form>
            <div class="mb-3">
              <label>帳號</label>
              <input class="form-control" />
            </div>
            <div class="mb-3">
              <label>密碼</label>
              <input class="form-control" type="password" />
            </div>
            <div class="mb-3">
              <input type="submit" class="btn btn-main-color01" value="登入" />
            </div>

            <div class="text-muted">
              還沒有帳號嗎? <RouterLink to="/Home/SignUp">註冊帳號</RouterLink>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import { RouterLink } from 'vue-router'

export default defineComponent({
  name: 'sign-up',
  components: {
    RouterLink
  },
  setup() {
    return {}
  }
})
</script>
