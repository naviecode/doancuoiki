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
            DanhSachFile("project");
            bool checkDataErr = false;
            string messErr = "Dòng ";
            string[] lines;
            string fileName = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên file cần đọc dữ liệu: ", true);
            string filePath = PATH.FilePathProjectRead + fileName;
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
                    else
                    {
                        PROJECT timKiem = projects.Find(x => x.ma_du_an == int.Parse(data[0].ToString()));
                        if (projects.Contains(timKiem))
                        {
                            messErr += "\n Mã dự án đã tồn tại";
                        }
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
            

        }
        public static void GhiFileTxtDuAn(List<PROJECT> projects)
        {
            string fileName = VALIDATION.KiemTraDoDaiNhapLieu("Đặt tên file để ghi ra dữ liệu: ", true);
            string fileLPath = PATH.FilePathProjectWrite + fileName + ".txt";
            string trangThai = "";

            if (projects.Count == 0)
            {
                Console.WriteLine("Không có dữ liệu để ghi file!");
                return;
            }

            if (!System.IO.Directory.Exists(PATH.FilePathFileAll + "write_file_project"))
            {
                System.IO.Directory.CreateDirectory(PATH.FilePathFileAll + "write_file_project");
            }

            if (System.IO.File.Exists(fileLPath))
            {
                FileStream fs = new FileStream(fileLPath, FileMode.Open);
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                writer.WriteLine("*---------------------------------------------------------------------------------------------------------------------------------------------------------------------*");
                writer.WriteLine("|                                                                     THÔNG TIN CHI TIẾT CÁC DỰ ÁN                                                                    |");
                writer.WriteLine("*---------------------------------------------------------------------------------------------------------------------------------------------------------------------*");
                writer.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-20} | {6,-13} | {7,-20} |", COLUMN_PROJECT.MA_DU_AN, COLUMN_PROJECT.TEN_DU_AN, COLUMN_PROJECT.MO_TA, COLUMN_PROJECT.TRANG_THAI, COLUMN_PROJECT.NGUOI_QUAN_LY, COLUMN_PROJECT.NGAY_BAT_DAU, COLUMN_PROJECT.NGAY_KET_THUC, COLUMN_PROJECT.GIA_TIEN);
                writer.WriteLine("|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                foreach (var prj in projects)
                {
                    if (DateTime.Now > prj.ngay_ket_thuc) trangThai = STATUS.DELAY;
                    else trangThai = prj.trang_thai;
                    writer.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-21} | {6,-13} | {7,-20} |", prj.ma_du_an, prj.ten_du_an, prj.mo_ta, trangThai, prj.nguoi_quan_ly, prj.ngay_bat_dau.ToShortDateString(), prj.ngay_ket_thuc.ToShortDateString(), string.Format("{0:#,####} VND", prj.gia_tien));
                    writer.WriteLine("|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                }
                writer.Flush();
                fs.Close();


                Console.WriteLine("Ghi File thành công, File nằm ở đường dẫn {0}", fileLPath);
            }
            else
            {
                FileStream fs = new FileStream(fileLPath, FileMode.Create);
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                writer.WriteLine("*---------------------------------------------------------------------------------------------------------------------------------------------------------------------*");
                writer.WriteLine("|                                                                     THÔNG TIN CHI TIẾT CÁC DỰ ÁN                                                                    |");
                writer.WriteLine("*---------------------------------------------------------------------------------------------------------------------------------------------------------------------*");
                writer.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-20} | {6,-13} | {7,-20} |", COLUMN_PROJECT.MA_DU_AN, COLUMN_PROJECT.TEN_DU_AN, COLUMN_PROJECT.MO_TA, COLUMN_PROJECT.TRANG_THAI, COLUMN_PROJECT.NGUOI_QUAN_LY, COLUMN_PROJECT.NGAY_BAT_DAU, COLUMN_PROJECT.NGAY_KET_THUC, COLUMN_PROJECT.GIA_TIEN);
                writer.WriteLine("|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                foreach (var prj in projects)
                {
                    if (DateTime.Now > prj.ngay_ket_thuc) trangThai = STATUS.DELAY;
                    else trangThai = prj.trang_thai;
                    //+ Ngày hiện tại > ngày kết thúc and các task chưa complete thì dự án trễ ngược lại thì done

                    writer.WriteLine("| {0,-12} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-21} | {6,-13} | {7,-20} |", prj.ma_du_an, prj.ten_du_an, prj.mo_ta, trangThai, prj.nguoi_quan_ly, prj.ngay_bat_dau.ToShortDateString(), prj.ngay_ket_thuc.ToShortDateString(), string.Format("{0:#,####} VND", prj.gia_tien));
                    writer.WriteLine("|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
                }
                writer.Flush();
                fs.Close();

                Console.WriteLine("Ghi File thành công, File nằm ở đường dẫn {0}", fileLPath);


            }
        }
        public static void DocFileTxtCongViec(List<PROJECT> projects, List<TASK> tasks)
        {
            DanhSachFile("task");
            bool checkDataErr = false;
            string messErr = "Dòng ";
            string[] lines; 
            int stt = 1, maDuAn;

            string fileName = VALIDATION.KiemTraDoDaiNhapLieu("Nhập tên file cần đọc ra dữ liệu: ", true);
            string filePath = PATH.FilePathTaskRead + fileName;
            if (System.IO.File.Exists(filePath))
            {
                lines = System.IO.File.ReadAllLines(filePath);
                string[] data;
                if(projects.Count == 0)
                {
                    Console.WriteLine("Hiện chưa có dự án nào để thêm công việc");
                    return;
                }    

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
                            if (!VALIDATION.isNumberInt(data[0].ToString()) 
                            || !VALIDATION.isDate(data[3].ToString()) 
                            || !VALIDATION.isDate(data[4].ToString()))
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
        }

        public static void DanhSachFile(string type = "")
        {
           
            if(type == "project")
            {
                IEnumerable<string> filePathsReadProject = Directory.EnumerateFiles(PATH.FilePathProjectRead);
                Console.WriteLine("Danh sách file đang có để cập nhâp dữ liệu dự án: ");
                foreach (string filePathProject in filePathsReadProject)
                {
                    string fileName = Path.GetFileName(filePathProject);
                    Console.WriteLine("{0}", fileName);
                }
            }
            else
            {
                IEnumerable<string> filePathsReadTask = Directory.EnumerateFiles(PATH.FilePathTaskRead);
                Console.WriteLine("Danh sách file đang có để cập nhâp dữ liệu công việc: ");
                foreach (string filePathTask in filePathsReadTask)
                {
                    string fileName = Path.GetFileName(filePathTask);
                    Console.WriteLine("{0}", fileName);
                }

            }

            
        }
        public static void GhiFileTxtCongViec(List<TASK> tasks)
        {
            string fileName = VALIDATION.KiemTraDoDaiNhapLieu("Đặt tên file để ghi ra dữ liệu: ", true);
            string fileLPath = PATH.FilePathTaskWrite + fileName + ".txt";
            int stt = 1;
            string status = "";
            if(tasks.Count == 0)
            {
                Console.WriteLine("Không có dữ liệu để ghi file!");
                return;
            }
            if (!System.IO.Directory.Exists(PATH.FilePathFileAll + "write_file_task"))
            {
                System.IO.Directory.CreateDirectory(PATH.FilePathFileAll + "write_file_task");
            }
            if (System.IO.File.Exists(fileLPath))
            {
                FileStream fs = new FileStream(fileLPath, FileMode.Open);
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                writer.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                writer.WriteLine("|                                                    THÔNG TIN CHI TIẾT CÁC CÔNG VIỆC                                                 |");
                writer.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                writer.WriteLine("| {0,-6} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} | {6,-12} |", "STT", "Tên dự án", "Tên công việc", "Người làm", "Thời gian bắt đầu", "Thời gian hoàn thành", "Trạng thái");
                writer.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                foreach (TASK task in tasks)
                {
                    if (task.ngay_bat_dau_lam > task.thoi_han_hoan_thanh) status = STATUS.DELAY;
                    else status = task.trang_thai_cong_viec;
                    writer.WriteLine("| {0,-6} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} |", stt, task.du_an_dang_thuc_hien, task.noi_dung_nhiem_vu, task.nguoi_lam, task.ngay_bat_dau_lam.ToShortDateString(), task.thoi_han_hoan_thanh.ToShortDateString(), task.trang_thai_cong_viec);
                    writer.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                    stt++;
                }
                writer.Flush();
                fs.Close();
                Console.WriteLine("Ghi File thành công, File nằm ở đường dẫn {0}", fileLPath);

            }
            else
            {
                FileStream fs = new FileStream(fileLPath, FileMode.Create);
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                writer.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                writer.WriteLine("|                                                    THÔNG TIN CHI TIẾT CÁC CÔNG VIỆC                                                 |");
                writer.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                writer.WriteLine("| {0,-6} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} | {6,-12} |", "STT", "Tên dự án", "Tên công việc", "Người làm", "Thời gian bắt đầu", "Thời gian hoàn thành", "Trạng thái");
                writer.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                foreach (TASK task in tasks)
                {
                    if (task.ngay_bat_dau_lam > task.thoi_han_hoan_thanh) status = STATUS.DELAY;
                    else status = task.trang_thai_cong_viec;
                    writer.WriteLine("| {0,-6} | {1,-26} | {2,-26} | {3,-12} | {4,-12} | {5,-12} |", stt, task.du_an_dang_thuc_hien, task.noi_dung_nhiem_vu, task.nguoi_lam, task.ngay_bat_dau_lam.ToShortDateString(), task.thoi_han_hoan_thanh.ToShortDateString(), task.trang_thai_cong_viec);
                    writer.WriteLine("*-------------------------------------------------------------------------------------------------------------------------------------*");
                    stt++;
                }
                writer.Flush();
                fs.Close();
                Console.WriteLine("Ghi File thành công, File nằm ở đường dẫn {0}", fileLPath);

            }



        }
    }
}
