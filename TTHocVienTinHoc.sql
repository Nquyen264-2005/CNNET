USE master;
GO

-- 1. Đặt Database mục tiêu vào chế độ SINGLE_USER để ngắt kết nối và xóa
IF DB_ID(N'QLHocVienTinHoc') IS NOT NULL
BEGIN
	ALTER DATABASE QLHocVienTinHoc SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE QLHocVienTinHoc;
END
GO

CREATE DATABASE QLHocVienTinHoc
GO
USE QLHocVienTinHoc
GO

--------------------------------------------------------------------------------
-- PHẦN 1: QUẢN LÝ TÀI KHOẢN VÀ PHÂN QUYỀN
--------------------------------------------------------------------------------

-- 1. BẢNG QUYENNGUOIDUNG

CREATE TABLE QUYENNGUOIDUNG(
    MAQUYEN CHAR(5) NOT NULL,
    TENQUYEN NVARCHAR(50) NOT NULL UNIQUE,
    MOTA NVARCHAR(250) NULL,
	CONSTRAINT PK_QUYENNGUOIDUNG PRIMARY KEY(MAQUYEN)
);

-- 2. BẢNG TAIKHOAN
CREATE TABLE TAIKHOAN(
    MATAIKHOAN CHAR(5) NOT NULL,
    TENDANGNHAP NVARCHAR(100) NOT NULL UNIQUE,
    MATKHAU NVARCHAR(100) NOT NULL, 
    HOTEN NVARCHAR(200) NULL,
    EMAIL NVARCHAR(200) NULL,
    SODIENTHOAI CHAR(10) NULL,
    MAQUYEN CHAR(5) NOT NULL,
    CONHOATDONG BIT DEFAULT 1, 
    NGAYTAO DATETIME2 DEFAULT SYSUTCDATETIME(),
	CONSTRAINT PK_TAIKHOAN PRIMARY KEY(MATAIKHOAN),
    CONSTRAINT FK_TAIKHOAN_QUYENNGUOIDUNG FOREIGN KEY(MAQUYEN) REFERENCES QUYENNGUOIDUNG(MAQUYEN)
);

--------------------------------------------------------------------------------
-- PHẦN 2: QUẢN LÝ NHÂN SỰ VÀ NGƯỜI DÙNG
--------------------------------------------------------------------------------

-- 3. BẢNG HOCVIEN
CREATE TABLE HOCVIEN(
    MAHOCVIEN CHAR(5) NOT NULL,
    MATAIKHOAN CHAR(5),  
    HOTEN NVARCHAR(100) NOT NULL, 
    GIOITINH NVARCHAR(10) NULL,
    NGAYSINH DATE NULL,
    EMAIL NVARCHAR(200) NULL,
    SODIENTHOAI CHAR(10) NULL,
    DIACHI NVARCHAR(200) NULL,
    NGAYTAOTKHV DATETIME2 DEFAULT SYSUTCDATETIME(),
	GHICHU NVARCHAR(100),
	CONSTRAINT PK_HOCVIEN PRIMARY KEY(MAHOCVIEN),
	CONSTRAINT FK_HOCVIEN_TAIKHOAN FOREIGN KEY(MATAIKHOAN) REFERENCES TAIKHOAN(MATAIKHOAN)
);
-- Thêm Filtered Unique Index: Đảm bảo MATAIKHOAN là DUY NHẤT chỉ khi nó KHÔNG NULL
CREATE UNIQUE INDEX UIX_HOCVIEN_MATAIKHOAN ON HOCVIEN (MATAIKHOAN) WHERE (MATAIKHOAN IS NOT NULL);
CREATE INDEX IDX_HOCVIEN_EMAIL ON HOCVIEN(EMAIL);

-- 4. BẢNG GIAOVIEN
CREATE TABLE GIAOVIEN (
    MAGIANGVIEN CHAR(5) NOT NULL,
    MATAIKHOAN CHAR(5),  
    HOTEN NVARCHAR(100) NOT NULL, 
    EMAIL NVARCHAR(200) NULL,
    SODIENTHOAI CHAR(10) NULL,
	TRINHDO NVARCHAR(50),
	CHUYENNGANH NVARCHAR(50),
	LOAIHOPDONG NVARCHAR(50),
	TRANGTHAI NVARCHAR(50),
	GHICHU NVARCHAR(100),
	CONSTRAINT PK_GIAOVIEN PRIMARY KEY(MAGIANGVIEN),
	CONSTRAINT FK_GIAOVIEN_TAIKHOAN FOREIGN KEY(MATAIKHOAN) REFERENCES TAIKHOAN(MATAIKHOAN)
);
-- Thêm Filtered Unique Index
CREATE UNIQUE INDEX UIX_GIAOVIEN_MATAIKHOAN ON GIAOVIEN (MATAIKHOAN) WHERE (MATAIKHOAN IS NOT NULL);

-- 5. BẢNG NHANSU
CREATE TABLE NHANSU(
   MANHANSU CHAR(5) NOT NULL,
   MATAIKHOAN CHAR(5),  
   HOTEN NVARCHAR(100) NOT NULL, 
   NGAYSINH DATE,
   CHUCDANH NVARCHAR(50), 
   DIACHI NVARCHAR(200),
   MOTA NVARCHAR(200),
   EMAIL NVARCHAR(200) NULL, 
   SODIENTHOAI CHAR(10) NULL,
   CONSTRAINT PK_NHANSU PRIMARY KEY(MANHANSU),
    CONSTRAINT FK_NHANSU_TAIKHOAN FOREIGN KEY(MATAIKHOAN) REFERENCES TAIKHOAN(MATAIKHOAN)
)
-- Thêm Filtered Unique Index
CREATE UNIQUE INDEX UIX_NHANSU_MATAIKHOAN ON NHANSU (MATAIKHOAN) WHERE (MATAIKHOAN IS NOT NULL);


-- 6. BẢNG TUYENDUNG 
CREATE TABLE TUYENDUNG(
    MATUYENDUNG CHAR(5) NOT NULL,
	MANHANSU CHAR(5) NOT NULL, 
	NGAYTUYENDUNG DATE,
	MOTA NVARCHAR(200),
	CONSTRAINT PK_TUYENDUNG PRIMARY KEY(MATUYENDUNG),
	CONSTRAINT FK_TUYENDUNG_NHANSU FOREIGN KEY(MANHANSU) REFERENCES NHANSU(MANHANSU)
);

--------------------------------------------------------------------------------
-- PHẦN 3: QUẢN LÝ KHÓA HỌC VÀ LỚP HỌC
--------------------------------------------------------------------------------

-- 7. BẢNG KHOAHOC 
CREATE TABLE KHOAHOC(
    MAKHOAHOC CHAR(5) NOT NULL,
    TENKHOAHOC NVARCHAR(100) NOT NULL UNIQUE, 
    MOTA NVARCHAR(200) NULL,
    TONGSOGIO INT NULL,
    HOCPHI MONEY,
	CHINHANH NVARCHAR(200),
	CONSTRAINT PK_KHOAHOC PRIMARY KEY(MAKHOAHOC)
);

