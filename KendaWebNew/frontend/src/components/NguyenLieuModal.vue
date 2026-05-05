<template>
  <el-dialog
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    title="Nguyên liệu quét vào"
    width="95%"
    :close-on-click-modal="false"
    top="2vh"
  >
    <template #header>
      <div class="modal-header">
        <span>Nguyên liệu quét vào</span>
        <el-button type="success" @click="$emit('export-excel')">Xuất Excel</el-button>
      </div>
    </template>

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
        <el-table-column prop="saveTime" label="Thời gian quét" width="180" />
        <el-table-column prop="equipId" label="Tên máy" width="80" />
        <el-table-column prop="materCode" label="Mã nguyên liệu" width="120" />
        <el-table-column prop="materName" label="Tên nguyên liệu" min-width="150" />
        <el-table-column prop="setNum" label="Số mẻ điều động" width="100" />
        <el-table-column prop="serialNum" label="Số mẻ hoàn thành" width="100" />
        <el-table-column prop="realWeight" label="Số ký quét" width="100" />
        <el-table-column prop="materBarcode" label="Tem quét" width="160" />
        <el-table-column prop="batchNo" label="Số lô" width="130" />
        <el-table-column label="Xem chi tiết" width="100" align="center">
          <template #default="{ row }">
            <el-button
              v-if="row.materBarcode"
              size="small"
              type="warning"
              @click="$emit('view-detail', row.materBarcode)"
            >
              <el-icon><View /></el-icon>
            </el-button>
          </template>
        </el-table-column>
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

defineEmits(['update:modelValue', 'view-detail', 'export-excel'])
</script>
