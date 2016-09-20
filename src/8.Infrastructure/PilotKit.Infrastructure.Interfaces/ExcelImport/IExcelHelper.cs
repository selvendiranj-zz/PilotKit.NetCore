using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Infrastructure.Interfaces.ExcelImport
{
    public interface IExcelHelper
    {
        IEnumerable<T> ReadExcelUsingEPPlus<T>(ExcelWorkbook eppWorkbook, string tabName) where T : ExcelModelBase;
        void ExportToExcel<T>(IList<T> dataSource, string fileName);
        void ProcessOverallRevLossWorksheet(DataTable dt, string vertical, ExcelPackage pack);
        void ProcessAccountRevLossWorksheet(DataTable dt, string vertical, string country, ExcelPackage pack);
        void ProcessOverallPositionWorksheet(DataTable dt, string level, string vertical, ExcelPackage pack);
        void ProcessAccountPositionsWorksheet(DataTable dt, string vertical, string country, ExcelPackage pack);
    }
}
