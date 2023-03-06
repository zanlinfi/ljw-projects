import { ElMessage } from 'element-plus'
import { defineStore, acceptHMRUpdate } from 'pinia'



interface Token {
  access_token: string
  token_type: string
}

export const useTokenStore = defineStore('loginToken', () => {
  // state
  const tokenJson = ref("")
  // getter
  const token = computed<Token>(() => {
    try {
      return JSON.parse(tokenJson.value || window.localStorage.getItem("TokenInfo") || "{}")
    } catch (err) {
      ElMessage.error("json parse error")
      window.localStorage.setItem("TokenInfo", "")
      throw err
    }
  })
  // actions
  function saveToken(data: string) {
    tokenJson.value = data
    window.localStorage.setItem("TokenInfo", data)
  }

  // expose token
  return { token, saveToken }
})


