<template>
  <el-header>
    <el-icon @click="isCollapse = !isCollapse">
      <Expand v-if="isCollapse" />
      <Fold v-else />
    </el-icon>
    <el-breadcrumb separator="/">
    <el-breadcrumb-item :to="{ path: '/' }">homepage</el-breadcrumb-item>
  </el-breadcrumb>
    <el-dropdown>
      <el-icon>
        <setting />
      </el-icon>
      <template #dropdown>
        <el-dropdown-menu>
          <el-dropdown-item>View</el-dropdown-item>
          <el-dropdown-item>Add</el-dropdown-item>
          <el-dropdown-item>Delete</el-dropdown-item>
        </el-dropdown-menu>
      </template>
    </el-dropdown>
    <span>Zane</span>
  </el-header>
</template>

<script setup lang="ts">
import { Menu as IconMenu, Message, Fold, Expand } from '@element-plus/icons-vue'
import {isCollapse} from './isCollapse'
</script>

<style lang="scss" scoped>
.el-header {
  height: 50px;
  display: flex;
  align-items: center;
  background-color:  #A3A6AD;
  justify-content: space-between;
  .el-icon {
    margin-right: 17px;
  }
}
.el-dropdown {
  margin-left: auto;
}
</style>