<script setup>
import { RouterLink } from 'vue-router'

defineProps({
  juego: { type: Object, required: true },
  puedeEditar: { type: Boolean, default: false },
})
defineEmits(['eliminar'])

const coloresCategoria = {
  Acción: '#ff5c7a',
  Aventura: '#38d9a9',
  Estrategia: '#ffb84d',
  Deportes: '#4dabf7',
  Plataformas: '#7c5cff',
  Simulación: '#c084fc',
  Fiesta: '#f472b6',
}

function colorDe(categoria) {
  return coloresCategoria[categoria] || '#9294a6'
}
</script>

<template>
  <article class="card">
    <div class="card__image-wrap">
      <img :src="juego.imagen" :alt="juego.nombre" class="card__image" loading="lazy" />
    </div>
    <div class="card__body">
      <h3 class="card__title">{{ juego.nombre }}</h3>
      <span class="card__tag" :style="{ '--tag-color': colorDe(juego.categoria) }">
        {{ juego.categoria }}
      </span>
    </div>
    <div v-if="puedeEditar" class="card__actions">
      <RouterLink :to="{ name: 'juegos-editar', params: { id: juego.id } }" class="btn btn-ghost">
        Editar
      </RouterLink>
      <button class="btn btn-danger" @click="$emit('eliminar', juego)">Eliminar</button>
    </div>
  </article>
</template>

<style scoped>
.card {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-md);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  transition: border-color 0.15s ease;
}

.card:hover {
  border-color: #3a3d54;
}

.card__image-wrap {
  aspect-ratio: 4 / 3;
  overflow: hidden;
  background: #0d0d14;
}

.card__image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

.card__body {
  padding: 0.9rem 1rem 0.6rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.card__title {
  font-size: 1rem;
  font-weight: 600;
}

.card__tag {
  align-self: flex-start;
  font-family: var(--font-mono);
  font-size: 0.72rem;
  padding: 0.2rem 0.55rem;
  border-radius: 999px;
  background: color-mix(in srgb, var(--tag-color) 18%, transparent);
  color: var(--tag-color);
  border: 1px solid color-mix(in srgb, var(--tag-color) 40%, transparent);
}

.card__actions {
  display: flex;
  gap: 0.5rem;
  padding: 0 1rem 1rem;
}

.card__actions a {
  text-decoration: none;
  flex: 1;
  text-align: center;
}

.card__actions button {
  flex: 1;
}
</style>
