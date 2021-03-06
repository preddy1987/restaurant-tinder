import Vue from 'vue';
import Router from 'vue-router';
import Register from './views/Register.vue'
import Login from './views/Login.vue'
import Landing from './views/Landing.vue'
import UserPreferences from './views/UserPreferences.vue'
import Liked from './views/Liked.vue'

import auth from './auth';

Vue.use(Router);

const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'landing',
      component: Landing,
    },
    {
      path: '/login',
      name: 'login',
      component: Login,
    },
    {
        path: '/register',
        name: 'register',
        component: Register,
    },
    {
      path: '/userpreferences',
      name: 'userpreferences',
      component: UserPreferences,
    },
    {
      path: '/liked',
      name: 'liked',
      component: Liked,
    },
  ],
})

router.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login', '/register', '/'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = auth.getUser();

  if (authRequired && !loggedIn) {
    return next('/');
  }

  next();
});

export default router;