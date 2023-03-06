<template>
  <div>
    <h3 class="title-holiday">holidays third party website </h3>
    <div class="el-holiday-header">
      <b>Country Code</b>
      <span style="margin-right: 5px; height: auto;">
        <el-input v-model="country" size="middle" placeholder="Type Country" class="input-with-select" />
      </span>
      <b>Year</b>
      <span style="margin-right: 5px; height: auto;">
        <el-input v-model="year" size="middle" placeholder="Type Year" class="input-with-select" />
      </span>
      <el-button style="margin-bottom: 10px; margin-top: 10px;" size="middle" type="primary" @click="handleSearch">Submit</el-button>
    </div>
    <div class="el-holiday-body">
      <el-table class="el-holiday-table" style="width: 100%" :data="tableData"
        :row-class-name="tableRowClassName">
        <el-table-column label="Name" prop="name" />
        <el-table-column label="LocalName" prop="localName" />
        <el-table-column label="Date" prop="date" />
        <el-table-column label="Global" prop="global" />
      </el-table>
    </div>
  </div>
</template>

<script setup lang="ts">
import Holiday from "../classes/Holiday"
import {getThirdApi} from "../api/index"
const country = ref('us')
const year = ref(2022)
const handleSearch = async ()=>{
  await load()
}
const tableData = ref<any[]>([])
const load = async () => {
   console.log(await getThirdApi(country.value  , year.value))
   let data = (await getThirdApi(country.value, year.value)).data
   tableData.value = data
   console.log(tableData.value)
  
}
onMounted(async () => {
  await load();
})


// table color
const tableRowClassName = ({
  row,
  rowIndex,
}: {
  row: Holiday
  rowIndex: number
}) => {
  if (rowIndex % 2 === 0) {
    return 'warning-row'
  } else if (rowIndex % 2 === 1) {
    return 'success-row'
  }
  return ''
}

</script>

<style lang="scss" scoped>
.el-holiday-table  {
  background-color: #E5EAF3;
}

.el-holiday-table .warning-row {
  --el-table-tr-bg-color: var(--el-color-warning-light-9);
}
.el-holiday-table .success-row {
  --el-table-tr-bg-color: var(--el-color-success-light-9);
}
</style>