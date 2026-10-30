import http from './http'

// AJUSTAR nombres de campos según lo que defina C en API_CONTRACT.md
// (por ejemplo puede ser "email" o "username").

export async function registrar({ email, password }) {
  const { data } = await http.post('/auth/register', { email, password })
  return data
}

export async function iniciarSesion({ email, password }) {
  const { data } = await http.post('/auth/login', { email, password })
  // Se asume que el backend responde { access_token }
  return data
}
