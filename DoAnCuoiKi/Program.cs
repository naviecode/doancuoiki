using System;
using System.Collections.Generic;
using System.Text;

namespace DoAnCuoiKi
{
    class Program
    {

        #region Const
        const string FilePathProject = @"..\..\..\Data\project";
        const string FilePathTask = @"..\..\..\Data\task";
        #endregion

        #region Struct
        struct PROJECT
        {
            public int ma_du_an;
            public string ten_du_an;
            public string mo_ta;
            public DateTime ngay_bat_dau;
            public DateTime ngay_ket_thuc;
            public string trang_thai;
            public string nguoi_quan_ly;
            public double gia_tien;
            public List<TASK> tasks;
        }
        struct TASK
        {
            public string du_an_dang_thuc_hien;
            public string noi_dung_nhiem_vu;
            public string nguoi_lam;
            public string trang_thai_cong_viec;
            public DateTime ngay_bat_dau_lam;
            public DateTime thoi_han_hoan_thanh;
        }
        #endregion

        #region Function
        static void NhapLieuDuAn(List<PROJECT> projects)
        {
            string tiepTuc;
            string tiepTucCV;
            do
            {
                PROJECT prN = new PROJECT();
                
                prN.ma_du_an = KiemTraDuLieuSo("mã dự án");
                prN.ten_du_an = KiemTraDoDaiNhapLieu("tên dự án");
                prN.mo_ta = KiemTraDoDaiNhapLieu("mô tả");
                prN.nguoi_quan_ly = KiemTraDoDaiNhapLieu("người quản lý");
                prN.gia_tien = KiemTraDuLieuSo("giá tiền");
                prN.trang_thai = "CHƯA LÀM";
                prN.ngay_bat_dau = KiemTraDuLieuThoiGian("thời gian bắt đầu dự án (DD/MM/YYYY)");
                prN.ngay_ket_thuc = KiemTraDuLieuThoiGian("thời gian bắt đầu dự án (DD/MM/YYYY)");


                //ràn buộc ngày bắt đầu phải < ngày kết thúc => Hàm bổ sung
                do
                {
                    TASK tN = new TASK();
                    tN.du_an_dang_thuc_hien = prN.ten_du_an;
                    tN.noi_dung_nhiem_vu = KiemTraDoDaiNhapLieu("nội dung công việc");
                    tN.nguoi_lam = KiemTraDoDaiNhapLieu("người làm phụ trách");
                    tN.ngay_bat_dau_lam = KiemTraDuLieuThoiGian("hạn hoàn thành công việc");
                    tN.thoi_han_hoan_thanh = KiemTraDuLieuThoiGian("hạn hoàn thành công việc");
                    prN.tasks.Add(tN);
                    Console.Write("Bạn có muốn nhập thêm công việc cho dự án này không?: (Y/N)");
                    tiepTucCV = Console.ReadLine();
                } while (tiepTucCV.ToUpper() == "Y" ? true : false);
                
                projects.Add(prN);
                Console.Write("Bạn có muốn nhập thêm dự án không?: (Y/N)");
                tiepTuc = Console.ReadLine();
            } while (tiepTuc.ToUpper() == "Y" ? true : false);
        }
        static void HienThiDuLieuDuAn(List<PROJECT> projects)
        {
            Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("|                                                    THÔNG TIN CHI TIẾT CÁC DỰ ÁN                                                     |");
            Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} | {6,-12} |", "Mã dự án", "Tên dự án", "Mô tả", "Trạng thái", "Ngày bắt đầu", "Ngày kết thúc", "Giá tiền");
            Console.WriteLine("|-------------------------------------------------------------------------------------------------------------------------------------|");
            foreach (var prj in projects)
            {
                //Kiểm tra trạng thái trước khi hiển thị
                //+ Chưa có tasks là chưa start
                //+ Có task là đang start
                //+ Ngày hiện tại > ngày kết thúc and các task chưa complete thì dự án trễ ngược lại thì done
                
                Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} | {6,-12} |", prj.ma_du_an, prj.ten_du_an, prj.mo_ta, prj.trang_thai, prj.ngay_bat_dau, prj.ngay_ket_thuc, FormatNumber(prj.gia_tien));
                Console.WriteLine("|-------------------------------------------------------------------------------------------------------------------------------------|");
            }


        }
        static void NhapLieuCongViec(List<PROJECT> projects, List<TASK> tasks)
        {
            string tiepTuc;
            int maDuAn;
            int stt = 1;
            do
            {
                Console.WriteLine("*-----------------------------------------------------------------------------------*");
                Console.WriteLine("|                                 CÁC DỰ ÁN HIỆN CÓ                                 |");
                Console.WriteLine("*-----------------------------------------------------------------------------------*");
                Console.WriteLine("| {0,-6} | {1,-12} | {2,-26} | {3,-12} | {4,-12} |", "STT", "Mã dự án", "Tên dự án", "Ngày bắt đầu", "Ngày kết thúc");
                Console.WriteLine("*-----------------------------------------------------------------------------------*");

                foreach (PROJECT prj in projects)
                {
                    Console.WriteLine("| {0,-6} | {1,-12} | {2,-26} | {3,-12} | {4,-12} |", stt, prj.ma_du_an, prj.ten_du_an, prj.ngay_bat_dau, prj.ngay_ket_thuc);
                    stt++;
                }
                Console.WriteLine("*-----------------------------------------------------------------------------------*");

                maDuAn = KiemTraDuLieuSo("mã dự án cần thêm công việc", 0);

                for (int i = 0; i < projects.Count; i++)
                {
                    if(projects[i].ma_du_an == maDuAn)
                    {
                        TASK taskNew = new TASK();
                        taskNew.nguoi_lam = KiemTraDoDaiNhapLieu("tên người làm công việc này");
                        taskNew.noi_dung_nhiem_vu = KiemTraDoDaiNhapLieu("nội dung công việc cần làm");
                        taskNew.thoi_han_hoan_thanh = KiemTraDuLieuThoiGian("thời gian hoàn thành");
                        taskNew.trang_thai_cong_viec = KiemTraDoDaiNhapLieu("trạng thái công việc hiện tại");
                        projects[i].tasks.Add(taskNew);
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy mã dự án đã nhập!");
                    }
                }
                
                


                Console.Write("Bạn có muốn nhập thêm công việc cho dự án không?: (Y/N)");
                tiepTuc = Console.ReadLine();
            } while (tiepTuc.ToUpper() == "Y" ? true : false);
        }
        static void HienThiCongViecTrongDuAn(List<TASK> tasks)
        {
            int stt = 1;
            Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("|                                                    THÔNG TIN CHI TIẾT CÁC DỰ ÁN                                                     |");
            Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
            Console.WriteLine("| {0,-6} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} | {6,-12} |", "STT", "Tên dự án", "Tên công việc", "Người làm", "Thời gian bắt đầu", "Thời gian hoàn thành", "Trạng thái");
            Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
            foreach (TASK task in tasks)
            {
                //Kiểm tra trạng thái trước khi hiển thị
                //+ Ngày hiện tại > ngày hoàn thành trễ
                Console.WriteLine("| {0,-6} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} |", stt, task.du_an_dang_thuc_hien, task.noi_dung_nhiem_vu, task.nguoi_lam, task.ngay_bat_dau_lam, task.thoi_han_hoan_thanh, task.trang_thai_cong_viec);
                Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                stt++;
            }
        }
        
        static void DocFileTxt()
        {
            string filePath = FilePathProject + "file.txt";
            string[] lines;
            string str;
            if (System.IO.File.Exists(filePath))
            {
                lines = System.IO.File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    Console.WriteLine("Line {0}: {1}", i, lines[i]);
                }
                Console.WriteLine();
                str = System.IO.File.ReadAllText(filePath);
                Console.WriteLine("String: {0}", str);
            }
            else
            {
                System.IO.File.Create(filePath);

                Console.WriteLine("File does not exist");
            }
        }
        static void GhiFileTxt()
        {
            string fileLPath = @"file1.txt";
            string fileSPath = @"file2.txt";

            string[] lines = new string[2];
            lines[0] = "Write data to file using C#.";
            lines[1] = ":)";

            System.IO.File.WriteAllLines(fileLPath, lines);

            string str;
            str = "Write data to file using C#.\r\n:D";

            System.IO.File.WriteAllText(fileSPath, str);
        }
        #endregion

        #region Validation
        static double FormatNumber(double number)
        {
            return number;
        }
        static int KiemTraDuLieuSo(string text = null, int length = 0)
        {
            int result = 0;
            bool ktr = false;
        nhaplai:
            Console.Write("Nhập {0}:", text);
            ktr = int.TryParse(Console.ReadLine(), out result);
            if (!ktr)
            {
                Console.WriteLine("Sai kiểu dữ liệu");
                goto nhaplai;
            }

            if (length == 0) return result;

            if (result.ToString().Length > length)
            {
                Console.WriteLine("Đã vượt số ký tự cho phép là {0} ký tự ", length);
                goto nhaplai;
            }


            return result;
        }
        static string KiemTraDoDaiNhapLieu(string text = null, int length = 0)
        {
            string result;
        nhaplai:
            Console.Write("Nhập {0}:", text);
            result = Console.ReadLine();

            if (string.IsNullOrEmpty(result.Trim()))
            {
                Console.WriteLine("Dữ liệu không được để trống");
                goto nhaplai;
            }

            if (length == 0) return result;

            if (result.ToString().Length > length)
            {
                Console.WriteLine("Đã vượt số ký tự cho phép là {0} ký tự ", length);
                goto nhaplai;
            }


            return result;
        }
        static DateTime KiemTraDuLieuThoiGian(string text = null)
        {
            DateTime reuslt = DateTime.Now;
            string str;
            bool checkDate = false;
        nhaplai:
            Console.Write("Nhập {0}:", text);
            str = Console.ReadLine();
            string[] arrStr = str.Split("/");
            if (arrStr.Length == 3)
            {
                checkDate = DateTime.TryParse(arrStr[0] + "/" + arrStr[1] + "/" + arrStr[2], out reuslt);
            }

            if (!checkDate)
            {
                Console.WriteLine("Nhập sai dữ liệu ngày!");
                goto nhaplai;
            }

            return reuslt;
        }
        static string KiemTraMaDu(List<PROJECT> project, string data)
        {

            return "";
        }
        #endregion

        #region Menu
        static void MenuDuAn()
        {
            Console.WriteLine("**\t   a. Thêm mới dự án                             **");
            Console.WriteLine("**\t   b. Chỉnh sửa dự án                            **");
            Console.WriteLine("**\t   c. Xóa dự án                                  **");
            Console.WriteLine("**\t   d. Hiển thị các dự án đang có                 **");
            Console.WriteLine("**\t   e. Các loại tìm kiếm                          **");
            Console.WriteLine("**\t   f. Các cách sắp xếp                           **");
            Console.WriteLine("**\t   g. Trở về                                     **");
        }
        static void MenuCongViec()
        {
            Console.WriteLine("**\t   a. Thêm mới công việc                         **");
            Console.WriteLine("**\t   b. Chỉnh sửa công việc                        **");
            Console.WriteLine("**\t   c. Xóa công việc                              **");
            Console.WriteLine("**\t   d. Hiển thị công việc đang có                 **");
            Console.WriteLine("**\t   e. Các loại tìm kiếm                          **");
            Console.WriteLine("**\t   f. Các cách sắp xếp                           **");
            Console.WriteLine("**\t   g. Trở về                                     **");

        }
        //cân nhắc có nên thêm không
        static void MenuDuAnVaCongViec()
        {

        }
        static void MenuPhu()
        {
            Console.WriteLine("**\t   a. Đọc file                                   **");
            Console.WriteLine("**\t   b. Ghi file                                   **");
            Console.WriteLine("**\t   c. Trở về                                     **");

        }
        #endregion
        static void ChuongTrinhQuanLyDuAn()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            
            List<PROJECT> lstProject = new List<PROJECT>();
            List<TASK> lstTask = new List<TASK>();

            string luaChon = "";
            bool thoat = true, isDuAn = false, isCongViec = false, isFile = false;
            troVe:
            Console.WriteLine("------------CHƯƠNG TRÌNH QUẢN LÝ DỰ ÁN CÁ NHÂN------------");
            Console.WriteLine("**************************MENU*****************************");
            Console.WriteLine("**\t 1. Thao tác trên dự án                          **");
            if (isDuAn) MenuDuAn();
            Console.WriteLine("**\t 2. Thao tác trên nhiệm vụ dự án                 **");
            if (isCongViec) MenuCongViec();
            Console.WriteLine("**\t 3. Thao tác trên file                           **");
            if (isFile) MenuPhu();
            Console.WriteLine("**\t 4. Làm sạch màn hình                            **");
            Console.WriteLine("**\t 5. Thoát.                                       **");
            Console.WriteLine("***********************************************************");

            do
            {
                
                luaChon = KiemTraDoDaiNhapLieu("tùy chọn",0);

                //Lua chon menu chính
                if (!isDuAn && !isCongViec && !isFile)
                {
                    switch (luaChon.ToLower())
                    {
                        case "1":
                            isDuAn = true;
                            luaChon = null;
                            Console.Clear();
                            goto troVe;
                        case "2":
                            luaChon = null;
                            isCongViec = true;
                            Console.Clear();
                            goto troVe;
                        case "3":
                            luaChon = null;
                            isFile = true;
                            Console.Clear();
                            goto troVe;
                        case "4":
                            Console.Clear();
                            goto troVe;
                        case "5":
                            System.Environment.Exit(0);
                            thoat = false;
                            break;
                        default:
                            Console.WriteLine("Không có thao tác này trong menu!");
                            break;
                    }
                }    
                

                //Lua chon menu du an
                if(isDuAn)
                {
                    if (string.IsNullOrEmpty(luaChon)) return;
                    switch (luaChon.ToLower())
                    {
                        case "a":
                            NhapLieuDuAn(lstProject);
                            break;
                        case "b":
                            HienThiDuLieuDuAn(lstProject);
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
                            isDuAn = false;
                            Console.Clear();
                            goto troVe;
                        default:
                            Console.WriteLine("Bạn đang trong màn hình dự án, vui lòng trở về nếu muốn thực hiện thao tác khác!");
                            break;
                    }
                }

                //Lua chon menu cong viec
                if (isCongViec)
                {
                    if (string.IsNullOrEmpty(luaChon)) return;
                    switch (luaChon.ToLower())
                    {
                        case "a":
                            NhapLieuCongViec(lstProject, lstTask);
                            break;
                        case "b":
                            HienThiCongViecTrongDuAn(lstTask);
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
                            isCongViec = false;
                            Console.Clear();
                            goto troVe;
                        default:
                            Console.WriteLine("Bạn đang trong màn hình công việc, vui lòng trở về nếu muốn thực hiện thao tác khác!");
                            break;
                    }
                }

                //Lua chon menu xử lý file
                if (isFile)
                {
                    if (string.IsNullOrEmpty(luaChon)) return;
                    switch (luaChon.ToLower())
                    {
                        case "a":
                            break;
                        case "b":
                            break;
                        case "c":
                            isFile = false;
                            Console.Clear();
                            goto troVe;
                        default:
                            Console.WriteLine("Bạn đang trong màn hình xử lý file, vui lòng trở về nếu muốn thực hiện thao tác khác!");
                            break;
                    }
                }




            } while (thoat);

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            ChuongTrinhQuanLyDuAn();
        }
    }
}
