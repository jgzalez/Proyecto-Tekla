<script setup>
import { ref, onMounted, computed } from 'vue'
import { useQuasar } from 'quasar'
import ProductForm from 'components/ProductForm.vue'
import { api } from 'boot/axios'

const $q = useQuasar()

// State ------------------------------------------------------------
const products   = ref([])
const loading    = ref(false)
const formModel  = ref(null)      // bound to ProductForm dialog
const search     = ref('')        // search box model

// Table columns ----------------------------------------------------
const cols = [
  { name: 'name',     label: 'Name',     field: 'name',     sortable: true },
  { name: 'price',    label: 'Price',    field: 'price',    sortable: true,
    format: val => `$${Number(val).toFixed(2)}` },
  { name: 'category', label: 'Category', field: 'category', sortable: true },
  { name: 'actions',  label: '',         field: 'actions' }
]

function showForm (p = null) {
  formModel.value = p ? { ...p } : {}
}

async function load () {
  loading.value = true
  try {
    products.value = (await api.get('/products')).data
  } catch (err) {
    $q.notify({ type: 'negative', message: err.message })
  } finally {
    loading.value = false
  }
}

async function confirmDelete (id) {
  $q.dialog({ title: 'Confirm', message: 'Delete this product?', cancel: true })
    .onOk(async () => {
      try {
        await api.delete(`/products/${id}`)
        load()
      } catch (err) {
        $q.notify({ type: 'negative', message: err.response?.data ?? err.message })
      }
    })
}

onMounted(load)

// Computed list filtered by search --------------------------------
const rows = computed(() =>
  products.value.filter(p =>
    p.name.toLowerCase().includes(search.value.toLowerCase()) ||
    (p.category ?? '').toLowerCase().includes(search.value.toLowerCase())
  )
)
</script>

<template>
  <q-page class="q-pa-md">
    <!-- Header ---------------------------------------------------- -->
    <div class="row items-center justify-between q-mb-md">
      <div class="text-h5">📦 Product Catalog</div>
      <div class="row items-center no-wrap">
        <q-input
          v-model="search"
          dense outlined clearable debounce="300" placeholder="Search…"
          class="q-mr-sm gt-xs"
        >
          <template #append><q-icon name="search" /></template>
        </q-input>
        <q-btn unelevated color="primary" icon="add" label="Add" @click="showForm" />
      </div>
    </div>

    <!-- Table ----------------------------------------------------- -->
    <q-card flat bordered class="q-pa-sm">
      <q-table
        :rows="rows"
        :columns="cols"
        row-key="id"
        :loading="loading"
        flat bordered wrap-cells
        class="my-table"
        :no-data-label="search ? 'No matches' : 'No products'"
      >
        <template #body-cell-actions="props">
          <q-td align="center" class="q-gutter-sm">
            <q-btn flat round dense icon="edit"    color="primary"  @click.stop="showForm(props.row)" />
            <q-btn flat round dense icon="delete"  color="negative" @click.stop="confirmDelete(props.row.id)" />
          </q-td>
        </template>
      </q-table>
    </q-card>

    <!-- Dialog ---------------------------------------------------- -->
    <ProductForm v-model="formModel" @saved="load" />
  </q-page>
</template>

<style scoped>
.my-table .q-td { padding-top: 12px; padding-bottom: 12px; }
</style>
