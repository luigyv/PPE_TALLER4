<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  modelValue: { type: String, default: '' },
})
const emit = defineEmits(['update:modelValue'])

const local = ref(props.modelValue)
let timeoutId = null

watch(local, (value) => {
  clearTimeout(timeoutId)
  timeoutId = setTimeout(() => emit('update:modelValue', value), 300)
})

watch(
  () => props.modelValue,
  (value) => {
    if (value !== local.value) local.value = value
  }
)
</script>

<template>
  <div class="search">
    <span class="search__icon" aria-hidden="true">⌕</span>
    <input
      v-model="local"
      type="text"
      placeholder="Buscar por nombre o categoría..."
      class="search__input"
      aria-label="Buscar juegos"
    />
  </div>
</template>

<style scoped>
.search {
  position: relative;
  flex: 1;
  max-width: 420px;
}

.search__icon {
  position: absolute;
  left: 0.8rem;
  top: 50%;
  transform: translateY(-50%);
  color: var(--text-muted);
  font-size: 1.1rem;
}

.search__input {
  width: 100%;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-sm);
  padding: 0.6rem 0.8rem 0.6rem 2.1rem;
  color: var(--text);
  font-size: 0.92rem;
}

.search__input::placeholder {
  color: var(--text-muted);
}
</style>
