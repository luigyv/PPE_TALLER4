<script setup>
import { ref } from 'vue'
import { useRouter, useRoute, RouterLink } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const auth = useAuthStore()
const router = useRouter()
const route = useRoute()

const form = ref({ email: '', password: '' })
const error = ref('')
const cargando = ref(false)

async function enviar() {
  error.value = ''
  cargando.value = true
  try {
    await auth.login(form.value)
    router.push(route.query.redirect || { name: 'juegos-lista' })
  } catch (e) {
    error.value = 'Correo o contraseña incorrectos.'
  } finally {
    cargando.value = false
  }
}
</script>

<template>
  <section class="form-page">
    <h2>Iniciar sesión</h2>
    <form class="form" @submit.prevent="enviar">
      <p v-if="error" class="field-error">{{ error }}</p>

      <div class="field">
        <label for="email">Correo</label>
        <input id="email" v-model="form.email" type="email" required autocomplete="username" />
      </div>

      <div class="field">
        <label for="password">Contraseña</label>
        <input id="password" v-model="form.password" type="password" required autocomplete="current-password" />
      </div>

      <button type="submit" class="btn btn-primary" :disabled="cargando">
        {{ cargando ? 'Entrando...' : 'Entrar' }}
      </button>
    </form>

    <p class="switch">
      ¿No tienes cuenta?
      <RouterLink :to="{ name: 'registro' }">Regístrate</RouterLink>
    </p>
  </section>
</template>

<style scoped>
.form-page {
  max-width: 380px;
  margin: 0 auto;
}

.form-page h2 {
  text-align: center;
  margin-bottom: 1.5rem;
}

.form button {
  width: 100%;
}

.switch {
  text-align: center;
  color: var(--text-muted);
  font-size: 0.88rem;
  margin-top: 1.2rem;
}
</style>
