<template>
  <el-dialog
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    title="Dữ liệu in tem"
    width="95%"
    :close-on-click-modal="false"
    top="2vh"
  >
    <template #header>
      <div class="modal-header">
        <div>
          <span style="font-size: 20px; font-weight: 600">Tổng số lượng:</span>
          <span style="font-size: 20px; font-weight: 600; margin-left: 15px">{{ totalCount }}</span>
        </div>
        <el-button type="success" @click="$emit('export-excel')">Xuất file Excel</el-button>
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
        <el-table-column prop="mesid" label="Mã MES" width="130" />
        <el-table-column prop="machno" label="Máy" width="100" />
        <el-table-column prop="daylimt" label="Hạn Sử Dụng" width="120" />
        <el-table-column prop="barcode" label="Mã Vạch" width="160" />
        <el-table-column prop="slipno" label="Số Lô" width="120" />
        <el-table-column prop="weight" label="Trọng Lượng" width="100" />
        <el-table-column prop="prodat" label="Ngày Sản Xuất" width="120" />
        <el-table-column prop="effdat" label="Ngày Hiệu Lực" width="120" />
        <el-table-column prop="class" label="Ca" width="60" />
        <el-table-column prop="partno" label="Tên KEO" min-width="150" />
        <el-table-column prop="intime" label="Thời Gian Quét" width="120" />
        <el-table-column prop="indat" label="Ngày Quét" width="120" />
        <el-table-column prop="usrno" label="Người Quét" width="100" />
        <el-table-column prop="palletNo" label="Mã Palet" width="120" />
        <el-table-column prop="someSx" label="Số mẻ" width="80" />
      </el-table>
    </div>
  </el-dialog>
</template>

<script setup>
defineProps({
  modelValue: Boolean,
  data: { type: Array, default: () => [] },
  totalCount: { type: Number, default: 0 },
  loading: Boolean
})

defineEmits(['update:modelValue', 'export-excel'])
</script>
