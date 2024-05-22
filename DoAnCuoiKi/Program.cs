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
         * Xử lý ngày bắt đầu công việc và hoàn thành phải nằm trong khoảng ngày bắt đầu dự án và ngày kết thúc dự án
         * Kiểm tra mã dự án có trùng không trước khi import vào
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
