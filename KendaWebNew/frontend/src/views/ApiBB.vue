<template>
  <div class="page-container">
    <!-- Filter Bar -->
    <div class="filter-bar">
      <span class="page-title">Mes YAML:</span>

      <el-select v-model="selectedMay" placeholder="----Chọn máy----" style="width: 150px">
        <el-option label="----Chọn máy----" value="" />
        <el-option v-for="m in machines" :key="m.value" :label="m.label" :value="m.value" />
      </el-select>

      <el-date-picker
        v-model="fromDay"
        type="date"
        placeholder="---Từ ngày---"
        format="YYYY-MM-DD"
        value-format="YYYY-MM-DD"
        style="width: 160px"
      />

      <el-date-picker
        v-model="toDay"
        type="date"
        placeholder="---Đến ngày---"
        format="YYYY-MM-DD"
        value-format="YYYY-MM-DD"
        style="width: 160px"
      />

      <el-button type="danger" @click="handleXemLieu" :loading="loading">Xem liệu</el-button>

      <el-input
        v-model="searchText"
        placeholder="Nhập mã keo tìm kiếm"
        style="width: 200px"
        clearable
      />

      <el-button type="primary" @click="handleTimKiem" :loading="loading">Tìm kiếm</el-button>

      <el-button type="success" @click="handleExportExcel" :loading="exporting">Xuất Excel</el-button>
    </div>

    <!-- Main Table -->
    <div class="table-container">
      <el-table
        :data="sanLuongData"
        border
        stripe
        style="width: 100%"
        :header-cell-style="{ backgroundColor: '#4a6fa5', color: 'white', textAlign: 'center', fontWeight: 'bold' }"
        :cell-style="{ textAlign: 'center', fontWeight: 'bold', fontFamily: 'Arial', fontSize: '16px' }"
      >
        <el-table-column label="Xem nguyên liệu quét vào" width="120" align="center">
          <template #default="{ row }">
            <el-button size="small" type="primary" @click="handleXemNguyenLieu(row)">
              <el-icon><View /></el-icon>
            </el-button>
          </template>
        </el-table-column>
        <el-table-column prop="maMesid" label="Mã mesid" />
        <el-table-column prop="soMay" label="Số máy" width="80" />
        <el-table-column prop="tenKeo" label="Tên keo" />
        <el-table-column prop="soLo" label="Số lô" />
        <el-table-column prop="soMeDieuDong" label="Số mẻ điều động" width="100" />
        <el-table-column prop="soMeHoanThanh" label="Số mẻ hoàn thành" width="100" />
        <el-table-column prop="soKyTieuChuan" label="Số ký tiêu chuẩn" width="100" />
        <el-table-column prop="soKyDaQuetTem" label="Số ký đã quét tem" width="100" />
        <el-table-column prop="soKyHoanThanh" label="Số ký hoàn thành" width="100" />
        <el-table-column prop="soKyChenhLech" label="Số ký chênh lệch" width="100" />
        <el-table-column label="Xem dữ liệu in tem" width="120" align="center">
          <template #default="{ row }">
            <el-button size="small" type="success" @click="handleXemInTem(row)">
              <el-icon><Document /></el-icon>
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>

    <!-- NguyenLieu Modal -->
    <NguyenLieuModal
      v-model="showNguyenLieu"
      :data="nguyenLieuData"
      :loading="nguyenLieuLoading"
      @view-detail="handleDoNguoc"
      @export-excel="handleExportNguyenLieu"
    />

    <!-- InTem Modal -->
    <InTemModal
      v-model="showInTem"
      :data="inTemData"
      :total-count="inTemTotal"
      :loading="inTemLoading"
      @export-excel="handleExportInTem"
    />

    <!-- DoNguoc Modal -->
    <DoNguocModal
      v-model="showDoNguoc"
      :data="doNguocData"
      :data-type="doNguocType"
      :loading="doNguocLoading"
      @view-hoachat="handleXemHoaChat"
      @export-excel="handleExportDoNguoc"
    />

    <!-- HoaChat Modal -->
    <HoaChatModal
      v-model="showHoaChat"
      :data="hoaChatData"
      :loading="hoaChatLoading"
      @view-barcode-log="handleXemBarcodeLog"
      @export-excel="handleExportHoaChat"
    />

    <!-- BarcodeLog Modal -->
    <BarcodeLogModal
      v-model="showBarcodeLog"
      :data="barcodeLogData"
      :loading="barcodeLogLoading"
    />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { ElMessage } from 'element-plus'
