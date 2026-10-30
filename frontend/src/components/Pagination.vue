<script setup>
const props = defineProps({
  page: { type: Number, required: true },
  totalPages: { type: Number, required: true },
})
const emit = defineEmits(['update:page'])

function ir(pagina) {
  if (pagina < 1 || pagina > props.totalPages || pagina === props.page) return
  emit('update:page', pagina)
}
</script>

<template>
  <nav class="pagination" aria-label="Paginación de resultados">
    <button class="btn btn-ghost" :disabled="page === 1" @click="ir(page - 1)">← Anterior</button>
    <span class="pagination__status">Página {{ page }} de {{ totalPages }}</span>
    <button class="btn btn-ghost" :disabled="page === totalPages" @click="ir(page + 1)">Siguiente →</button>
  </nav>
</template>

<style scoped>
.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1.2rem;
  margin-top: 2rem;
}

.pagination__status {
  font-family: var(--font-mono);
  font-size: 0.82rem;
  color: var(--text-muted);
}

.pagination button:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}
</style>
