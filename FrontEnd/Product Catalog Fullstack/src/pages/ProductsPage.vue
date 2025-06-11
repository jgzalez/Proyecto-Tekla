<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import ProductForm from 'components/ProductForm.vue'
import { api } from 'boot/axios' 

const $q = useQuasar()
const products = ref([])
const loading  = ref(false)
const formModel = ref(null)    // v-model para el diálogo

const cols = [
  { name:'name',     label:'Name',     field:'name',     sortable:true },
  { name:'price',    label:'Price',    field:'price',    sortable:true },
  { name:'category', label:'Category', field:'category' },
  { name:'actions',  label:'',         field:'actions' }
]

function showForm(p = null) { formModel.value = p ? { ...p } : {} }

async function load () {
  loading.value = true
  try   { products.value = (await api.get('/products')).data }
  catch (err) { $q.notify({ type:'negative', message: err.message }) }
  finally { loading.value = false }
}

async function confirmDelete (id) {
  $q.dialog({ title:'Confirm', message:'Delete this product?', cancel:true })
    .onOk(async () => { await api.delete(`/products/${id}`); load() })
}

onMounted(load)
</script>

<template>
  <q-page padding>
    <q-table :rows="products" :columns="cols" row-key="id" :loading="loading">
      <template #top-right>
        <q-btn color="primary" label="Add" @click="showForm()" />
      </template>

      <template #body-cell-actions="props">
        <q-td>
          <q-btn round flat icon="edit"    @click.stop="showForm(props.row)" />
          <q-btn round flat icon="delete" color="red"
                 @click.stop="confirmDelete(props.row.id)" />
        </q-td>
      </template>
    </q-table>

    <ProductForm v-model="formModel" @saved="load" />
  </q-page>
</template>
