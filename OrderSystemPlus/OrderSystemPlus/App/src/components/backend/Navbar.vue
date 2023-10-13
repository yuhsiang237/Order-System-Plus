<template>
  <header>
    <nav
      class="navbar navbar-expand-sm navbar-toggleable-sm bg-white navbar-light border-bottom box-shadow"
    >
      <div class="container">
        <a class="navbar-brand" asp-area="" asp-controller="Dashboard" asp-action="Index"
          >OrderSystem</a
        >
        <button
          class="navbar-toggler"
          type="button"
          data-toggle="collapse"
          data-target=".navbar-collapse"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row">
          <ul class="navbar-nav flex-grow-1">
            <li class="nav-item">
              <router-link class="nav-link text-dark" :to="{ name: 'dashboard' }">
                總覽
              </router-link>
            </li>
            <li class="nav-item dropdown">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="navbarDropdownMenuBasicLink"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
                >基本資料管理</a
              >
              <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuBasicLink">
                <router-link class="nav-link text-dark" :to="{ name: 'userSearch' }">
                  使用者管理
                </router-link>

                <a class="nav-link text-dark" asp-area="" asp-controller="Role" asp-action="Search">
                  權限角色管理
                </a>
              </div>
            </li>

            <li class="nav-item dropdown">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="navbarDropdownMenuProductLink"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
                >商品管理</a
              >
              <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuProductLink">
                <router-link class="nav-link text-dark" :to="{ name: 'productTypeSearch' }">
                  商品分類管理
                </router-link>
                <router-link class="nav-link text-dark" :to="{ name: 'productSearch' }">
                  商品管理
                </router-link>
                <router-link class="nav-link text-dark" :to="{ name: 'productInventorySearch' }">
                  商品庫存管理
                </router-link>
              </div>
            </li>

            <li class="nav-item dropdown">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="navbarDropdownMenuOrderLink"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
                >訂單管理</a
              >
              <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuOrderLink">
                <a
                  class="nav-link text-dark"
                  asp-area=""
                  asp-controller="Order"
                  asp-action="SearchShipmentOrder"
                >
                  出貨單管理
                </a>

                <a
                  class="nav-link text-dark"
                  asp-area=""
                  asp-controller="Order"
                  asp-action="SearchReturnShipmentOrder"
                >
                  退貨單管理
                </a>
              </div>
            </li>
          </ul>
          <ul class="navbar-nav mr-auto">
            <li class="nav-item">
              <a class="nav-link text-dark" asp-area="" asp-controller="User" asp-action="SignUp"
                >註冊</a
              >
            </li>
            <li class="nav-item">
              <a class="nav-link text-dark" asp-area="" asp-controller="User" asp-action="SignIn"
                >登入</a
              >
            </li>

            <li class="nav-item dropdown">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                id="navbarDropdownMenuUserLink"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
                >您好，@Account
              </a>
              <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuUserLink">
                <a
                  class="nav-link text-dark"
                  asp-area=""
                  asp-controller="User"
                  asp-action="UpdateUserSelf"
                >
                  個人資料
                </a>

                <a class="nav-link text-dark"> 登出 </a>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </header>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'backend-navbar',
  setup() {
    return {}
  }
})
</script>