-- 8. BẢNG LOPHOC
CREATE TABLE LOPHOC(
    MALOPHOC CHAR(5) NOT NULL,
    MAKHOAHOC CHAR(5) NOT NULL, 
    MAGIANGVIEN CHAR(5), 
    NGAYBATDAU DATE NOT NULL,
    NGAYKETTHUC DATE NOT NULL,
    SISOTOIDA INT NULL,
    DIADIEM NVARCHAR(200) NULL,
    TRANGTHAI NVARCHAR(50) DEFAULT N'Chưa mở', 
	CONSTRAINT PK_LOPHOC PRIMARY KEY(MALOPHOC),
    CONSTRAINT FK_LOPHOC_KHOAHOC FOREIGN KEY(MAKHOAHOC) REFERENCES KHOAHOC(MAKHOAHOC),
    CONSTRAINT FK_LOPHOC_GIAOVIEN FOREIGN KEY(MAGIANGVIEN) REFERENCES GIAOVIEN(MAGIANGVIEN)
);
CREATE INDEX IDX_LOPHOC_KHOAHOC_NGAY ON LOPHOC(MAKHOAHOC, NGAYBATDAU);

-- 9. BẢNG LICHHOC
CREATE TABLE LICHHOC(
    MALICHHOC INT IDENTITY(1,1),
    MALOPHOC CHAR(5) NOT NULL, 
    NGAYTRONGTUAN NVARCHAR(20) NOT NULL,
    GIOBATDAU TIME NOT NULL,
    GIOKETTHUC TIME NOT NULL,
    PHONGHOC NVARCHAR(50) NULL,
	CONSTRAINT PK_LICHHOC PRIMARY KEY(MALICHHOC),
    CONSTRAINT FK_LICHHOC_LOPHOC FOREIGN KEY(MALOPHOC) REFERENCES LOPHOC(MALOPHOC)
);
CREATE INDEX IDX_LICHHOC_LOPHOC ON LICHHOC(MALOPHOC);

--------------------------------------------------------------------------------
-- PHẦN 4: QUẢN LÝ GHI DANH VÀ TÀI CHÍNH
--------------------------------------------------------------------------------

-- 11. KHUYENMAI (Tạo trước để làm Khóa ngoại)
CREATE TABLE KHUYENMAI(
  MAKHUYENMAI CHAR(5) NOT NULL,
  TENKHUYENMAI NVARCHAR(50) NOT NULL UNIQUE, 
  GIAMGIA MONEY,
  LOAIGIAMGIA NVARCHAR(20) NULL, 
  NGAYBATDAUKM DATE,
  NGAYKETTHUCKM DATE,
  TRANGTHAI NVARCHAR(20) DEFAULT N'Sắp diễn ra',
  CONSTRAINT PK_KHUYENMAI PRIMARY KEY(MAKHUYENMAI)
)
-- 10. BẢNG GHIDANH
CREATE TABLE GHIDANH(
    MAGHIDANH INT IDENTITY(1,1),
    MAHOCVIEN CHAR(5) NOT NULL,
    MALOPHOC CHAR(5) NOT NULL,
    MAKHUYENMAI CHAR(5) NULL,
    NGAYGHIDANH DATETIME2 DEFAULT SYSUTCDATETIME(),
    TRANGTHAI NVARCHAR(50) DEFAULT N'Đang học',
    DIEMTONGKET DECIMAL(5,2) NULL,
    CONSTRAINT PK_GHIDANH PRIMARY KEY(MAGHIDANH),
    CONSTRAINT UQ_GHIDANH UNIQUE(MAHOCVIEN, MALOPHOC),
    CONSTRAINT FK_GHIDANH_HOCVIEN FOREIGN KEY(MAHOCVIEN) REFERENCES HOCVIEN(MAHOCVIEN),
    CONSTRAINT FK_GHIDANH_KHUYENMAI FOREIGN KEY(MAKHUYENMAI) REFERENCES KHUYENMAI(MAKHUYENMAI),
    CONSTRAINT FK_GHIDANH_LOPHOC FOREIGN KEY(MALOPHOC) REFERENCES LOPHOC(MALOPHOC)
);
CREATE INDEX IDX_GHIDANH_HOCVIEN ON GHIDANH(MAHOCVIEN);
CREATE INDEX IDX_GHIDANH_LOPHOC ON GHIDANH(MALOPHOC);

-- 12. BẢNG THANHTOAN
IF OBJECT_ID('THANHTOAN', 'U') IS NOT NULL
    DROP TABLE THANHTOAN;  
GO

-- Tạo bảng THANHTOAN chuẩn
CREATE TABLE THANHTOAN(
    MATHANHTOAN CHAR(5) NOT NULL,          
    MAKHOAHOC   CHAR(5) NOT NULL,          
    MANHANSU    CHAR(5) NULL,              
    MAGHIDANH   INT NOT NULL,              
    MAHOCVIEN   CHAR(5) NOT NULL,          
    SOTIEN      MONEY NOT NULL,            
    NGAYTHANHTOAN DATETIME2 DEFAULT SYSUTCDATETIME(), 
    PHUONGTHUC  NVARCHAR(50) NULL,         
    GHICHU      NVARCHAR(200) NULL,        
    
    CONSTRAINT PK_THANHTOAN PRIMARY KEY (MATHANHTOAN),

    -- Ràng buộc khóa ngoại
    CONSTRAINT FK_THANHTOAN_HOCVIEN FOREIGN KEY (MAHOCVIEN) REFERENCES HOCVIEN(MAHOCVIEN),
    CONSTRAINT FK_THANHTOAN_GHIDANH FOREIGN KEY (MAGHIDANH) REFERENCES GHIDANH(MAGHIDANH), -- Đã thêm lại ràng buộc bị thiếu
    CONSTRAINT FK_THANHTOAN_KHOAHOC FOREIGN KEY (MAKHOAHOC) REFERENCES KHOAHOC(MAKHOAHOC),
    CONSTRAINT FK_THANHTOAN_NHANSU  FOREIGN KEY (MANHANSU) REFERENCES NHANSU(MANHANSU)
);
-- Tạo index để tối ưu truy vấn
CREATE INDEX IDX_THANHTOAN_HOCVIEN ON THANHTOAN(MAHOCVIEN);
CREATE INDEX IDX_THANHTOAN_GHIDANH ON THANHTOAN(MAGHIDANH);
GO
--------------------------------------------------------------------------------
-- PHẦN 5: QUẢN LÝ ĐIỂM SỐ VÀ CHUYÊN CẦN
--------------------------------------------------------------------------------

-- 13. BẢNG BAIKIEMTRA
CREATE TABLE BAIKIEMTRA(
    MABAIKIEMTRA INT IDENTITY(1,1),
    MALOPHOC CHAR(5) NOT NULL, 
    TIEUDE NVARCHAR(200) NOT NULL,
    DIEMTOIDA DECIMAL(7,2) DEFAULT 100,
    TRONGSO DECIMAL(5,2) DEFAULT 1.0, 
    NGAYKIEMTRA DATE NULL,
	CONSTRAINT PK_BAIKIEMTRA PRIMARY KEY(MABAIKIEMTRA),
    CONSTRAINT FK_BAIKIEMTRA_LOPHOC FOREIGN KEY(MALOPHOC) REFERENCES LOPHOC(MALOPHOC)
);

