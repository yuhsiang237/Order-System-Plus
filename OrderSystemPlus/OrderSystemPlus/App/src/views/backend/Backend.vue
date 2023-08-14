<template>
  <navbar></navbar>
  <router-view></router-view>
  <backend-footer></backend-footer>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import Footer from '@/components/backend/Footer.vue'
import Navbar from '@/components/backend/Navbar.vue'
import { RouterView } from 'vue-router'

export default defineComponent({
  name: 'backend',
  components: {
    Navbar,
    'backend-footer': Footer,
    RouterView
  },
  setup() {}
})
</script>