import { sanLuongApi, nguyenLieuApi, inTemApi, doNguocApi, hoaChatApi } from '../api'
import { useExcelExport } from '../composables/useExcelExport'
import NguyenLieuModal from '../components/NguyenLieuModal.vue'
import InTemModal from '../components/InTemModal.vue'
import DoNguocModal from '../components/DoNguocModal.vue'
import HoaChatModal from '../components/HoaChatModal.vue'
import BarcodeLogModal from '../components/BarcodeLogModal.vue'

const { downloadExcel } = useExcelExport()

const machines = [
  { label: 'Máy 01', value: '01' },
  { label: 'Máy 02', value: '02' },
  { label: 'Máy 03', value: '03' },
  { label: 'Máy 04', value: '04' },
  { label: 'Máy 05', value: '05' },
  { label: 'Máy 06', value: '06' },
  { label: 'Máy 07', value: '07' },
  { label: 'Máy 08', value: '08' },
]

const selectedMay = ref('')
const fromDay = ref('')
const toDay = ref('')
const searchText = ref('')
const loading = ref(false)
const exporting = ref(false)

const sanLuongData = ref([])

// NguyenLieu state
const showNguyenLieu = ref(false)
const nguyenLieuData = ref([])
const nguyenLieuLoading = ref(false)
const currentNguyenLieuRow = ref(null)

// InTem state
const showInTem = ref(false)
const inTemData = ref([])
const inTemTotal = ref(0)
const inTemLoading = ref(false)
const currentInTemMesId = ref('')

// DoNguoc state
const showDoNguoc = ref(false)
const doNguocData = ref([])
const doNguocType = ref('rb')
const doNguocLoading = ref(false)

// HoaChat state
const showHoaChat = ref(false)
const hoaChatData = ref([])
const hoaChatLoading = ref(false)

// BarcodeLog state
const showBarcodeLog = ref(false)
const barcodeLogData = ref([])
const barcodeLogLoading = ref(false)

async function handleXemLieu() {
  searchText.value = ''
  await fetchSanLuong()
}

async function handleTimKiem() {
  await fetchSanLuong(searchText.value)
}

async function fetchSanLuong(maKeo) {
  if (!fromDay.value || !toDay.value) {
    ElMessage.warning('Vui lòng nhập ngày!')
    return
  }
  if (!selectedMay.value) {
    ElMessage.warning('Vui lòng chọn máy!')
    return
  }

  loading.value = true
  try {
    const params = {
      may: selectedMay.value,
      fromDay: fromDay.value,
      toDay: toDay.value
    }
    if (maKeo) params.maKeo = maKeo

    const { data } = await sanLuongApi.getSanLuong(params)
    if (data.success) {
      sanLuongData.value = data.data
    } else {
      ElMessage.error(data.message)
      sanLuongData.value = []
    }
  } catch (err) {
    ElMessage.error(err.response?.data?.message || 'Lỗi kết nối server!')
    sanLuongData.value = []
  } finally {
    loading.value = false
  }
}

async function handleExportExcel() {
  if (!fromDay.value || !toDay.value || !selectedMay.value) {
    ElMessage.warning('Vui lòng nhập đầy đủ thông tin!')
    return
  }

  exporting.value = true
  try {
    const params = {
      may: selectedMay.value,
      fromDay: fromDay.value,
      toDay: toDay.value
    }
    if (searchText.value) params.maKeo = searchText.value

    const { data } = await sanLuongApi.exportExcel(params)
    downloadExcel(data, `San luong BB ${fromDay.value} - ${toDay.value}.xlsx`)
  } catch {
    ElMessage.error('Không có dữ liệu để xuất Excel!')
  } finally {
    exporting.value = false
  }
}

