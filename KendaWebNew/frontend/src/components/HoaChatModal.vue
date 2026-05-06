<template>
  <el-dialog
    :model-value="modelValue"
    @update:model-value="emit('update:modelValue', $event)"
    width="95%"
    :close-on-click-modal="false"
    top="2vh"
    :show-close="true"
  >
    <template #header>
      <div class="modal-header">
        <span>Hóa chất CWSS</span>
        <el-button type="success" size="small" @click="emit('export-excel')" plain>
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
        <el-table-column prop="dosingId" label="Dosing" width="65" align="center" />
        <el-table-column prop="planId" label="Plan ID" min-width="110" show-overflow-tooltip />
        <el-table-column prop="equipDisplay" label="Máy" width="70" align="center" />
        <el-table-column prop="serialNum" label="STT" width="50" align="center" />
        <el-table-column prop="weightId" label="Wt" width="45" align="center" />
        <el-table-column prop="materialCode" label="Mã VL" min-width="75" show-overflow-tooltip />
        <el-table-column prop="materialName" label="Tên vật liệu" min-width="110" show-overflow-tooltip />
        <el-table-column prop="realWeight" label="Cân" width="65" align="right" />
        <el-table-column prop="realError" label="SS" width="60" align="right" />
        <el-table-column prop="overWeight" label="Q.cân" width="65" align="right" />
        <el-table-column prop="overError" label="Q.SS" width="60" align="right" />
        <el-table-column prop="wasteTime" label="TG thải" min-width="80" show-overflow-tooltip />
        <el-table-column prop="warningSign" label="CB" width="45" align="center" />
        <el-table-column prop="weightTime" label="TG cân" min-width="135" show-overflow-tooltip />
        <el-table-column prop="batchNumber" label="Batch" min-width="90" show-overflow-tooltip />
        <el-table-column prop="recipeCode" label="Recipe" min-width="90" show-overflow-tooltip />
        <el-table-column label="Bồn" width="65" align="center" fixed="right">
          <template #default="scope">
            <el-button
              class="action-btn"
              type="primary"
              @click="onViewBarcodeLog(scope.row)"
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

const emit = defineEmits(['update:modelValue', 'view-barcode-log', 'export-excel'])

function onViewBarcodeLog(row) {
  emit('view-barcode-log', {
    equipCode: row.equipCode,
    materialName: row.materialName,
    materialCode: row.materialCode,
    recordTime: row.weightTime
  })
}
</script>
