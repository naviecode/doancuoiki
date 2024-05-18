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
         * Viết hàm validate so sánh ngày nào bé hơn 
         * Format giá tiền có dấu phẩy và thêm VND
         * Khi chọn tìm kiếm cho người dùng chọn tìm kiếm theo cột nào, và sau đó sẽ dùng tìm kiếm gì để tìm kiếm
         * Khi chọn sắp xếp cho người dùng chọn sắp xếp theo cột nào, và sau đó sắp xếp xong in ra
         * Hiển thị file đã có trong thư mục, và kiểm tra file import phải ở dạng txt
         * Điều chỉnh thêm thoát nhanh bằng Nút ESC thay vì chọn option
         * 
         * **/
        
        static void Main(string[] args)
        {
            ChuongTrinhQuanLy project = new ChuongTrinhQuanLy();
            project.ChuongTrinhQuanLyDuAn();  
        }
    }
}