async function handleXemNguyenLieu(row) {
  currentNguyenLieuRow.value = row
  nguyenLieuLoading.value = true
  showNguyenLieu.value = true

  try {
    const { data } = await nguyenLieuApi.getNguyenLieu(row.maMesid, row.soMay)
    if (data.success) {
      nguyenLieuData.value = data.data
    } else {
      ElMessage.error(data.message)
      nguyenLieuData.value = []
    }
  } catch (err) {
    ElMessage.error(err.response?.data?.message || 'Lỗi!')
    nguyenLieuData.value = []
  } finally {
    nguyenLieuLoading.value = false
  }
}

async function handleXemInTem(row) {
  currentInTemMesId.value = row.maMesid
  inTemLoading.value = true
  showInTem.value = true

  try {
    const { data } = await inTemApi.getInTem(row.maMesid)
    if (data.success) {
      inTemData.value = data.data.items
      inTemTotal.value = data.data.totalCount
    } else {
      ElMessage.error(data.message)
      inTemData.value = []
      inTemTotal.value = 0
    }
  } catch (err) {
    ElMessage.error(err.response?.data?.message || 'Lỗi!')
    inTemData.value = []
    inTemTotal.value = 0
  } finally {
    inTemLoading.value = false
  }
}

async function handleDoNguoc(barcode) {
  if (!barcode) return

  doNguocLoading.value = true
  showDoNguoc.value = true

  const prefix = barcode.substring(0, 2)

  try {
    if (prefix === 'RL') {
      doNguocType.value = 'rl'
      const { data } = await doNguocApi.getDoNguocRL(barcode)
      doNguocData.value = data.success ? data.data : []
      if (!data.success) ElMessage.error(data.message)
    } else {
      doNguocType.value = 'rb'
      const { data } = await doNguocApi.getDoNguocRB(barcode)
      doNguocData.value = data.success ? data.data : []
      if (!data.success) ElMessage.error(data.message)
    }
  } catch (err) {
    ElMessage.error(err.response?.data?.message || 'Lỗi!')
    doNguocData.value = []
  } finally {
    doNguocLoading.value = false
  }
}

async function handleXemHoaChat(barcode) {
  if (!barcode) return

  hoaChatLoading.value = true
  showHoaChat.value = true

  try {
    const { data } = await hoaChatApi.getHoaChat(barcode)
    hoaChatData.value = data.success ? data.data : []
    if (!data.success) ElMessage.error(data.message)
  } catch (err) {
    ElMessage.error(err.response?.data?.message || 'Lỗi!')
    hoaChatData.value = []
  } finally {
    hoaChatLoading.value = false
  }
}

async function handleXemBarcodeLog({ equipCode, materialName, materialCode, recordTime }) {
  barcodeLogLoading.value = true
  showBarcodeLog.value = true

  try {
    const { data } = await hoaChatApi.getBarcodeLog({
      equipCode, materialName, materialCode, recordTime
    })
    barcodeLogData.value = data.success ? data.data : []
    if (!data.success) ElMessage.error(data.message)
  } catch (err) {
    ElMessage.error(err.response?.data?.message || 'Lỗi!')
    barcodeLogData.value = []
  } finally {
    barcodeLogLoading.value = false
  }
}

async function handleExportNguyenLieu() {
  if (!currentNguyenLieuRow.value) return
  try {
    const { data } = await nguyenLieuApi.exportExcel(
      currentNguyenLieuRow.value.maMesid, currentNguyenLieuRow.value.soMay)
    downloadExcel(data, 'San luong chi tiet MES.xlsx')
  } catch {
    ElMessage.error('Không có dữ liệu để xuất Excel!')
  }
}

async function handleExportInTem() {
  if (!currentInTemMesId.value) return
  try {
    const { data } = await inTemApi.exportExcel(currentInTemMesId.value)
    downloadExcel(data, 'San luong chi tiet MES.xlsx')
  } catch {
    ElMessage.error('Không có dữ liệu để xuất Excel!')
  }
}

async function handleExportDoNguoc() {
  ElMessage.info('Chức năng đang phát triển')
}

async function handleExportHoaChat() {
  ElMessage.info('Chức năng đang phát triển')
}
</script>
