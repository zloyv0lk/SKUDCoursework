using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SKUD.DataBase;
using OfficeOpenXml;

namespace SKUD.Excel
{
    public class ExcelDoc
    {
        public static void createTable()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            List<employeer> empList = DB.entities.employeers.ToList();
            List<@event> eventList = DB.entities.events.Where(c => c.date.Year == year && c.date.Month == month).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets.Add("Сотрудники");

            sheet.Column(1).Width = 25;
            sheet.Column(1).Style.WrapText = true;

            sheet.Cells[1, 1].Value = "Табель учёта использования рабочего времени";
            sheet.Cells[1, 1].Style.WrapText = true;
            sheet.Cells[1, 1].Style.Font.Size = 16;

            sheet.Cells[1, 2].Value = year + " год";
            sheet.Cells[1, 3].Value = month + " месяц";

            sheet.Cells[4, 1].Value = "Фамилия Имя Отчество, должность, таб.номер";

            for (int i = 0; i < DateTime.DaysInMonth(year, month); i++)
            {
                sheet.Cells[4, i + 2].Value = i+1;
            }

            for (int i = 0; i < empList.Count - 1; i++)
            {
                sheet.Cells[i + 5, 1].Value = empList[i].surname + " " + empList[i].name + " " + empList[i].patronomyc + ", " + empList[i].post.title + ", №" + empList[i].employeer_id;

                for (int j = 0; j < DateTime.DaysInMonth(year, month); j++)
                {
                    var entry_evnt = eventList.FirstOrDefault(c => c.employeer.employeer_id == empList[i].employeer_id && c.date.Day == (int)sheet.Cells[i + 4, j + 2].Value && c.event_type.event_type_id == 1);
                    var exit_evnt = eventList.FirstOrDefault(c => c.employeer.employeer_id == empList[i].employeer_id && c.date.Day == (int)sheet.Cells[i + 4, j + 2].Value && c.event_type.event_type_id == 2);
                    
                    DateTime dt = new DateTime(year, month, j+1);

                    if (entry_evnt != null && exit_evnt != null)
                    {
                        TimeSpan abc = exit_evnt.date - entry_evnt.date;
                        if(abc.Minutes <= 10)
                            sheet.Cells[i + 5, j + 2].Value = abc.Hours + ":0" + abc.Minutes;
                        else
                            sheet.Cells[i + 5, j + 2].Value = abc.Hours + ":" + abc.Minutes;
                        
                        if(abc.Hours < 9)
                        {
                            sheet.Cells[i + 5, j + 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            sheet.Cells[i + 5, j + 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                        }
                    }
                    else if (dt.DayOfWeek.ToString() == "Saturday" || dt.DayOfWeek.ToString() == "Sunday")
                    {
                        sheet.Cells[i + 5, j + 2].Value = "В";
                    }
                    else
                    {
                        sheet.Cells[i + 5, j + 2].Value = "Н";
                        sheet.Cells[i + 5, j + 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        sheet.Cells[i + 5, j + 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                    }
                }
            }

            var a = package.GetAsByteArray();
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Отчёт.xlsx", a);
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Отчёт.xlsx");

        }
    }
}