-- 14. BẢNG DIEMSO
CREATE TABLE DIEMSO(
    MAGHIDANH INT NOT NULL, 
    MABAIKIEMTRA INT NOT NULL,
    DIEM DECIMAL(7,2) NULL,
    NGAYCHAM DATETIME2 DEFAULT SYSUTCDATETIME(),
	CONSTRAINT PK_DIEM PRIMARY KEY(MAGHIDANH,MABAIKIEMTRA),
    CONSTRAINT FK_DIEMSO_GHIDANH FOREIGN KEY(MAGHIDANH) REFERENCES GHIDANH(MAGHIDANH),
    CONSTRAINT FK_DIEMSO_BAIKIEMTRA FOREIGN KEY(MABAIKIEMTRA) REFERENCES BAIKIEMTRA(MABAIKIEMTRA)
);
CREATE INDEX IDX_DIEMSO_GHIDANH ON DIEMSO(MAGHIDANH);
CREATE INDEX IDX_DIEMSO_BAIKIEMTRA ON DIEMSO(MABAIKIEMTRA);

-- 15. BẢNG DIEMDANH
CREATE TABLE DIEMDANH(
    MADIEMDANH INT IDENTITY(1,1),
    MAGHIDANH INT NOT NULL,
    NGAYHOC DATE NOT NULL,
    TRANGTHAI NVARCHAR(20) NOT NULL, 
    GHICHU NVARCHAR(200) NULL,
	CONSTRAINT PK_DIEMDANH PRIMARY KEY(MADIEMDANH),
    CONSTRAINT UQ_DIEMDANH UNIQUE(MAGHIDANH, NGAYHOC), 
    CONSTRAINT FK_DIEMDANH_GHIDANH FOREIGN KEY(MAGHIDANH) REFERENCES GHIDANH(MAGHIDANH)
);
CREATE INDEX IDX_DIEMDANH_HOC ON DIEMDANH(MAGHIDANH, NGAYHOC);


--------------------------------------------------------------------------------
-- PHẦN 6: QUẢN LÝ CHỨNG CHỈ VÀ THÔNG TIN BỔ SUNG
--------------------------------------------------------------------------------

-- 16. BẢNG CHUNGCHI 
CREATE TABLE CHUNGCHI(
    MACHUNGCHI INT IDENTITY(1,1),
    MAHOCVIEN CHAR(5) NOT NULL, 
    MAKHOAHOC CHAR(5) NOT NULL, 
    MASOCHUNGCHI NVARCHAR(100) NOT NULL UNIQUE,
    NGAYCAP DATE NOT NULL,
    NGAYHETHAN DATE NULL,
	CONSTRAINT PK_CHUNGCHI PRIMARY KEY(MACHUNGCHI),
    CONSTRAINT FK_CHUNGCHI_HOCVIEN FOREIGN KEY(MAHOCVIEN) REFERENCES HOCVIEN(MAHOCVIEN),
    CONSTRAINT FK_CHUNGCHI_KHOAHOC FOREIGN KEY(MAKHOAHOC) REFERENCES KHOAHOC(MAKHOAHOC)
);

--------------------------------------------------------------------------------
-- PHẦN 7: RÀNG BUỘC KIỂM TRA (CHECK CONSTRAINTS)
--------------------------------------------------------------------------------

-- Ràng buộc cho LOPHOC
ALTER TABLE LOPHOC ADD CONSTRAINT CHK_LOPHOC_TRANGTHAI CHECK (TRANGTHAI IN (N'Chưa mở', N'Đang học', N'Kết Thúc'));

-- Ràng buộc cho GHIDANH
ALTER TABLE GHIDANH ADD CONSTRAINT CHK_GHIDANH_TRANGTHAI CHECK (TRANGTHAI IN (N'Đang học', N'Đã hoàn thành', N'Hủy'));

-- Ràng buộc cho DIEMDANH
ALTER TABLE DIEMDANH ADD CONSTRAINT CHK_DIEMDANH_TRANGTHAI CHECK (TRANGTHAI IN (N'Có mặt', N'Vắng có phép', N'Vắng không phép'));

-- Ràng buộc cho KHUYENMAI (Ngày bắt đầu không được sau ngày kết thúc)
ALTER TABLE KHUYENMAI
ADD CONSTRAINT CHK_KHUYENMAI_NGAY CHECK (
    NGAYBATDAUKM IS NULL
    OR NGAYKETTHUCKM IS NULL
    OR NGAYBATDAUKM <= NGAYKETTHUCKM
);
GO
--------------------------DỮ LIỆU ĐÃ CHỈNH SỬA-----------------------
----------------------------------------
-- DỮ LIỆU MẪU 
----------------------------------------

-- 1. QUYENNGUOIDUNG 
INSERT INTO QUYENNGUOIDUNG (MAQUYEN, TENQUYEN, MOTA) VALUES
('Q0001', N'Admin', N'Quản trị viên hệ thống, có quyền cao nhất.'),
('Q0002', N'Nhân viên', N'Nhân viên hành chính/tư vấn, quản lý ghi danh và thanh toán.'),
('Q0003', N'Giáo viên', N'Giáo viên, có quyền quản lý điểm danh và điểm số của lớp mình.'),
('Q0004', N'Học viên', N'Học viên, chỉ có quyền xem thông tin cá nhân và lớp học đã đăng ký.');
GO
SELECT*FROM QUYENNGUOIDUNG

-- 2. TAIKHOAN 
INSERT INTO TAIKHOAN (MATAIKHOAN, TENDANGNHAP, MATKHAU, HOTEN, EMAIL, SODIENTHOAI, MAQUYEN) VALUES
('TK001', 'admin',    'admin@123456', N'Trần Thị Quỳnh (Admin)', 'admin@ttth.edu.vn',    '0900111222', 'Q0001'), -- ADMIN
('TK002', 'quanly',   '123456', N'Nguyễn Văn A (Quản lý)', 'nv.a@ttth.edu.vn',    '0900333444', 'Q0002'), -- NHÂN VIÊN/QUẢN LÝ
('TK003', 'giaovien', '123456', N'Lê Văn Minh (Giáo viên)', 'gv.minh@ttth.edu.vn', '0900555666', 'Q0003'), -- GIÁO VIÊN
('TK004', 'hocvien',  '123456', N'Phạm Thu An (Học viên)', 'hv.an@gmail.com',     '0900777888', 'Q0004'); -- HỌC VIÊN
GO
SELECT*FROM TAIKHOAN

