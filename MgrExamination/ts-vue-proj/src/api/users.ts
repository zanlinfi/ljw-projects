import axios from "axios";
import request from '../utils/request'

//type
type LoginInfo = {
  username: string
  password: string
}
// return data type
type LoginResult = {
  success: boolean
  state: number
  message: string
  content: string
}

//login request{phone:"1111", password:"111"}
// export const login = (loginInfo: LoginInfo) => {
// return request<LoginResult>({
//   methods: 'post',
//   url: 'api/Login/login',
//   data: `username=${loginInfo.username}&password=${loginInfo.password}`, // loginInfo(json)
// })
// }
export const login = (req: LoginInfo) => {
  return axios.post("/api/Login/login", req)
}
