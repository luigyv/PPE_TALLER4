// Arreglo de prueba que respeta EXACTAMENTE la forma acordada en API_CONTRACT.md
// para el modelo Juego: { id, nombre, imagen, categoria }
//
// Cuando A tenga listo el backend real, este archivo deja de usarse:
// solo hay que cambiar VITE_USE_MOCK=false en el .env (ver src/services/juegosService.js).

let juegos = [
  { id: 1, nombre: 'Hollow Knight', imagen: 'https://upload.wikimedia.org/wikipedia/en/d/de/Hollow_Knight_2026_cover_art.jpg?utm_source=en.wikipedia.org&utm_campaign=index&utm_content=original', categoria: 'Aventura' },
  { id: 2, nombre: 'Celeste', imagen: 'https://assets.nintendo.com/image/upload/q_auto/f_auto/store/software/switch/70010000006442/691ba3e0801180a9864cc8a7694b6f98097f9d9799bc7e3dc6db92f086759252', categoria: 'Plataformas' },
  { id: 3, nombre: 'FIFA 22', imagen: 'https://media.gq.com.mx/photos/60e9cc9c33c54bdef6761922/1:1/w_2125,h_2125,c_limit/E53RdQ5WQAIs7HC-scaled.jpg', categoria: 'Deportes' },
  { id: 4, nombre: 'Civilization VI', imagen: 'https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/store/software/switch/70010000013704/918c0badde3aeba760e2185f382a2402248a1292322cf540fd8d098eeb292e1e', categoria: 'Estrategia' },
  { id: 5, nombre: 'Doom Eternal', imagen: 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/782330/3efb12f884eaee39b4853721f5cf1c6e2b706495/capsule_616x353.jpg?t=1783432602', categoria: 'Acción' },
  { id: 6, nombre: 'Stardew Valley', imagen: 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/413150/capsule_616x353.jpg?t=1786554168', categoria: 'Simulación' },
  { id: 7, nombre: 'League of Legends', imagen: 'https://cdn1.epicgames.com/offer/24b9b5e323bc40eea252a10cdd3b2f10/EGS_LeagueofLegends_RiotGames_S1_2560x1440-47eb328eac5ddd63ebd096ded7d0d5ab', categoria: 'Estrategia' },
  { id: 8, nombre: 'God of War Ragnarök', imagen: 'https://i.blogs.es/eadb27/1366_2000/1200_630.jpeg', categoria: 'Acción' },
  { id: 9, nombre: 'Among Us', imagen: 'https://assets.nintendo.com/image/upload/ar_16:9,c_lpad,w_1240/b_white/f_auto/q_auto/store/software/switch/70010000036098/758ab0b61205081da2466386940752c70e0e5ea43bd39e8b9b13eaa455c69b7e', categoria: 'Fiesta' },
  { id: 10, nombre: 'The Sims 4', imagen: 'https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1222670/capsule_616x353.jpg?t=1787850077', categoria: 'Simulación' },
  { id: 11, nombre: 'Fortnite', imagen: 'https://cms-assets.unrealengine.com/cm6l5gfpm05kr07my04cqgy2x/cmt14h2ardvpq07o76t76zt0m', categoria: 'Acción' },
  { id: 12, nombre: 'Minecraft', imagen: 'https://www.nintendo.com/eu/media/images/news_12/2026/march_42/tiny_takeover_drop_is_now_available_for_minecraft_on_nintendo_switch/16x9_MinecraftTinyTakeover_Textless.png', categoria: 'Aventura' },
  { id: 13, nombre: 'Hollow Knight: Silksong', imagen: 'https://assets.nintendo.com/image/upload/ar_16:9,c_lpad,w_1240/b_white/f_auto/q_auto/store/software/switch2/70010000105851/8787627be7f26ae7984456ffd9af17bea845032cebbf59fe6eeb596dea6bb20e', categoria: 'Aventura' },
]

// --- Helpers internos para simular el backend en memoria ---

export function _getAll() {
  return juegos
}

export function _findById(id) {
  return juegos.find((j) => j.id === Number(id)) || null
}

export function _create(data) {
  const nextId = juegos.length ? Math.max(...juegos.map((j) => j.id)) + 1 : 1
  const nuevo = { id: nextId, nombre: data.nombre, imagen: data.imagen, categoria: data.categoria }
  juegos = [nuevo, ...juegos]
  return nuevo
}

export function _update(id, data) {
  const idx = juegos.findIndex((j) => j.id === Number(id))
  if (idx === -1) return null
  juegos[idx] = { ...juegos[idx], ...data, id: juegos[idx].id }
  return juegos[idx]
}

export function _remove(id) {
  const idx = juegos.findIndex((j) => j.id === Number(id))
  if (idx === -1) return false
  juegos.splice(idx, 1)
  return true
}
