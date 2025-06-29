const routes = [{
        path: '/',
        component: () =>
            import ('layouts/MainLayout.vue'),
        children: [
            { path: '', component: () =>
                    import ('pages/ProductsPage.vue') } // ←
        ]
    },
    { path: '/:catchAll(.*)*', component: () =>
            import ('pages/ErrorNotFound.vue') }
]

export default routes