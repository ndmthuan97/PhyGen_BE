# PhyGen Backend - Hệ Thống Quản Lý Đào Tạo & Tạo Đề Thi Vật Lý Tự Động

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Cloudinary](https://img.shields.io/badge/Cloudinary-3448C5?style=for-the-badge&logo=cloudinary&logoColor=white)

## 📌 Giới Thiệu
**PhyGen Backend** là hệ thống máy chủ cốt lõi cho nền tảng giáo dục chuyên sâu về vật lý. Dự án được thiết kế để quản lý chương trình học, ngân hàng câu hỏi và đặc biệt là khả năng **tạo đề thi tự động** dựa trên ma trận đề thi phức tạp. 

Hệ thống được xây dựng trên nền tảng **.NET 8** mạnh mẽ, áp dụng kiến trúc **Clean Architecture** và các nguyên lý **DDD (Domain-Driven Design)** để đảm bảo tính mở rộng, bảo trì và hiệu suất cao.

## ✨ Tính Năng Chính
- 🏫 **Quản Lý Chương Trình Học**: Tổ chức linh hoạt theo Chương trình (Curriculum) -> Môn học (Subject) -> Chương (Chapter) -> Chủ đề (Topic) -> Mục (Section).
- 📝 **Ngân Hàng Câu Hỏi**: Hỗ trợ đa dạng loại câu hỏi, đính kèm hình ảnh/media qua Cloudinary, phân loại theo độ khó và mục tiêu đào tạo.
- ⚙️ **Tạo Đề Thi Tự Động (Matrixized Exam)**: Xây dựng ma trận đề thi chi tiết và hệ thống sẽ tự động tổng hợp đề thi đáp ứng yêu cầu chính xác về kiến thức và độ khó.
- 💳 **Tích Hợp Thanh Toán**: Hỗ trợ giao dịch an toàn qua cổng **PayOS**.
- 🔐 **Bảo Mật & Xác Thực**: 
  - Đăng nhập/Đăng ký với mã OTP qua Email.
  - Phân quyền người dùng chi tiết (RBAC).
  - Sử dụng JWT Bearer token để bảo mật API.
- 📧 **Thông Báo**: Hệ thống thông báo thời gian thực về trạng thái khóa học, đề thi và giao dịch.
- 💬 **Hỗ Trợ Trực Tuyến**: Tích hợp module Chatbox hỗ trợ giải đáp thắc mắc và tương tác người dùng.
- 📄 **Xuất Bản Tài Liệu**: Hỗ trợ xuất đề thi và tài liệu ra định dạng Word/PDF chuyên nghiệp.

## 🛠 Công Nghệ Sử Dụng
### Backend & Core
- **Framework**: .NET 8.0 (Web API)
- **Database**: PostgreSQL (Entity Framework Core)
- **Caching**: Redis
- **Auth**: JWT, BCrypt for hashing
- **Mapping**: AutoMapper / Mapster

### Integrations
- **Cloud Storage**: Cloudinary (Quản lý hình ảnh và tài nguyên tĩnh)
- **Payment Gateway**: PayOS
- **Docs Generation**: FreeSpire.Doc, ClosedXML
- **Containerization**: Docker & Docker Compose

## 🏗 Kiến Trúc Dự Án
Dự án tuân thủ mô hình **Clean Architecture**:
- `PhyGen.Domain`: Chứa các thực thể (Entities), giá trị (Value Objects), giao diện (Interfaces) và logic cốt lõi.
- `PhyGen.Application`: Chứa logic ứng dụng, xử lý các Commands/Queries, DTOs và Mapping.
- `PhyGen.Infrastructure`: Thực thi các giao diện từ Domain như Persistence (EF Core), External Services (Cloudinary, PayOS), Identity.
- `PhyGen.API`: Các Endpoint RESTful, cấu hình Middleware và Authentication.
- `PhyGen.Shared`: Chứa các tiện ích, hằng số dùng chung cho toàn bộ solution.

## 🚀 Hướng Dẫn Cài Đặt

### Yêu Cầu Hệ Thống
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Redis](https://redis.io/download/) (tùy chọn nhưng khuyến nghị cho production)
- Docker Desktop (nếu chạy qua container)

### Các Bước Thực Hiện
1. **Clone repository**:
   ```bash
   git clone https://github.com/your-username/PhyGen_BE.git
   cd PhyGen_BE
   ```

2. **Cấu hình môi trường**:
   - Cập nhật Connection String trong `PhyGen.API/appsettings.json`.
   - Cấu hình các API Key cho Cloudinary và PayOS.

3. **Cập nhật Database**:
   ```bash
   dotnet ef database update --project PhyGen.Infrastructure --startup-project PhyGen.API
   ```

4. **Chạy ứng dụng**:
   ```bash
   dotnet run --project PhyGen.API
   ```

5. **Truy cập Swagger**:
   Mở trình duyệt và truy cập `http://localhost:<port>/swagger` để xem tài liệu API.

## 📄 Tài Liệu API
Toàn bộ API được tài liệu hóa tự động qua **Swagger UI**, cung cấp giao diện thử nghiệm trực quan cho các nhà phát triển frontend.

## 🤝 Đóng Góp
Mọi đóng góp nhằm hoàn thiện dự án đều được trân trọng. Vui lòng tạo **Issue** hoặc gửi **Pull Request** nếu bạn có ý tưởng cải tiến.

---
*Phát triển bởi đội ngũ kỹ thuật PhyGen.*