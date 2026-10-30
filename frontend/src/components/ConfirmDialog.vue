<script setup>
defineProps({
  visible: { type: Boolean, default: false },
  titulo: { type: String, default: '¿Estás seguro?' },
  mensaje: { type: String, default: '' },
})
defineEmits(['confirmar', 'cancelar'])
</script>

<template>
  <div v-if="visible" class="overlay" @click.self="$emit('cancelar')">
    <div class="dialog" role="dialog" aria-modal="true">
      <h3>{{ titulo }}</h3>
      <p class="dialog__msg">{{ mensaje }}</p>
      <div class="dialog__actions">
        <button class="btn btn-ghost" @click="$emit('cancelar')">Cancelar</button>
        <button class="btn btn-danger" @click="$emit('confirmar')">Eliminar</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.55);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.dialog {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-md);
  padding: 1.5rem;
  width: min(360px, 90vw);
}

.dialog__msg {
  color: var(--text-muted);
  font-size: 0.9rem;
  margin: 0.5rem 0 1.2rem;
}

.dialog__actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.6rem;
}
</style>
