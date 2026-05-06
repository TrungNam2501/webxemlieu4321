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
        <span>Nguyên liệu quét vào</span>
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
        <el-table-column prop="saveTime" label="Thời gian quét" width="145" />
        <el-table-column prop="equipId" label="Máy" width="55" align="center" />
        <el-table-column prop="materCode" label="Mã NL" width="95" />
        <el-table-column prop="materName" label="Tên nguyên liệu" width="110" />
        <el-table-column prop="setNum" label="Mẻ ĐĐ" width="65" align="center" />
        <el-table-column prop="serialNum" label="Mẻ HT" width="65" align="center" />
        <el-table-column prop="realWeight" label="Kg quét" width="80" align="right" />
        <el-table-column label="Tem quét" min-width="155">
          <template #default="{ row }">
            <a
              v-if="row.materBarcode"
              class="barcode-link"
              @click="$emit('view-detail', row.materBarcode)"
            >{{ row.materBarcode }}</a>
          </template>
        </el-table-column>
        <el-table-column prop="batchNo" label="Số lô" width="100" />
        <el-table-column label="Chi tiết" width="80" align="center">
          <template #default="{ row }">
            <el-button
              v-if="row.materBarcode"
              class="action-btn"
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
import { Download, View } from '@element-plus/icons-vue'

defineProps({
  modelValue: Boolean,
  data: { type: Array, default: () => [] },
  loading: Boolean
})

defineEmits(['update:modelValue', 'view-detail', 'export-excel'])
</script>
