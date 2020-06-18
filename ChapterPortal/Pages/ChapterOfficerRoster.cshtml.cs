using System;
using System.IO;
using System.Data;
using System.Drawing;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using OfficeOpenXml;
using PdfRpt.Core.Contracts;
//using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;
using Aspose.Cells;

namespace ChapterPortal.Pages
{
    public class Report {
        public string MemberId { get; set; }
        public string Name{ get; set;}
        public string Description { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ChapterOfficerModel : PageModel
    {
        public DataTable OfficerList;

        private readonly ILogger<ChapterOfficerModel> _logger;
        public IConfiguration Configuration { get; }

        public ChapterOfficerModel(ILogger<ChapterOfficerModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        private void ProcessData(DataSet ds)
        {
            ds.Tables[0].Columns.Add("BEGIN_DATE", System.Type.GetType("System.DateTime"));
            ds.Tables[0].Columns.Add("END_DATE", System.Type.GetType("System.DateTime"));
            DateTime bdate = DateTime.Parse("1/1/1900");
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                string date = item["POSITION_BEGIN_DATE"].ToString();
                if (!string.IsNullOrEmpty(date))
                    item["BEGIN_DATE"] = DateTime.Parse(date);
                else item["BEGIN_DATE"] = bdate;

                date = item["POSITION_END_DATE"].ToString();
                if (!string.IsNullOrEmpty(date))
                    item["END_DATE"] = DateTime.Parse(date);
                else item["END_DATE"] = bdate;
            }
        }

        private void ProcessData(DataTable ds)
        {
            ds.Columns.Add("ID", System.Type.GetType("System.Int32"));
            DateTime bdate = DateTime.Parse("1/1/1900");
            int i = 0;
            foreach (DataRow item in ds.Rows)
                item["ID"] = i++;
        }

        public void OnGet()
        {
            string chapterId = HttpContext.Session.GetString("OfficerChapterId");
            DataSet ds = Utility.AMS_Query_Result("ACR_GET_STATE_CHAPTER_ROSTER_PORTAL_SP", "@master_customer_id", chapterId).Result;
            ProcessData(ds);

            string colName = HttpContext.Session.GetString("MyColumn");
            string mySort = HttpContext.Session.GetString("MySort");
            if (string.IsNullOrEmpty(colName))
            {
                colName = "LABEL_NAME";
                HttpContext.Session.SetString("MyColumn", colName);
            }

            if (string.IsNullOrEmpty(mySort))
            {
                mySort = "asc";
                HttpContext.Session.SetString("MySort", mySort);
            }

            ds.Tables[0].DefaultView.Sort = (colName + " " + mySort);
            OfficerList = ds.Tables[0].DefaultView.ToTable();
            ProcessData(OfficerList);
        }

        //-------------------------Load license-------------------------

        public static bool LoadAsposeCellLicense()
        {
            bool asposeCellLicenseLoaded = false;
            //string asposeCellLicenseFilePath = Path.Combine(licenseFileLocation, asposeCellsLicenseFileName);
            try
            {
                //Aspose.Cells.License license = new Aspose.Cells.License();
                //license.SetLicense(Path.Combine(licenseFileLocation, asposeCellsLicenseFileName));
                //asposeCellLicenseLoaded = true;
            }
            catch (Exception ex)
            {
                //LogHelper.LogError($"DocumentViewerLogs/{DateTime.Now.ToString("yyyyMMdd")}/ErrorLog/ConvertorError", ex);
            }

            return asposeCellLicenseLoaded;
        }


        //-------------------------Convert Excel to PDF-------------------------

        // read excel stream and save it to file system
        internal void ConvertExcelToPDF(Stream srcStream, string dstFilePath)
        {
            try
            {
                Aspose.Cells.Workbook xls = new Aspose.Cells.Workbook(srcStream);
                xls.Save(dstFilePath, Aspose.Cells.SaveFormat.Pdf);
            }
            catch (Exception ex)
            {
                //if (_logError)
                //    LogHelper.LogError($"DocumentViewerLogs/{DateTime.Now.ToString("yyyyMMdd")}/ErrorLog/ConvertorError", ex);
                throw new Exception(@"Convert Excel to PDF fails");
            }
        }

        // read excel from file system and return it as PDF stream
        internal MemoryStream ConvertExcelToPDF(string srcFilename)
        {
            try
            {
                MemoryStream outputStream = new MemoryStream();
                Aspose.Cells.Workbook xls = new Aspose.Cells.Workbook(srcFilename);
                xls.Save(outputStream, Aspose.Cells.SaveFormat.Pdf);
                outputStream.Position = 0;
                //if (_logTrace)
                //    LogHelper.LogTrace($"DocumentViewerLogs/{DateTime.Now.ToString("yyyyMMdd")}/TraceLog/UploaderTrace", DateTime.Now.ToLongTimeString() + "  ------DocumentConvertor: Convert Excel to PDF stream succeeds------");
                return outputStream;
            }
            catch (Exception ex)
            {
                //if (_logError)
                //    LogHelper.LogError($"DocumentViewerLogs/{DateTime.Now.ToString("yyyyMMdd")}/ErrorLog/ConvertorError", ex);
                throw new Exception(@"Convert Excel to PDF fails");
            }
        }

        //-------------------------Generate an Excel sheet-------------------------
        private byte[] GetExcelGrid(System.Collections.ICollection data)
        {
			// create a work book for an excel file
            Workbook wb = new Workbook();
            // specify the excel work sheet
            Worksheet sheet = wb.Worksheets[0];
            sheet.Cells.ImportCustomObjects(data, 0, 0, null);

            // create and customize style
            var styleHeader = wb.CreateStyle();
            var styleWrapped = wb.CreateStyle();
            styleHeader.Font.IsBold = true;
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;
            styleHeader.Pattern = BackgroundType.Solid;
            styleHeader.ForegroundArgbColor = Color.FromArgb(220, 238, 194).ToArgb();
            styleWrapped.IsTextWrapped = true;

            // get the collection of all excel cells
            var cells = sheet.Cells;
            
            //if (addGeneratedInfo)	// ACRedit Plus logic, please ignore
            //{
            //    // Set cell with value
            //    cells["A3"].PutValue("Generated By");
            //    cells["B3"].PutValue(UserContext.CurrentUser.FirstName.ToString() + " " + UserContext.CurrentUser.LastName.ToString());

            //    // Set cell with value
            //    cells["A4"].PutValue("Generated On");
            //    cells["B4"].PutValue(DateTime.Now.ToShortDateString());

            //    // Apply pre-defined style
            //    cells["B3"].SetStyle(styleWrapped);
            //}

            //if (searchModelheaders != null && searchModelheaders.Any())	// ACRedit Plus logic, please ignore
            //{
            //    // Set cell with value
            //    cells["A6"].PutValue("Search Criteria");
            //    cells["B6"].PutValue("Value");

            //    cells["A6"].SetStyle(styleHeader);
            //    cells["B6"].SetStyle(styleHeader);
            //    searchSectionRowCount++;	// ACRedit Plus logic, please ignore

            //    ...
            //}

            //...

            // Delete blank columns
            cells.DeleteBlankColumns();
            int maxDataColumnIndex = cells.MaxDataColumn;
            // merge specific cells
            //if (addGeneratedInfo)
            //    cells.Merge(2, 2, 2, maxDataColumnIndex - 1);
            //cells.Merge(rowIndex_searchStart - 1, 0, 1, maxDataColumnIndex + 1);
            //cells.Merge(rowIndex_searchStart, colIndex_searchEnd + 1, searchSectionRowCount, maxDataColumnIndex - 1);
            //cells.Merge(rowIndex_searchStart + searchSectionRowCount, 0, 1, maxDataColumnIndex + 1);

            // set style
            for (int rIndex = 0; rIndex <= cells.MaxDataRow; rIndex++)
            {
                for (int cIndex = 0; cIndex <= cells.MaxDataColumn; cIndex++)
                {
                    var cell = cells[rIndex, cIndex];
                    if (cell != null)
                    {
                        var styleBorderFont = cell.GetStyle();
                        styleBorderFont.Font.Size = 11;
                        styleBorderFont.Font.Name = "Calibri";
                        styleBorderFont.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
                        styleBorderFont.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
                        styleBorderFont.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
                        styleBorderFont.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
                        cell.SetStyle(styleBorderFont);
                    }
                }
            }
            sheet.AutoFitRows(new AutoFitterOptions() { MaxRowHeight = 45 });
            sheet.AutoFitColumns();

            // save generated excel file to stream
            MemoryStream ms = new MemoryStream();
            wb.Save(ms, SaveFormat.Xlsx);
            ms.Seek(0, SeekOrigin.Begin);

            //return ms;
            byte[] buffer = new byte[(int)ms.Length];
            buffer = ms.ToArray();
            return buffer;
        }

        public async Task<IActionResult> OnGetExportExcel()
        {
            string chapterId = HttpContext.Session.GetString("OfficerChapterId");
            DataSet ds = Utility.AMS_Query_Result("ACR_GET_STATE_CHAPTER_ROSTER_PORTAL_SP", "@master_customer_id", chapterId).Result;
            ProcessData(ds);

            string colName = HttpContext.Session.GetString("MyColumn");
            string mySort = HttpContext.Session.GetString("MySort");
            if (string.IsNullOrEmpty(colName))
            {
                colName = "LABEL_NAME";
                HttpContext.Session.SetString("MyColumn", colName);
            }

            if (string.IsNullOrEmpty(mySort))
            {
                mySort = "asc";
                HttpContext.Session.SetString("MySort", mySort);
            }

            ds.Tables[0].DefaultView.Sort = (colName + " " + mySort);
            OfficerList = ds.Tables[0].DefaultView.ToTable();

            // query data from database  
            await Task.Yield();
            IList<Report> report = new List<Report>();
            var record = new Report();
            record.MemberId = "MEMBER_CUSTOMER_ID";
            record.Name = "LABEL_NAME";
            record.Description = "POSITION_DESCRIPTION";
            record.BeginDate = "POSITION_BEGIN_DATE";
            record.EndDate = "POSITION_END_DATE";
            record.Status = "VOTING_STATUS";
            record.EmailAddress = "PRIMARY_EMAIL_ADDRESS";
            record.Phone = "PRIMARY_PHONE";
            report.Add(record);
            foreach (DataRow item in OfficerList.Rows)
            {
                record = new Report();
                record.MemberId = item["MEMBER_CUSTOMER_ID"].ToString();
                record.Name = item["LABEL_NAME"].ToString();
                record.Description = item["POSITION_DESCRIPTION"].ToString();
                record.BeginDate = item["POSITION_BEGIN_DATE"].ToString();
                record.EndDate = item["POSITION_END_DATE"].ToString();
                record.Status = item["VOTING_STATUS"].ToString();
                record.EmailAddress = item["PRIMARY_EMAIL_ADDRESS"].ToString();
                record.Phone = item["PRIMARY_PHONE"].ToString();

                report.Add(record);
            }

            string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            var stream = GetExcelGrid((System.Collections.ICollection)report);
            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> OnGetExportExcelEPPlus()
        {
            string chapterId = HttpContext.Session.GetString("OfficerChapterId");
            DataSet ds = Utility.AMS_Query_Result("ACR_GET_STATE_CHAPTER_ROSTER_PORTAL_SP", "@master_customer_id", chapterId).Result;
            ProcessData(ds);

            string colName = HttpContext.Session.GetString("MyColumn");
            string mySort = HttpContext.Session.GetString("MySort");
            if (string.IsNullOrEmpty(colName))
            {
                colName = "LABEL_NAME";
                HttpContext.Session.SetString("MyColumn", colName);
            }

            if (string.IsNullOrEmpty(mySort))
            {
                mySort = "asc";
                HttpContext.Session.SetString("MySort", mySort);
            }

            ds.Tables[0].DefaultView.Sort = (colName + " " + mySort);
            OfficerList = ds.Tables[0].DefaultView.ToTable();

            // query data from database  
            await Task.Yield();
            IList<Report> report = new List<Report>();
            var record = new Report();
            record.MemberId = "MEMBER_CUSTOMER_ID";
            record.Name = "LABEL_NAME";
            record.Description = "POSITION_DESCRIPTION";
            record.BeginDate = "POSITION_BEGIN_DATE";
            record.EndDate = "POSITION_END_DATE";
            record.Status = "VOTING_STATUS";
            record.EmailAddress = "PRIMARY_EMAIL_ADDRESS";
            record.Phone = "PRIMARY_PHONE";
            report.Add(record);
            foreach (DataRow item in OfficerList.Rows)
            {
                record = new Report();
                record.MemberId = item["MEMBER_CUSTOMER_ID"].ToString();
                record.Name = item["LABEL_NAME"].ToString();
                record.Description = item["POSITION_DESCRIPTION"].ToString();
                record.BeginDate = item["POSITION_BEGIN_DATE"].ToString();
                record.EndDate = item["POSITION_END_DATE"].ToString();
                record.Status = item["VOTING_STATUS"].ToString();
                record.EmailAddress = item["PRIMARY_EMAIL_ADDRESS"].ToString();
                record.Phone = item["PRIMARY_PHONE"].ToString();

                report.Add(record);
            }

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(report, false);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        //public async Task<IActionResult> OnGetExportPDF()
        //{
        //    string chapterId = HttpContext.Session.GetString("OfficerChapterId");
        //    DataSet ds = Utility.AMS_Query_Result("ACR_GET_STATE_CHAPTER_ROSTER_PORTAL_SP", "@master_customer_id", chapterId).Result;
        //    ProcessData(ds);

        //    string colName = HttpContext.Session.GetString("MyColumn");
        //    string mySort = HttpContext.Session.GetString("MySort");
        //    if (string.IsNullOrEmpty(colName))
        //    {
        //        colName = "LABEL_NAME";
        //        HttpContext.Session.SetString("MyColumn", colName);
        //    }

        //    if (string.IsNullOrEmpty(mySort))
        //    {
        //        mySort = "asc";
        //        HttpContext.Session.SetString("MySort", mySort);
        //    }

        //    ds.Tables[0].DefaultView.Sort = (colName + " " + mySort);
        //    OfficerList = ds.Tables[0].DefaultView.ToTable();

        //    // query data from database  
        //    await Task.Yield();
        //    IList<Report> report = new List<Report>();
        //    //var record = new Report();
        //    //record.MemberId = "MEMBER_CUSTOMER_ID";
        //    //record.Name = "LABEL_NAME";
        //    //record.Description = "POSITION_DESCRIPTION";
        //    //record.BeginDate = "POSITION_BEGIN_DATE";
        //    //record.EndDate = "POSITION_END_DATE";
        //    //record.Status = "VOTING_STATUS";
        //    //record.EmailAddress = "PRIMARY_EMAIL_ADDRESS";
        //    //record.Phone = "PRIMARY_PHONE";
        //    //report.Add(record);
        //    foreach (DataRow item in OfficerList.Rows)
        //    {
        //        var record = new Report();
        //        record.MemberId = item["MEMBER_CUSTOMER_ID"].ToString();
        //        record.Name = item["LABEL_NAME"].ToString();
        //        record.Description = item["POSITION_DESCRIPTION"].ToString();
        //        record.BeginDate = item["POSITION_BEGIN_DATE"].ToString();
        //        record.EndDate = item["POSITION_END_DATE"].ToString();
        //        record.Status = item["VOTING_STATUS"].ToString();
        //        record.EmailAddress = item["PRIMARY_EMAIL_ADDRESS"].ToString();
        //        record.Phone = item["PRIMARY_PHONE"].ToString();

        //        report.Add(record);
        //    }

        //    string pdfName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
        //}

        public async Task<IActionResult> OnGetExportPDFEPPlus()
        {
            string chapterId = HttpContext.Session.GetString("OfficerChapterId");
            DataSet ds = Utility.AMS_Query_Result("ACR_GET_STATE_CHAPTER_ROSTER_PORTAL_SP", "@master_customer_id", chapterId).Result;
            ProcessData(ds);

            string colName = HttpContext.Session.GetString("MyColumn");
            string mySort = HttpContext.Session.GetString("MySort");
            if (string.IsNullOrEmpty(colName))
            {
                colName = "LABEL_NAME";
                HttpContext.Session.SetString("MyColumn", colName);
            }

            if (string.IsNullOrEmpty(mySort))
            {
                mySort = "asc";
                HttpContext.Session.SetString("MySort", mySort);
            }

            ds.Tables[0].DefaultView.Sort = (colName + " " + mySort);
            OfficerList = ds.Tables[0].DefaultView.ToTable();

            // query data from database  
            await Task.Yield();
            IList<Report> report = new List<Report>();
            //var record = new Report();
            //record.MemberId = "MEMBER_CUSTOMER_ID";
            //record.Name = "LABEL_NAME";
            //record.Description = "POSITION_DESCRIPTION";
            //record.BeginDate = "POSITION_BEGIN_DATE";
            //record.EndDate = "POSITION_END_DATE";
            //record.Status = "VOTING_STATUS";
            //record.EmailAddress = "PRIMARY_EMAIL_ADDRESS";
            //record.Phone = "PRIMARY_PHONE";
            //report.Add(record);
            foreach (DataRow item in OfficerList.Rows)
            {
                var record = new Report();
                record.MemberId = item["MEMBER_CUSTOMER_ID"].ToString();
                record.Name = item["LABEL_NAME"].ToString();
                record.Description = item["POSITION_DESCRIPTION"].ToString();
                record.BeginDate = item["POSITION_BEGIN_DATE"].ToString();
                record.EndDate = item["POSITION_END_DATE"].ToString();
                record.Status = item["VOTING_STATUS"].ToString();
                record.EmailAddress = item["PRIMARY_EMAIL_ADDRESS"].ToString();
                record.Phone = item["PRIMARY_PHONE"].ToString();

                report.Add(record);
            }

            string pdfName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            var data = new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "ACR", Application = "PdfRpt", Keywords = "User List", Subject = "Chapter Officer Roster", Title = "Chapter Portal Officer Roster" });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
                doc.PrintingPreferences(new PrintingPreferences
                {
                    ShowPrintDialogAutomatically = false
                });
            })
            .DefaultFonts(fonts =>
            {
                    //fonts.Path(TestUtils.GetVerdanaFontPath(),
                    //            TestUtils.GetTahomaFontPath());
                fonts.Size(9);
                fonts.Color(System.Drawing.Color.Black);
            })
            .PagesFooter(footer =>
            {
                footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
            })
            .PagesHeader(header =>
            {
                header.CacheHeader(cache: false); // It's a default setting (true) to improve the performance.
                    header.DefaultHeader(defaultHeader =>
                    {
                        defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                            //defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                            defaultHeader.Message("Chapter Portal Officer Roster");
                    });
            })
            .MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.ClassicTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                //table.NumberOfDataRowsPerPage(5);
            })
            .MainTableDataSource(dataSource =>
            {
                    //var listOfRows = new List<Report>
                    //{
                    //    new User {Id = 0, LastName = "Test Degree Sign: 120°", Name = "Celsius", Balance = 0}
                    //};

                    //for (var i = 1; i <= 200; i++)
                    //{
                    //    listOfRows.Add(new User { Id = i, LastName = "LastName " + i, Name = "Name " + i, Balance = i + 1000 });
                    //}
                    dataSource.StronglyTypedList(report);
            })
            .MainTableSummarySettings(summarySettings =>
            {
                summarySettings.OverallSummarySettings("Summary");
                summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                summarySettings.PageSummarySettings("Page Summary");
            })
            .MainTableColumns(columns =>
            {
                //columns.AddColumn(column =>
                //{
                //    column.PropertyName("rowNo");
                //    column.IsRowNumber(true);
                //    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                //    column.IsVisible(true);
                //    column.Order(0);
                //    column.Width(1);
                //    column.HeaderCell("#");
                //});
                columns.AddColumn(column =>
                {
                    column.PropertyName<Report>(x => x.MemberId);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("Member Customer ID");
                });

                columns.AddColumn(column =>
                    {
                        column.PropertyName<Report>(x => x.Name);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(1);
                        column.Width(3);
                        column.HeaderCell("Label Name");
                        column.Font(font =>
                        {
                            font.Size(10);
                            font.Color(System.Drawing.Color.Brown);
                        });
                    });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Report>(x => x.Description);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(4);
                    column.HeaderCell("Position Description", horizontalAlignment: HorizontalAlignment.Left);
                    });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Report>(x => x.BeginDate);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(2);
                    column.HeaderCell("Position Begin Date");
                    column.PaddingLeft(5);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Report>(x => x.EndDate);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(4);
                    column.Width(2);
                    column.HeaderCell("Position End Date");
                    column.PaddingLeft(5);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Report>(x => x.Status);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(5);
                    column.Width(2);
                    column.HeaderCell("Voting Status");
                    column.PaddingLeft(5);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Report>(x => x.EmailAddress);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(6);
                    column.Width(3);
                    column.HeaderCell("Primary Email Address");
                    column.PaddingLeft(5);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Report>(x => x.Phone);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(7);
                    column.Width(2);
                    column.HeaderCell("Primary Phone");
                    column.PaddingLeft(5);
                });

                    //columns.AddColumn(column =>
                    //{
                    //    column.PropertyName<Report>(x => x.EndDate);
                    //    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    //    column.IsVisible(true);
                    //    column.Order(4);
                    //    column.Width(2);
                    //    column.HeaderCell("Balance");
                    //    column.ColumnItemsTemplate(template =>
                    //    {
                    //        template.TextBlock();
                    //        template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                    //                                            ? string.Empty : string.Format("{0:n0}", obj));
                    //    });
                    //    column.AggregateFunction(aggregateFunction =>
                    //    {
                    //        aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                    //        aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                    //                                            ? string.Empty : string.Format("{0:n0}", obj));
                    //    });
                    //});

                })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            }).MainTableSummarySettings(summarySettings =>
            {
                summarySettings.PreviousPageSummarySettings("Cont.");
                summarySettings.OverallSummarySettings("Total: " + report.Count);
                //summarySettings.AllGroupsSummarySettings("Groups Sum");
            })
            .Export(export =>
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            }).GenerateAsByteArray();
            //.Generate(data => data.AsPdfStream(stream, closeStream: false));

            return File(data, "application/pdf", pdfName);
        }

        public void OnPostSort(string colName)
        {
            string lastCol = HttpContext.Session.GetString("MyColumn");
            string mySort = HttpContext.Session.GetString("MySort");
            if (lastCol==colName)
            {
                if (mySort == "asc")
                    mySort = "desc";
                else
                    mySort = "asc";
            }
            else
            {
                mySort = "asc";
                HttpContext.Session.SetString("MyColumn", colName);
            }

            HttpContext.Session.SetString("MySort", mySort);

            string chapterId = HttpContext.Session.GetString("OfficerChapterId");
            DataSet ds = Utility.AMS_Query_Result("ACR_GET_STATE_CHAPTER_ROSTER_PORTAL_SP", "@master_customer_id", chapterId).Result;
            ProcessData(ds);
            ds.Tables[0].DefaultView.Sort = (colName + " "+mySort);
            OfficerList = ds.Tables[0].DefaultView.ToTable();
            ProcessData(OfficerList);
        }

        public ActionResult OnGetOfficer(int id)
        {
            string colName = HttpContext.Session.GetString("MyColumn");
            string mySort = HttpContext.Session.GetString("MySort");
            if (string.IsNullOrEmpty(colName))
                colName = "LABEL_NAME";

            if (string.IsNullOrEmpty(mySort))
                mySort = "asc";

            string chapterId = HttpContext.Session.GetString("OfficerChapterId");
            DataSet ds = Utility.AMS_Query_Result("ACR_GET_STATE_CHAPTER_ROSTER_PORTAL_SP", "@master_customer_id", chapterId).Result;
            ProcessData(ds);
            ds.Tables[0].DefaultView.Sort = (colName + " " + mySort);
            OfficerList = ds.Tables[0].DefaultView.ToTable();
            ProcessData(OfficerList);
            if (id>-1 && id<OfficerList.Rows.Count)
            {
                DataRow item = OfficerList.Rows[id];
                HttpContext.Session.SetString("FullName", item["LABEL_NAME"].ToString());
                HttpContext.Session.SetString("Description", item["POSITION_DESCRIPTION"].ToString());
                HttpContext.Session.SetString("BeginDate", item["POSITION_BEGIN_DATE"].ToString());
                HttpContext.Session.SetString("EndDate", item["POSITION_END_DATE"].ToString());
                HttpContext.Session.SetString("Status", item["VOTING_STATUS"].ToString());
                HttpContext.Session.SetString("Email", item["PRIMARY_EMAIL_ADDRESS"].ToString());
                HttpContext.Session.SetString("Phone", item["PRIMARY_PHONE"].ToString());
                HttpContext.Session.SetString("Comment", string.Empty);
            }
            return Redirect("/ChapterOfficerEdit");
        }
    }
}
