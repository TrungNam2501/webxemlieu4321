import axios from 'axios'

const api = axios.create({
  baseURL: '/api',
  timeout: 30000
})

export const sanLuongApi = {
  getSanLuong(params) {
    return api.get('/SanLuong', { params })
  },
  exportExcel(params) {
    return api.get('/SanLuong/export-excel', {
      params,
      responseType: 'blob'
    })
  }
}

export const nguyenLieuApi = {
  getNguyenLieu(mesId, soMay) {
    return api.get(`/NguyenLieu/${mesId}`, { params: { soMay } })
  },
  exportExcel(mesId, soMay) {
    return api.get(`/NguyenLieu/${mesId}/export-excel`, {
      params: { soMay },
      responseType: 'blob'
    })
  }
}

export const inTemApi = {
  getInTem(mesId) {
    return api.get(`/InTem/${mesId}`)
  },
  exportExcel(mesId) {
    return api.get(`/InTem/${mesId}/export-excel`, {
      responseType: 'blob'
    })
  }
}

export const doNguocApi = {
  getDoNguocRL(barcode) {
    return api.get(`/DoNguoc/rl/${barcode}`)
  },
  getDoNguocRB(barcode) {
    return api.get(`/DoNguoc/rb/${barcode}`)
  },
  exportExcelRB(barcode) {
    return api.get(`/DoNguoc/rb/${barcode}/export-excel`, {
      responseType: 'blob'
    })
  }
}

export const hoaChatApi = {
  getHoaChat(barcode) {
    return api.get(`/HoaChat/${barcode}`)
  },
  getBarcodeLog(params) {
    return api.get('/HoaChat/barcode-log', { params })
  },
  exportExcel(barcode) {
    return api.get(`/HoaChat/${barcode}/export-excel`, {
      responseType: 'blob'
    })
  }
}

export default api
