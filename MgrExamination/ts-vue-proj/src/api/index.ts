import axios, { AxiosRequestHeaders } from 'axios'
import BookDto from '../classes/BookDto'
import Holiday from '../classes/Holiday'
import { useTokenStore } from '../stores/loginToken'
//import request from '../utils/request'

axios.interceptors.request.use((config) => {
  if (!config.headers) {
    config.headers = {} as AxiosRequestHeaders
  }
  const store = useTokenStore()
  config.headers.Authorization = store.token.access_token
  return config
})

// retrieve
export const getAllBooks = (req: BookDto) => {
  return axios.post("/api/books/page", req)
}
// add
export const add = (req: {}) => {
  return axios.post("/api/books/plus", req)
}
// delete
export const del = (id: number) => {
  return axios.delete("/api/books/" + id)
}
export const delMore = (ids: string) => {
  return axios.delete("/api/booksDapper/bulk?ids=" + ids)
}

// update
export const update = (req: {}) => {
  return axios.put("/api/books/page", req)
}

// third party api
export const getThirdApi = (countryCode: string, year: number) => {
  return axios.get("/api/thirdParty/" + countryCode + "/" + year)
}


