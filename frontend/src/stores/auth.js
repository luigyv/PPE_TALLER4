import { defineStore } from 'pinia'
import { iniciarSesion, registrar } from '../services/authService'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('access_token') || null,
  }),

  getters: {
    estaAutenticado: (state) => !!state.token,
  },

  actions: {
    async login(credenciales) {
      const data = await iniciarSesion(credenciales)
      this.token = data.access_token
      localStorage.setItem('access_token', data.access_token)
    },

    async register(credenciales) {
      return registrar(credenciales)
    },

    logout() {
      this.token = null
      localStorage.removeItem('access_token')
    },
  },
})
