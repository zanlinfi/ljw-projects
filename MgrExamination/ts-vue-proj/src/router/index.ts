import { createRouter, createWebHistory } from 'vue-router'
import { useTokenStore } from '../stores/loginToken'
//const store = useTokenStore()
const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'login',
      component: () => import("../views/login/Login.vue")
    },

    {
      name: "home",
      path: "/main",
      meta: { requiresAuth: true },
      component: () => import("../components/layout/Layout.vue"),
      children: [
        { path: "", component: () => import("../views/main/Main.vue") },
        { path: "/books", component: () => import("../views/main/Main.vue") },
        { path: "/thirdph", component: () => import("../views/ThirdPartyHoliday.vue") },
        {
          path: '/:xxx(.*)*',
          name: 'error',
          component: () => import("../views/ErrorPage.vue")
        },

      ]
    },
  ]
})

// matched : divide route check respectively
router.beforeEach((to, from, next) => {
  if (to.matched.some(m => m.meta?.requiresAuth)) {
    const store = useTokenStore()
    if (!store.token.access_token) {
      next({ name: 'login', query: { redirect: to.fullPath } })
      return
    }
  }
  next()

})

export default router