using DoAnCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAnCuoiKi.Validation;
using DoAnCuoiKi.Function;


namespace DoAnCuoiKi.QuanLyManHinh
{
    public class ChuongTrinhQuanLy
    {
        static void MenuDuAn()
        {
            Console.WriteLine("**\t   a. Thêm mới dự án                             **");
            Console.WriteLine("**\t   b. Chỉnh sửa thông tin dự án                  **");
            Console.WriteLine("**\t   c. Cập nhập trang thái dự án                  **");
            Console.WriteLine("**\t   d. Xóa dự án                                  **");
            Console.WriteLine("**\t   e. Hiển thị các dự án đang có                 **");
            Console.WriteLine("**\t   f. Tìm kiếm thông tin dự án                   **");
            Console.WriteLine("**\t   g. Sắp xếp dự án                              **");
        }
        static void MenuCongViec()
        {
            Console.WriteLine("**\t   a. Thêm mới công việc                         **");
            Console.WriteLine("**\t   b. Chỉnh sửa công việc                        **");
            Console.WriteLine("**\t   c. Cập nhập trang thái công việc              **");
            Console.WriteLine("**\t   d. Xóa công việc                              **");
            Console.WriteLine("**\t   e. Hiển thị công việc đang có                 **");
            Console.WriteLine("**\t   f. Các loại tìm kiếm                          **");
            Console.WriteLine("**\t   g. Các cách sắp xếp                           **");

        }
        static void MenuPhu()
        {
            Console.WriteLine("**\t   a. Đọc file dự án                             **");
            Console.WriteLine("**\t   b. Đọc file công việc                         **");
            Console.WriteLine("**\t   c. Ghi file dự án                             **");
            Console.WriteLine("**\t   d. Ghi file công việc                         **");
            Console.WriteLine("**\t   e. Danh sách các file đang có                 **");
        }

        public void ChuongTrinhQuanLyDuAn()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            List<PROJECT> lstProject = new List<PROJECT>();
            List<TASK> lstTask = new List<TASK>();

            ConsoleKeyInfo luaChon;
            
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
            Console.WriteLine("**\t 4. Xóa dữ liệu đang hiển thị trên màn hình      **");
            Console.WriteLine("**\t 5. Thoát.                                       **");
            Console.WriteLine("***********************************************************");

            do
            {
                if (!isDuAn && !isCongViec && !isFile)
                {
                    luaChon = VALIDATION.NhapOption("Nhập tùy chọn: ");
                }
                else
                {
                    luaChon = VALIDATION.NhapOption("Nhập tùy chọn (Bấm ESC để trở về menu chính hoặc hủy bỏ thao tác): ");
                    if(luaChon.Key == ConsoleKey.Escape)
                    {
                        isDuAn = false;
                        isFile = false;
                        isCongViec = false;
                        Console.Clear();
                        goto troVe;
                    }
                    Console.WriteLine();
                }

                //Lua chon menu chính
                if (!isDuAn && !isCongViec && !isFile)
                {
                    switch (luaChon.KeyChar)
                    {
                        case '1':
                            isDuAn = true;
                            Console.Clear();
                            goto troVe;
                        case '2':
                            isCongViec = true;
                            Console.Clear();
                            goto troVe;
                        case '3':
                            isFile = true;
                            Console.Clear();
                            goto troVe;
                        case '4':
                            Console.Clear();
                            goto troVe;
                        case '5':
                            System.Environment.Exit(0);
                            thoat = false;
                            break;
                        default:
                            Console.WriteLine("Không có thao tác này trong menu!");
                            break;
                    }
                }


                //Lua chon menu du an
                if (isDuAn)
                {
                    switch (luaChon.KeyChar)
                    {
                        case 'a':
                            HandleProject.NhapLieuDuAn(lstProject, lstTask);
                            break;
                        case 'b':
                            HandleProject.CapNhapDuAn(lstProject);
                            break;
                        case 'c':
                            HandleProject.CapNhapTrangThai(lstProject);
                            break;
                        case 'd':
                            HandleProject.XoaDuAn(lstProject);
                            break;
                        case 'e':
                            HandleProject.HienThiDuLieuDuAn(lstProject);
                            break;
                        case 'f':
                            HandleProject.TimKiemThongTinDuAn(lstProject);
                            break;
                        case 'g':
                            HandleProject.SapXepThongTinDuAn(ref lstProject);
                            break;
                        default:
                            Console.WriteLine("Bạn đang trong màn hình dự án, vui lòng trở về nếu muốn thực hiện thao tác khác!");
                            break;
                    }
                }

                //Lua chon menu cong viec
                if (isCongViec)
                {
                    switch (luaChon.KeyChar)
                    {
                        case 'a':
                            HandleTask.NhapLieuCongViec(lstProject, lstTask);
                            break;
                        case 'b':
                            HandleTask.CapNhapCongViec(lstTask);
                            break;
                        case 'c':
                            HandleTask.CapNhapTrangThai(lstTask);
                            break;
                        case 'd':
                            HandleTask.XoaDuAn(lstTask);
                            break;
                        case 'e':
                            HandleTask.HienThiCongViecTrongDuAn(lstTask);
                            break;
                        case 'f':
                            HandleTask.TimKiemThongTinCongViec(lstTask);
                            break;
                        case 'g':
                            HandleTask.SapXepThongTinDuAn(ref lstTask);
                            break;
                        default:
                            Console.WriteLine("Bạn đang trong màn hình công việc, vui lòng trở về nếu muốn thực hiện thao tác khác!");
                            break;
                    }
                }

                //Lua chon menu xử lý file
                if (isFile)
                {
                    switch (luaChon.KeyChar)
                    {
                        case 'a':
                            HandleFile.DocFileTxtDuAn(lstProject);
                            break;
                        case 'b':
                            break;
                        case 'c':
                            HandleFile.DocFileTxtCongViec(lstProject, lstTask);
                            break;
                        case 'd':
                            break;
                        case 'e':
                            HandleFile.DanhSachFile();
                            break;
                        default:
                            Console.WriteLine("Bạn đang trong màn hình xử lý file, vui lòng trở về nếu muốn thực hiện thao tác khác!");
                            break;
                    }
                }




            } while (thoat);
            Console.ReadLine();
            


        }
    }
}
