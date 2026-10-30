<script setup>
import { RouterLink } from 'vue-router'
</script>

<template>
  <section class="not-found">
    <h2>404</h2>
    <p>Esta página no existe.</p>
    <RouterLink :to="{ name: 'juegos-lista' }" class="btn btn-primary">Volver al catálogo</RouterLink>
  </section>
</template>

<style scoped>
.not-found {
  text-align: center;
  padding: 4rem 0;
}

.not-found h2 {
  font-size: 3rem;
  color: var(--accent);
}

.not-found p {
  color: var(--text-muted);
  margin: 0.5rem 0 1.5rem;
}
</style>
