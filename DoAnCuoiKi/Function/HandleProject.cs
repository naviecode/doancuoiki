using DoAnCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAnCuoiKi.Validation;
using DoAnCuoiKi.Const;
namespace DoAnCuoiKi.Function
{
    internal class HandleProject
    {
        public static void NhapLieuDuAn(List<PROJECT> projects, List<TASK> tasks)
        {
            string tiepTuc;
            string tiepTucCV;
            do
            {
                PROJECT prN = new PROJECT();

                prN.ma_du_an = VALIDATION.KiemTraMaDuAn(projects);
                prN.ten_du_an = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên dự án: ");
                prN.mo_ta = VALIDATION.KiemTraDoDaiNhapLieu("Nhập mô tả: ");
                prN.nguoi_quan_ly = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên người quản lý: ");
                prN.gia_tien = VALIDATION.KiemTraDuLieuSo("Nhập giá tiền: ");
                prN.trang_thai = STATUS.NOT_START;
                prN.ngay_bat_dau = VALIDATION.SoSanhNgay(DateTime.Now, "Nhập ngày bắt đầu (dd/MM/yyyy): ", "Ngày bắt đầu", "ngày hiện tại", "nhỏ");
                prN.ngay_ket_thuc = VALIDATION.SoSanhNgay(prN.ngay_bat_dau,"Nhập ngày kết thúc (dd/MM/yyyy): ", "Ngày kết thúc", "ngày bắt đầu", "nhỏ");
                prN.tasks = new List<TASK>();

                //ràn buộc ngày bắt đầu và ngày hoàn thành phải nằm trong khung giờ dự án

                Console.Write("Bạn có muốn nhập công việc cho dự án này không (Y/N)?: ");
                tiepTucCV = Console.ReadLine();
                if (tiepTucCV.ToUpper() == "Y")
                {
                    do
                    {
                        TASK tN = new TASK();
                        tN.ma_du_an = prN.ma_du_an; 
                        tN.ma_cong_viec = VALIDATION.KiemTraDuLieuSo("Nhập mã công việc: ");
                        tN.du_an_dang_thuc_hien = prN.ten_du_an;
                        tN.noi_dung_nhiem_vu = VALIDATION.KiemTraDoDaiNhapLieu("Nhập nội dung công việc: ");
                        tN.nguoi_lam = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên người làm công việc này: ");
                        tN.ngay_bat_dau_lam = VALIDATION.KiemTraDuLieuThoiGian("Nhập ngày bắt đầu thực hiện (dd/MM/yyyy): ");
                        tN.thoi_han_hoan_thanh = VALIDATION.SoSanhNgay(tN.ngay_bat_dau_lam, "Nhập ngày hoàn thành công việc (dd/MM/yyyy): ", "Ngày hoàn thành", "ngày bắt đầu", "nhỏ");
                        tN.trang_thai_cong_viec = STATUS.NOT_START;
                        prN.tasks.Add(tN);
                        tasks.Add(tN);

                        Console.Write("Bạn có muốn nhập thêm công việc cho dự án này không (Y/N)?: ");
                        tiepTucCV = Console.ReadLine();
                    } while (tiepTucCV.ToUpper() == "Y" ? true : false);
                }


                projects.Add(prN);
                Console.Write("Bạn có muốn nhập thêm dự án không (Y/N)?: ");
                tiepTuc = Console.ReadLine();
            } while (tiepTuc.ToUpper() == "Y" ? true : false);
        }
        public static void HienThiDuLieuDuAn(List<PROJECT> projects)
        {
            string trangThai = "";
            if(projects.Count == 0)
            {
                Console.WriteLine("Không có dự án nào có để hiển thị !");
                return;
            }   
            
            Console.WriteLine("*---------------------------------------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("|                                                                     THÔNG TIN CHI TIẾT CÁC DỰ ÁN                                                                    |");
            Console.WriteLine("*---------------------------------------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-20} | {6,-13} | {7,-20} |", COLUMN_PROJECT.MA_DU_AN, COLUMN_PROJECT.TEN_DU_AN, COLUMN_PROJECT.MO_TA, COLUMN_PROJECT.TRANG_THAI, COLUMN_PROJECT.NGUOI_QUAN_LY, COLUMN_PROJECT.NGAY_BAT_DAU, COLUMN_PROJECT.NGAY_KET_THUC, COLUMN_PROJECT.GIA_TIEN);
            Console.WriteLine("|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            foreach (var prj in projects)
            {
                if (DateTime.Now > prj.ngay_ket_thuc) trangThai = STATUS.DELAY;
                else trangThai = prj.trang_thai;

                Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-21} | {6,-13} | {7,-20} |", prj.ma_du_an, prj.ten_du_an, prj.mo_ta, trangThai, prj.nguoi_quan_ly, prj.ngay_bat_dau.ToShortDateString(), prj.ngay_ket_thuc.ToShortDateString(), string.Format("{0:#,####} VND",prj.gia_tien));
                Console.WriteLine("|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            }

            //cần làm
            //Console.WriteLine("Xem chi tiết công việc của dự án");

        }
        public static void CapNhapDuAn(List<PROJECT> projects)
        {
            HienThiDuLieuDuAn(projects);
            if (projects.Count == 0) return;

            int maDuAn = VALIDATION.KiemTraDuLieuSo("Nhập mã dự án cần điều chỉnh: ");
            PROJECT projectSearch = projects.Find(x => x.ma_du_an == maDuAn);
            if (projects.Contains(projectSearch))
            {
                if (projectSearch.trang_thai == STATUS.DONE)
                {
                    Console.WriteLine("Dự án đã hoàn thành không thê điều chỉnh được nữa!");
                }
                else
                {
                    Console.WriteLine("Nhập thông tin điều chỉnh dự án");
                    projectSearch.ten_du_an = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên dự án: ", true);
                    projectSearch.mo_ta = VALIDATION.KiemTraDoDaiNhapLieu("Nhập mô tả dự án: ");
                    projectSearch.nguoi_quan_ly = VALIDATION.KiemTraDoDaiNhapLieu("Người quản lý dự án: ", true);
                    Console.WriteLine("Cập nhập thành công!");
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy mã dự án này!");
            }

        }
        public static void CapNhapTrangThai(List<PROJECT> projects)
        {
            HienThiDuLieuDuAn(projects);
            if (projects.Count == 0) return;

            int maDuAn = VALIDATION.KiemTraDuLieuSo("Nhập mã dự án cập nhập trạng thái: ");


            PROJECT projectSearch = projects.Find(x => x.ma_du_an == maDuAn);
            if (projects.Contains(projectSearch))
            {
                if (DateTime.Now > projectSearch.ngay_ket_thuc)
                {
                    Console.WriteLine("Dự án đã bị trễ không thể cập nhập được nữa !");
                    return;
                }

                Console.WriteLine("Nhập trạng thái mới cho dự án: ");
                Console.WriteLine("N: chưa bắt đầu");
                Console.WriteLine("P: đang tiến hành");
                Console.WriteLine("D: bị trễ");
                Console.WriteLine("A: Hoàn thành");
                string trangThai = VALIDATION.KiemTraDoDaiNhapLieu("Nhập trạng thái điểu chỉnh theo ký tự (N-P-D-A) tương ứng:", true);
                if (trangThai.ToUpper() == "N")
                {
                    projectSearch.trang_thai = STATUS.NOT_START;
                }
                else if (trangThai.ToUpper() == "P")
                {
                    projectSearch.trang_thai = STATUS.IN_PROGRESS;
                }
                else if (trangThai.ToUpper() == "D")
                {
                    projectSearch.trang_thai = STATUS.DELAY;
                }
                else if (trangThai.ToUpper() == "A")
                {
                    projectSearch.trang_thai = STATUS.DONE;
                }
                else
                {
                    Console.WriteLine("Trạng thái chọn không hợp lệ");
                }

            }
            else
            {
                Console.WriteLine("Không tìm thấy mã dự án này");
            }
        }

        public static void XoaDuAn(List<PROJECT> projects)
        {
            HienThiDuLieuDuAn(projects);
            if (projects.Count == 0) return;

            int maDuAn = VALIDATION.KiemTraDuLieuSo("Nhập mã dự án cần xóa: ");
            PROJECT projectSearch = projects.Find(x => x.ma_du_an == maDuAn);
            if (projects.Contains(projectSearch))
            {
                ConsoleKeyInfo confirm = VALIDATION.NhapOption("Bạn có chắc muốn xóa dự án này không (Y: Có | N - Ký tự khác: không) ?: ");
                string confirmStr = Convert.ToString(confirm.KeyChar);
                if (confirmStr.ToLower() == "y")
                {
                    projects.Remove(projectSearch);
                    Console.WriteLine("Xóa dự án thành công!");
                }
                else
                {
                    Console.WriteLine("Xóa dự án không thành công!");
                }  
               
            }
            else
            {
                Console.WriteLine("Không tìm thấy mã dự án này!");
            }
        }

        public static void TimKiemThongTinDuAn(List<PROJECT> projects)
        {
            string luaChonTimKiem;
            string thuatToanTimKiem;
            List<PROJECT> result = new List<PROJECT>();
            Console.WriteLine("Bạn muốn tìm kiếm theo: ");
            Console.WriteLine("a. Mã dự án");
            Console.WriteLine("b. Tên dự án");
            Console.WriteLine("c. Trạng thái");
            Console.WriteLine("d. Người quản lý");
            Console.WriteLine("f. Giá tiền");
            Console.WriteLine("g. Ngày bắt đầu");
            Console.WriteLine("h. Ngày kết thúc");
            luaChonTimKiem = VALIDATION.KiemTraDoDaiNhapLieu("Nhập lựa chọn: ", true);
            thuatToanTimKiem = VALIDATION.KiemTraDoDaiNhapLieu("Nhập chọn thuật toán tìm kiếm bạn muốn dùng (a: Tuần tự | b: Nhị phân | Khác: Tuần tự): ", true);
            switch (luaChonTimKiem.ToLower())
            {
                case "a":
                    int maDuAnSearch = VALIDATION.KiemTraDuLieuSo("Nhập mã dự án cần tìm: ");
                    if(thuatToanTimKiem.ToLower() == "b")
                    {
                        result = TimKiemNhiPhan(projects, COLUMN_PROJECT.MA_DU_AN, maDuAnSearch.ToString());
                    }
                    else
                    {
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.MA_DU_AN, maDuAnSearch.ToString());
                    }
                    break;
                case "b":
                    string tenDuAnSearch = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên dự án cần tìm: ", true);
                    if (thuatToanTimKiem.ToLower() == "b")
                    {
                        result = TimKiemNhiPhan(projects, COLUMN_PROJECT.TEN_DU_AN, tenDuAnSearch.ToString());
                    }
                    else
                    {
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.TEN_DU_AN, tenDuAnSearch);
                    }
                    break;
                case "c":
                    Console.WriteLine("Chọn trạng thái (N: Chưa bắt đầu | I: Đang tiến hành | D: Bị trễ | A: Hoàn thành ):");
                    string trangThaiSearch = VALIDATION.KiemTraDoDaiNhapLieu("Nhập trạng thái cần tìm: ", true);
                    if(trangThaiSearch.ToUpper() == "I") { 
                        trangThaiSearch = STATUS.IN_PROGRESS; 
                    }else if(trangThaiSearch.ToUpper() == "D") { 
                        trangThaiSearch = STATUS.DELAY; 
                    }else if(trangThaiSearch.ToUpper() == "A") {
                        trangThaiSearch = STATUS.DONE;
                    }else {
                        trangThaiSearch = STATUS.NOT_START;
                    }
                    if (thuatToanTimKiem.ToLower() == "b")
                    {
                        result = TimKiemNhiPhan(projects, COLUMN_PROJECT.TRANG_THAI, trangThaiSearch.ToString());
                    }
                    else
                    {
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.TRANG_THAI, trangThaiSearch);
                    }
                    
                    break;
                case "d":
                    string tenNguoiQuanLy = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên người quản lý cần tìm: ", true);
                    if (thuatToanTimKiem.ToLower() == "b")
                    {
                        result = TimKiemNhiPhan(projects, COLUMN_PROJECT.NGUOI_QUAN_LY, tenNguoiQuanLy.ToString());
                    }
                    else
                    {
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.NGUOI_QUAN_LY, tenNguoiQuanLy);
                    }
                    break;
                case "f":
                    int giaTien = VALIDATION.KiemTraDuLieuSo("Nhập giá tiền cần tìm: ");
                    if (thuatToanTimKiem.ToLower() == "b")
                    {
                        result = TimKiemNhiPhan(projects, COLUMN_PROJECT.GIA_TIEN, giaTien.ToString());
                    }
                    else
                    {
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.GIA_TIEN, giaTien.ToString());
                    }
                    break;
                case "g":
                    DateTime tuNgay_1 = VALIDATION.KiemTraDuLieuThoiGian("Từ ngày: ");
                    DateTime denNgay_1 = VALIDATION.KiemTraDuLieuThoiGian("Đến ngày: ");
                    if (thuatToanTimKiem.ToLower() == "b")
                    {

                    }
                    else
                    {
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.NGAY_BAT_DAU, null, tuNgay_1.ToString(), denNgay_1.ToString());
                    }
                    break;
                case "h":
                    DateTime tuNgay_2 = VALIDATION.KiemTraDuLieuThoiGian("Từ ngày: ");
                    DateTime denNgay_2 = VALIDATION.KiemTraDuLieuThoiGian("Đến ngày: ");
                    if (thuatToanTimKiem.ToLower() == "b")
                    {

                    }
                    else
                    {
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.NGAY_KET_THUC, null, tuNgay_2.ToString(), denNgay_2.ToString());
                    }
                    break;
                default:
                    Console.WriteLine("Không có lựa chọn này");
                    break;
            }

