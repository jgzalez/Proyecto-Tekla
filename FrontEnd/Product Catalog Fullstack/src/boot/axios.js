import { boot } from 'quasar/wrappers'
import axios from 'axios'


export const api = axios.create({
    baseURL: 'https://root.jgonzalezfals.dev/api'
})

export default boot(({ app }) => {
    // sigue registrándolo para plantillas, por si acaso
    app.config.globalProperties.$api = api
})