-- 5. NHANSU
INSERT INTO NHANSU
(MANHANSU, MATAIKHOAN, HOTEN, NGAYSINH, CHUCDANH, DIACHI, MOTA, EMAIL, SODIENTHOAI)
VALUES
('NS001', 'TK001', N'Trần Thị Quỳnh ', '1980-03-10', N'Quản trị hệ thống', N'123 Đường 3/2, Q.10', N'Quản lý hệ thống', 'admin@ttth.edu.vn', '0900111222'),
('NS002', 'TK002', N'Nguyễn Văn A ', '1990-06-15', N'Tư vấn ghi danh', N'456 Lý Thường Kiệt, Q.11', N'Tư vấn ghi danh, hỗ trợ tuyển sinh', 'nv.a@ttth.edu.vn', '0900333444'),
('NS003', NULL, N'Đỗ Thị Phương ', '1985-12-01', N'Trưởng phòng Đào tạo', N'78A Nguyễn Văn Cừ, Q.5', N'Điều phối chương trình đào tạo', 'phuong.dt@ttth.edu.vn', '0903333444'),
('NS004', NULL, N'Lê Minh Hoàng ', '1992-09-20', N'Nhân viên Kế toán', N'99 Pasteur, Q.3', N'Xử lý chứng từ, quản lý thu chi', 'hoang.lm@ttth.edu.vn', '0905777888');
GO
SELECT*FROM NHANSU

-- 4. GIAOVIEN 
INSERT INTO GIAOVIEN (MAGIANGVIEN, MATAIKHOAN, HOTEN, EMAIL, SODIENTHOAI, TRINHDO, CHUYENNGANH, LOAIHOPDONG, TRANGTHAI) VALUES
('GV001', 'TK003', N'Lê Văn Minh ', 'gv.minh@ttth.edu.vn', '0900555666', N'Thạc sĩ', N'Phát triển phần mềm', N'Toàn thời gian', N'Đang làm việc'),
('GV002',NULL, N'Lê Minh Huy ', 'gv.huy@ttth.edu.vn', '0900555665', N'Thạc sĩ', N'Phát triển phần mềm', N'Toàn thời gian', N'Đang làm việc'),
('GV003',NULL, N'Lê Trân Thành ', 'gv.thanh@ttth.edu.vn', '0900555668', N'Tiến sĩ', N'Phát triển phần mềm', N'Toàn thời gian', N'Đang làm việc'),
('GV004', NULL, N'Nguyễn Thị Hoài Thương', 'gv.thuong@ttth.edu.vn', '0900555667', N'Thạc sĩ', N'Phát triển phần mềm', N'Toàn thời gian', N'Đang làm việc'),
('GV005', NULL, N'Trần Kim Ngân ', 'gv.ngan@ttth.edu.vn', '0900555669', N'Tiến sĩ', N'Phát triển phần mềm', N'Toàn thời gian', N'Đang làm việc');
GO
SELECT*FROM GIAOVIEN

