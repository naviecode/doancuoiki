using DoAnCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAnCuoiKi.Const;
using DoAnCuoiKi.Function;
using DoAnCuoiKi.Validation;

namespace DoAnCuoiKi.Function
{
    internal class HandleTask
    {
        public static void NhapLieuCongViec(List<PROJECT> projects, List<TASK> tasks)
        {
            string tiepTuc;
            int maDuAn;
            int stt = 1;
            Console.WriteLine("*-----------------------------------------------------------------------------------*");
            Console.WriteLine("|                                 CÁC DỰ ÁN HIỆN CÓ                                 |");
            Console.WriteLine("*-----------------------------------------------------------------------------------*");
            Console.WriteLine("| {0,-6} | {1,-12} | {2,-26} | {3,-12} | {4,-12} |", "STT", COLUMN_PROJECT.MA_DU_AN, COLUMN_PROJECT.TEN_DU_AN, COLUMN_PROJECT.NGAY_BAT_DAU, COLUMN_PROJECT.NGAY_KET_THUC);
            Console.WriteLine("*-----------------------------------------------------------------------------------*");

            foreach (PROJECT prj in projects)
            {
                Console.WriteLine("| {0,-6} | {1,-12} | {2,-26} | {3,-12} | {4,-12} |", stt, prj.ma_du_an, prj.ten_du_an, prj.ngay_bat_dau.ToShortDateString(), prj.ngay_ket_thuc.ToShortDateString());
                Console.WriteLine("*-----------------------------------------------------------------------------------*");
                stt++;
            }
            maDuAn = VALIDATION.KiemTraDuLieuSo("Nhập mã dự án cần thêm công việc: ");

            do
            {
                foreach (var item in projects)
                {
                    if (item.ma_du_an == maDuAn)
                    {
                        TASK taskNew = new TASK();
                        taskNew.du_an_dang_thuc_hien = item.ten_du_an;
                        taskNew.ma_cong_viec = VALIDATION.KiemTraDuLieuSo("Nhập mã công việc: ");
                        taskNew.nguoi_lam = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên người làm công việc này: ",true);
                        taskNew.noi_dung_nhiem_vu = VALIDATION.KiemTraDoDaiNhapLieu("Nhập nội dung công việc cần làm: ",true);
                        taskNew.ngay_bat_dau_lam = VALIDATION.KiemTraDuLieuThoiGian("Nhập thời gian bắt đầu thực hiện: ");
                        taskNew.thoi_han_hoan_thanh = VALIDATION.KiemTraDuLieuThoiGian("Nhập thời gian hoàn thành công việc: ");
                        taskNew.trang_thai_cong_viec = STATUS.NOT_START;
                        tasks.Add(taskNew);
                        item.tasks.Add(taskNew);
                    }
                }
                Console.Write("Bạn có muốn nhập thêm công việc cho dự án không (Y/N)?: ");
                tiepTuc = Console.ReadLine();
            } while (tiepTuc.ToUpper() == "Y" ? true : false);
        }
        public static void HienThiCongViecTrongDuAn(List<TASK> tasks)
        {
            int stt = 1;
            string status = "";
            Console.WriteLine("*-----------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("|                                                    THÔNG TIN CHI TIẾT CÁC CÔNG VIỆC                                                     |");
            Console.WriteLine("*-----------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-14} | {5,-14} | {6,-12} |", COLUMN_TASK.MA_CONG_VIEC, COLUMN_TASK.TEN_DU_AN, COLUMN_TASK.NOI_DUNG, COLUMN_TASK.NGUOI_LAM, COLUMN_TASK.NGAY_BAT_DAU, COLUMN_TASK.NGAY_KET_THUC, COLUMN_TASK.TRANG_THAI);
            Console.WriteLine("*-----------------------------------------------------------------------------------------------------------------------------------------*");
            foreach (TASK task in tasks)
            {
                if (task.ngay_bat_dau_lam > task.thoi_han_hoan_thanh) status = STATUS.DELAY;
                else status = task.trang_thai_cong_viec; 
                Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-14} | {5,-14} | {6,-12} |", task.ma_cong_viec, task.du_an_dang_thuc_hien, task.noi_dung_nhiem_vu, task.nguoi_lam, task.ngay_bat_dau_lam.ToShortDateString(), task.thoi_han_hoan_thanh.ToShortDateString(), status);
                Console.WriteLine("*-----------------------------------------------------------------------------------------------------------------------------------------*");
                stt++;
            }
        }

