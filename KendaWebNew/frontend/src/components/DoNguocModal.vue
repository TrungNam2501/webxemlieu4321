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
        <span>{{ dataType === 'rl' ? 'Đổ ngược RL' : 'Đổ ngược RB/RD/RC' }}</span>
        <el-button type="success" size="small" @click="emit('export-excel')" plain>
          <el-icon><Download /></el-icon>&nbsp;Xuất Excel
        </el-button>
      </div>
    </template>

    <div class="modal-body">
      <!-- RL Table -->
      <el-table
        v-if="dataType === 'rl'"
        :data="data"
        border
        stripe
        v-loading="loading"
        style="width: 100%"
        empty-text="Không có dữ liệu"
      >
        <el-table-column prop="pday" label="Ngày" min-width="100" show-overflow-tooltip />
        <el-table-column prop="class" label="Ca" width="50" align="center" />
        <el-table-column prop="machno" label="Máy" min-width="100" show-overflow-tooltip />
        <el-table-column prop="mesid" label="MES ID" min-width="130" show-overflow-tooltip />
        <el-table-column prop="barcode" label="Barcode" min-width="140" show-overflow-tooltip />
        <el-table-column prop="partno" label="Tên keo" min-width="130" show-overflow-tooltip />
        <el-table-column prop="qty" label="SL" width="65" align="right" />
        <el-table-column prop="bacode" label="Ba code" min-width="140" show-overflow-tooltip />
        <el-table-column prop="itnbr" label="ITN" min-width="100" show-overflow-tooltip />
        <el-table-column prop="slipno" label="Số lô" min-width="100" show-overflow-tooltip />
        <el-table-column prop="intime" label="TG quét" min-width="90" show-overflow-tooltip />
        <el-table-column prop="indat" label="Ngày quét" min-width="100" show-overflow-tooltip />
        <el-table-column prop="usrno" label="Người quét" min-width="85" show-overflow-tooltip />
      </el-table>

      <!-- RB/RD/RC Table -->
      <el-table
        v-else
        :data="data"
        border
        stripe
        v-loading="loading"
        style="width: 100%"
        empty-text="Không có dữ liệu"
      >
        <el-table-column prop="saveTime" label="Thời gian quét" min-width="145" show-overflow-tooltip />
        <el-table-column prop="equipId" label="Máy" width="55" align="center" />
        <el-table-column prop="materCode" label="Mã NL" min-width="90" show-overflow-tooltip />
        <el-table-column prop="materName" label="Tên nguyên liệu" min-width="100" show-overflow-tooltip />
        <el-table-column prop="setNum" label="Mẻ ĐĐ" width="65" align="center" />
        <el-table-column prop="serialNum" label="Mẻ HT" width="65" align="center" />
        <el-table-column prop="realWeight" label="Kg quét" width="80" align="right" />
        <el-table-column prop="materBarcode" label="Tem quét" min-width="140" show-overflow-tooltip />
        <el-table-column prop="batchNo" label="Số lô" min-width="95" show-overflow-tooltip />
        <el-table-column label="Chi tiết" width="80" align="center" fixed="right">
          <template #default="scope">
            <el-button
              v-if="scope.row.materBarcode"
              class="action-btn"
              type="warning"
              @click="onTraceBarcode(scope.row.materBarcode)"
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
  dataType: { type: String, default: 'rb' },
  loading: Boolean
})

const emit = defineEmits(['update:modelValue', 'view-hoachat', 'trace-barcode', 'export-excel'])

function onTraceBarcode(barcode) {
  if (!barcode) return
  const prefix = String(barcode).charAt(0)
  if (prefix === 'V') {
    emit('view-hoachat', barcode)
  } else {
    emit('trace-barcode', barcode)
  }
}
</script>
