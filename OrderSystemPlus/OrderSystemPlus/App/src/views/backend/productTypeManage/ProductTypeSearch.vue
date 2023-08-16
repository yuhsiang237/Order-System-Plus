<template>
  <div>
    <div>
      <div class="search-area">
        <div class="container">
          <div class="row">
            <div class="col">
              <div>
                <h1 class="pt-5 pb-3 main-color01">商品分類列表</h1>
              </div>
              <div class="row">
                <div class="col-sm col-md-2 col-lg-2 col-xl-2">
                  <label class="custom-label">分類名稱</label>
                  <div class="form-group form-row">
                    <div class="col">
                      <input
                        id="input_name"
                        type="text"
                        class="form-control mr-2"
                        name="searchStringName"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div class="text-right">
                <button @click="search" class="btn btn-main-color01 mb-2 mr-1">查詢資料</button>
                <button id="btn-clearSearch" class="btn btn-main-color02 outline-btn mb-2">
                  清空查詢
                </button>
              </div>
              <hr class="mt-0" />
              <div class="d-flex justify-content-end">
                <div class="col-12 col-sm-2 px-0">
                  <div class="form-group">
                    <select class="form-control" name="sortOrder">
                      <option value="0">預設排序</option>
                      <option value="1">商品名稱 高→低</option>
                      <option value="2">商品名稱 低→高</option>
                      <option value="3">商品庫存量 高→低</option>
                      <option value="4">商品庫存量 低→高</option>
                      <option value="5">商品價格 高→高</option>
                      <option value="6">商品價格 低→高</option>
                    </select>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="container">
        <div class="row my-3">
          <div class="col">
            <div class="text-right">
              <button id="btn_productCategoryModal" type="button" class="btn btn-main-color01">
                新增分類
              </button>
            </div>
          </div>
        </div>
        <div class="row">
          <div class="col">
            <div class="table-responsive" id="productTable">
              <table class="table table-hover">
                <thead>
                  <tr>
                    <th>分類名稱</th>
                    <th>描述</th>
                    <th>操作</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, index) in listData">
                    <td>{{ item.name }}</td>
                    <td>{{ item.description }}</td>
                    <td>
                      <button type="button" class="mr-1 btn btn-main-color02 outline-btn">
                        編輯
                      </button>
                      <button type="button" class="btn btn-red">刪除</button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>

      <div class="list-pagination mt-3">
        <div class="form-inline text-center">
          <div class="mx-auto">
            每頁
            <select
              @change="pageSizeChange($event)"
              class="custom-select"
              name="pageSize"
              v-model="pageSize"
            >
              <option value="3">3</option>
              <option value="10">10</option>
              <option value="30">30</option>
              <option value="50">50</option></select
            >，第 <span>{{ pageIndex }}</span> 頁，共
            <span>{{ Math.ceil(totalCount / pageSize) }}</span> 頁，
            <span v-if="pageIndex - 1 > 0">
              <a class="btn btn-outline-secondary btn-sm @prevDisabled" @click="prevPage">
                上一頁
              </a>
              跳至第 ｜</span
            >
            <select
              @change="goPage($event)"
              v-model="pageIndex"
              class="custom-select"
              name="goToPageNumber"
            >
              <option
                v-for="(item, index) in Array.from(
                  { length: Math.ceil(totalCount / pageSize) },
                  (_, i) => i + 1
                )"
                :value="item"
              >
                {{ item }}
              </option>
            </select>
            頁
            <span v-if="pageIndex + 1 <= Math.ceil(totalCount / pageSize)"
              >｜
              <a class="btn btn-outline-secondary btn-sm @nextDisabled" @click="nextPage">
                下一頁
              </a>
            </span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import HttpClient from '@/utils/HttpClient.ts'

export default defineComponent({
  name: 'productTypeSearch',
  components: {},
  setup() {
    const pageIndex = ref(1)
    const pageSize = ref(3)
    const totalCount = ref(0)
    const listData = ref([])

    const goPage = async (event) => {
      pageIndex.value = Number(event.target.value)
      await search()
    }
    const prevPage = async () => {
      pageIndex.value = pageIndex.value - 1
      await search()
    }
    const nextPage = async () => {
      pageIndex.value = pageIndex.value + 1
      await search()
    }
    const search = async () => {
      var res = await HttpClient.post(
        import.meta.env.VITE_APP_AXIOS_PRODUCTTYPEMANAGE_GETPRODUCTTYPELIST,
        {
          pageSize: pageSize.value,
          pageIndex: pageIndex.value
        }
      )
      totalCount.value = res.totalCount
      listData.value = res.data
      console.log(res)
    }
    const pageSizeChange = async (event) => {
      pageIndex.value = 1
      pageSize.value = event.target.value
      await search()
    }
    return {
      search,
      pageIndex,
      pageSize,
      totalCount,
      prevPage,
      nextPage,
      goPage,
      pageSizeChange,
      listData
    }
  }
})
</script>
