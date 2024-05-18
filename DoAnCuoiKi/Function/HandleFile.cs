using DoAnCuoiKi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAnCuoiKi.Validation;
using DoAnCuoiKi.Const;
using System.IO;

namespace DoAnCuoiKi.Function
{
    internal class HandleFile
    {
        public static void DocFileTxtDuAn(List<PROJECT> projects)
        {
            bool checkDataErr = false;
            string tiepTuc = "";
            string messErr = "Dòng ";
            string[] lines;

            do
            {
                string fileName = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên file: ",true);
                string filePath = PATH.FilePathProject + fileName;
                if (System.IO.File.Exists(filePath))
                {
                    lines = System.IO.File.ReadAllLines(filePath);
                    string[] data;

                    //validation
                    for (int i = 0; i < lines.Length; i++)
                    {
                        data = lines[i].Split(",");
                        if (!VALIDATION.isNumberInt(data[0].ToString()) || !VALIDATION.isDate(data[3].ToString()) || !VALIDATION.isDate(data[4].ToString()) || !VALIDATION.isNumberDouble(data[6].ToString()))
                        {
                            messErr += (i + 1) + ": ";
                            checkDataErr = true;
                        }
                        if (!VALIDATION.isNumberInt(data[0].ToString()))
                        {
                            messErr += "Mã dự án không đúng định dạng";
                        }
                        if (!VALIDATION.isDate(data[3].ToString()))
                        {
                            messErr += "\n Ngày bắt đầu không đúng định dạng";
                        }
                        if (!VALIDATION.isDate(data[4].ToString()))
                        {
                            messErr += "\n Ngày kết thúc không đúng định dạng";
                        }
                        if (!VALIDATION.isNumberDouble(data[6].ToString()))
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
                            prj.ngay_bat_dau = VALIDATION.converStringToDateTime(data[3].ToString());
                            prj.ngay_ket_thuc = VALIDATION.converStringToDateTime(data[4].ToString());
                            prj.nguoi_quan_ly = data[5].ToString();
                            prj.gia_tien = double.Parse(data[6].ToString());
                            prj.trang_thai = STATUS.NOT_START;
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
        public static void GhiFileTxtDuAn()
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
        public static void DocFileTxtCongViec(List<PROJECT> projects, List<TASK> tasks)
        {
            bool checkDataErr = false;
            string tiepTuc = "";
            string messErr = "Dòng ";
            string[] lines;
            int stt = 1, maDuAn;

            do
            {
                string fileName = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên file: ",true);
                string filePath = PATH.FilePathTask + fileName;
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
                    maDuAn = VALIDATION.KiemTraDuLieuSo("Nhập mã dự án cần thêm danh sách công việc: ");
                    foreach (var project in projects)
                    {
                        if (project.ma_du_an == maDuAn)
                        {
                            //validation
                            for (int i = 0; i < lines.Length; i++)
                            {
                                data = lines[i].Split(",");
                                if (!VALIDATION.isNumberInt(data[0].ToString()) || !VALIDATION.isDate(data[3].ToString()) || !VALIDATION.isDate(data[4].ToString()))
                                {
                                    messErr += (i + 1) + ": ";
                                    checkDataErr = true;
                                }
                                if (!VALIDATION.isNumberInt(data[0].ToString()))
                                {
                                    messErr += "Mã công việc không đúng định dạng";
                                }
                                if (!VALIDATION.isDate(data[3].ToString()))
                                {
                                    messErr += "\n Ngày bắt đầu không đúng định dạng";
                                }
                                if (!VALIDATION.isDate(data[4].ToString()))
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
                                    taskNew.ngay_bat_dau_lam = VALIDATION.converStringToDateTime(data[3].ToString());
                                    taskNew.thoi_han_hoan_thanh = VALIDATION.converStringToDateTime(data[4].ToString());
                                    taskNew.trang_thai_cong_viec = STATUS.NOT_START;
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

        public static void DanhSachFile()
        {
            if (!Directory.Exists(PATH.FilePathProject))
            {
                Console.WriteLine("Không tồn tại: " + PATH.FilePathProject);
                return;
            }

            IEnumerable<string> filePaths = Directory.EnumerateFiles(PATH.FilePathProject);
            Console.WriteLine("Danh sách file đang có: ");
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                Console.WriteLine(fileName);
            }
        }
        public static void GhiFileTxtCongViec()
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
    }
}
