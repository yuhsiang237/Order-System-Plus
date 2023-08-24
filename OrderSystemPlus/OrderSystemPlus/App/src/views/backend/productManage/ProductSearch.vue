<template>
  <div>
    <div>
      <div class="search-area">
        <div class="container">
          <div class="row">
            <div class="col">
              <div>
                <h1 class="pt-5 pb-3 main-color01">商品列表</h1>
              </div>
              <div class="row">
                <div class="col-sm col-md-2 col-lg-2 col-xl-2">
                  <label class="custom-label">商品名稱</label>
                  <div class="form-group form-row">
                    <div class="col">
                      <input type="text" class="form-control mr-2" v-model="searchfilter.name" />
                    </div>
                  </div>
                </div>
                <div class="col-sm col-md-2 col-lg-2 col-xl-2">
                  <label class="custom-label">商品編號</label>
                  <div class="form-group form-row">
                    <div class="col">
                      <input type="text" class="form-control mr-2" v-model="searchfilter.number" />
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
              <button @click="openAddModel" type="button" class="btn btn-main-color01">
                新增商品
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
                    <th>名稱</th>
                    <th>編號</th>
                    <th>價格</th>
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
                    <td>{{ item.number }}</td>
                    <td>{{ item.price }}</td>
                    <td>{{ item.description }}</td>
                    <td>
                      <button
                        type="button"
                        class="mr-1 btn btn-main-color02 outline-btn"
                        @click="openUpdateModel(item)"
                      >
                        編輯
                      </button>
                      <button type="button" class="btn btn-red" @click="deleteProductType(item)">
                        刪除
                      </button>
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
  <custom-modal modalname="createModal">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">新增商品</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body">
          <div class="mb-2">
            <label>商品名稱</label>
            <input class="form-control" v-model="reqCreate.name" />
            <error-message :errors="errorCreate.name" />
          </div>
          <div class="mb-2">
            <label>商品編號</label>
            <input class="form-control" v-model="reqCreate.number" />
            <error-message :errors="errorCreate.number" />
          </div>
          <div class="mb-2">
            <label>商品價格</label>
            <input class="form-control" v-model="reqCreate.price" />
            <error-message :errors="errorCreate.price" />
          </div>
          <div class="mb-2">
            <label>商品分類</label>
            <VueMultiselect
              v-model="reqCreate.productTypeIds"
              :options="productTypeOption"
              :multiple="true"
              :taggable="true"
              placeholder="請選擇"
              label="name"
              track-by="id"
            >
            </VueMultiselect>
            <error-message :errors="errorCreate.ProductTypeIds" />
          </div>
          <div class="mb-2">
            <label>商品庫存</label>
            <input class="form-control" v-model="reqCreate.quantity" />
            <error-message :errors="errorCreate.quantity" />
          </div>
          <div class="mb-2">
            <label>描述</label>
            <input class="form-control" v-model="reqCreate.description" />
            <error-message :errors="errorCreate.description" />
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="mr-1 btn btn-main-color02 outline-btn" data-dismiss="modal">
            取消
          </button>
          <button type="button" class="btn btn-main-color01" @click="createProductType">
            新增
          </button>
        </div>
      </div>
    </div>
  </custom-modal>
  <custom-modal modalname="updateModal">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">更新商品</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body">
          <div class="mb-2">
            <label>商品名稱</label>
            <input class="form-control" v-model="reqUpdate.name" />
            <error-message :errors="errorUpdate.name" />
          </div>
          <div class="mb-2">
            <label>描述</label>
            <input class="form-control" v-model="reqUpdate.description" />
            <error-message :errors="errorUpdate.description" />
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="mr-1 btn btn-main-color02 outline-btn" data-dismiss="modal">
            取消
          </button>
          <button type="button" class="btn btn-main-color01" @click="updateProductType">
            更新
          </button>
        </div>
      </div>
    </div>
  </custom-modal>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import HttpClient from '@/utils/HttpClient.ts'
