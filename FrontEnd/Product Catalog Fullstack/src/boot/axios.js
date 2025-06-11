import { boot } from 'quasar/wrappers'
import axios from 'axios'


export const api = axios.create({
  baseURL: 'http://localhost:5080/api'
})

export default boot(({ app }) => {
  // sigue registrándolo para plantillas, por si acaso
  app.config.globalProperties.$api = api
})