<template>
  <div class="container">
    <div class="page p-5">
      <div class="row">
        <div class="col-12">
          <h1 class="mb-4 main-color01">庫存歷程管理</h1>
        </div>
      </div>
      <div class="row" id="Form">
        <div class="col-6 mb-3">
          <label>商品名稱</label>
          <div>{{ productInfoData.name }}</div>
        </div>
        <div class="col-6 mb-3">
          <label>商品編號</label>
          <div>{{ productInfoData.number }}</div>
        </div>
        <div class="col-6 mb-3">
          <label>目前數量</label>
          <div>{{ productInfoData.quantity }}</div>
        </div>
      </div>
      <div class="row">
        <div class="col-12 text-right">
          <button
            type="button"
            class="mr-1 btn btn-main-color02 outline-btn"
            @click="openUpdateModel"
          >
            手動調整庫存
          </button>
        </div>
      </div>

      <div class="row">
        <div class="col-12">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>異動時間</th>
                <th>異動數量</th>
                <th>異動描述</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in historyList">
                <td>
                  {{ DateTool.toDateString(item.createdOn) }}
                </td>
                <td><span v-if="item.adjustQuantity >= 0">+</span>{{ item.adjustQuantity }}</td>
                <td>
                  {{ item.remark }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="col-12">
          <div class="text-right my-3">
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
    <custom-modal modalname="updateModal">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">手動調整庫存</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <div class="mb-2">
              <label>調整方式</label>
              <vue-multiselect
                v-model="reqUpdate.type"
                deselect-label=""
                track-by="value"
                label="label"
                placeholder="Select one"
                :options="adjustProductInventoryTypeOption"
                :searchable="true"
                :allow-empty="false"
              >
              </vue-multiselect>
              <error-message :errors="errorUpdate['x[0].type']" />
            </div>
            <div class="mb-2">
              <label>數量</label>
              <input class="form-control" v-model="reqUpdate.adjustQuantity" />
              <error-message :errors="errorUpdate['x[0].adjustQuantity']" />
            </div>
            <div class="mb-2">
              <label>描述</label>
              <input class="form-control" v-model="reqUpdate.description" />
              <error-message :errors="errorUpdate['x[0].description']" />
            </div>
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="mr-1 btn btn-main-color02 outline-btn"
              data-dismiss="modal"
            >
              取消
            </button>
            <button type="button" class="btn btn-main-color01" @click="updateInventory">
              更新
            </button>
          </div>
        </div>
      </div>
    </custom-modal>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import HttpClient from '@/utils/HttpClient.ts'
import { AdjustProductInventoryTypeMap } from '@/enums/AdjustProductInventoryType.ts'
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
  name: 'productInventory-edit',
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
    const productId = ref(route.query.id)
    const historyList = ref([])
    const productInfoData = ref([])

    const reqUpdate = ref({})
    const errorUpdate = ref({})
    const adjustProductInventoryTypeOption = ref(
      Array.from(AdjustProductInventoryTypeMap.entries()).map(([key, value]) => ({
        label: value,
        value: key.toString()
      }))
    )

    const fetchData = async () => {
      isLoading.value = true
      var res = await Promise.all([
        await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_PRODUCTINVENTORYMANAGE_GETPRODUCTINVENTORYHISTORYLIST,
          {
            productId: productId.value
          }
        ),
        await HttpClient.post(import.meta.env.VITE_APP_AXIOS_PRODUCTMANAGE_GETPRODUCTINFO, {
          id: productId.value
        })
      ])
      historyList.value = res[0]
      productInfoData.value = res[1]
      isLoading.value = false
    }

    const openUpdateModel = () => {
      reqUpdate.value = { type: adjustProductInventoryTypeOption.value[0] }
      errorUpdate.value = {}
      showModal('updateModal')
    }
    const updateInventory = async () => {
      const data = reqUpdate.value
      try {
        var res = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_PRODUCTINVENTORYMANAGE_UPDATEPRODUCTINVENTORY,
          [
            {
              type: Number(data?.type?.value),
              productId: productId.value,
              adjustQuantity: Number(data?.adjustQuantity),
              description: data.description
            }
          ]
        )
        hideModal('updateModal')
        await MessageBox.showSuccessMessage('更新成功', false, 800)
        await fetchData()
      } catch (ex) {
        errorUpdate.value = ex?.response?.data?.errors ?? {}
      }
    }

    fetchData()
    return {
      updateInventory,
      AdjustProductInventoryTypeMap,
      reqUpdate,
      errorUpdate,
      openUpdateModel,
      isLoading,
      DateTool,
      historyList,
      productInfoData,
      adjustProductInventoryTypeOption
    }
  }
})
</script>
