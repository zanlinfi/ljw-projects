<template>
  <el-container class="layout-container-demo">
    <Aside></Aside>
    <el-container class="header-and-main">
      <Header></Header>
      <el-main>
        <el-scrollbar>
          <RouterView />
        </el-scrollbar>
      </el-main>
    </el-container>
  </el-container>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import { Menu as IconMenu, Message, Setting } from '@element-plus/icons-vue'
import Aside from './Aside.vue';
import Header from './Header.vue';

const item = {
  date: '2016-05-02',
  name: 'Tom',
  address: 'No. 189, Grove St, Los Angeles',
}
const tableData = ref(Array.from({ length: 20 }).fill(item))
</script>



<style lang="scss" scoped>
.layout-container-demo {
  height: 100%;
  width: 100%;
  background-color: #E5EAF3;
  padding: none;

}

.header-and-main {
  flex-direction: column;
  height: 100%;
}

.el-main {
  background-color: #E5EAF3;
  border: none;
  padding-right: 0;
}

.el-main .el-table {
  background-color: #E5EAF3;
  border: none;
}
</style>
