using System;
using System.Collections.Generic;
using System.Text;

namespace DoAnCuoiKi
{
    class Program
    {

        #region Const
        const string FilePathProject = @"..\..\..\Data\project\";
        const string FilePathTask = @"..\..\..\Data\task\";


        const string NOT_START = "Chưa bắt đầu";
        const string IN_PROGRESS = "Đang tiến hành";
        const string PENDING = "Đang chờ";
        const string DELAY = "Bị trễ";
        const string DONE = "Hoàn thành";
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
            public int ma_cong_viec;
            public string du_an_dang_thuc_hien;
            public string noi_dung_nhiem_vu;
            public string nguoi_lam;
            public string trang_thai_cong_viec;
            public DateTime ngay_bat_dau_lam;
            public DateTime thoi_han_hoan_thanh;
        }
        #endregion

        #region Function
        static void NhapLieuDuAn(List<PROJECT> projects, List<TASK> tasks)
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
                prN.trang_thai = NOT_START;
                prN.ngay_bat_dau = KiemTraDuLieuThoiGian("ngày bắt đầu (dd/MM/yyyy)");
                prN.ngay_ket_thuc = KiemTraDuLieuThoiGian("ngày kết thúc (dd/MM/yyyy)");
                prN.tasks = new List<TASK>();


                //ràn buộc ngày bắt đầu phải < ngày kết thúc => Hàm bổ sung
                Console.Write("Bạn có muốn nhập công việc cho dự án này không (Y/N)?: ");
                tiepTucCV = Console.ReadLine();
                if(tiepTucCV.ToUpper() == "Y")
                {
                    do
                    {
                        TASK tN = new TASK();
                        tN.ma_cong_viec = KiemTraDuLieuSo("mã công việc");
                        tN.du_an_dang_thuc_hien = prN.ten_du_an;
                        tN.noi_dung_nhiem_vu = KiemTraDoDaiNhapLieu("nội dung công việc");
                        tN.nguoi_lam = KiemTraDoDaiNhapLieu("người làm phụ trách");
                        tN.ngay_bat_dau_lam = KiemTraDuLieuThoiGian("ngày bắt đầu thực hiện");
                        tN.thoi_han_hoan_thanh = KiemTraDuLieuThoiGian("hạn hoàn thành công việc");
                        tN.trang_thai_cong_viec = NOT_START;
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
                
                Console.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} | {6,-12} |", prj.ma_du_an, prj.ten_du_an, prj.mo_ta, prj.trang_thai, prj.ngay_bat_dau.ToShortDateString(), prj.ngay_ket_thuc.ToShortDateString(), FormatNumber(prj.gia_tien));
                Console.WriteLine("|-------------------------------------------------------------------------------------------------------------------------------------|");
            }