-- 3. HOCVIEN 
INSERT INTO HOCVIEN (MAHOCVIEN, MATAIKHOAN, HOTEN, GIOITINH, NGAYSINH, EMAIL, SODIENTHOAI, DIACHI) VALUES
('HV001', 'TK004', N'Phạm Thu An', N'Nữ', '2000-05-15', 'hv.an@gmail.com', '0900777888', N'123 Đường 3/2, Q.10, TP.HCM'),
('HV002', NULL, N'Trần Quốc Bảo', N'Nam','2001-11-20', 'bao.tq@gmail.com', '0900999000', N'456 Lý Thường Kiệt, Q.11, TP.HCM'),
('HV003', NULL, N'Nguyễn Hoàng Quân', N'Nam','2003-01-01', 'quan.nh@gmail.com', '0902111222', N'789 Nguyễn Văn Cừ, Q.5, TP.HCM'),
('HV004', NULL, N'Lê Thị Hương', N'Nữ', '1999-12-12', 'huong.lt@hotmail.com', '0903333444', N'555 Hậu Giang, Q.6, TP.HCM'),
('HV005', NULL, N'Võ Đình Hiếu', N'Nam', '2005-07-25', 'hieu.vd@yahoo.com', '0904555666', N'111 Phan Văn Trị, Q.Gò Vấp, TP.HCM'),
('HV006', NULL, N'Đỗ Minh Trung', N'Nam', '1998-03-03', 'trung.dm@gmail.com', '0910111222', N'34 Nguyễn Chí Thanh, Q.5'),
('HV007', NULL, N'Hoàng Thị Lan', N'Nữ', '2002-08-18', 'lan.ht@gmail.com', '0911333444', N'56 Cao Thắng, Q.3'),
('HV008', NULL, N'Nguyễn Tiến Đạt', N'Nam', '2004-01-22', 'dat.nt@gmail.com', '0912555666', N'78 Lê Lợi, Q.1'),
('HV009', NULL, N'Trịnh Mai Linh', N'Nữ', '2000-06-05', 'linh.tm@gmail.com', '0913777888', N'90 Điện Biên Phủ, Q.Bình Thạnh'),
('HV010', NULL, N'Phan Đình Phùng', N'Nam', '1997-10-10', 'phung.pd@gmail.com', '0914999000', N'101 Cộng Hòa, Q.Tân Bình'),
('HV011', NULL, N'Bùi Thanh Thảo', N'Nữ', '2003-04-01', 'thao.bt@gmail.com', '0915111222', N'11 Hai Bà Trưng, Q.1'),
('HV012', NULL, N'Vũ Trọng Hùng', N'Nam', '2001-09-29', 'hung.vt@gmail.com', '0916333444', N'12 Trường Chinh, Q.Tân Phú'),
('HV013', NULL, N'Dương Ánh Tuyết', N'Nữ', '2000-02-14', 'tuyet.da@gmail.com', '0917555666', N'13 Phan Đăng Lưu, Q.Phú Nhuận'),
('HV014', NULL, N'Mai Văn Khang', N'Nam', '1999-07-07', 'khang.mv@gmail.com', '0918777888', N'14 Hoàng Văn Thụ, Q.Phú Nhuận'),
('HV015', NULL, N'Huỳnh Ngọc Sương', N'Nữ', '2002-12-30', 'suong.hn@gmail.com', '0919999000', N'15 Quang Trung, Q.Gò Vấp'),
('HV016', NULL, N'Nguyễn Bá Long', N'Nam', '2004-05-19', 'long.nb@gmail.com', '0920111222', N'16 Bạch Đằng, Q.Bình Thạnh'),
('HV017', NULL, N'Trần Thanh Phong', N'Nam', '2003-11-11', 'phong.tt@gmail.com', '0921333444', N'17 Trần Hưng Đạo, Q.5'),
('HV018', NULL, N'Phan Thúy Vy', N'Nữ', '2000-08-25', 'vy.pt@gmail.com', '0922555666', N'18 Nguyễn Tri Phương, Q.10'),
('HV019', NULL, N'Lê Quốc Việt', N'Nam', '1998-04-12', 'viet.lq@gmail.com', '0923777888', N'19 Sư Vạn Hạnh, Q.10'),
('HV020', NULL, N'Hồ Ngọc Diệp', N'Nữ', '2001-01-08', 'diep.hn@gmail.com', '0924999000', N'20 Pasteur, Q.3');
GO
SELECT*FROM HOCVIEN
-- 6. TUYENDUNG 
INSERT INTO TUYENDUNG 
(MATUYENDUNG, MANHANSU, NGAYTUYENDUNG, MOTA)
VALUES
('TD001', 'NS001', '2024-01-10', N'Tuyển dụng Giảng viên Lập trình Web'),
('TD002', 'NS002', '2024-05-20', N'Tuyển dụng Nhân viên Tư vấn khóa học'),
('TD003', 'NS003', '2024-08-01', N'Tuyển dụng Kế toán mới'),
('TD004', 'NS001', '2024-09-15', N'Tuyển dụng Trưởng phòng Marketing'),
('TD005', 'NS004', '2024-10-25', N'Tuyển dụng Cộng tác viên Kế toán (Thay NS005 cũ)');
GO
SELECT *FROM TUYENDUNG
-- 7. KHOAHOC
INSERT INTO KHOAHOC (MAKHOAHOC, TENKHOAHOC, MOTA, TONGSOGIO, HOCPHI, CHINHANH) VALUES
('KH001', N'Lập trình Web', N'Khóa học giúp học viên làm quen với HTML, CSS, JavaScript và xây dựng website tĩnh.', 40, 3000000.00, N'Chi nhánh Quận 10'),
('KH002', N'Thiết kế Đồ họa Photoshop & AI', N'Học cách sử dụng Adobe Photoshop và Illustrator để thiết kế ấn phẩm quảng cáo chuyên nghiệp.', 60, 4500000.00, N'Chi nhánh Quận 10'),
('KH003', N'Hệ quản trị Cơ sở Dữ liệu SQL',N'Trang bị kiến thức về SQL Server, cách tạo, truy vấn và quản lý cơ sở dữ liệu.',30, 2500000.00, N'Chi nhánh Quận 5'),
('KH004', N'Lập trình Python cơ bản', N'Học cú pháp cơ bản của Python, làm việc với biến, vòng lặp, hàm và xử lý dữ liệu.', 35, 3200000.00, N'Chi nhánh Quận 5'),
('KH005', N'Kỹ năng Excel nâng cao', N'Khóa học tập trung vào công thức nâng cao, PivotTable, Macro và VBA trong Excel.',20, 1800000.00, N'Online');
GO
SELECT*FROM KHOAHOC
-- 8. LOPHOC 
INSERT INTO LOPHOC (MALOPHOC, MAKHOAHOC, MAGIANGVIEN, NGAYBATDAU, NGAYKETTHUC, SISOTOIDA, DIADIEM, TRANGTHAI) VALUES
('L0001', 'KH001', 'GV001', '2025-11-01', '2025-12-30', 25, N'Phòng 301', N'Chưa mở'),
('L0002', 'KH002', 'GV001', '2025-10-25', '2026-01-25', 20, N'Phòng 202', N'Đang học'),
('L0003', 'KH003', 'GV001', '2025-12-01', '2026-01-15', 30, N'Phòng 302', N'Chưa mở'), 
('L0004', 'KH004', 'GV001', '2025-11-15', '2026-01-05', 25, N'Online', N'Đang học'),
('L0005', 'KH005', 'GV001', '2025-10-20', '2025-11-20', 40, N'Online', N'Kết thúc'); 
GO
SELECT*FROM LOPHOC
-- 9. LICHHOC 
INSERT INTO LICHHOC (MALOPHOC, NGAYTRONGTUAN, GIOBATDAU, GIOKETTHUC, PHONGHOC) VALUES
('L0001', N'Thứ Ba',   '18:00', '20:30', N'Phòng A301'),
('L0001', N'Thứ Năm',  '18:00', '20:30', N'Phòng A301'),
('L0002', N'Thứ Bảy',  '08:00', '11:00', N'Phòng A202'),
('L0003', N'Thứ Hai',  '19:00', '21:00', N'Phòng A302'),
('L0003', N'Thứ Tư',   '19:00', '21:00', N'Phòng A302'),
('L0004', N'Thứ Hai',  '20:00', '22:00', N'Online'),
('L0004', N'Thứ Sáu',  '20:00', '22:00', N'Online'),
('L0005', N'Chủ Nhật','09:00', '12:00', N'Online');
GO
SELECT*FROM LICHHOC
-- 10. KHUYENMAI 
INSERT INTO KHUYENMAI (MAKHUYENMAI, TENKHUYENMAI, GIAMGIA, LOAIGIAMGIA, NGAYBATDAUKM, NGAYKETTHUCKM, TRANGTHAI) VALUES
('KM001', N'EarlyBird', 10.00,    N'Phần trăm', '2025-10-01', '2025-10-31', N'Đang diễn ra'),
('KM002', N'SinhVien',  500000.00, N'Tiền mặt', '2025-01-01', '2026-12-31', N'Đang diễn ra'),
('KM003', N'GioVang',    15.00,    N'Phần trăm', '2025-11-01', '2025-11-07', N'Sắp diễn ra'),
('KM004', N'UuDaiGV',   1000000.00,N'Tiền mặt', '2025-10-01', '2025-12-31', N'Đang diễn ra'),
('KM005', N'MoiBan',    5.00,     N'Phần trăm', '2025-09-01', '2025-12-31', N'Đang diễn ra');
GO
SELECT*FROM KHUYENMAI
-- 11. GHIDANH 
INSERT INTO GHIDANH 
(MAHOCVIEN, MALOPHOC, MAKHUYENMAI, NGAYGHIDANH, TRANGTHAI, DIEMTONGKET)
VALUES
('HV001', 'L0001', 'KM001', '2025-10-20', N'Đang học', NULL), -- MAGHIDANH = 1
('HV002', 'L0002', NULL,     '2025-10-24', N'Đang học', NULL), -- MAGHIDANH = 2
('HV003', 'L0004', 'KM005',  '2025-11-05', N'Đang học', NULL), -- MAGHIDANH = 3
('HV004', 'L0005', 'KM002',  '2025-10-18', N'Đã hoàn thành', 8.7), -- MAGHIDANH = 4
('HV005', 'L0001', NULL,     '2025-10-25', N'Hủy', NULL); -- MAGHIDANH = 5
GO
SELECT*FROM GHIDANH

