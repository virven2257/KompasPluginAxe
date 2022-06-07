using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using KompasPluginAxe.Core;
using KompasPluginAxe.Core.Models;
using KompasPluginAxe.UI;
using NUnit.Framework;
using OfficeOpenXml;

namespace KompasSocket.UnitTests
{
    [TestFixture]
    public class ResourcesTest
    {
        private Axe _axe = new Axe();
        private AxeCreator _creator = new AxeCreator();

        [TestCase(TestName = "Нагрузочный тест потребления памяти и времени построения")]
        public void TestCpuRam()
        {
            File.Create(@"E:\res.xls").Close();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Замеры затрат времени и RAM");
            
            sheet.Cells[1, 1].Value = "Построение";
            sheet.Cells[1, 1].Style.Font.Bold = true;
            sheet.Cells[1, 1].Style.Font.Name = "Arial";
            sheet.Cells[1, 1].Style.Font.Size = 10;
            sheet.Cells[1, 2].Value = "Время, сек";
            sheet.Cells[1, 2].Style.Font.Bold = true;
            sheet.Cells[1, 2].Style.Font.Name = "Arial";
            sheet.Cells[1, 2].Style.Font.Size = 10;
            sheet.Cells[1, 3].Value = "ОЗУ, МиБ";
            sheet.Cells[1, 3].Style.Font.Bold = true;
            sheet.Cells[1, 3].Style.Font.Name = "Arial";
            sheet.Cells[1, 3].Style.Font.Size = 10;
            sheet.Cells[1, 4].Value = "Прирост ОЗУ";
            sheet.Cells[1, 4].Style.Font.Bold = true;
            sheet.Cells[1, 4].Style.Font.Name = "Arial";
            sheet.Cells[1, 4].Style.Font.Size = 10;
            sheet.Cells[1, 5].Value = "Причина завершения";
            sheet.Cells[1, 5].Style.Font.Bold = true;
            sheet.Cells[1, 5].Style.Font.Name = "Arial";
            sheet.Cells[1, 5].Style.Font.Size = 10;

            _creator.Axe = _axe;

            long? prevRam = 0;

            try
            {
                for (int i = 1; i <= 1000; i++)
                {
                    var time = Stopwatch.StartNew();
                    _creator.Init();
                    _creator.CreateAxe();
                    time.Stop();
                
                    var elapsed = time.ElapsedMilliseconds;
                    var processes = Process.GetProcessesByName("KOMPAS");
                    var ram = processes.FirstOrDefault()?.WorkingSet64;

                    sheet.Cells[1 + i, 1].Value = i;
                    sheet.Cells[1 + i, 1].Style.Font.Name = "Arial";
                    sheet.Cells[1 + i, 1].Style.Font.Size = 10;
                
                    sheet.Cells[1 + i, 2].Value = "=" + elapsed/1000;
                    sheet.Cells[1 + i, 2].Style.Font.Name = "Arial";
                    sheet.Cells[1 + i, 2].Style.Font.Size = 10;
                
                    sheet.Cells[1 + i, 3].Value = ram/(Math.Pow(1024, 2));
                    sheet.Cells[1 + i, 3].Style.Font.Name = "Arial";
                    sheet.Cells[1 + i, 3].Style.Font.Size = 10;

                    if (i > 1)
                    {
                        sheet.Cells[1 + i, 4].Value = (ram - prevRam)/(Math.Pow(1024, 2));
                        sheet.Cells[1 + i, 4].Style.Font.Size = 10;
                        sheet.Cells[1 + i, 4].Style.Font.Name = "Arial";
                    }
                
                    prevRam = ram;
                }
                
                sheet.Cells[2, 5].Value = "Успешное завершение";
                sheet.Cells[2, 5].Style.Font.Name = "Arial";
                sheet.Cells[2, 5].Style.Font.Size = 10;
            }
            catch (Exception ex)
            {
                sheet.Cells[2, 5].Value = ex.Message;
                sheet.Cells[2, 5].Style.Font.Name = "Arial";
                sheet.Cells[2, 5].Style.Font.Size = 10;
            }
            finally
            {
                File.WriteAllBytes(@"E:\res.xls", package.GetAsByteArray());
            }

            Assert.IsTrue(true);
        }
    }
}