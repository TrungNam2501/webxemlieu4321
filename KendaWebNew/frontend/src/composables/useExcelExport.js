import { saveAs } from 'file-saver'

export function useExcelExport() {
  const downloadExcel = (blob, filename) => {
    saveAs(new Blob([blob]), filename)
  }

  return { downloadExcel }
}
