using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKi.Model
{
    class PROJECT
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
}
