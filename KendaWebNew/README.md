# KendaWeb New - ASP.NET Core 8 + Vue 3

Dự án mới thay thế ASP.NET Web Forms cũ (`ApiBB.aspx`), sử dụng kiến trúc hiện đại.

## Kiến trúc

```
KendaWebNew/
├── KendaWeb.Api/           ← Backend ASP.NET Core 8 Web API
│   ├── Controllers/        ← API endpoints
│   ├── Services/           ← Business logic layer
│   ├── Repositories/       ← Data access layer (Dapper)
│   ├── Models/DTOs/        ← Data transfer objects
│   └── Configuration/      ← DB connection factory, Machine router
└── frontend/               ← Frontend Vue 3 + Vite
    ├── src/
    │   ├── views/          ← Trang chính (ApiBB.vue)
    │   ├── components/     ← Modal components
    │   ├── api/            ← API client (Axios)
    │   ├── composables/    ← Vue composables
    │   └── router/         ← Vue Router
    └── package.json
```

## Cải tiến so với bản cũ

| Vấn đề cũ | Giải pháp mới |
|-----------|---------------|
| SQL Injection (string concatenation) | Parameterized queries (Dapper) |
| Hardcode password trong source | `appsettings.json` + environment variables |
| 1 file 1592 dòng | Tách rõ Controller → Service → Repository |
| Copy-paste query 50 dòng x2 lần | 1 method dùng chung `GetWeighDataWithBarcodeAsync` |
| Switch/case máy lặp 5-6 chỗ | `MachineConfig` + `MachineRouter` (config-driven) |
| Postback mỗi click | SPA (Vue.js), AJAX calls |
| Không responsive | Element Plus responsive tables |
| Static TempData (thread-unsafe) | Stateless API, data trả về client |

## API Endpoints

| Endpoint | Mô tả | Tương đương code cũ |
|----------|-------|---------------------|
| `GET /api/SanLuong` | Xem sản lượng MES | `LoadData()` / `LoadDataTimkiem()` |
| `GET /api/SanLuong/export-excel` | Xuất Excel sản lượng | `btnExportExcel_Click` |
| `GET /api/NguyenLieu/{mesId}` | Xem nguyên liệu quét vào | `gvKQ_RowCommand (btnIn)` |
| `GET /api/InTem/{mesId}` | Xem dữ liệu in tem | `gvKQ_RowCommand (btnOut)` |
| `GET /api/DoNguoc/rl/{barcode}` | Đổ ngược barcode RL | `DoNguoc()` + case "RL" |
| `GET /api/DoNguoc/rb/{barcode}` | Đổ ngược barcode RB/RD/RC | `DoNguoc()` + case "RB"/"RD"/"RC" |
| `GET /api/HoaChat/{barcode}` | Xem hóa chất CWSS | `gvDoNguoc_RowCommand` case "V" |
| `GET /api/HoaChat/barcode-log` | Xem barcode log bồn HC | `gvHC_RowCommand` |

## Yêu cầu

### Backend
- .NET 8 SDK
- SQL Server (các server hiện có)

### Frontend
- Node.js >= 18
- npm

## Cách chạy

### Backend
```bash
cd KendaWeb.Api
# Cập nhật connection strings trong appsettings.json
dotnet restore
dotnet run
# API chạy tại http://localhost:5000
# Swagger UI: http://localhost:5000/swagger
```

### Frontend
```bash
cd frontend
npm install
npm run dev
# Frontend chạy tại http://localhost:5173
```

## Cấu hình

### Connection Strings (`appsettings.json`)

Thay `<USER>` và `<PASSWORD>` bằng thông tin thực tế:

```json
{
  "ConnectionStrings": {
    "ErpHome": "Data Source=198.1.10.33;...;User ID=<USER>;Password=<PASSWORD>;",
    "BbHome": "Data Source=198.1.10.33;...;User ID=<USER>;Password=<PASSWORD>;"
  },
  "MachineConfig": {
    "MfnsDbTemplate": "Data Source={ip};...;User ID=<USER>;Password=<PASSWORD>;",
    "CwssDbTemplate": "Data Source={ip};...;User ID=<USER>;Password=<PASSWORD>;"
  }
}
```

### Thêm máy mới

Chỉ cần thêm vào `MachineConfig` trong `appsettings.json`:

```json
"MfnsMachines": {
  "09": "198.1.8.39"
}
```

Không cần sửa code!

## Thư viện sử dụng

### Backend
- **Dapper** - Micro ORM, parameterized queries
- **ClosedXML** - Xuất Excel
- **Microsoft.Data.SqlClient** - SQL Server connection
- **Swashbuckle** - Swagger API documentation

### Frontend
- **Vue 3** - UI framework
- **Element Plus** - UI component library
- **Axios** - HTTP client
- **Vue Router** - Client-side routing
- **file-saver** - Download file
