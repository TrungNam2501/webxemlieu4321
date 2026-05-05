<template>
  <el-dialog
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    width="95%"
    :close-on-click-modal="false"
    top="2vh"
    :show-close="true"
  >
    <template #header>
      <div class="modal-header">
        <span>Hóa chất CWSS</span>
        <el-button type="success" size="small" @click="$emit('export-excel')" plain>
          <el-icon><Download /></el-icon>&nbsp;Xuất Excel
        </el-button>
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
        <el-table-column prop="dosingId" label="Dosing" width="80" align="center" />
        <el-table-column prop="planId" label="Plan ID" width="140" />
        <el-table-column prop="equipCode" label="Máy" width="70" align="center" />
        <el-table-column prop="serialNum" label="STT" width="55" align="center" />
        <el-table-column prop="weightId" label="Weight" width="70" align="center" />
        <el-table-column prop="materialCode" label="Mã VL" width="90" />
        <el-table-column prop="materialName" label="Tên vật liệu" min-width="140" />
        <el-table-column prop="realWeight" label="Cân thực" width="85" align="right" />
        <el-table-column prop="realError" label="Sai số" width="75" align="right" />
        <el-table-column prop="overWeight" label="Quá cân" width="80" align="right" />
        <el-table-column prop="overError" label="Quá SS" width="75" align="right" />
        <el-table-column prop="wasteTime" label="TG thải" width="90" />
        <el-table-column prop="warningSign" label="CB" width="50" align="center" />
        <el-table-column prop="weightTime" label="Thời gian cân" width="165" />
        <el-table-column prop="batchNumber" label="Batch" width="120" />
        <el-table-column prop="recipeCode" label="Recipe" width="110" />
        <el-table-column label="Bồn" width="65" align="center">
          <template #default="{ row }">
            <el-button
              class="action-btn"
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
    </div>
  </el-dialog>
</template>

<script setup>
import { Download, View } from '@element-plus/icons-vue'

defineProps({
  modelValue: Boolean,
  data: { type: Array, default: () => [] },
  loading: Boolean
})

defineEmits(['update:modelValue', 'view-barcode-log', 'export-excel'])
</script>
