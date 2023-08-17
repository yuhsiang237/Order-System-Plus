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
                        v-model="searchfilter.name"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div class="text-right">
                <button @click="search" class="btn btn-main-color01 mb-2 mr-1">查詢資料</button>
                <button @click="resetSearch" class="btn btn-main-color02 outline-btn mb-2">
                  清空查詢
                </button>
              </div>
              <hr class="mt-0" />
              <div class="d-flex justify-content-end">
                <div class="col-12 col-sm-2 px-0">
                  <div class="form-group">
                    <select
                      v-model="currentSort"
                      @change="sortChange($event)"
                      class="form-control"
                      name="sortOrder"
                    >
                      <option v-for="(item, index) in sortData" :value="index">
                        {{ item.label }}
                      </option>
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
                <loading
                  v-model:active="isLoading"
                  :can-cancel="false"
                  :on-cancel="onCancel"
                  :is-full-page="false"
                />
                <tbody v-if="isLoading == false">
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
              <div v-if="isLoading == false && listData.length > 0">
                <pagination
                  @change="pageOnChange"
                  :pageIndex="pageIndex"
                  :pageSize="pageSize"
                  :totalCount="totalCount"
                ></pagination>
              </div>
              <div v-else class="text-center text-muted">
                <h2>沒有資料</h2>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import HttpClient from '@/utils/HttpClient.ts'
import { SortType } from '@/enums/SortType.ts'
import Pagination from '@/components/commons/Pagination.vue'
import Loading from 'vue-loading-overlay'

export default defineComponent({
  name: 'productTypeSearch',
  components: {
    Pagination,
    Loading
  },
  setup() {
    const isLoading = ref(false)
    const searchfilter = ref({})
    const pageIndex = ref(1)
    const pageSize = ref(10)
    const totalCount = ref(0)
    const listData = ref([])
    const currentSort = ref(0)
    const sortData = ref([
      { sortField: null, sortType: null, label: '預設排序' },
      { sortField: 'name', sortType: SortType.DESC, label: '分類名稱 高→低' },
      { sortField: 'name', sortType: SortType.ASC, label: '分類名稱 低→高' }
    ])

    const sortChange = async ($event) => {
      currentSort.value = Number($event.target.value)
      await fetchData()
    }
    const pageOnChange = async ($event) => {
      pageIndex.value = $event.pageIndex
      pageSize.value = $event.pageSize
      await fetchData()
    }
    const search = async () => {
      pageIndex.value = 1
      await fetchData()
    }
    const resetSearch = async () => {
      searchfilter.value = {}
      pageIndex.value = 1
      await fetchData()
    }
    const fetchData = async () => {
      isLoading.value = true
      var res = await HttpClient.post(
        import.meta.env.VITE_APP_AXIOS_PRODUCTTYPEMANAGE_GETPRODUCTTYPELIST,
        {
          name: searchfilter.value?.name,
          pageSize: pageSize.value,
          pageIndex: pageIndex.value,
          sortField: sortData.value[currentSort.value]?.sortField,
          sortType: sortData.value[currentSort.value]?.sortType
        }
      )
      totalCount.value = res.totalCount
      listData.value = res.data
      isLoading.value = false
      console.log(res)
    }

    search()

    return {
      isLoading,
      pageOnChange,
      search,
      pageIndex,
      pageSize,
      totalCount,
      listData,
      sortData,
      sortChange,
      currentSort,
      searchfilter,
      resetSearch
    }
  }
})
</script>
