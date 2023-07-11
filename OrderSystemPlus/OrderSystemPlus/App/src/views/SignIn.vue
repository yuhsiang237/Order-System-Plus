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
            <div class="text-muted">還沒有帳號嗎?<a href="/User/SignUp">註冊帳號</a></div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