-- 12. THANHTOAN 
INSERT INTO THANHTOAN 
(MATHANHTOAN, MAKHOAHOC, MANHANSU, MAGHIDANH, MAHOCVIEN, SOTIEN, NGAYTHANHTOAN, PHUONGTHUC, GHICHU) VALUES
('TT001', 'KH001', 'NS001', 1, 'HV001', 3000000, '2025-10-25', N'Tiền mặt', N'Đợt 1'),
('TT002', 'KH002', 'NS002', 2, 'HV002', 4500000, '2025-10-25', N'Chuyển khoản', N'Thanh toán đủ'),
('TT003', 'KH004', 'NS001', 3, 'HV003', 3200000, '2025-10-25', N'Tiền mặt', N'Thanh toán 50%'), 
('TT004', 'KH005', 'NS003', 4, 'HV004', 1800000, '2025-10-25', N'Tiền mặt',N'Thanh toán đủ'), 
('TT005', 'KH001', 'NS004', 5, 'HV005', 3000000, '2025-10-25', N'Tiền mặt',N'Thanh toán đủ trước khi hủy');
GO
SELECT*FROM THANHTOAN
-- 13. BAIKIEMTRA 
INSERT INTO BAIKIEMTRA (MALOPHOC, TIEUDE, DIEMTOIDA, TRONGSO, NGAYKIEMTRA) VALUES
('L0002', N'Kiểm tra giữa khóa', 100.00, 0.40, '2025-11-25'), -- MABAIKIEMTRA = 1
('L0002', N'Bài tập lớn',        100.00, 0.60, '2026-01-20'), -- MABAIKIEMTRA = 2
('L0005', N'Bài tập cuối khóa',  100.00, 1.00,  '2025-11-18'), -- MABAIKIEMTRA = 3
('L0004', N'Quiz 1',             10.00,  0.10,  '2025-11-20'), -- MABAIKIEMTRA = 4
('L0004', N'Quiz 2',             10.00,  0.15,  '2025-12-10'); -- MABAIKIEMTRA = 5
GO
SELECT*FROM BAIKIEMTRA
-- 14. DIEMSO 
INSERT INTO DIEMSO (MAGHIDANH, MABAIKIEMTRA, DIEM) VALUES
(2, 1, 85.50),
(2, 2, 92.00),
(4, 3, 98.00),
(3, 4, 9.50),
(3, 5, 8.00);
GO
SELECT*FROM DIEMSO
-- 15. DIEMDANH 
INSERT INTO DIEMDANH (MAGHIDANH, NGAYHOC, TRANGTHAI,GHICHU) VALUES
(2, '2025-10-25', N'Có mặt', N'Học đúng giờ'),
(2, '2025-11-01', N'Vắng có phép', N'Nghỉ ốm'),
(1, '2025-11-05', N'Có mặt', N'Học tích cực'),
(1, '2025-11-07', N'Có mặt', N'Trả bài tốt'),
(3, '2025-11-15', N'Có mặt', N'Học đầy đủ'),
(3, '2025-11-22', N'Vắng không phép', N'Không báo trước');
GO
SELECT*FROM DIEMDANH
-- 16. CHUNGCHI 
INSERT INTO CHUNGCHI (MAHOCVIEN, MAKHOAHOC, MASOCHUNGCHI, NGAYCAP, NGAYHETHAN) VALUES
('HV001', 'KH001', 'CCWEB20240901', '2024-09-01', NULL),
('HV001', 'KH005', 'CCEXCEL20251120', '2025-11-20', '2030-11-20'),
('HV001', 'KH002', 'CCGRAPHIC20260125', '2026-01-25', NULL),
('HV001', 'KH004', 'CCPYTHON20251201', '2025-12-01', NULL),
('HV001', 'KH003', 'CCSQL20260130', '2026-01-30', NULL);
GO
SELECT*FROM CHUNGCHI

--------------------------------------------------------------------------------
-- PHẦN 8: STORED PROCEDURES (VỚI CƠ CHẾ PHÂN QUYỀN)
--------------------------------------------------------------------------------

-- SP_AuthoLogin
IF OBJECT_ID('SP_AuthoLogin', 'P') IS NOT NULL
    DROP PROCEDURE SP_AuthoLogin;  
GO

CREATE PROCEDURE SP_AuthoLogin
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    -- Kiểm tra xem tài khoản có tồn tại không
    IF EXISTS (SELECT 1 FROM TAIKHOAN WHERE TENDANGNHAP = @UserName)
    BEGIN
        -- Kiểm tra tên đăng nhập và mật khẩu
        IF EXISTS (SELECT 1 FROM TAIKHOAN WHERE TENDANGNHAP = @UserName AND MATKHAU = @Password AND CONHOATDONG = 1)
        BEGIN
            -- Lấy MAQUYEN của tài khoản
            DECLARE @MaQuyen CHAR(5);
            SELECT @MaQuyen = MAQUYEN FROM TAIKHOAN WHERE TENDANGNHAP = @UserName;

            -- Giả định logic phân quyền theo mã C#:
            -- Nếu là Admin (Q0001) trả về 1, nếu là bất kỳ quyền nào khác trả về 0.
            IF @MaQuyen = 'Q0001'
            BEGIN
                SELECT 1; -- Admin
            END
            ELSE
            BEGIN
                SELECT 0; -- User/Giáo viên/Nhân viên/Trưởng phòng
            END
        END
        ELSE
        BEGIN
            SELECT 2; -- Sai mật khẩu hoặc tài khoản bị vô hiệu hóa
        END
    END
    ELSE
    BEGIN
        SELECT 3; -- Tài khoản không tồn tại
    END
END
GO
----- SP_RegisterNewStudent
IF OBJECT_ID('SP_RegisterNewStudent', 'P') IS NOT NULL
    DROP PROCEDURE SP_RegisterNewStudent;
GO

