using PilotKit.Domain.Interfaces.RevenueLoss;
using PilotKit.Domain.Model.RevenueLoss;
using PilotKit.Infrastructure.Interfaces.ExcelImport;
using PilotKit.Orchestration.Concrete.Common;
using PilotKit.Orchestration.Interfaces.RevenueLoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PilotKit.Infrastructure.CrossCutting.Constants;
using PilotKit.Domain.Model.Common;
using System.IO;
using OfficeOpenXml;
using System.Data;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Extensions.Options;

namespace PilotKit.Orchestration.Concrete.RevenueLoss
{
    public class RevenueLossOrchestrator : OrchestratorBase, IRevenueLossOrchestrator
    {
        private IRevenueLossService service;
        private IExcelHelper excelHelper;
        private AppSettings appSettings;

        private IList<Summary> revenueLossOverall;
        private DataTable dtRevenueLossOverall = new DataTable();
        private IList<Positions> detailSummary;
        private DataTable dtDetailSummary = new DataTable();
        private IList<SummaryDetail> revenueLoss;
        private DataTable dtRevenueLoss = new DataTable();
        private IList<PositionSummary> positionOverall;
        private DataTable dtPositionOverall = new DataTable();
        private DataSet ds = new DataSet();

        public RevenueLossOrchestrator(IRevenueLossService service, IExcelHelper excelHelper,
                                       IOptions<AppSettings> appSettings)
        {
            this.service = service;
            this.excelHelper = excelHelper;
            this.appSettings = appSettings.Value;
        }

        public void UploadRevenueLoss(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (ExcelPackage eppPackage = new ExcelPackage(fs)) //new ExcelPackage(fs, bookConfig.Password)
                {
                    using (ExcelWorkbook eppWorkbook = eppPackage.Workbook)
                    {
                        var eppSheets = eppWorkbook.Worksheets.ToList();
                        this.service.RemoveAllPositions();
                        foreach (var sheet in eppSheets)
                        {
                            if (!sheet.Name.Contains('|')) continue;
                            List<Positions> positions = this.excelHelper.ReadExcelUsingEPPlus<Positions>(
                                eppWorkbook, sheet.Name).ToList();
                            this.service.UploadPositions(positions, sheet.Name);
                        }
                    }
                }
            }
        }

        public void ExportToExcel()
        {
            var positions = this.service.GetPositions();
            this.excelHelper.ExportToExcel<Positions>(positions, AppSettings.ExcelStageLocation);
        }

        public IList<Positions> GetRevenueLossDetail(string vertical, string country)
        {
            return this.service.GetRevenueLossDetail(vertical, country);
        }

        public IList<Summary> GetRevenueLossSummary(string vertical)
        {
            return this.service.GetRevenueLossSummary(vertical);
        }

        public IList<SummaryDetail> GetRevenueLossSummaryDetail(string vertical, string country)
        {
            return this.service.GetRevenueLossSummaryDetail(vertical, country);
        }

        public IList<PositionSummary> GetPositionSummary(string level, string vertical)
        {
            return this.service.GetPositionSummary(level, vertical);
        }

        public IList<PositionMonthly> GetPositionMonthly(string level, string vertical)
        {
            return this.service.GetPositionMonthly(level, vertical);
        }

        public string ExportSummaryToExcel()
        {
            // string level = "Horizontal";

            string vertical;
            string country;
            DateTimeFormatInfo dt = new DateTimeFormatInfo();
            string abbMonth = dt.GetAbbreviatedMonthName(DateTime.Now.Month);
            string date = DateTime.Now.Day.ToString();
            string fileName = AppSettings.RevenueLossReportFileName + abbMonth + "_" + date + ".xlsx";
            ExcelPackage pack = CreateExcel(fileName);
            CreateWorksheets(pack);
            
            country = null;
            vertical = null;

            GetOverallRevLossSummary(vertical, pack);
            GetOverallRevLossSummary("TD", pack);
            GetOverallRevLossSummary("RBC", pack);
            GetOverallRevLossSummary("Scotia", pack);
            GetOverallRevLossSummary("CIBC", pack);
            GetOverallRevLossSummary("BMO", pack);
            GetAccountRevLossSummary("TD", "Canada", pack);
            GetAccountRevLossSummary("RBC", "Canada", pack);
            GetAccountRevLossSummary("Scotia", "Canada", pack);
            GetAccountRevLossSummary("CIBC", "Canada", pack);
            GetAccountRevLossSummary("BMO", "Canada", pack);
            GetAccountRevLossSummary("TD", "US", pack);
            GetAccountRevLossSummary("RBC", "US", pack);
            GetAccountRevLossSummary("RBC", "UK", pack);

            GetOverallPositionSummary("Horizontal", vertical, pack);
            GetOverallPositionSummary("Vertical", vertical, pack);
            GetOverallPositionSummary("Horizontal", "TD", pack);
            GetOverallPositionSummary("Horizontal", "RBC", pack);
            GetOverallPositionSummary("Horizontal", "Scotia", pack);
            GetOverallPositionSummary("Horizontal", "CIBC", pack);
            GetOverallPositionSummary("Horizontal", "BMO", pack);

            GetAccountPositionsSummary("TD", "Canada", pack);
            GetAccountPositionsSummary("TD", "US", pack);
            GetAccountPositionsSummary("RBC", "Canada", pack);
            GetAccountPositionsSummary("RBC", "US", pack);
            GetAccountPositionsSummary("RBC", "UK", pack);
            GetAccountPositionsSummary("Scotia", country, pack);
            GetAccountPositionsSummary("CIBC", country, pack);
            GetAccountPositionsSummary("BMO", country, pack);

            pack.Save();

            return fileName;
        }

