<template>
  <div v-if="action == 'create'" class="container">
    <div class="page mt-5 p-5">
      <div class="row">
        <div class="col-12">
          <h1 class="mb-4 main-color01">出貨單新增</h1>
        </div>
      </div>
      <div class="row" id="Form">
        <div class="col-12 col-md-6 mb-3">
          <label>出貨單編號</label>
          <input disabled type="text" class="form-control" />
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>出貨日</label>
          <date-picker
            class="form-control"
            placeholder="選擇出貨日"
            date-format="yy-mm-dd"
            v-model="DeliveryDate"
          ></date-picker>
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>出貨地址</label>
          <input class="form-control" placeholder="地址" type="text" v-model="Address" />
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>完成日</label>
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>總金額(自動計算)</label>
          <input type="text" class="form-control" />
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>簽收者</label>
          <input type="text" class="form-control" />
        </div>
        <div class="col-12 mb-3">
          <label>備註</label>
          <textarea class="form-control"></textarea>
        </div>
      </div>
    </div>
    <div class="page p-5">
      <div class="row">
        <div class="col-12">
          <div>
            <table class="table table-bordered">
              <thead>
                <tr>
                  <th>#</th>
                  <th>(商品編號)商品名稱</th>
                  <th>價格</th>
                  <th>數量</th>
                  <th>備註</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item, index) in OrderDetails">
                  <td>
                    <span v-text="index + 1"></span>
                  </td>
                  <td>
                    (<span v-text="item.ProductNumber"></span>)
                    <span v-text="item.ProductName"></span>
                  </td>
                  <td>
                    <span v-text="item.ProductPrice"></span>
                  </td>
                  <td>
                    <span v-text="item.ProductUnit"></span>
                  </td>
                  <td>
                    <span v-text="item.ProductRemarks"></span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div class="col-12">
          <div class="text-right my-3">
            <button @click="updateUser" type="button" class="btn btn-main-color01 mr-2">
              新增
            </button>
            <button
              onclick="history.go(-1);"
              type="button"
              class="btn btn-main-color02 outline-btn"
            >
              返回
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div v-if="action == 'edit'" class="container">
    <div class="page mt-5 p-5">
      <div class="row">
        <div class="col-12">
          <h1 class="mb-4 main-color01">出貨單編輯</h1>
        </div>
      </div>
      <div class="row" id="Form">
        <div class="col-12 col-md-6 mb-3">
          <label>出貨單編號</label>
          <input disabled type="text" class="form-control" />
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>出貨日</label>
          <date-picker
            class="form-control"
            placeholder="選擇出貨日"
            date-format="yy-mm-dd"
            v-model="DeliveryDate"
          ></date-picker>
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>出貨地址</label>
          <input class="form-control" placeholder="地址" type="text" v-model="Address" />
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>完成日</label>
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>總金額(自動計算)</label>
          <input type="text" class="form-control" />
        </div>
        <div class="col-12 col-md-6 mb-3">
          <label>簽收者</label>
          <input type="text" class="form-control" />
        </div>
        <div class="col-12 mb-3">
          <label>備註</label>
          <textarea class="form-control"></textarea>
        </div>
      </div>
    </div>
    <div class="page p-5">
      <div class="row">
        <div class="col-12">
          <div>
            <table class="table table-bordered">
              <thead>
                <tr>
                  <th>#</th>
                  <th>(商品編號)商品名稱</th>
                  <th>價格</th>
                  <th>數量</th>
                  <th>備註</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item, index) in OrderDetails">
                  <td>
                    <span v-text="index + 1"></span>
                  </td>
                  <td>
                    (<span v-text="item.ProductNumber"></span>)
                    <span v-text="item.ProductName"></span>
                  </td>
                  <td>
                    <span v-text="item.ProductPrice"></span>
                  </td>
                  <td>
                    <span v-text="item.ProductUnit"></span>
                  </td>
                  <td>
                    <span v-text="item.ProductRemarks"></span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div class="col-12">
          <div class="text-right my-3">
            <button @click="updateUser" type="button" class="btn btn-main-color01 mr-2">
              更新
            </button>
            <button
              onclick="history.go(-1);"
              type="button"
              class="btn btn-main-color02 outline-btn"
            >
              返回
            </button>
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
import CustomModal, {
  toggleModal,
  showModal,
  hideModal
} from '@/components/commons/CustomModal.vue'
import ErrorMessage from '@/components/commons/ErrorMessage.vue'
import MessageBox from '@/utils/MessageBox.ts'
import DateTool from '@/utils/DateTool.ts'
import VueMultiselect from 'vue-multiselect'
import { useRoute } from 'vue-router'

export default defineComponent({
  name: 'user-edit',
  components: {
    Pagination,
    Loading,
    CustomModal,
    ErrorMessage,
    VueMultiselect
  },
  setup() {
    const isLoading = ref(false)
    const route = useRoute()
    const userId = ref(route.query.id)
    const action = ref(route.query.action)
    const userInfoData = ref({})
    const errors = ref({})

    const fetchData = async () => {
      isLoading.value = true
      var res = await Promise.all([
        await HttpClient.post(import.meta.env.VITE_APP_AXIOS_USERMANAGE_GETUSERINFO, {
          id: userId.value
        })
      ])
      userInfoData.value = res[0]
      isLoading.value = false
    }
    fetchData()

    const updateUser = async () => {
      try {
        const m = userInfoData.value
        const response = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_USERMANAGE_UPDATEUSER,
          {
            id: m.id,
            name: m.name,
            email: m.email
          }
        )
        await MessageBox.showSuccessMessage('更新成功')
        errors.value = {}
      } catch (ex) {
        errors.value = ex?.response?.data?.errors ?? {}
      }
    }

    return {
      isLoading,
      DateTool,
      userInfoData,
      updateUser,
      errors,
      action
    }
  }
})
</script>