CREATE PROCEDURE SP_RegisterNewStudent
    @HoTen NVARCHAR(200),
    @Email NVARCHAR(200),
    @SDT CHAR(10),
    @TaiKhoan NVARCHAR(100),
    @MatKhau NVARCHAR(100),
    @MaQuyen CHAR(5) = 'Q0004' -- Mặc định là Học viên
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE; -- Đảm bảo tính duy nhất khi tạo ID

    -- Return codes:
    -- 0 = Success
    -- 1 = Username already exists
    -- 2 = Email already exists

    -- Bắt đầu Transaction
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- 1. KIỂM TRA TỒN TẠI TÊN ĐĂNG NHẬP / EMAIL
        IF EXISTS (SELECT 1 FROM TAIKHOAN WHERE TENDANGNHAP = @TaiKhoan)
        BEGIN
            -- Rollback và trả về lỗi 1
            IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
            SELECT 1; 
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM TAIKHOAN WHERE EMAIL = @Email)
        BEGIN
            -- Rollback và trả về lỗi 2
            IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
            SELECT 2;
            RETURN;
        END

        -- 2. TẠO MÃ TỰ ĐỘNG (MATAIKHOAN)
        DECLARE @NewMaTK CHAR(5);
        SELECT @NewMaTK = 'TK' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(MATAIKHOAN, 3, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
        FROM TAIKHOAN;

        -- 3. TẠO MÃ TỰ ĐỘNG (MAHOCVIEN)
        DECLARE @NewMaHV CHAR(5);
        SELECT @NewMaHV = 'HV' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(MAHOCVIEN, 3, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
        FROM HOCVIEN;


        -- 4. INSERT VÀO TAIKHOAN
        INSERT INTO TAIKHOAN (MATAIKHOAN, TENDANGNHAP, MATKHAU, HOTEN, EMAIL, SODIENTHOAI, MAQUYEN)
        VALUES (@NewMaTK, @TaiKhoan, @MatKhau, @HoTen, @Email, @SDT, @MaQuyen);

        -- 5. INSERT VÀO HOCVIEN
        INSERT INTO HOCVIEN (MAHOCVIEN, MATAIKHOAN, HOTEN, EMAIL, SODIENTHOAI)
        VALUES (@NewMaHV, @NewMaTK, @HoTen, @Email, @SDT);

        -- 6. COMMIT TRANSACTION
        COMMIT TRANSACTION;
        SELECT 0; -- Success
    END TRY
    BEGIN CATCH
        -- Nếu có lỗi, Rollback
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW; -- Ném lỗi ra để C# bắt được
        SELECT 99; -- Lỗi không xác định
    END CATCH
END
GO
--- FN_GetMaQuyenByUsername (Hàm hỗ trợ phân quyền)
IF OBJECT_ID('FN_GetMaQuyenByUsername', 'FN') IS NOT NULL
    DROP FUNCTION FN_GetMaQuyenByUsername;
GO

CREATE FUNCTION FN_GetMaQuyenByUsername (@TenDangNhap NVARCHAR(100))
RETURNS CHAR(5)
AS
BEGIN
    DECLARE @MaQuyen CHAR(5);
    SELECT @MaQuyen = MAQUYEN
    FROM TAIKHOAN
    WHERE TENDANGNHAP = @TenDangNhap AND CONHOATDONG = 1;
    RETURN @MaQuyen;
END
GO
-----
IF OBJECT_ID('SP_ChangePassword', 'P') IS NOT NULL
    DROP PROCEDURE SP_ChangePassword;
GO

CREATE PROCEDURE SP_ChangePassword
    @TenDangNhap NVARCHAR(100),
    @MatKhauCu NVARCHAR(100),
    @MatKhauMoi NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Các mã trả về:
    -- 0 = Đổi mật khẩu thành công
    -- 1 = Tên đăng nhập không tồn tại
    -- 2 = Sai mật khẩu cũ
    -- 3 = Mật khẩu mới trùng với mật khẩu cũ

    -- 1. Kiểm tra sự tồn tại của tài khoản
    IF NOT EXISTS (SELECT 1 FROM TAIKHOAN WHERE TENDANGNHAP = @TenDangNhap)
    BEGIN
        SELECT 1; -- Tên đăng nhập không tồn tại
        RETURN;
    END

    -- 2. Kiểm tra mật khẩu cũ
    IF NOT EXISTS (SELECT 1 FROM TAIKHOAN WHERE TENDANGNHAP = @TenDangNhap AND MATKHAU = @MatKhauCu AND CONHOATDONG = 1)
    BEGIN
        SELECT 2; -- Sai mật khẩu cũ (hoặc tài khoản bị vô hiệu hóa)
        RETURN;
    END

    -- 3. Kiểm tra mật khẩu mới có trùng mật khẩu cũ không
    IF @MatKhauCu = @MatKhauMoi
    BEGIN
        SELECT 3; -- Mật khẩu mới trùng với mật khẩu cũ
        RETURN;
    END
    
    -- 4. Thực hiện đổi mật khẩu
    BEGIN TRY
        UPDATE TAIKHOAN
        SET MATKHAU = @MatKhauMoi
        WHERE TENDANGNHAP = @TenDangNhap AND MATKHAU = @MatKhauCu;

        SELECT 0; -- Đổi mật khẩu thành công
    END TRY
    BEGIN CATCH
        -- Trong trường hợp có lỗi không mong muốn (ví dụ: lỗi hệ thống)
        THROW;
        -- SELECT 99; -- Lỗi không xác định (tùy chọn)
    END CATCH
END
GO
------SP_GetStudentCourses: Lấy danh sách Khóa học đã Ghi danh
IF OBJECT_ID('SP_GetStudentCourses', 'P') IS NOT NULL
    DROP PROCEDURE SP_GetStudentCourses;
GO

CREATE PROCEDURE SP_GetStudentCourses
    @TenDangNhap NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaHocVien CHAR(5);
    
    -- Lấy Mã học viên từ Tài khoản
    SELECT @MaHocVien = H.MAHOCVIEN
    FROM TAIKHOAN TK
    JOIN HOCVIEN H ON TK.MATAIKHOAN = H.MATAIKHOAN
    WHERE TK.TENDANGNHAP = @TenDangNhap AND TK.MAQUYEN = 'Q0004' AND TK.CONHOATDONG = 1;

    -- Kiểm tra nếu không tìm thấy Học viên
    IF @MaHocVien IS NULL
    BEGIN
        SELECT N'Không tìm thấy thông tin Học viên hoặc tài khoản không hợp lệ.' AS ErrorMessage;
        RETURN;
    END

    -- Truy vấn thông tin các lớp đã ghi danh
    SELECT
        GD.MAGHIDANH,
        K.TENKHOAHOC,
        L.MALOPHOC,
        L.NGAYBATDAU,
        L.NGAYKETTHUC,
        L.TRANGTHAI AS TrangThaiLop,
        GD.TRANGTHAI AS TrangThaiGhiDanh,
        GD.DIEMTONGKET,
        GV.HOTEN AS TenGiangVien,
        K.HOCPHI,
        KM.TENKHUYENMAI,
        KM.GIAMGIA,
        KM.LOAIGIAMGIA
    FROM GHIDANH GD
    JOIN LOPHOC L ON GD.MALOPHOC = L.MALOPHOC
    JOIN KHOAHOC K ON L.MAKHOAHOC = K.MAKHOAHOC
    LEFT JOIN GIAOVIEN GV ON L.MAGIANGVIEN = GV.MAGIANGVIEN
    LEFT JOIN KHUYENMAI KM ON GD.MAKHUYENMAI = KM.MAKHUYENMAI
    WHERE GD.MAHOCVIEN = @MaHocVien
    ORDER BY L.NGAYBATDAU DESC;

END
GO
--------


-- Dùng để lấy chi tiết điểm thành phần cho 1 lần ghi danh cụ thể
IF OBJECT_ID('SP_GetEnrollmentDetailedScore', 'P') IS NOT NULL
    DROP PROCEDURE SP_GetEnrollmentDetailedScore;
GO

CREATE PROCEDURE SP_GetEnrollmentDetailedScore
    @MaGhiDanh INT -- MAGHIDANH là kiểu INT trong bảng GHIDANH
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Lấy chi tiết điểm số của học viên (MAGHIDANH) cho các bài kiểm tra (BAIKIEMTRA)
    SELECT 
        BKT.TIEUDE AS TenDiemThanhPhan,   -- Tên Bài kiểm tra (thay thế TenDiemThanhPhan)
        BKT.TRONGSO AS TrongSo,          -- Trọng số (thay thế HeSo)
        DS.DIEM AS DiemSo,               -- Điểm thực tế của học viên
        BKT.DIEMTOIDA,                  -- Điểm tối đa của bài kiểm tra
        BKT.NGAYKIEMTRA
    FROM DIEMSO DS
    JOIN BAIKIEMTRA BKT ON DS.MABAIKIEMTRA = BKT.MABAIKIEMTRA
    WHERE DS.MAGHIDANH = @MaGhiDanh
    ORDER BY BKT.NGAYKIEMTRA ASC;
END
GO
-----
IF OBJECT_ID('SP_GetStudentProfile', 'P') IS NOT NULL
    DROP PROCEDURE SP_GetStudentProfile;
GO

CREATE PROCEDURE SP_GetStudentProfile
    @TenDangNhap NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        H.MAHOCVIEN,
        TK.TENDANGNHAP,
        H.HOTEN, 
        H.GIOITINH,
        H.NGAYSINH,
        H.EMAIL,
        H.SODIENTHOAI,
        H.DIACHI,
        H.GHICHU,
        Q.TENQUYEN,
        TK.NGAYTAO
    FROM TAIKHOAN TK
    JOIN HOCVIEN H ON TK.MATAIKHOAN = H.MATAIKHOAN
    JOIN QUYENNGUOIDUNG Q ON TK.MAQUYEN = Q.MAQUYEN
    WHERE TK.TENDANGNHAP = @TenDangNhap AND TK.CONHOATDONG = 1;

END
GO
--------------Cập nhật địa chỉ của học viên
IF OBJECT_ID('SP_UpdateStudentAddress', 'P') IS NOT NULL
    DROP PROCEDURE SP_UpdateStudentAddress;
GO

CREATE PROCEDURE SP_UpdateStudentAddress
    @MaHocVien CHAR(5),
    @NewAddress NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Chỉ cập nhật địa chỉ cho học viên có Mã học viên hợp lệ
    UPDATE HOCVIEN
    SET DIACHI = @NewAddress
    WHERE MAHOCVIEN = @MaHocVien;

    -- Trả về số dòng bị ảnh hưởng (số 1 nếu thành công)
    SELECT @@ROWCOUNT; 
END
GO
-------------
--------------------------------------------------------------------------------
--  THIẾT LẬP PHÂN QUYỀN CƠ BẢN
--------------------------------------------------------------------------------
USE QLHocVienTinHoc
GO
-- 1. TẠO CÁC ROLE (VAI TRÒ) TRONG DATABASE 
-- Tương ứng với bảng QUYENNGUOIDUNG
IF DATABASE_PRINCIPAL_ID('AdminRole') IS NULL
    CREATE ROLE AdminRole AUTHORIZATION dbo;
IF DATABASE_PRINCIPAL_ID('NhanVienRole') IS NULL
    CREATE ROLE NhanVienRole AUTHORIZATION dbo;
IF DATABASE_PRINCIPAL_ID('GiaoVienRole') IS NULL
    CREATE ROLE GiaoVienRole AUTHORIZATION dbo;
IF DATABASE_PRINCIPAL_ID('HocVienRole') IS NULL
    CREATE ROLE HocVienRole AUTHORIZATION dbo;
GO

-- 2. GÁN CÁC QUYỀN CƠ BẢN CHO CÁC ROLE

-- **AdminRole (Q0001):** Quyền cao nhất, có thể là db_owner hoặc quyền tương đương.
-- Ta sẽ không gán quyền trực tiếp ở đây, mà giả định tài khoản Admin được tạo là một User có quyền cao nhất.

-- **NhanVienRole (Q0002):** Quản lý Ghi danh, Thanh toán, xem thông tin Cơ bản
GRANT SELECT, INSERT, UPDATE ON HOCVIEN TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON GHIDANH TO NhanVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON THANHTOAN TO NhanVienRole;
GRANT SELECT ON KHOAHOC TO NhanVienRole;
GRANT SELECT ON LOPHOC TO NhanVienRole;
GRANT SELECT ON GIAOVIEN TO NhanVienRole;
GRANT EXECUTE ON SP_RegisterNewStudent TO NhanVienRole; -- Có thể cần để hỗ trợ đăng ký
-- **GiaoVienRole (Q0003):** Quản lý Điểm danh và Điểm số lớp mình phụ trách, xem thông tin Học viên, Khóa học
GRANT SELECT ON HOCVIEN TO GiaoVienRole;
GRANT SELECT ON KHOAHOC TO GiaoVienRole;
GRANT SELECT ON LOPHOC TO GiaoVienRole;
GRANT SELECT ON GHIDANH TO GiaoVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON DIEMDANH TO GiaoVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON BAIKIEMTRA TO GiaoVienRole;
GRANT SELECT, INSERT, UPDATE, DELETE ON DIEMSO TO GiaoVienRole;
-- **HocVienRole (Q0004):** Chỉ xem thông tin cá nhân, lớp đã đăng ký, điểm số
GRANT SELECT ON HOCVIEN TO HocVienRole;
GRANT SELECT ON KHOAHOC TO HocVienRole;
GRANT SELECT ON LOPHOC TO HocVienRole;
GRANT SELECT ON GHIDANH TO HocVienRole;
GRANT SELECT ON LICHHOC TO HocVienRole;
GRANT SELECT ON BAIKIEMTRA TO HocVienRole;
GRANT SELECT ON DIEMSO TO HocVienRole;
GRANT SELECT ON DIEMDANH TO HocVienRole;
GRANT EXECUTE ON SP_ChangePassword TO HocVienRole;
GRANT EXECUTE ON SP_GetStudentCourses TO HocVienRole;
GRANT EXECUTE ON SP_GetEnrollmentDetailedScore TO HocVienRole;
GRANT EXECUTE ON SP_GetStudentProfile TO HocVienRole;
GRANT EXECUTE ON SP_UpdateStudentAddress TO HocVienRole;
GO

-- 3. TẠO LOGIN VÀ USER TRONG DATABASE (Dựa trên dữ liệu mẫu TAIKHOAN)

-- Tài khoản Admin (TK001)
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'login_admin')
    CREATE LOGIN login_admin WITH PASSWORD = 'admin@123456', CHECK_POLICY = OFF;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'user_admin')
    CREATE USER user_admin FOR LOGIN login_admin;
