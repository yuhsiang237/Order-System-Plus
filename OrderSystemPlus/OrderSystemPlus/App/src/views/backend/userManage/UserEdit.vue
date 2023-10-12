<template>
  <div class="container">
    <div class="page p-5">
      <div class="row">
        <div class="col-12">
          <h1 class="mb-4 main-color01">使用者編輯</h1>
        </div>
      </div>
      <div class="row" id="Form">
       
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
                <td>
                  <span v-if="item.adjustQuantity >= 0">+</span>
                  <span v-if="item.adjustQuantity < 0">-</span>{{ item.adjustQuantity }}
                </td>
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
    const userInfoData = ref({})

    const fetchData = async () => {
      isLoading.value = true
      var res = await Promise.all([
        await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_USERMANAGE_GETUSERINFO,
          {
            id: userId.value
          }
        )
      ])
      userInfoData.value = res[0]
      isLoading.value = false
    }
    fetchData()
    return {
      isLoading,
      DateTool,
      userInfoData
    }
  }
})
</script>
