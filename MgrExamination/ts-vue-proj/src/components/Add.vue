<template>
  <div>
    <el-dialog v-model="dialogVisible" :title="info?.title ? 'Edit' : 'Add'" width="30%" @close="$emit('closedAdd')"
      draggable>
      <el-form ref="ruleFormRef" :model="form" :rules="rules" label-width="100px" >
        <el-form-item label="Title" prop="title">
          <el-input v-model="form.title" />
        </el-form-item>
        <el-form-item label="Price" number clearable prop="price">
          <el-input v-model="form.price"/>
        </el-form-item>
        <el-form-item label="Authorname" prop="authorname">
          <el-input v-model="form.authorname" />
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="closedAdd(ruleFormRef)">Cancel</el-button>
          <el-button type="primary" @click="save(ruleFormRef)">Confirm</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { Ref, ref, computed, watch, reactive } from 'vue'
import Book from '../classes/Book'
import { ElMessage, FormInstance, FormRules } from 'element-plus';
import { add, update } from '../api/index'
import { loadRouteLocation } from 'vue-router';

const props = defineProps({
  isShow: Boolean,
  info: Book
})

const rules = reactive<FormRules>({
  price: [{ required: true, message: 'please input price', trigger: 'blur'  },
          
          ],// { type: "number", message: 'need a number',trigger: 'blur'  }
  authorname: [{ required: true, message: 'please input authorname', trigger: 'blur' }],
  title: [{ required: true, message: 'please input a title', trigger: 'blur' }],
})

const dialogVisible = computed(() => props.isShow)
const ruleFormRef = ref<FormInstance>()
const form: Ref<Book> = ref<Book>({
  id: 0,
  title: "",
  price: 0,
  authorname: ""
})
watch(() => props.info, (newInfo) => {
  if (newInfo) {
    form.value = {
      id: newInfo.id,
      title: newInfo.title,
      price: newInfo.price,
      authorname: newInfo.authorname
    }
  }
})

const emits = defineEmits(["closedAdd", "success"])
const closedAdd = async (formEl: FormInstance | undefined) => {
  if (!formEl) return
  emits("closedAdd")
}
const save = async (formEl: FormInstance | undefined) => {
  if (!formEl) return
  await formEl.validate((valid, fields) => {
    if (valid) {
      if (form.value.id) {
        update(form.value).then(function (res) {
          if (res.data) {
            emits("success", "Success update")
          } else {
            ElMessage.error("update error")
          }
        })
      } else {
        add(form.value).then(function (res) {
          if (res.data) {
            emits("success", "Success add")
          } else {
            ElMessage.error("add error")
          }
        })
      }
    } else {
      console.log("submit error!", fields)
    }
  })
}
</script>

<style lang="scss" scoped></style>