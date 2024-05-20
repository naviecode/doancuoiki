using System;
using System.Collections.Generic;
using System.Text;
using DoAnCuoiKi.QuanLyManHinh;
using DoAnCuoiKi.Function;

namespace DoAnCuoiKi
{
    class Program
    {
        /**
         * Những thứ cần làm:
         * Làm thêm tìm kiếm nhị phân
         * Các loại sắp xếp cần
         * Xem chi tiết cộng việc trong mỗi dự án khi hiện thị dự án lên
         * Cancel thao tác khi đang nhập chọn option = ESC
         * **/
        
        static void Main(string[] args)
        {
            ChuongTrinhQuanLy program = new ChuongTrinhQuanLy();
            program.ChuongTrinhQuanLyDuAn();  
        }
    }
}