            if (result.Count > 0)
            {
                Console.WriteLine("DANH SÁCH DỰ ÁN ĐƯỢC TÌM THẤY");
                HienThiDuLieuDuAn(result);
            }
            else
            {
                Console.WriteLine("Không tìm thấy được dự án nào phù hợp thông tin yêu cầu");
            }

        }
        public static void SapXepThongTinDuAn(ref List<PROJECT> projects)
        {
            string luaChonSapXep;
            string tangHayGiam;
            string thuatToanSapXep;
            Console.WriteLine("Bạn muốn sắp xếp theo: ");
            Console.WriteLine("a. Mã dự án");
            Console.WriteLine("b. Tên dự án");
            Console.WriteLine("c. Trạng thái");
            Console.WriteLine("d. Ngày bắt đầu");
            Console.WriteLine("e. Ngày kết thúc");
            Console.WriteLine("f. Người quản lý");
            Console.WriteLine("g. Giá tiền");
            luaChonSapXep = VALIDATION.KiemTraDoDaiNhapLieu("Nhập lựa chọn sắp xếp theo: ", true);
            tangHayGiam = VALIDATION.KiemTraDoDaiNhapLieu("Chọn sắp xếp TĂNG hay GIẢM (T: Tăng | G: Giảm | Khác: Tăng): ", true, 1);
            thuatToanSapXep = VALIDATION.KiemTraDoDaiNhapLieu("Chọn thuật toán sắp xếp bạn muốn dùng (a: Sắp xếp chèn | b: Sắp xếp lựa chọn | c: Sắp xếp nổi bọt | Khác: Sắp xếp chèn ): ", true);
            switch (luaChonSapXep)
            {
                case "a":
                    switch (thuatToanSapXep)
                    {
                        case "a":
                            SapXepChen(ref projects, COLUMN_PROJECT.MA_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "b":
                            SapXepLuaChon(ref projects, COLUMN_PROJECT.MA_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chọn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "c":
                            SapXepNoiBot(ref projects, COLUMN_PROJECT.MA_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp nổi bọt");
                            HienThiDuLieuDuAn(projects);
                            break;
                        default:
                            SapXepChen(ref projects, COLUMN_PROJECT.MA_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                    }
                    break;
                case "b":
                    switch (thuatToanSapXep)
                    {
                        case "a":
                            SapXepChen(ref projects, COLUMN_PROJECT.TEN_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "b":
                            SapXepLuaChon(ref projects, COLUMN_PROJECT.TEN_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chọn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "c":
                            SapXepNoiBot(ref projects, COLUMN_PROJECT.TEN_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp nổi bọt");
                            HienThiDuLieuDuAn(projects);
                            break;
                        default:
                            SapXepChen(ref projects, COLUMN_PROJECT.TEN_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                    }
                    break;
                case "c":
                    switch (thuatToanSapXep)
                    {
                        case "a":
                            SapXepChen(ref projects, COLUMN_PROJECT.TRANG_THAI, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "b":
                            SapXepLuaChon(ref projects, COLUMN_PROJECT.TRANG_THAI, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chọn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "c":
                            SapXepNoiBot(ref projects, COLUMN_PROJECT.TRANG_THAI, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp nổi bọt");
                            HienThiDuLieuDuAn(projects);
                            break;
                        default:
                            SapXepChen(ref projects, COLUMN_PROJECT.TRANG_THAI, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                    }
                    break;
                case "d":
                    switch (thuatToanSapXep)
                    {
                        case "a":
                            SapXepChen(ref projects, COLUMN_PROJECT.NGAY_BAT_DAU, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "b":
                            SapXepLuaChon(ref projects, COLUMN_PROJECT.NGAY_BAT_DAU, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chọn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "c":
                            SapXepNoiBot(ref projects, COLUMN_PROJECT.NGAY_BAT_DAU, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp nổi bọt");
                            HienThiDuLieuDuAn(projects);
                            break;
                        default:
                            SapXepChen(ref projects, COLUMN_PROJECT.NGAY_BAT_DAU, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                    }
                    break;
                case "e":
                    switch (thuatToanSapXep)
                    {
                        case "a":
                            SapXepChen(ref projects, COLUMN_PROJECT.NGAY_KET_THUC, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "b":
                            SapXepLuaChon(ref projects, COLUMN_PROJECT.NGAY_KET_THUC, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chọn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "c":
                            SapXepNoiBot(ref projects, COLUMN_PROJECT.NGAY_KET_THUC, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp nổi bọt");
                            HienThiDuLieuDuAn(projects);
                            break;
                        default:
                            SapXepChen(ref projects, COLUMN_PROJECT.NGAY_KET_THUC, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                    }
                    break;
                case "f":
                    switch (thuatToanSapXep)
                    {
                        case "a":
                            SapXepChen(ref projects, COLUMN_PROJECT.NGUOI_QUAN_LY, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "b":
                            SapXepLuaChon(ref projects, COLUMN_PROJECT.NGUOI_QUAN_LY, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chọn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "c":
                            SapXepNoiBot(ref projects, COLUMN_PROJECT.NGUOI_QUAN_LY, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp nổi bọt");
                            HienThiDuLieuDuAn(projects);
                            break;
                        default:
                            SapXepChen(ref projects, COLUMN_PROJECT.NGUOI_QUAN_LY, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                    }
                    break;
                case "g":
                    switch (thuatToanSapXep)
                    {
                        case "a":
                            SapXepChen(ref projects, COLUMN_PROJECT.GIA_TIEN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "b":
                            SapXepLuaChon(ref projects, COLUMN_PROJECT.GIA_TIEN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chọn");
                            HienThiDuLieuDuAn(projects);
                            break;
                        case "c":
                            SapXepNoiBot(ref projects, COLUMN_PROJECT.GIA_TIEN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp nổi bọt");
                            HienThiDuLieuDuAn(projects);
                            break;
                        default:
                            SapXepChen(ref projects, COLUMN_PROJECT.GIA_TIEN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp chèn");
                            HienThiDuLieuDuAn(projects);
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Không có lựa chọn này");
                    break;
            }

        }
        public static List<PROJECT> TimKiemTuyenTinh(List<PROJECT> projects, string column, string timKiem, string tuNgay = null, string denNgay = null)
        {
            List<PROJECT> result = new List<PROJECT>();
            foreach (var item in projects)
            {
                if ((column.ToUpper() == COLUMN_PROJECT.MA_DU_AN && item.ma_du_an == int.Parse(timKiem))
                    || (column.ToUpper() == COLUMN_PROJECT.TEN_DU_AN && item.ten_du_an.Contains(timKiem))
                    || (column.ToUpper() == COLUMN_PROJECT.TRANG_THAI && item.trang_thai == timKiem)
                    || (column.ToUpper() == COLUMN_PROJECT.NGUOI_QUAN_LY && item.nguoi_quan_ly == timKiem)
                    || (column.ToUpper() == COLUMN_PROJECT.GIA_TIEN && item.gia_tien == double.Parse(timKiem))
                    || (column.ToUpper() == COLUMN_PROJECT.NGAY_BAT_DAU && (item.ngay_bat_dau > DateTime.Parse(tuNgay) || item.ngay_bat_dau < DateTime.Parse(denNgay)))
                    || (column.ToUpper() == COLUMN_PROJECT.NGAY_KET_THUC && (item.ngay_ket_thuc > DateTime.Parse(tuNgay) || item.ngay_ket_thuc < DateTime.Parse(denNgay)))
                )
                {
                    result.Add(item);
                }
            }

            return result;
        }
        public static List<PROJECT> TimKiemNhiPhan(List<PROJECT> projects, string column, string timKiem, string tuNgay = null, string denNgay = null)
        {
            SapXepChen(ref projects, column, "T");
            List<PROJECT> result = new List<PROJECT>();
            bool ktr = false;
            int left = 0;
            int right = projects.Count - 1;
            int mid;
            while (left <= right)
            {
                mid = (left + right) / 2;
                if ((column == COLUMN_PROJECT.MA_DU_AN && projects[mid].ma_du_an == int.Parse(timKiem))
                || (column == COLUMN_PROJECT.TEN_DU_AN && projects[mid].ten_du_an == timKiem.Trim())
                || (column == COLUMN_PROJECT.TRANG_THAI && projects[mid].trang_thai == timKiem.Trim())
                || (column == COLUMN_PROJECT.NGUOI_QUAN_LY && projects[mid].nguoi_quan_ly == timKiem.Trim())
                || (column == COLUMN_PROJECT.GIA_TIEN && projects[mid].gia_tien == double.Parse(timKiem))
               
                )
                {
                    result.Add(projects[mid]);
                    break;

                }
                else if ((column == COLUMN_PROJECT.MA_DU_AN && projects[mid].ma_du_an < int.Parse(timKiem))
                || (column == COLUMN_PROJECT.TEN_DU_AN && SoSanhChuoiKyTu(projects[mid].ten_du_an, timKiem))
                || (column == COLUMN_PROJECT.TRANG_THAI && SoSanhChuoiKyTu(projects[mid].trang_thai, timKiem))
                || (column == COLUMN_PROJECT.NGUOI_QUAN_LY && SoSanhChuoiKyTu(projects[mid].nguoi_quan_ly, timKiem))
                || (column == COLUMN_PROJECT.GIA_TIEN && projects[mid].gia_tien < double.Parse(timKiem))
                )
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }

            }

            return result;
        }
        public static void SapXepChen(ref List<PROJECT> projects, string column, string tangHayGiam)
        {
            for (int i = 0; i < projects.Count - 1; i++)
            {
                for (int j = i + 1; j < projects.Count; j++)
                {

                    if(((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.MA_DU_AN && projects[i].ma_du_an < projects[j].ma_du_an)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.MA_DU_AN && projects[i].ma_du_an > projects[j].ma_du_an)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.TEN_DU_AN && SoSanhChuoiKyTu(projects[i].ten_du_an, projects[j].ten_du_an))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.TEN_DU_AN  && !SoSanhChuoiKyTu(projects[i].ten_du_an, projects[j].ten_du_an))
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.TRANG_THAI && SoSanhChuoiKyTu(projects[i].trang_thai, projects[j].trang_thai))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.TRANG_THAI && !SoSanhChuoiKyTu(projects[i].trang_thai, projects[j].trang_thai))
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGAY_BAT_DAU && projects[i].ngay_bat_dau < projects[j].ngay_bat_dau)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGAY_BAT_DAU && projects[i].ngay_bat_dau > projects[j].ngay_bat_dau)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGAY_KET_THUC && projects[i].ngay_ket_thuc < projects[j].ngay_ket_thuc)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGAY_KET_THUC && projects[i].ngay_ket_thuc > projects[j].ngay_ket_thuc)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.GIA_TIEN && projects[i].gia_tien < projects[j].gia_tien)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.GIA_TIEN && projects[i].gia_tien > projects[j].gia_tien)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGUOI_QUAN_LY && SoSanhChuoiKyTu(projects[i].nguoi_quan_ly, projects[j].nguoi_quan_ly))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGUOI_QUAN_LY && !SoSanhChuoiKyTu(projects[i].trang_thai, projects[j].trang_thai))
                    )
                    {
                        PROJECT temp;
                        temp = projects[i];
                        projects[i] = projects[j];
                        projects[j] = temp;
                    }    
                }
            }
        }
        public static void SapXepLuaChon(ref List<PROJECT> projects, string column, string tangHayGiam)
        {
            int min;
            for (int i = 0; i < projects.Count - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < projects.Count; j++)
                {
                    if (((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.MA_DU_AN && projects[i].ma_du_an < projects[j].ma_du_an)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.MA_DU_AN && projects[i].ma_du_an > projects[j].ma_du_an)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.TEN_DU_AN && SoSanhChuoiKyTu(projects[i].ten_du_an, projects[j].ten_du_an))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.TEN_DU_AN && !SoSanhChuoiKyTu(projects[i].ten_du_an, projects[j].ten_du_an))
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.TRANG_THAI && SoSanhChuoiKyTu(projects[i].trang_thai, projects[j].trang_thai))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.TRANG_THAI && !SoSanhChuoiKyTu(projects[i].trang_thai, projects[j].trang_thai))
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGAY_BAT_DAU && projects[i].ngay_bat_dau < projects[j].ngay_bat_dau)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGAY_BAT_DAU && projects[i].ngay_bat_dau > projects[j].ngay_bat_dau)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGAY_KET_THUC && projects[i].ngay_ket_thuc < projects[j].ngay_ket_thuc)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGAY_KET_THUC && projects[i].ngay_ket_thuc > projects[j].ngay_ket_thuc)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.GIA_TIEN && projects[i].gia_tien < projects[j].gia_tien)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.GIA_TIEN && projects[i].gia_tien > projects[j].gia_tien)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGUOI_QUAN_LY && SoSanhChuoiKyTu(projects[i].nguoi_quan_ly, projects[j].nguoi_quan_ly))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGUOI_QUAN_LY && !SoSanhChuoiKyTu(projects[i].trang_thai, projects[j].trang_thai))
                    )
                    {
                        min = j;
                    }    
                }

                PROJECT temp = projects[i];
                projects[i] = projects[min];
                projects[min] = temp;
            }
        }
        public static void SapXepNoiBot(ref List<PROJECT> projects, string column, string tangHayGiam)
        {
            for (int i = 0; i < projects.Count; i++)
            {
                for (int j = 0; i < projects.Count - i - 1; i++)
                {
                    if (((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.MA_DU_AN && projects[j].ma_du_an < projects[j+1].ma_du_an)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.MA_DU_AN && projects[j].ma_du_an > projects[j+1].ma_du_an)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.TEN_DU_AN && SoSanhChuoiKyTu(projects[j].ten_du_an, projects[j+1].ten_du_an))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.TEN_DU_AN && !SoSanhChuoiKyTu(projects[j].ten_du_an, projects[j+1].ten_du_an))
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.TRANG_THAI && SoSanhChuoiKyTu(projects[j].trang_thai, projects[j+1].trang_thai))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.TRANG_THAI && !SoSanhChuoiKyTu(projects[j].trang_thai, projects[j+1].trang_thai))
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGAY_BAT_DAU && projects[j].ngay_bat_dau < projects[j+1].ngay_bat_dau)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGAY_BAT_DAU && projects[j].ngay_bat_dau > projects[j+1].ngay_bat_dau)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGAY_KET_THUC && projects[j].ngay_ket_thuc < projects[j+1].ngay_ket_thuc)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGAY_KET_THUC && projects[j].ngay_ket_thuc > projects[j+1].ngay_ket_thuc)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.GIA_TIEN && projects[j].gia_tien < projects[j+1].gia_tien)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.GIA_TIEN && projects[j].gia_tien > projects[j+1].gia_tien)
                    || ((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.NGUOI_QUAN_LY && SoSanhChuoiKyTu(projects[j].nguoi_quan_ly, projects[j+1].nguoi_quan_ly))
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.NGUOI_QUAN_LY && !SoSanhChuoiKyTu(projects[j].trang_thai, projects[j+1].trang_thai))
                    )
                    {
                        PROJECT temp = projects[i];
                        projects[i] = projects[j + 1];
                        projects[j + 1] = temp;
                    }
                }
            }

        }
                                                                                                                                                                                                                
        static bool SoSanhChuoiKyTu(string a, string b)
        {
            int count = 0;
            string aTemp = a;
            string bTemp = b;
            int aLength = a.Length;
            int bLength = b.Length;
            char[] arrA;
            char[] arrB;

            if (aLength < bLength)
            {
                for (int i = 0; i < bLength; i++)
                {
                    aTemp += " ";
                }
            }
            else
            {
                for (int i = 0; i < aLength; i++)
                {
                    bTemp += " ";
                }

            }

            arrA = bTemp.ToCharArray();
            arrB = aTemp.ToCharArray();

            while (count < aTemp.Length)
            {
                char a1 = arrA[count];
                char a2 = arrB[count];
                if (a1 < a2)
                {
                    return true;
                }
                else if (a1 == a2)
                {
                    count++;
                }
                else
                {
                    return false;
                }
            }

            return false;

        }

    }
}
