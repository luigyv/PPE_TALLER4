import { createRouter, createWebHashHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const routes = [
  {
    path: '/',
    name: 'juegos-lista',
    component: () => import('../views/JuegosListView.vue'),
  },
  {
    path: '/juegos/nuevo',
    name: 'juegos-crear',
    component: () => import('../views/JuegoFormView.vue'),
    meta: { requiereAuth: true },
  },
  {
    path: '/juegos/:id/editar',
    name: 'juegos-editar',
    component: () => import('../views/JuegoFormView.vue'),
    props: true,
    meta: { requiereAuth: true },
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/LoginView.vue'),
  },
  {
    path: '/registro',
    name: 'registro',
    component: () => import('../views/RegisterView.vue'),
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'not-found',
    component: () => import('../views/NotFoundView.vue'),
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes,
})

// Guard: protege las rutas de creación/edición (que a su vez llaman a
// endpoints protegidos por el Guard de C: POST, PATCH, DELETE).
router.beforeEach((to) => {
  const auth = useAuthStore()
  if (to.meta.requiereAuth && !auth.estaAutenticado) {
    return { name: 'login', query: { redirect: to.fullPath } }
  }
  return true
})

export default router
