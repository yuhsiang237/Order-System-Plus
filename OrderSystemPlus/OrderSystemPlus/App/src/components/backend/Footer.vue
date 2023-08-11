<template>
  <footer class="footer text-muted"></footer>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'backend-footer',
  props: {
    errors: {
      type: Array
    }
  },
  setup() {
    return {}
  }
})
</script>
