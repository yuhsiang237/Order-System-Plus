<template>
  <div class="modal fade" :id="modalname" tabindex="-1" aria-hidden="true">
    <slot></slot>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, toRefs, defineEmits } from 'vue'

const toggleModal = (modalname: string) => {
  $('#' + modalname).modal('toggle')
}
const hideModal = (modalname: string) => {
  $('#' + modalname).modal('hide')
}
const showModal = (modalname: string) => {
  $('#' + modalname).modal('show')
}

export { toggleModal, hideModal, showModal }

export default defineComponent({
  name: 'custom-modal',
  emits: {},
  props: {
    modalname: String
  },
  setup(props) {
    const { modalname } = toRefs(props)
    return {
      modalname
    }
  }
})
</script>
