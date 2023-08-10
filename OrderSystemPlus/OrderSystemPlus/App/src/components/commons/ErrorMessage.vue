<template>
  <span>
    <div class="error-message" v-for="(value, key) in errors">{{ value }}</div>
  </span>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'error-message',
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