        private void Clear(DataTable dt)
        {
            //Deleting the IList collection and datatable rows
            if (dt.Rows.Count > 0)
            {
                dt.Rows.Clear();
            }

        }

        private void GetOverallRevLossSummary(string vertical, ExcelPackage pack)
        {
            // string country = null;

            if (vertical == null || vertical.Equals("null")) vertical = null;
            Clear(dtRevenueLossOverall);
            revenueLossOverall = this.service.GetRevenueLossSummary(vertical);
            dtRevenueLossOverall = ToDataTable<Summary>(revenueLossOverall);
            this.excelHelper.ProcessOverallRevLossWorksheet(dtRevenueLossOverall, vertical, pack);
        }

        private void GetAccountRevLossSummary(string vertical, string country, ExcelPackage pack)
        {
            if (vertical == null || vertical.Equals("null")) vertical = null;
            if (country == null || country.Equals("null")) country = null;
            Clear(dtRevenueLoss);
            revenueLoss = this.service.GetRevenueLossSummaryDetail(vertical, country);
            dtRevenueLoss = ToDataTable<SummaryDetail>(revenueLoss);
            if (country == "US")
            {
                HideColumns(ref dtRevenueLoss, "CAD");
                HideColumns(ref dtRevenueLoss, "GBP");
            }
            else if (country == "UK")
            {
                HideColumns(ref dtRevenueLoss, "CAD");
            }
            else if (country == "Canada")
            {
                HideColumns(ref dtRevenueLoss, "GBP");
            }
            this.excelHelper.ProcessAccountRevLossWorksheet(dtRevenueLoss, vertical, country, pack);
        }

        private void GetOverallPositionSummary(string level, string vertical, ExcelPackage pack)
        {
            if (vertical == null || vertical.Equals("null")) vertical = null;
            Clear(dtPositionOverall);
            positionOverall = this.service.GetPositionSummary(level, vertical);
            dtPositionOverall = ToDataTable<PositionSummary>(positionOverall);
            this.excelHelper.ProcessOverallPositionWorksheet(dtPositionOverall, level, vertical, pack);

        }

        private void GetAccountPositionsSummary(string vertical, string country, ExcelPackage pack)
        {
            if (vertical == null || vertical.Equals("null")) vertical = null;
            if (country == null || country.Equals("null")) country = null;
            Clear(dtDetailSummary);
            detailSummary = this.service.GetRevenueLossDetail(vertical, country);
            dtDetailSummary = ToDataTable<Positions>(detailSummary);
            this.excelHelper.ProcessAccountPositionsWorksheet(dtDetailSummary, vertical, country, pack);

        }

        private ExcelPackage CreateExcel(string fileName)
        {
            DirectoryInfo outputDir = new DirectoryInfo(AppSettings.RevenueLossReportPath);
            FileInfo newFile = new FileInfo(outputDir + @"\" + fileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(outputDir + @"\" + fileName);
            }
            else
            {
                newFile = new FileInfo(outputDir + @"\" + fileName);
            }
            ExcelPackage package = new ExcelPackage(newFile);
            return package;
        }

        private void CreateWorksheets(ExcelPackage package)
        {
            ExcelWorksheet ws;
            ws = package.Workbook.Worksheets.Add("Summary");
            ws.Name = "Summary";
            ws.View.ZoomScale = 70;
            ws = package.Workbook.Worksheets.Add("TD Rev Loss Summary");
            ws.Name = "TD Rev Loss Summary";
            ws = package.Workbook.Worksheets.Add("RBC Rev Loss Summary");
            ws.Name = "RBC Rev Loss Summary";
            ws = package.Workbook.Worksheets.Add("Scotia Rev Loss Summary");
            ws.Name = "Scotia Rev Loss Summary";
            ws = package.Workbook.Worksheets.Add("CIBC Rev Loss Summary");
            ws.Name = "CIBC Rev Loss Summary";
            ws = package.Workbook.Worksheets.Add("BMO Rev Loss Summary");
            ws.Name = "BMO Rev Loss Summary";
            ws = package.Workbook.Worksheets.Add("TD Canada");
            ws.Name = "TD Canada";
            ws = package.Workbook.Worksheets.Add("TD US");
            ws.Name = "TD US";
            ws = package.Workbook.Worksheets.Add("RBC Canada");
            ws.Name = "RBC Canada";
            ws = package.Workbook.Worksheets.Add("RBC US");
            ws.Name = "RBC US";
            ws = package.Workbook.Worksheets.Add("RBC UK");
            ws.Name = "RBC UK";
            ws = package.Workbook.Worksheets.Add("Scotia");
            ws.Name = "Scotia";
            ws = package.Workbook.Worksheets.Add("CIBC");
            ws.Name = "CIBC";
            ws = package.Workbook.Worksheets.Add("BMO");
            ws.Name = "BMO";
        }

        private void HideColumns(ref DataTable dt, string currency)
        {

            DataTable dtcopy = dt;
            dtcopy.Columns.Cast<DataColumn>().ToList().ForEach(column =>
            {
                if (column.ColumnName.IndexOf(currency) > -1)
                {
                    dtcopy.Columns.Remove(column);
                }
            });
            dt.AcceptChanges();
        }

        private DataTable ToDataTable<T>(IList<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
