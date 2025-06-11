<template>
  <q-dialog v-model="isOpen"
            persistent
            transition-show="jump-up"
            transition-hide="jump-down">
    <!-- Responsive card: full‑width on phones, max 480px on larger screens -->
    <q-card class="q-pa-md shadow-4" style="width:100%;max-width:480px">
      <!-- Header -->
      <q-card-section class="row items-center q-pb-none">
        <div class="text-subtitle1 text-weight-medium">
          {{ model.id ? 'Edit Product' : 'Add Product' }}
        </div>
        <q-space />
        <q-btn icon="close" flat round dense v-close-popup @click="close" />
      </q-card-section>

      <q-separator />

      <!-- Form -->
      <q-card-section>
        <q-form ref="formRef" @submit.prevent="save" class="q-gutter-md">
          <!-- Name -->
          <q-input v-model="model.name" filled label="Name" lazy-rules
                   :rules="[v => !!v || 'Name is required']">
            <template #prepend>
              <q-icon name="label" />
            </template>
          </q-input>

          <!-- Description -->
          <q-input v-model="model.description" filled autogrow type="textarea" label="Description" />

          <!-- Price -->
          <q-input v-model.number="model.price" filled type="number" label="Price" lazy-rules
                   :rules="[
                     v => v !== null && v !== undefined || 'Required',
                     v => v >= 0.01 || '≥ 0.01',
                     v => v <= 999999.99 || 'Too high'
                   ]">
            <template #prepend>
              <q-icon name="attach_money" />
            </template>
          </q-input>

          <!-- Category -->
          <q-input v-model="model.category" filled label="Category" lazy-rules
                   :rules="[v => !!v || 'Category required']">
            <template #prepend>
              <q-icon name="category" />
            </template>
          </q-input>

          <!-- Actions -->
          <div class="row justify-end q-gutter-sm q-mt-lg">
            <q-btn flat label="Cancel" @click="close" />
            <q-btn color="primary" label="Save" @click="save" />
          </div>
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script setup>
import { ref, watch } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios'

const $q = useQuasar()

// Props & emits
const props = defineProps({ modelValue: Object })
const emit  = defineEmits(['update:modelValue', 'saved'])

// State
const isOpen  = ref(false)
const model   = ref({})
const formRef = ref(null)

// Open / close dialog when parent updates v-model
watch(() => props.modelValue, v => {
  model.value = v ? { ...v } : { price: 0 }
  isOpen.value = !!v
})

function close () {
  emit('update:modelValue', null)
}

// Save handler with validation & backend call
async function save () {
  const valid = await formRef.value?.validate()
  if (!valid) return

  try {
    if (model.value.id)
      await api.put(`/products/${model.value.id}`, model.value)
    else
      await api.post('/products', model.value)

    $q.notify({ type: 'positive', message: 'Saved successfully' })
    emit('saved')
    close()
  } catch (err) {
    if (err.response?.status === 400 && err.response.data?.errors) {
      const messages = Object.values(err.response.data.errors).flat().join(', ')
      $q.notify({ type: 'negative', message: `Validation: ${messages}` })
    } else {
      $q.notify({ type: 'negative', message: err.response?.data ?? err.message })
    }
  }
}
</script>

<style scoped>
/* Rounded corners & subtle shadow for modern look */
.q-card {
  border-radius: 16px;
}
/* Improve spacing inside inputs when icons are present */
.q-field__prepend .q-icon {
  font-size: 20px;
}
</style>
