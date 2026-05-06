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
        <span>{{ dataType === 'rl' ? 'Đổ ngược RL' : 'Đổ ngược RB/RD/RC' }}</span>
        <el-button type="success" size="small" @click="$emit('export-excel')" plain>
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
        <el-table-column prop="pday" label="Ngày" width="110" />
        <el-table-column prop="class" label="Ca" width="50" align="center" />
        <el-table-column prop="machno" label="Máy" width="110" />
        <el-table-column prop="mesid" label="MES ID" width="140" />
        <el-table-column prop="barcode" label="Barcode" width="160" />
        <el-table-column prop="partno" label="Tên keo" min-width="140" />
        <el-table-column prop="qty" label="SL" width="70" align="right" />
        <el-table-column prop="bacode" label="Ba code" width="160" />
        <el-table-column prop="itnbr" label="ITN" width="110" />
        <el-table-column prop="slipno" label="Số lô" width="110" />
        <el-table-column prop="intime" label="TG quét" width="100" />
        <el-table-column prop="indat" label="Ngày quét" width="110" />
        <el-table-column prop="usrno" label="Người quét" width="95" />
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
        <el-table-column prop="saveTime" label="Thời gian quét" width="170" />
        <el-table-column prop="equipId" label="Máy" width="70" align="center" />
        <el-table-column prop="materCode" label="Mã NL" width="110" />
        <el-table-column prop="materName" label="Tên nguyên liệu" min-width="150" />
        <el-table-column prop="setNum" label="Mẻ ĐĐ" width="80" align="center" />
        <el-table-column prop="serialNum" label="Mẻ HT" width="80" align="center" />
        <el-table-column prop="realWeight" label="Kg quét" width="90" align="right" />
        <el-table-column prop="materBarcode" label="Tem quét" width="160">
          <template #default="{ row }">
            <a
              v-if="row.materBarcode && (row.materBarcode.startsWith('RC') || row.materBarcode.startsWith('RB') || row.materBarcode.startsWith('RD') || row.materBarcode.startsWith('RL'))"
              class="barcode-link"
              @click="$emit('trace-barcode', row.materBarcode)"
            >{{ row.materBarcode }}</a>
            <span v-else>{{ row.materBarcode }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="batchNo" label="Số lô" width="130" />
        <el-table-column label="HC" width="70" align="center">
          <template #default="{ row }">
            <el-button
              v-if="row.materBarcode && row.materBarcode.startsWith('V')"
              class="action-btn"
              type="info"
              @click="$emit('view-hoachat', row.materBarcode)"
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

defineEmits(['update:modelValue', 'view-hoachat', 'trace-barcode', 'export-excel'])
</script>

<style scoped>
.barcode-link {
  color: var(--primary, #2563eb);
  cursor: pointer;
  text-decoration: underline;
  font-weight: 600;
}
.barcode-link:hover {
  color: var(--primary-dark, #1d4ed8);
}
</style>
