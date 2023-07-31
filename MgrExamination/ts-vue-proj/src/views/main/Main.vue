<script setup lang="ts">
import { Setting, Compass } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus';
import { loadRouteLocation } from 'vue-router';
import { computed, ref, onMounted } from 'vue'
import Add from '../../components/Add.vue';
import Book from '../../classes/Book';
import DeleteBook from '../../classes/DeleteBook';
import { getAllBooks, del, delMore } from '../../api/index'
import BookDto from '../../classes/BookDto';

const tableData = ref<any[]>([])
const isShow = ref(false)
const info = ref<Book>(new Book())
const multipleTableRef = ref()
let bookDto = ref<BookDto>(new BookDto())
const total = ref(20)
const load = async () => {
  //console.log(await getAllBooks(bookDto.value))
  let data = (await getAllBooks(bookDto.value)).data
  tableData.value = data.res
  console.log(tableData.value)
  total.value = data.total
}
onMounted(async () => {
  await load();
})

const search = ref("")
const enterSearch = async () => {
  bookDto.value.KeyWord = search.value
  await load();
}

const handleEdit = async (index: number, row: Book) => {
  console.log(index, row)
  info.value = row
  isShow.value = true
  await load();
}

const handleDeleteMore = async () => {
  //get lot rows
  let rows = multipleTableRef.value?.getSelectionRows() as Array<DeleteBook>
  if (rows.length > 0) {
    console.log(rows)
    console.log(rows.map(item => { return `'${item.id}'` }).join(","))
    //'1','2','3'
    let res = (await delMore(rows.map(item => { return `'${item.id}'` }).join(","))).data
    if (res) {
      ElMessage.success("deleted success")
      await load()
    } else {
      ElMessage.error("deleted error")
    }

  } else {
    ElMessage.warning("pls select some rows")
  }
}
const handleDelete = async (index: number, row: Book) => {
  console.log(index, row)
  if (index >= 0) {
    let res = (await del(row.id as number)).data
    if (res >= 0) {
      ElMessage.error("deleted success")
    } else {
      ElMessage.error("deleted error")
    }
  }
  await load()
}

const currentChange = async (val: number) => {
  console.log(val)
  bookDto.value.PageIndex = val
  await load()
}
const tableRowClassName = ({
  row,
  rowIndex,
}: {
  row: Book
  rowIndex: number
}) => {
  if (rowIndex % 2 === 0) {
    return 'warning-row'
  } else if (rowIndex % 2 === 1) {
    return 'success-row'
  }
  return ''
}

const openAdd = () => {
  isShow.value = true
}

const closedAdd = () => {
  isShow.value = false
  info.value = new Book()
}
const success = (message: string) => {
  isShow.value = false
  info.value = new Book()
  ElMessage.success(message)
}

const handleAdd =  () => {
  isShow.value = true
}
</script>

<template>
  <div>
    <div class="el-main-header">
      <span style="margin-right: 5px; height: auto;">
        <el-input v-model="search" size="middle" placeholder="Type to search" class="input-with-select"
          @keyup.enter="enterSearch" />
      </span>
      <span style="margin-right: 30px;"><el-button style="margin-right: 700px;" type="primary"
          :icon="Compass" @click="enterSearch" circle /></span>
      <span style="margin: 10px; "><el-button type="add" size="middle" round @click="handleAdd" @openAdd="openAdd">
          Add
        </el-button></span>
      <span style="margin-right: 10px;"><el-button size="middle" type="danger"
          @click="handleDeleteMore">Delete</el-button></span>
    </div>

    <div class="el-main-body">
      <el-table ref="multipleTableRef" class="el-table" style="width: 100%" :data="tableData" :row-class-name="tableRowClassName">
        <el-table-column type="selection" width="50px" />
        <el-table-column label="Order" prop="id" />
        <el-table-column label="Title" prop="title" />
        <el-table-column label="Price" prop="price" />
        <el-table-column label="Authorname" prop="authorName" />
        <el-table-column align="right">

          <!-- (scope.$index, scope.row) -->
          <template #default="scope">
            <el-button size="middle" @click="handleEdit(scope.$index, scope.row)" @openAdd="openAdd">Edit</el-button>
            <el-button size="middle" type="danger" @click="handleDelete(scope.$index, scope.row)">Delete</el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination background layout="prev, pager, next" :total="total" @current-change="currentChange" />
    </div>


  </div>
  <Add :isShow="isShow" :info="info" @closedAdd="closedAdd" @success="success"></Add>
</template>

<style lang="scss" scoped>
.el-main-header {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  margin-left: 50px;
}
.el-main-header .el-input{
  margin-right: 10px;
}

.el-table  {
  background-color: #E5EAF3;
}

.el-table .warning-row {
  --el-table-tr-bg-color: var(--el-color-warning-light-9);
}
.el-table .success-row {
  --el-table-tr-bg-color: var(--el-color-success-light-9);
}
</style>