import { SortType } from '@/enums/SortType.ts'
import Pagination from '@/components/commons/Pagination.vue'
import Loading from 'vue-loading-overlay'
import CustomModal, {
  toggleModal,
  showModal,
  hideModal
} from '@/components/commons/CustomModal.vue'
import ErrorMessage from '@/components/commons/ErrorMessage.vue'
import MessageBox from '@/utils/MessageBox.ts'
import VueMultiselect from 'vue-multiselect'

export default defineComponent({
  name: 'productTypeSearch',
  components: {
    Pagination,
    Loading,
    CustomModal,
    ErrorMessage,
    VueMultiselect
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
      { sortField: 'number', sortType: SortType.DESC, label: '商品編號 高→低' },
      { sortField: 'number', sortType: SortType.ASC, label: '商品編號 低→高' },
      { sortField: 'name', sortType: SortType.DESC, label: '商品名稱 高→低' },
      { sortField: 'name', sortType: SortType.ASC, label: '商品名稱 低→高' },
      { sortField: 'price', sortType: SortType.DESC, label: '商品價格 高→低' },
      { sortField: 'price', sortType: SortType.ASC, label: '商品價格 低→高' }
    ])
    const productTypeOption = ref([])
    const reqCreate = ref({})
    const errorCreate = ref({})

    const reqUpdate = ref({})
    const errorUpdate = ref({})

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
      var res = await HttpClient.post(import.meta.env.VITE_APP_AXIOS_PRODUCTMANAGE_GETPRODUCTLIST, {
        name: searchfilter.value?.name,
        number: searchfilter.value?.number,
        pageSize: pageSize.value,
        pageIndex: pageIndex.value,
        sortField: sortData.value[currentSort.value]?.sortField,
        sortType: sortData.value[currentSort.value]?.sortType
      })

      productTypeOption.value = (
        await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_PRODUCTTYPEMANAGE_GETPRODUCTTYPELIST,
          {}
        )
      ).data

      totalCount.value = res.totalCount
      listData.value = res.data
      isLoading.value = false
      console.log(res)
    }
    const openAddModel = () => {
      reqCreate.value = {
        name: '',
        number: '',
        description: '',
        price: null,
        quantity: null,
        productTypeIds: []
      }
      errorCreate.value = {}
      showModal('createModal')
    }
    const openUpdateModel = (item) => {
      reqUpdate.value = Object.assign({}, item)
      errorUpdate.value = {}
      showModal('updateModal')
    }
    const createProductType = async () => {
      const data = reqCreate.value
      try {
        const productTypeIds = []
        reqCreate.value.productTypeIds.forEach((item) => {
          productTypeIds.push(item.id)
        })
        const res = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_PRODUCTMANAGE_CREATEPRODUCT,
          {
            name: data.name,
            number: data.number,
            description: data.description,
            price: data.price,
            quantity: data.quantity,
            productTypeIds: productTypeIds
          }
        )
        hideModal('createModal')
        await MessageBox.showSuccessMessage('建立成功', false, 800)
        search()
      } catch (ex) {
        errorCreate.value = ex?.response?.data?.errors ?? {}
      }
    }

    const updateProductType = async () => {
      const data = reqUpdate.value
      try {
        var res = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_PRODUCTMANAGE_UPDATEPRODUCT,
          {
            id: data.id,
            name: data.name,
            description: data.description
          }
        )
        hideModal('updateModal')
        await MessageBox.showSuccessMessage('更新成功', false, 800)
        fetchData()
      } catch (ex) {
        errorUpdate.value = ex?.response?.data?.errors ?? {}
      }
    }
    const deleteProductType = async (item) => {
      if (!confirm('確定要刪除' + item.name)) return

      try {
        var res = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_PRODUCTMANAGE_DELETEPRODUCT,
          [
            {
              id: item.id
            }
          ]
        )
        await MessageBox.showSuccessMessage('刪除成功', false, 800)
        await fetchData()
      } catch (ex) {
        errorCreate.value = ex?.response?.data?.errors ?? {}
      }
    }
    search()

    return {
      productTypeOption,
      openUpdateModel,
      openAddModel,
      createProductType,
      updateProductType,
      deleteProductType,
      reqCreate,
      reqUpdate,
      errorCreate,
      errorUpdate,
      showModal,
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
