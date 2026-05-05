<template>
  <el-dialog
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    :title="dataType === 'rl' ? 'Đổ ngược RL' : 'Đổ ngược RB/RD/RC'"
    width="95%"
    :close-on-click-modal="false"
    top="2vh"
  >
    <template #header>
      <div class="modal-header">
        <el-button type="success" @click="$emit('export-excel')">Xuất Excel</el-button>
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
        :header-cell-style="{ backgroundColor: '#4a6fa5', color: 'white', textAlign: 'center', fontWeight: 'bold' }"
        :cell-style="{ textAlign: 'center', fontWeight: 'bold', fontFamily: 'Arial', fontSize: '14px' }"
      >
        <el-table-column prop="pday" label="Ngày" width="120" />
        <el-table-column prop="class" label="Ca" width="60" />
        <el-table-column prop="machno" label="Máy" width="120" />
        <el-table-column prop="mesid" label="MES ID" width="150" />
        <el-table-column prop="barcode" label="Barcode" width="160" />
        <el-table-column prop="partno" label="Tên keo" min-width="150" />
        <el-table-column prop="qty" label="Số lượng" width="100" />
        <el-table-column prop="bacode" label="Ba code" width="160" />
        <el-table-column prop="itnbr" label="ITN" width="120" />
        <el-table-column prop="slipno" label="Số lô" width="120" />
        <el-table-column prop="intime" label="Thời gian" width="120" />
        <el-table-column prop="indat" label="Ngày quét" width="120" />
        <el-table-column prop="usrno" label="Người quét" width="100" />
      </el-table>

      <!-- RB/RD/RC Table -->
      <el-table
        v-else
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
        <el-table-column prop="setNum" label="Số mẻ ĐĐ" width="80" />
        <el-table-column prop="serialNum" label="Số mẻ HT" width="80" />
        <el-table-column prop="realWeight" label="Số ký quét" width="100" />
        <el-table-column prop="materBarcode" label="Tem quét" width="160" />
        <el-table-column prop="batchNo" label="Số lô" width="130" />
        <el-table-column label="Xem HC" width="80" align="center">
          <template #default="{ row }">
            <el-button
              v-if="row.materBarcode && row.materBarcode.startsWith('V')"
              size="small"
              type="info"
              @click="$emit('view-hoachat', row.materBarcode)"
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
  dataType: { type: String, default: 'rb' },
  loading: Boolean
})

defineEmits(['update:modelValue', 'view-hoachat', 'export-excel'])
</script>
