<template>
  <el-dialog
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    title="Hóa chất CWSS"
    width="95%"
    :close-on-click-modal="false"
    top="2vh"
  >
    <template #header>
      <div class="modal-header">
        <span>Hóa chất CWSS</span>
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
        <el-table-column prop="dosingId" label="Dosing ID" width="100" />
        <el-table-column prop="planId" label="Plan ID" width="150" />
        <el-table-column prop="equipCode" label="Máy" width="80" />
        <el-table-column prop="serialNum" label="STT" width="60" />
        <el-table-column prop="weightId" label="Weight ID" width="80" />
        <el-table-column prop="materialCode" label="Mã vật liệu" width="100" />
        <el-table-column prop="materialName" label="Tên vật liệu" min-width="150" />
        <el-table-column prop="realWeight" label="Cân thực" width="90" />
        <el-table-column prop="realError" label="Sai số" width="80" />
        <el-table-column prop="overWeight" label="Quá cân" width="80" />
        <el-table-column prop="overError" label="Quá sai số" width="80" />
        <el-table-column prop="wasteTime" label="Thời gian thải" width="100" />
        <el-table-column prop="warningSign" label="Cảnh báo" width="80" />
        <el-table-column prop="weightTime" label="Thời gian cân" width="180" />
        <el-table-column prop="batchNumber" label="Batch Number" width="130" />
        <el-table-column prop="recipeCode" label="Recipe Code" width="120" />
        <el-table-column label="Xem bồn" width="80" align="center">
          <template #default="{ row }">
            <el-button
              size="small"
              type="primary"
              @click="$emit('view-barcode-log', {
                equipCode: row.equipCode,
                materialName: row.materialName,
                materialCode: row.materialCode,
                recordTime: row.weightTime
              })"
            >
              <el-icon><View /></el-icon>
            </el-button>
          </template>
        </el-table-column>
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

defineEmits(['update:modelValue', 'view-barcode-log', 'export-excel'])
</script>
