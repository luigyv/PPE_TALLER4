<script setup>
import { RouterView } from 'vue-router'
import AppHeader from './components/AppHeader.vue'
</script>

<template>
  <AppHeader />
  <main class="container" style="flex: 1; padding-top: 2rem; padding-bottom: 3rem">
    <RouterView />
  </main>
</template>
