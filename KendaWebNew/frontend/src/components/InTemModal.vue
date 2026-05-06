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
        <div style="display: flex; align-items: center; gap: 16px;">
          <span>Dữ liệu in tem</span>
          <span style="background: rgba(255,255,255,0.2); padding: 4px 12px; border-radius: 20px; font-size: 13px;">
            Tổng: <strong>{{ totalCount }}</strong>
          </span>
        </div>
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
        <el-table-column prop="mesid" label="Mã MES" width="120" />
        <el-table-column prop="machno" label="Máy" width="70" />
        <el-table-column prop="daylimt" label="Hạn SD" width="100" />
        <el-table-column prop="barcode" label="Mã vạch" min-width="130" />
        <el-table-column prop="slipno" label="Số lô" width="100" />
        <el-table-column prop="weight" label="KG" width="70" align="right" />
        <el-table-column prop="prodat" label="Ngày SX" width="100" />
        <el-table-column prop="effdat" label="Ngày HL" width="100" />
        <el-table-column prop="class" label="Ca" width="45" align="center" />
        <el-table-column prop="partno" label="Tên keo" min-width="110" />
        <el-table-column prop="intime" label="TG quét" width="85" />
        <el-table-column prop="indat" label="Ngày quét" width="100" />
        <el-table-column prop="usrno" label="Người" width="75" />
        <el-table-column prop="palletNo" label="Pallet" width="100" />
        <el-table-column prop="someSx" label="Mẻ" width="50" align="center" />
      </el-table>
    </div>
  </el-dialog>
</template>

<script setup>
import { Download } from '@element-plus/icons-vue'

defineProps({
  modelValue: Boolean,
  data: { type: Array, default: () => [] },
  totalCount: { type: Number, default: 0 },
  loading: Boolean
})

defineEmits(['update:modelValue', 'export-excel'])
</script>
