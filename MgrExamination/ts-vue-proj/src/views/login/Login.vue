<template>
  <div class="login">
    <el-form :model="form" :rules="rules" ref="formRef" label-width="120px" label-position="top" size="large">
      <h2>Login</h2>
      <el-form-item label="Username
        " prop="username">
        <el-input v-model="form.username" />
      </el-form-item>
      <el-form-item label="Password
        " prop="password">
        <el-input v-model="form.password" />
      </el-form-item>

      <el-form-item>
        <el-button type="primary" @click="onSubmit" :loading="isLoading">Login</el-button>

      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts" setup>
import { reactive, ref, } from 'vue'
import { ElMessage, FormInstance, FormRules } from 'element-plus';
import {login} from '../../api/users'
import {useTokenStore} from '../../stores/loginToken'
import { useRouter, useRoute } from 'vue-router';
// page skip
const router = useRouter()
const route = useRoute()

// get token
const store = useTokenStore()

// avoid repeat login
const isLoading = ref(false)

//rules
const formRef = ref<FormInstance>()
const rules = reactive<FormRules>({
  phone: [
    { required: true, message: 'Please input username', trigger: 'blur' },
    { min: 2, max: 15, message: 'Length should be 11', trigger: 'blur' },//pattern: /^1\d{10}$/
  ], 
  password: [
    {
      required: true,
      message: 'Please select input password',
      trigger: 'blur',
    },
    { min: 6, max: 18, message: 'Length should be 6 to 18', trigger: 'blur' },
  ],
})

// do not use same name with ref
const form = reactive({
  username: 'user',
  password: '0000000',

})

//login
const onSubmit =async () => {
  // form validation
  isLoading.value = true
  await formRef.value?.validate().catch((err)=>{
    ElMessage.error('validation failed')
    isLoading.value = false
    throw err
    //return new Promise(()=>{}) // padding error
})
  //login reqest
  let data
  try{
     data = (await login(form)).data
     if (!data.success){
     isLoading.value = false
     ElMessage.error('login failure')
     throw new Error('login info wrong')
  }
  }catch(err) {
    isLoading.value = false
    console.log('login failure')
    ElMessage.error('login failure')
    throw err
  } 
  
  console.log(data)
  //save token
  store.saveToken(data.content)
  console.log(data.content)
  isLoading.value = false
  
  ElMessage.success("login success")
  //router.push("/main")
  // remember target address after success skip to it
  router.push(route.query.redirect as string || "/main")

} 
</script>  

<style lang="scss" scoped>
.login {
  background-color: #ddd;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.el-form {
  width: 300px;
  background-color: #fff;
  padding: 30px;
  border-radius: 20px;
}

.el-form-item {
  margin-top: 20px;
}

.el-button {
  width: 100%;
  margin-top: 30px;
}</style>