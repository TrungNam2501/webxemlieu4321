<template>
  <el-dialog
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    title="Barcode Log - Bồn hóa chất"
    width="80%"
    :close-on-click-modal="false"
    top="5vh"
  >
    <div class="modal-body">
      <el-table
        :data="data"
        border
        stripe
        v-loading="loading"
        style="width: 100%"
        :header-cell-style="{ backgroundColor: '#4a6fa5', color: 'white', textAlign: 'center', fontWeight: 'bold' }"
        :cell-style="{ textAlign: 'center', fontWeight: 'bold', fontFamily: 'Arial', fontSize: '14px' }"
      >
        <el-table-column prop="scanState" label="Trạng thái" width="120" />
        <el-table-column prop="equipCode" label="Máy" width="80" />
        <el-table-column prop="material" label="Vật liệu" min-width="150" />
        <el-table-column prop="scanBar" label="Scan Bar" width="200" />
        <el-table-column prop="scanTime" label="Thời gian quét" width="200" />
        <el-table-column prop="bin" label="Bồn" width="80" />
      </el-table>

      <div v-if="data.length === 0 && !loading" style="text-align: center; padding: 20px; color: #999;">
        Không có dữ liệu
      </div>
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
