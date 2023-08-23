<template>
  <div class="list-pagination mt-3">
    <div class="form-inline text-center">
      <div class="mx-auto">
        每頁
        <select
          @change="pageSizeChange($event)"
          class="custom-select"
          name="pageSize"
          :value="pageSize"
        >
          <option v-for="item in pageSizeOption">{{ item }}</option></select
        >，第 <span>{{ pageIndex }}</span> 頁，共
        <span>{{ Math.ceil(totalCount / pageSize) }}</span> 頁，
        <span v-if="pageIndex - 1 > 0">
          <a class="btn btn-outline-secondary btn-sm" @click="prevPage"> 上一頁 </a>
          跳至第 ｜</span
        >
        <select
          @change="goPage($event)"
          :value="pageIndex"
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
          <a class="btn btn-outline-secondary btn-sm" @click="nextPage"> 下一頁 </a>
        </span>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, toRefs, defineEmits } from 'vue'

export default defineComponent({
  name: 'pagination',
  emits: {
    change: (pageIndex: Number, pageSize: Number) => true
  },
  props: {
    pageIndex: Number,
    pageSize: Number,
    totalCount: Number
  },
  setup(props, { emit }) {
    const { pageIndex, pageSize, totalCount } = toRefs(props)

    const pageSizeOption = ref([10, 15, 30, 50])

    const goPage = async (event) => {
      emit('change', { pageIndex: Number(event.target.value), pageSize: pageSize.value })
    }
    const prevPage = async () => {
      emit('change', { pageIndex: pageIndex.value - 1, pageSize: pageSize.value })
    }
    const nextPage = async () => {
      emit('change', { pageIndex: pageIndex.value + 1, pageSize: pageSize.value })
    }
    const pageSizeChange = async (event) => {
      emit('change', {
        pageIndex: 1,
        pageSize: Number(event.target.value)
      })
    }

    return {
      pageIndex,
      pageSize,
      totalCount,
      pageSizeOption,
      pageSizeChange,
      nextPage,
      prevPage,
      goPage
    }
  }
})
</script>
