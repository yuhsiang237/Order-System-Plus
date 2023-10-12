<template>
  <div class="container">
    <div class="page p-5">
      <div class="row">
        <div class="col-12">
          <h1 class="mb-4 main-color01">使用者編輯</h1>
        </div>
      </div>
      <div class="row" id="Form">
        <div class="col-12 col-md-6">
          <div class="mb-2">
            <label>帳號</label>
            <div>{{userInfoData.account}}</div>
          </div>
          <div class="mb-2">
            <label>姓名</label>
            <input class="form-control" v-model="userInfoData.name" />
            <error-message :errors="errors.name" />
          </div>
          <div class="mb-3">
            <label>信箱</label>
            <input class="form-control" v-model="userInfoData.email" />
            <error-message :errors="errors.email" />
          </div>
        </div>
       
      </div>
      <div class="row">
        <div class="col-12">
          <div class="text-right my-3">
            <button
            @click="updateUser"
            type="button"
            class="btn btn-main-color01 mr-2"
          >
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
    const userInfoData = ref({})
    const errors = ref({})

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
    
    const updateUser= async () => {
      try {
        const m = userInfoData.value
        const response = await HttpClient.post(
          import.meta.env.VITE_APP_AXIOS_USERMANAGE_UPDATEUSER,
          {
            id:m.id,
            name: m.name,
            email: m.email,
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
      errors
    }
  }
})
</script>
