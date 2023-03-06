import axios from "axios";
//'https://localhost:7166',
const request = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
})
export default request