-- Gán quyền cao nhất cho Admin (ví dụ: db_owner)
EXEC sp_addrolemember 'db_owner', 'user_admin';

-- Tài khoản Quản lý/Nhân viên (TK002)
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'login_quanly')
    CREATE LOGIN login_quanly WITH PASSWORD = '123456', CHECK_POLICY = OFF;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'user_quanly')
    CREATE USER user_quanly FOR LOGIN login_quanly;
EXEC sp_addrolemember 'NhanVienRole', 'user_quanly';

-- Tài khoản Giáo viên (TK003)
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'login_giaovien')
    CREATE LOGIN login_giaovien WITH PASSWORD = '123456', CHECK_POLICY = OFF;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'user_giaovien')
    CREATE USER user_giaovien FOR LOGIN login_giaovien;
EXEC sp_addrolemember 'GiaoVienRole', 'user_giaovien';

-- Tài khoản Học viên (TK004)
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'login_hocvien')
    CREATE LOGIN login_hocvien WITH PASSWORD = '123456', CHECK_POLICY = OFF;
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'user_hocvien')
    CREATE USER user_hocvien FOR LOGIN login_hocvien;
EXEC sp_addrolemember 'HocVienRole', 'user_hocvien';
GO
-- CẬP NHẬT: THIẾT LẬP CHẾ ĐỘ PHỤC HỒI (RECOVERY MODEL)
--------------------------------------------------------------------------------
USE master;
GO
-- Bắt buộc phải đặt chế độ phục hồi là FULL để hỗ trợ Differential và Log Backup
ALTER DATABASE QLHocVienTinHoc SET RECOVERY FULL;
GO

USE QLHocVienTinHoc;
GO
-- Sao lưu Full Backup trực tiếp
BACKUP DATABASE QLHocVienTinHoc
TO DISK = 'E:\Backups\QLHocVienTinHoc_Full_20251208_1200.bak'
WITH INIT, NAME = N'Full Backup of QLHocVienTinHoc', STATS = 10;

