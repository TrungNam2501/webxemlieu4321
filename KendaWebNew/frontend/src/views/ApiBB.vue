<template>
  <div class="page-container">
    <!-- Page Header -->
    <div class="page-header">
      <el-icon :size="28" color="#2563eb"><Monitor /></el-icon>
      <div>
        <span class="page-title">Quản lý sản lượng BB</span>
        <span class="page-subtitle">&nbsp;&mdash;&nbsp;Theo dõi &amp; tra cứu dữ liệu sản xuất</span>
      </div>
    </div>

    <!-- Filter Card -->
    <div class="filter-card">
      <div class="filter-bar">
        <div class="filter-group">
          <span class="filter-label">Máy</span>
          <el-select v-model="selectedMay" placeholder="Chọn máy" style="width: 140px">
            <el-option v-for="m in machines" :key="m.value" :label="m.label" :value="m.value" />
          </el-select>
        </div>

        <div class="filter-divider" />

        <div class="filter-group">
          <span class="filter-label">Từ</span>
          <el-date-picker
            v-model="fromDay"
            type="date"
            placeholder="Từ ngày"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
            style="width: 160px"
          />
        </div>

        <div class="filter-group">
          <span class="filter-label">Đến</span>
          <el-date-picker
            v-model="toDay"
            type="date"
            placeholder="Đến ngày"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
            style="width: 160px"
          />
        </div>

        <el-button type="danger" @click="handleXemLieu" :loading="loading">
          <el-icon><Search /></el-icon>&nbsp;Xem liệu
        </el-button>

        <div class="filter-divider" />

        <div class="filter-group">
          <el-input
            v-model="searchText"
            placeholder="Nhập mã keo tìm kiếm..."
            style="width: 220px"
            clearable
            :prefix-icon="Search"
          />
          <el-button type="primary" @click="handleTimKiem" :loading="loading">Tìm kiếm</el-button>
        </div>

        <el-button type="success" @click="handleExportExcel" :loading="exporting" plain>
          <el-icon><Download /></el-icon>&nbsp;Xuất Excel
        </el-button>
      </div>
    </div>

    <!-- Table Card -->
    <div class="table-card">
      <div class="table-toolbar" v-if="sanLuongData.length > 0">
        <span class="count-badge">
          Hiển thị <strong>{{ sanLuongData.length }}</strong> dòng
        </span>
      </div>

      <div class="table-container">
        <el-table
          :data="sanLuongData"
          border
          stripe
          style="width: 100%"
          empty-text="Chọn máy và ngày rồi bấm Xem liệu"
        >
          <el-table-column label="NL" width="70" align="center" fixed="left">
            <template #default="{ row }">
              <el-button class="action-btn" type="primary" @click="handleXemNguyenLieu(row)">
                <el-icon><View /></el-icon>
              </el-button>
            </template>
          </el-table-column>
          <el-table-column prop="maMesid" label="Mã MES ID" min-width="140" />
          <el-table-column prop="soMay" label="Máy" width="70" align="center" />
          <el-table-column prop="tenKeo" label="Tên keo" min-width="120" />
          <el-table-column prop="soLo" label="Số lô" min-width="100" />
          <el-table-column prop="soMeDieuDong" label="Mẻ ĐĐ" width="80" align="center" />
          <el-table-column prop="soMeHoanThanh" label="Mẻ HT" width="80" align="center" />
          <el-table-column prop="soKyTieuChuan" label="Kg TC" width="80" align="right" />
          <el-table-column prop="soKyDaQuetTem" label="Kg quét" width="80" align="right" />
          <el-table-column prop="soKyHoanThanh" label="Kg HT" width="80" align="right" />
          <el-table-column prop="soKyChenhLech" label="Chênh lệch" width="95" align="right">
            <template #default="{ row }">
              <span :style="{ color: Number(row.soKyChenhLech) < 0 ? '#ef4444' : '#10b981', fontWeight: 600 }">
                {{ row.soKyChenhLech }}
              </span>
            </template>
          </el-table-column>
          <el-table-column label="In tem" width="70" align="center" fixed="right">
            <template #default="{ row }">
              <el-button class="action-btn" type="success" @click="handleXemInTem(row)">
                <el-icon><Document /></el-icon>
              </el-button>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </div>

    <!-- Modals -->
    <NguyenLieuModal
      v-model="showNguyenLieu"
      :data="nguyenLieuData"
      :loading="nguyenLieuLoading"
      @view-detail="handleDoNguoc"
      @export-excel="handleExportNguyenLieu"
    />

    <InTemModal
      v-model="showInTem"
      :data="inTemData"
      :total-count="inTemTotal"
      :loading="inTemLoading"
      @export-excel="handleExportInTem"
    />

    <DoNguocModal
      v-model="showDoNguoc"
      :data="doNguocData"
      :data-type="doNguocType"
      :loading="doNguocLoading"
      @view-hoachat="handleXemHoaChat"
      @export-excel="handleExportDoNguoc"
    />

    <HoaChatModal
      v-model="showHoaChat"
      :data="hoaChatData"
      :loading="hoaChatLoading"
      @view-barcode-log="handleXemBarcodeLog"
      @export-excel="handleExportHoaChat"
    />

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
import { Search, Download, View, Document, Monitor } from '@element-plus/icons-vue'
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

  const prefix2 = barcode.substring(0, 2)
  const prefix1 = barcode.substring(0, 1)

  if (prefix1 === 'V') {
    await handleXemHoaChat(barcode)
    return
  }

  if (prefix2 === 'RL' || prefix2 === 'RB' || prefix2 === 'RD' || prefix2 === 'RC') {
    doNguocLoading.value = true
    showDoNguoc.value = true

    try {
      if (prefix2 === 'RL') {
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
    return
  }

  ElMessage.info('Barcode này không hỗ trợ dò ngược')
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
