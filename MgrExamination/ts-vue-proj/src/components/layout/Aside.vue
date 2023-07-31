<template>
  <el-scrollbar>
    <el-aside class="aside">
      <span>
        <a href="/" class="logo" style="margin-top: 20px;">
          <img src="../../assets/vue.svg" alt="">
          <!-- <h1>目录展示</h1> -->
        </a>
      </span>
      <el-menu style="margin-top: 30px;" @open="handleOpen" @close="handleClose" :default-openeds="['1', '3']" router
        :collapse="isCollapse">
        <el-sub-menu index="1">
          <template #title>
            <el-icon>
              <message />
            </el-icon><span>Items Manage</span>
          </template>
          <el-menu-item-group>
            <template #title>Group 1</template>
            <el-menu-item index="/books"><span>Books Manage</span></el-menu-item>
            <el-menu-item index="1-2"><span>Pen Manage</span> </el-menu-item>
          </el-menu-item-group>
          <el-menu-item-group title="Group 2">
            <el-menu-item index="1-3"><span>Dresses Manage</span></el-menu-item>
          </el-menu-item-group>
          <el-sub-menu index="1-4">
            <template #title><span>Clothes Manage</span></template>
            <el-menu-item index="1-4-1">Type one</el-menu-item>
          </el-sub-menu>
        </el-sub-menu>
        <el-sub-menu index="2">
          <template #title>
            <el-icon><icon-user /></el-icon><span>User Manage</span>
          </template>
          <el-menu-item index="/users"><span>User Manage</span></el-menu-item>
          <el-menu-item index="roles"><span>Role Manage</span></el-menu-item>
        </el-sub-menu>
        <el-sub-menu index="3">
          <template #title>
            <el-icon>
              <setting />
            </el-icon><span>Third API Call</span>
          </template>
          <el-menu-item index="/thirdph">Holidays</el-menu-item>
          <el-menu-item index="bms">BMS</el-menu-item>
        </el-sub-menu>
      </el-menu>
    </el-aside>
  </el-scrollbar>
</template>

<script setup lang="ts">
import { Menu as IconMenu, Message, Setting, User as IconUser } from '@element-plus/icons-vue'
// collapse
import { isCollapse } from './isCollapse'
const handleOpen = (key: string, keyPath: string[]) => {
  console.log(key, keyPath)
}
const handleClose = (key: string, keyPath: string[]) => {
  console.log(key, keyPath)
}

</script>

<style lang="scss" scoped>
.aside {
  background-color: #CFD3DC;
  border: none;
  height: 100vh;
  width: auto;
  & .el-menu--collpase {
    width: 10px;
    &h1{
      display: none;
    }
  }
}

.el-menu {
  background-color: #CFD3DC;
  border: none;
  width: auto;

  &.el-menu--collpase {
    width: 10px;
    &h1{
      display: none;
    }
  }
}

.el-menu .el-menu-item {
  background-color: #CFD3DC;
  border: none;
  width: auto;
  &.el-menu--collpase {
    width: 10px;
    &h1{
      display: none;
    }
  }
}

.el-menu .el-sub-menu {
  background-color: #CFD3DC;
  border: none;
  width: auto;

  &.el-menu--collpase {
    width: 10px;
    &h1{
      display: none;
    }
  }
}

.el-menu-item-group {
  background-color: #CFD3DC;
  border: none;
  width: auto;

  &.el-menu--collpase {
    width: 10px;
    &h1{
      display: none;
    }
  }
}

.logo {
  display: flex;
  justify-content: center;
  align-items: center;
  text-decoration: none;
  color: black;
  height: 10px;

  img {
    width: 32px;
    height: 32px;
  }
}</style>