            //cần làm
            Console.WriteLine("Xem chi tiết công việc của dự án");

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
                    Console.WriteLine("| {0,-6} | {1,-12} | {2,-26} | {3,-12} | {4,-12} |", stt, prj.ma_du_an, prj.ten_du_an, prj.ngay_bat_dau.ToShortDateString(), prj.ngay_ket_thuc.ToShortDateString());
                    Console.WriteLine("*-----------------------------------------------------------------------------------*");
                    stt++;
                }
                maDuAn = KiemTraDuLieuSo("mã dự án cần thêm công việc");
                
                foreach (var item in projects)
                {
                    if(item.ma_du_an == maDuAn)
                    {
                        TASK taskNew = new TASK();
                        taskNew.du_an_dang_thuc_hien = item.ten_du_an;
                        taskNew.ma_cong_viec = KiemTraDuLieuSo("mã công việc");
                        taskNew.nguoi_lam = KiemTraDoDaiNhapLieu("tên người làm công việc này");
                        taskNew.noi_dung_nhiem_vu = KiemTraDoDaiNhapLieu("nội dung công việc cần làm");
                        taskNew.ngay_bat_dau_lam = KiemTraDuLieuThoiGian("thời gian bắt đầu thực hiện");
                        taskNew.thoi_han_hoan_thanh = KiemTraDuLieuThoiGian("thời gian hoàn thành công việc");
                        taskNew.trang_thai_cong_viec = NOT_START;
                        tasks.Add(taskNew);
                        item.tasks.Add(taskNew);
                    }
                }



                Console.Write("Bạn có muốn nhập thêm công việc cho dự án không(Y/N)?: ");
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
                Console.WriteLine("| {0,-6} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} |", stt, task.du_an_dang_thuc_hien, task.noi_dung_nhiem_vu, task.nguoi_lam, task.ngay_bat_dau_lam.ToShortDateString(), task.thoi_han_hoan_thanh.ToShortDateString(), task.trang_thai_cong_viec);
                Console.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                stt++;
            }



       
        }
        
        static void DocFileTxtDuAn(List<PROJECT> projects)
        {
            bool checkDataErr = false;
            string tiepTuc = "";
            string messErr = "Dòng ";
            string[] lines;

            do
            {
                string fileName = KiemTraDoDaiNhapLieu("tên file");
                string filePath = FilePathProject + fileName;
                if (System.IO.File.Exists(filePath))
                {
                    lines = System.IO.File.ReadAllLines(filePath);
                    string[] data;

                    //validation
                    for (int i = 0; i < lines.Length; i++)
                    {
                        data = lines[i].Split(",");
                        if (!isNumberInt(data[0].ToString()) || !isDate(data[3].ToString()) || !isDate(data[4].ToString()) || !isNumberDouble(data[6].ToString()))
                        {
                            messErr += (i + 1) + ": ";
                            checkDataErr = true;
                        }
                        if (!isNumberInt(data[0].ToString()))
                        {
                            messErr += "Mã dự án không đúng định dạng";
                        }
                        if (!isDate(data[3].ToString()))
                        {
                            messErr += "\n Ngày bắt đầu không đúng định dạng";
                        }
                        if (!isDate(data[4].ToString()))
                        {
                            messErr += "\n Ngày kết thúc không đúng định dạng";
                        }
                        if (!isNumberDouble(data[6].ToString()))
                        {
                            messErr += "\n Số tiền không đúng định dạng";
                        }
                    }

                    //insert data
                    if (checkDataErr)
                    {
                        Console.WriteLine(messErr);
                    }
                    else
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            PROJECT prj = new PROJECT();
                            data = lines[i].Split(",");
                            prj.ma_du_an = int.Parse(data[0].ToString());
                            prj.ten_du_an = data[1].ToString();
                            prj.mo_ta = data[2].ToString();
                            prj.ngay_bat_dau = converStringToDateTime(data[3].ToString());
                            prj.ngay_ket_thuc = converStringToDateTime(data[4].ToString());
                            prj.nguoi_quan_ly = data[5].ToString();
                            prj.gia_tien = double.Parse(data[6].ToString());
                            prj.trang_thai = NOT_START;
                            prj.tasks = new List<TASK>();
                            projects.Add(prj);
                        }
                        Console.WriteLine("Import danh sách dự án thành công");
                    }

                }
                else
                {
                    Console.WriteLine("File không tồn tại");
                }
                Console.Write("Bạn có muốn tiếp tục thao tác import danh sách dự án không (Y/N)?: ");
                tiepTuc = Console.ReadLine();
            } while (tiepTuc.ToUpper() == "Y" ? true : false); 
            
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
        static void DocFileTxtCongViec(List<PROJECT> projects, List<TASK> tasks)
        {
            bool checkDataErr = false;
            string tiepTuc = "";
            string messErr = "Dòng ";
            string[] lines;
            int stt = 1, maDuAn;

            do
            {
                string fileName = KiemTraDoDaiNhapLieu("tên file");
                string filePath = FilePathTask + fileName;
                if (System.IO.File.Exists(filePath))
                {
                    lines = System.IO.File.ReadAllLines(filePath);
                    string[] data;

                    Console.WriteLine("*-----------------------------------------------------------------------------------*");
                    Console.WriteLine("|                                 CÁC DỰ ÁN HIỆN CÓ                                 |");
                    Console.WriteLine("*-----------------------------------------------------------------------------------*");
                    Console.WriteLine("| {0,-6} | {1,-12} | {2,-26} | {3,-12} | {4,-12} |", "STT", "Mã dự án", "Tên dự án", "Ngày bắt đầu", "Ngày kết thúc");
                    Console.WriteLine("*-----------------------------------------------------------------------------------*");

                    foreach (PROJECT prj in projects)
                    {
                        Console.WriteLine("| {0,-6} | {1,-12} | {2,-26} | {3,-12} | {4,-12} |", stt, prj.ma_du_an, prj.ten_du_an, prj.ngay_bat_dau.ToShortDateString(), prj.ngay_ket_thuc.ToShortDateString());
                        Console.WriteLine("*-----------------------------------------------------------------------------------*");
                        stt++;
                    }
                    maDuAn = KiemTraDuLieuSo("mã dự án cần thêm danh sách công việc");
                    foreach (var project in projects)
                    {
                        if (project.ma_du_an == maDuAn)
                        {
                            //validation
                            for (int i = 0; i < lines.Length; i++)
                            {
                                data = lines[i].Split(",");
                                if (!isNumberInt(data[0].ToString()) || !isDate(data[3].ToString()) || !isDate(data[4].ToString()))
                                {
                                    messErr += (i + 1) + ": ";
                                    checkDataErr = true;
                                }
                                if (!isNumberInt(data[0].ToString()))
                                {
                                    messErr += "Mã công việc không đúng định dạng";
                                }
                                if (!isDate(data[3].ToString()))
                                {
                                    messErr += "\n Ngày bắt đầu không đúng định dạng";
                                }
                                if (!isDate(data[4].ToString()))
                                {
                                    messErr += "\n Ngày hoàn thành không đúng định dạng";
                                }
                                
                            }

                            //insert data
                            if (checkDataErr)
                            {
                                Console.WriteLine(messErr);
                                break;
                            }
                            else
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    TASK taskNew = new TASK();
                                    data = lines[i].Split(",");
                                    taskNew.du_an_dang_thuc_hien = project.ten_du_an;
                                    taskNew.ma_cong_viec = int.Parse(data[0].ToString());
                                    taskNew.noi_dung_nhiem_vu = data[1].ToString();
                                    taskNew.nguoi_lam = data[2].ToString();
                                    taskNew.ngay_bat_dau_lam = converStringToDateTime(data[3].ToString());
                                    taskNew.thoi_han_hoan_thanh = converStringToDateTime(data[4].ToString());
                                    taskNew.trang_thai_cong_viec = NOT_START;
                                    project.tasks.Add(taskNew);
                                    tasks.Add(taskNew);
                                }
                                Console.WriteLine("Import danh sách công việc thành công");
                            }

                        }
                    }

                    

                }
                else
                {
                    Console.WriteLine("File không tồn tại");
                }
                Console.Write("Bạn có muốn tiếp tục thao tác import danh sách công việc không (Y/N)?: ");
                tiepTuc = Console.ReadLine();
            } while (tiepTuc.ToUpper() == "Y" ? true : false);
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
                checkDate = DateTime.TryParse(arrStr[1] + "/" + arrStr[0] + "/" + arrStr[2], out reuslt);
            }

            if (!checkDate)
            {
                Console.WriteLine("Nhập sai dữ liệu ngày!");
                goto nhaplai;
            }

            return reuslt;
        }
        static string KiemTraMaDuAn(List<PROJECT> project, string data)
        {

            return "";
        }
        static DateTime SoSanhNgay(string operatorStr, DateTime dateSs, string text = null )
        {

            return DateTime.Now;
        }
        static bool isDate(string data)
        {
            DateTime reuslt = DateTime.Now;
            bool checkDate = false;
            string[] arrStr = data.Split("/");
            if (arrStr.Length == 3)
            {
                checkDate = DateTime.TryParse(arrStr[1] + "/" + arrStr[0] + "/" + arrStr[2], out reuslt);
            }
            else
            {
                return checkDate;
            }

            return checkDate;
        }

        static bool isNumberDouble(string data)
        {
            return double.TryParse(data, out double a); 
        }
        static bool isNumberInt(string data)
        {   
            return int.TryParse(data, out int a); ;
        }
        static DateTime converStringToDateTime(string data)
        {
            string[] arrStr = data.Split("/");
            return DateTime.Parse(arrStr[1] + "/" + arrStr[0] + "/" + arrStr[2]); ;
        }
        
        #endregion

        #region Menu
        static void MenuDuAn()
        {
            Console.WriteLine("**\t   a. Thêm mới dự án                             **");
            Console.WriteLine("**\t   b. Chỉnh sửa thông tin dự án                  **");
            Console.WriteLine("**\t   c. Cập nhập trang thái dự án                  **");
            Console.WriteLine("**\t   d. Xóa dự án                                  **");
            Console.WriteLine("**\t   e. Hiển thị các dự án đang có                 **");
            Console.WriteLine("**\t   f. Các loại tìm kiếm                          **");
            Console.WriteLine("**\t   g. Các cách sắp xếp                           **");
            Console.WriteLine("**\t   h. Trở về                                     **");
        }
        static void MenuCongViec()
        {
            Console.WriteLine("**\t   a. Thêm mới công việc                         **");
            Console.WriteLine("**\t   b. Chỉnh sửa công việc                        **");
            Console.WriteLine("**\t   c. Cập nhập trang thái dự án                  **");
            Console.WriteLine("**\t   d. Xóa công việc                              **");
            Console.WriteLine("**\t   e. Hiển thị công việc đang có                 **");
            Console.WriteLine("**\t   f. Các loại tìm kiếm                          **");
            Console.WriteLine("**\t   g. Các cách sắp xếp                           **");
            Console.WriteLine("**\t   h. Trở về                                     **");

        }
        static void MenuPhu()
        {
            Console.WriteLine("**\t   a. Đọc file dự án                             **");
            Console.WriteLine("**\t   b. Đọc file công việc                         **");
            Console.WriteLine("**\t   c. Ghi file dự án                             **");
            Console.WriteLine("**\t   d. Ghi file công việc                         **");
            Console.WriteLine("**\t   e. Danh sách các file đang có                 **");
            Console.WriteLine("**\t   f. Trở về                                     **");

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
                            NhapLieuDuAn(lstProject , lstTask);
                            break;
                        case "b":
                            break;
                        case "c":
                            break;
                        case "d":
                            break;
                        case "e":
                            HienThiDuLieuDuAn(lstProject);
                            break;
                        case "f":
                            break;
                        case "g":
                            break;
                        case "h":
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
                            break;
                        case "c":
                            break;
                        case "d":
                            break;
                        case "e":
                            HienThiCongViecTrongDuAn(lstTask);
                            break;
                        case "f":
                            break;
                        case "g":
                            break;
                        case "h":
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
                            DocFileTxtDuAn(lstProject);
                            break;
                        case "b":
                            break;
                        case "c":
                            DocFileTxtCongViec(lstProject, lstTask);
                            break;
                        case "d":
                            break;
                        case "e":
                            break;
                        case "f":
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
