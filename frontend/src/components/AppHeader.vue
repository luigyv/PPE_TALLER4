<script setup>
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const auth = useAuthStore()
const router = useRouter()

function cerrarSesion() {
  auth.logout()
  router.push({ name: 'juegos-lista' })
}
</script>

<template>
  <header class="header">
    <div class="container header__inner">
      <RouterLink :to="{ name: 'juegos-lista' }" class="header__brand">
        <span class="header__brand-mark">▮▮</span>
        <span>Catálogo de Juegos</span>
      </RouterLink>

      <nav class="header__nav">
        <template v-if="auth.estaAutenticado">
          <RouterLink :to="{ name: 'juegos-crear' }" class="btn btn-primary">+ Nuevo juego</RouterLink>
          <button class="btn btn-ghost" @click="cerrarSesion">Cerrar sesión</button>
        </template>
        <template v-else>
          <RouterLink :to="{ name: 'login' }" class="btn btn-ghost">Iniciar sesión</RouterLink>
          <RouterLink :to="{ name: 'registro' }" class="btn btn-primary">Crear cuenta</RouterLink>
        </template>
      </nav>
    </div>
  </header>
</template>

<style scoped>
.header {
  border-bottom: 1px solid var(--border);
  background: var(--bg);
  position: sticky;
  top: 0;
  z-index: 10;
}

.header__inner {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding-top: 1.1rem;
  padding-bottom: 1.1rem;
  gap: 1rem;
}

.header__brand {
  display: flex;
  align-items: center;
  gap: 0.6rem;
  text-decoration: none;
  color: var(--text);
  font-family: var(--font-display);
  font-weight: 700;
  font-size: 1.05rem;
}

.header__brand-mark {
  color: var(--accent);
  font-family: var(--font-mono);
  letter-spacing: -2px;
}

.header__nav {
  display: flex;
  align-items: center;
  gap: 0.6rem;
}

.header__nav a {
  text-decoration: none;
}
</style>