        public static void CapNhapCongViec(List<TASK> tasks)
        {

            HienThiCongViecTrongDuAn(tasks);
            int maCongViec = VALIDATION.KiemTraDuLieuSo("Nhập mã công việc cần điều chỉnh: ");
            TASK taskSearch = tasks.Find(x => x.ma_cong_viec == maCongViec);
            if (tasks.Contains(taskSearch))
            {
                Console.WriteLine("Nhập thông tin điều chỉnh công việc");
                taskSearch.noi_dung_nhiem_vu = VALIDATION.KiemTraDoDaiNhapLieu("Nhập nội dung nhiệm vụ: ", true);
                taskSearch.nguoi_lam = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên người làm: ");

                Console.WriteLine("Cập nhập thành công!");
            }
            else
            {
                Console.WriteLine("Không tìm thấy mã công việc này!");
            }



        }
        public static void CapNhapTrangThai(List<TASK> tasks)
        {
            HienThiCongViecTrongDuAn(tasks);
            int maCongViec = VALIDATION.KiemTraDuLieuSo("Nhập mã công việc cần cập nhập trạng thái: ");
            TASK taskSearch = tasks.Find(x => x.ma_cong_viec == maCongViec);
            if (tasks.Contains(taskSearch))
            {
                Console.WriteLine("Nhập trạng thái mới cho dự án: ");
                Console.WriteLine("N: chưa bắt đầu");
                Console.WriteLine("P: đang tiến hành");
                Console.WriteLine("D: bị trễ");
                Console.WriteLine("A: Hoàn thành");
                string trangThai = VALIDATION.KiemTraDoDaiNhapLieu("Nhập trạng thái điểu chỉnh theo ký tự (N-P-D-A) tương ứng:", true);
                if(DateTime.Now > taskSearch.thoi_han_hoan_thanh)
                {
                    Console.WriteLine("Dự án đã bị trễ không thể cập nhập được nữa !");
                    return;
                }    
                if (trangThai.ToUpper() == "N")
                {
                    taskSearch.trang_thai_cong_viec = STATUS.NOT_START;
                    Console.WriteLine("Cập nhập thành công!");
                }
                else if (trangThai.ToUpper() == "P")
                {
                    taskSearch.trang_thai_cong_viec = STATUS.IN_PROGRESS;
                    Console.WriteLine("Cập nhập thành công!");
                }
                else if (trangThai.ToUpper() == "D")
                {
                    taskSearch.trang_thai_cong_viec = STATUS.DELAY;
                    Console.WriteLine("Cập nhập thành công!");
                }
                else if (trangThai.ToUpper() == "A")
                {
                    taskSearch.trang_thai_cong_viec = STATUS.DONE;
                    Console.WriteLine("Cập nhập thành công!");
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

        public static void XoaDuAn(List<TASK> tasks)
        {
            HienThiCongViecTrongDuAn(tasks);
            int maCongViec = VALIDATION.KiemTraDuLieuSo("Nhập mã công việc cần xóa: ");
            TASK taskSearch = tasks.Find(x => x.ma_cong_viec == maCongViec);
            if (tasks.Contains(taskSearch))
            {
                ConsoleKeyInfo confirm = VALIDATION.NhapOption("Bạn có chắc muốn xóa công việc này không (Y: Có | N - Ký tự khác: không) ?");
                if (Convert.ToString(confirm.KeyChar).Trim().ToLower() == "Y")
                {
                    tasks.Remove(taskSearch);
                    Console.WriteLine("Xóa công việc thành công!");
                }
                else
                {
                    Console.WriteLine("Xóa công việc không thành công!");
                }

            }
            else
            {
                Console.WriteLine("Không tìm thấy mã công việc này!");
            }
        }
        public static void TimKiemThongTinCongViec(List<TASK> tasks)
        {
            string luaChonTimKiem;
            string thuatToanTimKiem;
            List<TASK> result = new List<TASK>();
            Console.WriteLine("Bạn muốn tìm kiếm theo: ");
            Console.WriteLine("a. Mã dự án");
            Console.WriteLine("b. Tên dự án đang làm");
            Console.WriteLine("c. Trạng thái");
            Console.WriteLine("d. Người làm công việc");
            Console.WriteLine("e. Ngày bắt đầu");
            Console.WriteLine("f. Ngày kết thúc");
            luaChonTimKiem = VALIDATION.KiemTraDoDaiNhapLieu("Nhập lựa chọn: ", true);
            thuatToanTimKiem = VALIDATION.KiemTraDoDaiNhapLieu("Nhập chọn thuật toán tìm kiếm bạn muốn dùng (a: Tuần tự | b: Nhị phân): ", true);
            switch (luaChonTimKiem.ToUpper())
            {
                case "a":
                    int maDuAnSearch = VALIDATION.KiemTraDuLieuSo("Nhập mã công việc cần tìm: ");
                    if (thuatToanTimKiem.ToLower() == "a")
                    {
                        result = TimKiemTuyenTinh(tasks, COLUMN_TASK.MA_CONG_VIEC, maDuAnSearch.ToString());
                    }
                    else
                    {

                    }
                    break;
                case "b":
                    string tenDuAnSearch = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên công việc cần tìm: ", true);
                    if (thuatToanTimKiem.ToLower() == "a")
                    {
                        result = TimKiemTuyenTinh(tasks, COLUMN_TASK.TEN_DU_AN, tenDuAnSearch);
                    }
                    break;
                case "c":
                    Console.WriteLine("Chọn trạng thái (N: Chưa bắt đầu | I: Đang tiến hành | D: Bị trễ | A: Hoàn thành");
                    string trangThaiSearch = VALIDATION.KiemTraDoDaiNhapLieu("Nhập trạng thái cần tìm: ", true);
                    if (trangThaiSearch.ToUpper() == "N") trangThaiSearch = STATUS.NOT_START;
                    else if (trangThaiSearch.ToUpper() == "I") trangThaiSearch = STATUS.IN_PROGRESS;
                    else if (trangThaiSearch.ToUpper() == "D") trangThaiSearch = STATUS.DELAY;
                    else if (trangThaiSearch.ToUpper() == "A") trangThaiSearch = STATUS.DONE;
                    if (thuatToanTimKiem.ToLower() == "a")
                    {
                        result = TimKiemTuyenTinh(tasks, COLUMN_TASK.TRANG_THAI, trangThaiSearch);
                    }
                    break;
                case "d":
                    string tenNguoiQuanLy = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên người người làm cần tìm: ", true);
                    if (thuatToanTimKiem.ToLower() == "a")
                    {
                        result = TimKiemTuyenTinh(tasks, COLUMN_TASK.NGUOI_LAM, tenNguoiQuanLy);

                    }
                    break;
                case "e":
                    DateTime tuNgay_1 = VALIDATION.KiemTraDuLieuThoiGian("Từ ngày: ");
                    DateTime denNgay_1 = VALIDATION.KiemTraDuLieuThoiGian("Đến ngày: ");
                    if (thuatToanTimKiem.ToLower() == "a")
                    {
                        result = TimKiemTuyenTinh(tasks, COLUMN_TASK.NGAY_BAT_DAU, null, tuNgay_1.ToString(), denNgay_1.ToString());

                    }
                    break;
                case "f":
                    DateTime tuNgay_2 = VALIDATION.KiemTraDuLieuThoiGian("Từ ngày: ");
                    DateTime denNgay_2 = VALIDATION.KiemTraDuLieuThoiGian("Đến ngày: ");
                    if (thuatToanTimKiem.ToLower() == "a")
                    {
                        result = TimKiemTuyenTinh(tasks, COLUMN_TASK.NGAY_KET_THUC, null, tuNgay_2.ToString(), denNgay_2.ToString());

                    }
                    break;
                default:
                    Console.WriteLine("Không có lựa chọn này");
                    break;
            }

            if (result.Count > 0)
            {
                Console.WriteLine("DANH SÁCH DỰ ÁN ĐƯỢC TÌM THẤY");
                HienThiCongViecTrongDuAn(result);
            }
            else
            {
                Console.WriteLine("Không tìm thấy được dự án nào phù hợp thông tin yêu cầu");
            }

        }
        public static void SapXepThongTinDuAn(ref List<TASK> tasks)
        {
            string luaChonSapXep;
            string tangHayGiam;
            string thuatToanSapXep;
            Console.WriteLine("Bạn muốn sắp xếp theo: ");
            Console.WriteLine("a. Mã dự án");
            Console.WriteLine("b. Tên dự án đang làm");
            Console.WriteLine("c. Trạng thái");
            Console.WriteLine("d. Ngày bắt đầu");
            Console.WriteLine("e. Ngày kết thúc");
            Console.WriteLine("f. Người làm");
            luaChonSapXep = VALIDATION.KiemTraDoDaiNhapLieu("Nhập lựa chọn sắp xếp theo: ", true);
            tangHayGiam = VALIDATION.KiemTraDoDaiNhapLieu("Chọn sắp xếp TĂNG hay GIẢM (T: Tăng | G: Giảm | Khác: Tăng): ", true, 1);

            thuatToanSapXep = VALIDATION.KiemTraDoDaiNhapLieu("Chọn thuật toán sắp xếp bạn muốn dùng (a: Sắp xếp chèn | b: Sắp xếp lựa chọn | c: Sắp xếp nổi bọt | d: Sắp xếp nhanh | Khác: Sắp xếp chèn ): ", true);
            switch (luaChonSapXep)
            {
                case "a":
                    switch (thuatToanSapXep)
                    {
                        case "a":
                            SapXepChen(ref tasks, COLUMN_PROJECT.MA_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp");
                            HienThiCongViecTrongDuAn(tasks);
                            break;
                        default:
                            SapXepChen(ref tasks, COLUMN_PROJECT.MA_DU_AN, tangHayGiam);
                            Console.WriteLine("Sau khi sắp xếp");
                            HienThiCongViecTrongDuAn(tasks);
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
                default:
                    Console.WriteLine("Không có lựa chọn này");
                    break;
            }

        }
        public static List<TASK> TimKiemTuyenTinh(List<TASK> tasks, string column, string timKiem, string tuNgay = null, string denNgay = null)
        {
            List<TASK> result = new List<TASK>();
            foreach (var item in tasks)
            {
                if ((column.ToUpper() == COLUMN_TASK.MA_CONG_VIEC && item.ma_cong_viec == int.Parse(timKiem))
                    || (column.ToUpper() == COLUMN_TASK.TEN_DU_AN && item.du_an_dang_thuc_hien.Contains(timKiem))
                    || (column.ToUpper() == COLUMN_TASK.TRANG_THAI && item.trang_thai_cong_viec == timKiem)
                    || (column.ToUpper() == COLUMN_TASK.NGUOI_LAM && item.nguoi_lam == timKiem)
                    || (column.ToUpper() == COLUMN_TASK.NGAY_BAT_DAU && (item.ngay_bat_dau_lam > DateTime.Parse(tuNgay) || item.ngay_bat_dau_lam < DateTime.Parse(denNgay)))
                    || (column.ToUpper() == COLUMN_TASK.NGAY_KET_THUC && (item.thoi_han_hoan_thanh > DateTime.Parse(tuNgay) || item.thoi_han_hoan_thanh < DateTime.Parse(denNgay)))
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
        public static void SapXepChen(ref List<TASK> tasks, string column, string tangHayGiam)
        {
            for (int i = 0; i < tasks.Count - 1; i++)
            {
                for (int j = i + 1; j < tasks.Count; j++)
                {

                    if ((tangHayGiam.ToUpper() == "G" && column == COLUMN_TASK.MA_CONG_VIEC && tasks[i].ma_cong_viec < tasks[j].ma_cong_viec)
                    || ((tangHayGiam.ToUpper() == "T" || tangHayGiam.ToUpper() != "G") && column == COLUMN_TASK.MA_CONG_VIEC && tasks[i].ma_cong_viec > tasks[j].ma_cong_viec)
                    )
                    {
                        TASK temp;
                        temp = tasks[i];
                        tasks[i] = tasks[j];
                        tasks[j] = temp;
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
