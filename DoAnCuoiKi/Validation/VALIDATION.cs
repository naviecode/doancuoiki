using DoAnCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKi.Validation
{
    internal static class VALIDATION
    {
        public static double FormatNumber(double number)
        {
            return number;
        }
        public static int KiemTraDuLieuSo(string text = null, int length = 0)
        {
            int result = 0;
            bool ktr = false;
        nhaplai:
            Console.Write("{0}", text);
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
        public static string KiemTraDoDaiNhapLieu(string text = null, bool required = false, int length = 0)
        {
            string result;
            nhaplai:
            Console.Write("{0}", text);
            result = Console.ReadLine();

            if (required && string.IsNullOrEmpty(result.Trim()))
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

        public static ConsoleKeyInfo NhapOption(string text)
        {
            nhaplai:
            Console.Write("{0}", text);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            string charString = Convert.ToString(keyInfo.KeyChar).Trim();
            if(string.IsNullOrEmpty(charString))
            {
                Console.WriteLine("Lựa chọn không được để trống!");
                goto nhaplai;
            }    
            return keyInfo;

        }
        public static int KiemTraMaDuAn(List<PROJECT> projects)
        {
            nhapLai:
            int ma_du_an = KiemTraDuLieuSo("Nhập mã dự án: ");
            PROJECT timKiem = projects.Find(x => x.ma_du_an == ma_du_an);
            if (projects.Contains(timKiem))
            {
                Console.WriteLine("Mã dự án đã tồn tại");
                goto nhapLai;
            }

            return ma_du_an;
        }
        public static DateTime KiemTraDuLieuThoiGian(string text = null)
        {
            DateTime reuslt = DateTime.Now;
            string str;
            bool checkDate = false;
            nhaplai:
            Console.Write("{0}", text);
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
        public static DateTime SoSanhNgay(DateTime fromDate,string text = null, string fromDateStr = null, string toDateStr = null, string operatorStr = null)
        {

            DateTime reuslt = DateTime.Now;
            string str;
            bool checkDate = false;
            nhaplai:
            Console.Write("{0}", text);
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

            if (reuslt < fromDate)
            {
                Console.WriteLine("{0} không được {1} hơn {2}!", fromDateStr, operatorStr, toDateStr);
                goto nhaplai;
            }

            return reuslt;
        }
        public static bool isDate(string data)
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

        public static bool isNumberDouble(string data)
        {
            return double.TryParse(data, out double a);
        }
        public static bool isNumberInt(string data)
        {
            return int.TryParse(data, out int a); ;
        }
        public static DateTime converStringToDateTime(string data)
        {
            string[] arrStr = data.Split("/");
            return DateTime.Parse(arrStr[1] + "/" + arrStr[0] + "/" + arrStr[2]); ;
        }
    }
}
