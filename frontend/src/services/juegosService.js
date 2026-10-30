import http from './http'
import { _getAll, _findById, _create, _update, _remove } from '../data/mockJuegos'

// --- INTERRUPTOR MOCK <-> BACKEND  --------------------------------
// Mientras A no avise que el backend está listo: VITE_USE_MOCK=true (usa mockJuegos.js)
// Cuando A avise: cambia VITE_USE_MOCK=false en el .env y estas mismas funciones
// empiezan a llamar al backend con axios. El resto de la app (vistas,
// componentes) no cambia porque siempre le habla a este archivo, nunca al mock
// ni a axios directamente.
const USE_MOCK = import.meta.env.VITE_USE_MOCK !== 'false'

function paginateAndFilter(list, { search = '', page = 1, limit = 8 } = {}) {
  const term = search.trim().toLowerCase()
  const filtered = term
    ? list.filter(
        (j) =>
          j.nombre.toLowerCase().includes(term) || j.categoria.toLowerCase().includes(term)
      )
    : list

  const total = filtered.length
  const totalPages = Math.max(1, Math.ceil(total / limit))
  const currentPage = Math.min(Math.max(1, page), totalPages)
  const start = (currentPage - 1) * limit
  const items = filtered.slice(start, start + limit)

  return { items, total, page: currentPage, limit, totalPages }
}

export async function listarJuegos({ search = '', page = 1, limit = 8 } = {}) {
  if (USE_MOCK) {
    // Simula latencia de red para que el loading/estado se sienta real
    await new Promise((r) => setTimeout(r, 150))
    return paginateAndFilter(_getAll(), { search, page, limit })
  }

  // AJUSTAR según API_CONTRACT.md real (nombres de query params y forma de la respuesta)
  const { data } = await http.get('/juegos', { params: { search, page, limit } })
  return data
}

export async function obtenerJuego(id) {
  if (USE_MOCK) {
    await new Promise((r) => setTimeout(r, 100))
    const juego = _findById(id)
    if (!juego) throw new Error('Juego no encontrado')
    return juego
  }
  const { data } = await http.get(`/juegos/${id}`)
  return data
}

export async function crearJuego(payload) {
  if (USE_MOCK) {
    await new Promise((r) => setTimeout(r, 150))
    return _create(payload)
  }
  const { data } = await http.post('/juegos', payload)
  return data
}

export async function editarJuego(id, payload) {
  if (USE_MOCK) {
    await new Promise((r) => setTimeout(r, 150))
    return _update(id, payload)
  }
  const { data } = await http.patch(`/juegos/${id}`, payload)
  return data
}

export async function eliminarJuego(id) {
  if (USE_MOCK) {
    await new Promise((r) => setTimeout(r, 150))
    return _remove(id)
  }
  await http.delete(`/juegos/${id}`)
  return true
}
