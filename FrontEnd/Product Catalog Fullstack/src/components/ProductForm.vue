<script setup>
import { ref, watch } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios' 

const $q = useQuasar()
const props = defineProps({ modelValue: Object })
const emit  = defineEmits(['update:modelValue', 'saved'])

const isOpen = ref(false)
const model  = ref({})

watch(() => props.modelValue, v => {
  model.value = v ? { ...v } : { price: 0 }
  isOpen.value = !!v
})

async function save () {
  try {
    if (!model.value.name) { $q.notify({type:'warning', message:'Name required'}); return }

    if (model.value.id)
      await api.put(`/products/${model.value.id}`, model.value)
    else
      await api.post('/products', model.value)

    emit('update:modelValue', null) // cerrar diálogo
    emit('saved')
  } catch (err) {
    $q.notify({ type:'negative', message: err.response?.data ?? err.message })
  }
}
</script>

<template>
  <q-dialog v-model="isOpen" persistent>
    <q-card style="min-width:400px">
      <q-card-section class="text-h6">
        {{ model.id ? 'Edit' : 'Add' }} Product
      </q-card-section>

      <q-card-section>
        <q-form @submit.prevent="save" class="q-gutter-md">
          <q-input v-model="model.name" label="Name" autofocus
                   :rules="[v=>!!v||'Required']"/>
          <q-input v-model="model.description" label="Description" type="textarea"/>
          <q-input v-model.number="model.price" label="Price" type="number"
                   :rules="[v=>v>=0||'≥ 0']"/>
          <q-input v-model="model.category" label="Category"
                   :rules="[v=>!!v||'Required']"/>
        </q-form>
      </q-card-section>

      <q-card-actions align="right">
        <q-btn flat label="Cancel" v-close-popup />
        <q-btn color="primary" label="Save" @click="save" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>
