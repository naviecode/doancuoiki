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
                prN.ngay_bat_dau = VALIDATION.KiemTraDuLieuThoiGian("Nhập ngày bắt đầu (dd/MM/yyyy): ");
                prN.ngay_ket_thuc = VALIDATION.SoSanhNgay(prN.ngay_bat_dau,"Nhập ngày kết thúc (dd/MM/yyyy): ", "ngày kết thúc", "ngày bắt đầu", "bé");
                prN.tasks = new List<TASK>();

                //ràn buộc ngày bắt đầu phải < ngày kết thúc => Hàm bổ sung

                Console.Write("Bạn có muốn nhập công việc cho dự án này không (Y/N)?: ");
                tiepTucCV = Console.ReadLine();
                if (tiepTucCV.ToUpper() == "Y")
                {
                    do
                    {
                        TASK tN = new TASK();
                        tN.ma_cong_viec = VALIDATION.KiemTraDuLieuSo("Nhập mã công việc");
                        tN.du_an_dang_thuc_hien = prN.ten_du_an;
                        tN.noi_dung_nhiem_vu = VALIDATION.KiemTraDoDaiNhapLieu("Nhập nội dung công việc: ");
                        tN.nguoi_lam = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên người làm công việc này: ");
                        tN.ngay_bat_dau_lam = VALIDATION.KiemTraDuLieuThoiGian("Nhập ngày bắt đầu thực hiện");
                        tN.thoi_han_hoan_thanh = VALIDATION.SoSanhNgay(tN.ngay_bat_dau_lam, "ngày bắt đầu thực hiện", "ngày hoàn thành", "lớn");
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
                Console.WriteLine("Không có dự án nào được thêm !");
                return;
            }   
            
            Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("|                                                                 THÔNG TIN CHI TIẾT CÁC DỰ ÁN                                                                |");
            Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-20} | {6,-13} | {7,-12} |", "Mã dự án", "Tên dự án", "Mô tả", "Trạng thái","Người quản lý", "Ngày bắt đầu", "Ngày kết thúc", "Giá tiền");
            Console.WriteLine("|-------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            foreach (var prj in projects)
            {
                if (DateTime.Now > prj.ngay_ket_thuc) trangThai = STATUS.DELAY;
                else trangThai = prj.trang_thai;
                //+ Ngày hiện tại > ngày kết thúc and các task chưa complete thì dự án trễ ngược lại thì done

                Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-21} | {6,-13} | {7,-12} |", prj.ma_du_an, prj.ten_du_an, prj.mo_ta, trangThai, prj.nguoi_quan_ly, prj.ngay_bat_dau.ToShortDateString(), prj.ngay_ket_thuc.ToShortDateString(), VALIDATION.FormatNumber(prj.gia_tien));
                Console.WriteLine("|-------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            }

            //cần làm
            //Console.WriteLine("Xem chi tiết công việc của dự án");

        }
        public static void CapNhapDuAn(List<PROJECT> projects)
        {

            string tiepTuc;
            do
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
                        projectSearch.ten_du_an = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên dự án: ",true);
                        projectSearch.mo_ta = VALIDATION.KiemTraDoDaiNhapLieu("Nhập mô tả dự án: ");
                        projectSearch.nguoi_quan_ly = VALIDATION.KiemTraDoDaiNhapLieu("Người quản lý dự án: ",true);
                        Console.WriteLine("Cập nhập thành công!");
                    }
                }
                else
                {
                    Console.WriteLine("Không tìm thấy mã dự án này!");
                }

                Console.Write("Bạn có muốn cập nhập thêm thông tin dự án không (Y/N)?: ");
                tiepTuc = Console.ReadLine();

            } while (tiepTuc.ToUpper() == "Y" ? true : false);



        }
        public static void CapNhapTrangThai(List<PROJECT> projects)
        {
            HienThiDuLieuDuAn(projects);
            if (projects.Count == 0) return;

            int maDuAn = VALIDATION.KiemTraDuLieuSo("Nhập mã dự án cập nhập trạng thái: ");


            PROJECT projectSearch = projects.Find(x => x.ma_du_an == maDuAn);
            if (projects.Contains(projectSearch))
            {
                Console.WriteLine("Nhập trạng thái mới cho dự án: ");
                Console.WriteLine("N: chưa bắt đầu");
                Console.WriteLine("P: đang tiến hành");
                Console.WriteLine("D: bị trễ");
                Console.WriteLine("A: Hoàn thành");
                string trangThai = VALIDATION.KiemTraDoDaiNhapLieu("Nhập trạng thái điểu chỉnh theo ký tự (N-P-D-A) tương ứng", true);
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
                ConsoleKeyInfo confirm = VALIDATION.NhapOption("Bạn có chắc muốn xóa dự án này không (Y: Có | N - Ký tự khác: không) ?");
                if(confirm.KeyChar != 'Y')
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
            string tiepTuc;
            string luaChonTimKiem;
            string thuatToanTimKiem;
            List<PROJECT> result = new List<PROJECT>();
            do
            {
                Console.WriteLine("Bạn muốn tìm kiếm theo: ");
                Console.WriteLine("a. Mã dự án");
                Console.WriteLine("b. Tên dự án");
                Console.WriteLine("c. Trạng thái");
                Console.WriteLine("d. Người quản lý");
                Console.WriteLine("f. Giá tiền");
                Console.WriteLine("g. Ngày bắt đầu");
                Console.WriteLine("h. Ngày kết thúc");
                luaChonTimKiem = VALIDATION.KiemTraDoDaiNhapLieu("Nhập lựa chọn: ", true);
                thuatToanTimKiem = VALIDATION.KiemTraDoDaiNhapLieu("Nhập chọn thuật toán tìm kiếm bạn muốn dùng (a: Tuần tự | b: Nhị phân): ",true);
                switch (luaChonTimKiem.ToUpper())
                {
                    case "a":
                        int maDuAnSearch = VALIDATION.KiemTraDuLieuSo("Nhập mã dự án cần tìm: ");
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.MA_DU_AN, maDuAnSearch.ToString());
                        break;
                    case "b":
                        string tenDuAnSearch = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên dự án cần tìm: ",true);
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.TEN_DU_AN, tenDuAnSearch);
                        break;
                    case "c":
                        Console.WriteLine("Chọn trạng thái (N: Chưa bắt đầu | I: Đang tiến hành | D: Bị trễ | A: Hoàn thành");
                        string trangThaiSearch = VALIDATION.KiemTraDoDaiNhapLieu("Nhập trạng thái cần tìm: ",true);
                        if (trangThaiSearch.ToUpper() == "N") trangThaiSearch = STATUS.NOT_START;
                        else if (trangThaiSearch.ToUpper() == "I") trangThaiSearch = STATUS.IN_PROGRESS;
                        else if (trangThaiSearch.ToUpper() == "D") trangThaiSearch = STATUS.DELAY;
                        else if (trangThaiSearch.ToUpper() == "A") trangThaiSearch = STATUS.DONE;
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.TRANG_THAI, trangThaiSearch);
                        break;
                    case "d":
                        string tenNguoiQuanLy = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên người quản lý cần tìm: ", true);
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.NGUOI_QUAN_LY, tenNguoiQuanLy);
                        break;
                    case "f":
                        int giaTien = VALIDATION.KiemTraDuLieuSo("Nhập giá tiền cần tìm: ");
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.GIA_TIEN, giaTien.ToString());
                        break;
                    case "g":
                        DateTime tuNgay_1 = VALIDATION.KiemTraDuLieuThoiGian("Từ ngày: ");
                        DateTime denNgay_1 = VALIDATION.KiemTraDuLieuThoiGian("Đến ngày: ");
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.NGAY_BAT_DAU, null, tuNgay_1.ToString(), denNgay_1.ToString());
                        break;
                    case "h":
                        DateTime tuNgay_2 = VALIDATION.KiemTraDuLieuThoiGian("Từ ngày: ");
                        DateTime denNgay_2 = VALIDATION.KiemTraDuLieuThoiGian("Đến ngày: ");
                        result = TimKiemTuyenTinh(projects, COLUMN_PROJECT.NGAY_KET_THUC, null, tuNgay_2.ToString(), denNgay_2.ToString());
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
                Console.Write("Bạn có muốn tìm kiếm thêm thông tin của dự án không (Y/N)?: ");
                tiepTuc = Console.ReadLine();

            } while (tiepTuc.ToUpper() == "Y" ? true : false);

        }
        public static void SapXepThongTinDuAn(ref List<PROJECT> projects)
        {
            string tiepTuc;
            string luaChonSapXep;
            string tangHayGiam;
            string thuatToanSapXep;
            do
            {
                Console.WriteLine("Bạn muốn sắp xếp theo: ");
                Console.WriteLine("a. Mã dự án");
                Console.WriteLine("b. Tên dự án");
                Console.WriteLine("c. Trạng thái");
                Console.WriteLine("d. Ngày bắt đầu");
                Console.WriteLine("e. Ngày kết thúc");
                Console.WriteLine("f. Người quản lý");
                Console.WriteLine("g. Giá tiền");
                luaChonSapXep = VALIDATION.KiemTraDoDaiNhapLieu("Nhập lựa chọn sắp xếp theo: ",true);
                tangHayGiam = VALIDATION.KiemTraDoDaiNhapLieu("Chọn sắp xếp TĂNG hay GIẢM (T: Tăng | G: Giảm | Khác: Tăng): ",true, 1);

                thuatToanSapXep = VALIDATION.KiemTraDoDaiNhapLieu("Chọn thuật toán sắp xếp bạn muốn dùng (a: Sắp xếp chèn | b: Sắp xếp lựa chọn | c: Sắp xếp nổi bọt | d: Sắp xếp trộn | e: Sắp xếp nhanh | Khác: Sắp xếp chèn ): ",true);
                switch (luaChonSapXep)
                {
                    case "a":
                        switch (thuatToanSapXep)
                        {
                            case "a":
                                SapXepChen(ref projects, COLUMN_PROJECT.MA_DU_AN, tangHayGiam);
                                Console.WriteLine("Sau khi sắp xếp");
                                HienThiDuLieuDuAn(projects);
                                break;
                            default:
                                SapXepChen(ref projects, COLUMN_PROJECT.MA_DU_AN, tangHayGiam);
                                Console.WriteLine("Sau khi sắp xếp");
                                HienThiDuLieuDuAn(projects);
                                break;
                        }
                        break;
                    case "b":
                        break;
                    case "c":
                        break;
                    case "d":
                        break;
                    case "e":
                        break;
                    case "f":
                        break;
                    case "g":
                        break;
                    default:
                        Console.WriteLine("Không có lựa chọn này");
                        break;
                }

                
                Console.Write("Bạn có muốn tiếp tục thao tác sắp xếp không (Y/N)?: ");
                tiepTuc = Console.ReadLine();

            } while (tiepTuc.ToUpper() == "Y" ? true : false);
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
        public static List<PROJECT> TimKiemNhiPhan(List<PROJECT> projects)
        {
            return null;
        }
        public static void SapXepChen(ref List<PROJECT> projects, string column, string tangHayGiam)
        {
            for (int i = 0; i < projects.Count - 1; i++)
            {
                for (int j = i + 1; j < projects.Count; j++)
                {

                    if((tangHayGiam.ToUpper() == "G" && column == COLUMN_PROJECT.MA_DU_AN && projects[i].ma_du_an < projects[j].ma_du_an)
                    || (tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_PROJECT.MA_DU_AN && projects[i].ma_du_an > projects[j].ma_du_an
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
        public static void SapXepLuaChon(ref List<PROJECT> projects)
        {

        }
        public static void SapXepNoiBot(ref List<PROJECT> projects)
        {

        }
        public static void SapXepTron(ref List<PROJECT> projects)
        {

        }
        public static void SapXepNhanh(ref List<PROJECT> projects)
        {

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
