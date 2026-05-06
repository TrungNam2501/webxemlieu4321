<template>
  <el-dialog
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    width="70%"
    :close-on-click-modal="false"
    top="5vh"
    :show-close="true"
  >
    <template #header>
      <div class="modal-header">
        <span>Barcode Log — Bồn hóa chất</span>
      </div>
    </template>

    <div class="modal-body">
      <el-table
        :data="data"
        border
        stripe
        v-loading="loading"
        style="width: 100%"
        empty-text="Không có dữ liệu"
      >
        <el-table-column prop="scanState" label="Trạng thái" width="100" align="center" />
        <el-table-column prop="equipCode" label="Máy" width="60" align="center" />
        <el-table-column prop="material" label="Vật liệu" min-width="130" />
        <el-table-column prop="scanBar" label="Scan Bar" min-width="170" />
        <el-table-column prop="scanTime" label="Thời gian quét" min-width="160" />
        <el-table-column prop="bin" label="Bồn" width="60" align="center" />
      </el-table>
    </div>
  </el-dialog>
</template>

<script setup>
defineProps({
  modelValue: Boolean,
  data: { type: Array, default: () => [] },
  loading: Boolean
})

defineEmits(['update:modelValue'])
</script>
