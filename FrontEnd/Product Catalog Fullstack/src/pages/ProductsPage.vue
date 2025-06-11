<script setup>
import { ref, onMounted} from 'vue'
import { useQuasar } from 'quasar'
import ProductForm from 'components/ProductForm.vue'
import { api } from 'boot/axios'

const $q = useQuasar()

// --- state -----------------------------------------------------------
const products   = ref([])
const loading    = ref(false)
const formModel  = ref(null)
const filter     = ref('')

// table columns definition
const cols = [
  { name: 'name',     label: 'Name',     field: 'name',     sortable: true, align: 'left' },
  { name: 'price',    label: 'Price',    field: 'price',    sortable: true, align: 'right' },
  { name: 'category', label: 'Category', field: 'category', sortable: true, align: 'left' },
  { name: 'actions',  label: '',         field: 'actions',  align: 'center' }
]

// --- methods --------------------------------------------------------
function showForm (p = null) {
  formModel.value = p ? { ...p } : {}
}

async function load () {
  loading.value = true
  try {
    products.value = (await api.get('/products')).data
  } catch (err) {
    $q.notify({ type: 'negative', message: err.response?.data ?? err.message })
  } finally {
    loading.value = false
  }
}

async function confirmDelete (id) {
  $q.dialog({
    title: 'Confirm',
    message: 'Delete this product?',
    cancel: true,
    ok: { flat: true, color: 'negative', label: 'Delete' }
  }).onOk(async () => {
    try {
      await api.delete(`/products/${id}`)
      load()
    } catch (err) {
      $q.notify({ type: 'negative', message: err.response?.data ?? err.message })
    }
  })
}

onMounted(load)
</script>

<template>
  <q-page class="q-pa-md">
    <!-- Header bar -------------------------------------------------->
    <div class="row items-center justify-between q-mb-md">
      <div class="row items-center text-h5 text-weight-medium">
        <q-icon name="inventory_2" class="q-mr-sm" size="26px" />
        Product Catalog
      </div>

      <div class="row items-center q-gutter-sm">
        <q-input v-model="filter"
                 outlined dense debounce="300"
                 placeholder="Search…"
                 class="bg-white rounded-borders">
          <template #append>
            <q-icon name="search" />
          </template>
        </q-input>

        <q-btn color="primary" unelevated icon="add" label="Add" @click="showForm" />
      </div>
    </div>

    <!-- Products table --------------------------------------------->
    <q-card flat bordered class="shadow-2">
      <q-table
        :rows="products"
        :columns="cols"
        row-key="id"
        :loading="loading"
        :filter="filter"
        wrap-cells
        flat bordered
        class="my-table"
      >
        <!-- custom price format -->
        <template #body-cell-price="props">
          <q-td :props="props" align="right">
            {{ Number(props.row.price).toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
          </q-td>
        </template>

        <!-- action buttons -->
        <template #body-cell-actions="props">
          <q-td align="center" class="q-gutter-xs">
            <q-btn flat round dense icon="edit"  color="primary"  @click.stop="showForm(props.row)" />
            <q-btn flat round dense icon="delete" color="negative" @click.stop="confirmDelete(props.row.id)" />
          </q-td>
        </template>

        <!-- no data slot -->
        <template #no-data>
          <div class="text-grey q-pa-md text-center">No products found</div>
        </template>
      </q-table>
    </q-card>

    <!-- Product dialog -->
    <ProductForm v-model="formModel" @saved="load" />
  </q-page>
</template>

<style scoped>
.my-table .q-td {
  padding-top: 12px;
  padding-bottom: 12px;
}
</style>
