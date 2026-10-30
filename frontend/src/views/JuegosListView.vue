<script setup>
import { ref, watch, onMounted } from 'vue'
import { listarJuegos, eliminarJuego } from '../services/juegosService'
import { useAuthStore } from '../stores/auth'
import SearchBar from '../components/SearchBar.vue'
import JuegoCard from '../components/JuegoCard.vue'
import Pagination from '../components/Pagination.vue'
import ConfirmDialog from '../components/ConfirmDialog.vue'

const auth = useAuthStore()

const juegos = ref([])
const search = ref('')
const page = ref(1)
const limit = ref(8)
const totalPages = ref(1)
const cargando = ref(false)
const error = ref('')

const juegoAEliminar = ref(null)

async function cargar() {
  cargando.value = true
  error.value = ''
  try {
    const resultado = await listarJuegos({ search: search.value, page: page.value, limit: limit.value })
    juegos.value = resultado.items
    totalPages.value = resultado.totalPages
  } catch (e) {
    error.value = 'No se pudo cargar el catálogo. Intenta de nuevo.'
  } finally {
    cargando.value = false
  }
}

watch(search, () => {
  page.value = 1
  cargar()
})
watch(page, cargar)

onMounted(cargar)

function pedirEliminar(juego) {
  juegoAEliminar.value = juego
}

async function confirmarEliminacion() {
  if (!juegoAEliminar.value) return
  await eliminarJuego(juegoAEliminar.value.id)
  juegoAEliminar.value = null
  await cargar()
}
</script>

<template>
  <section>
    <div class="toolbar">
      <SearchBar v-model="search" />
    </div>

    <p v-if="error" class="state state--error">{{ error }}</p>
    <p v-else-if="cargando" class="state">Cargando juegos...</p>
    <p v-else-if="juegos.length === 0" class="state">
      No encontramos juegos que coincidan con "{{ search }}".
    </p>

    <div v-else class="grid">
      <JuegoCard
        v-for="juego in juegos"
        :key="juego.id"
        :juego="juego"
        :puede-editar="auth.estaAutenticado"
        @eliminar="pedirEliminar"
      />
    </div>

    <Pagination v-if="!cargando && juegos.length > 0" v-model:page="page" :total-pages="totalPages" />

    <ConfirmDialog
      :visible="!!juegoAEliminar"
      titulo="Eliminar juego"
      :mensaje="`Esta acción va a borrar «${juegoAEliminar?.nombre}» del catálogo.`"
      @confirmar="confirmarEliminacion"
      @cancelar="juegoAEliminar = null"
    />
  </section>
</template>

<style scoped>
.toolbar {
  display: flex;
  margin-bottom: 1.5rem;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 1.1rem;
}

.state {
  color: var(--text-muted);
  padding: 3rem 0;
  text-align: center;
}

.state--error {
  color: var(--danger);
}
</style>
