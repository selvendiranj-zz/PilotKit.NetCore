using NPOI.SS.Util;
using OfficeOpenXml;
using PilotKit.Infrastructure.CrossCutting.Common;
using PilotKit.Infrastructure.CrossCutting.Extensions;
using PilotKit.Infrastructure.CrossCutting.Utilities;
using PilotKit.Infrastructure.Interfaces.ExcelImport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PilotKit.Infrastructure.CrossCutting.Constants;
using OfficeOpenXml.Style;
using System.Drawing;

namespace PilotKit.Infrastructure.CrossCutting.ExcelImport
{
    public class ExcelHelper : InfrastructureBase, IExcelHelper
    {
        private const String STR_ACCOUNTING_FORMAT = @"_(""$""* #,##0.00_);_(""$""* \(#,##0.00\);_(""$""* ""-""??_);_(@_)";

        public DataTable ReadExcelUsingEPPlus(ExcelWorkbook workbook, string tabName,
                IDictionary<string, string> propertyMapping, IDictionary<string, string> propertyTypeMapping,
                int endRow, string strEndColumn, int[] rowsToIgnore, int[] columnsToIgnore)
        {
            DataTable excelSheetData;
            DataRow dataRow;
            ExcelWorksheet eppSheet;
            ExcelRange eppRow;
            ExcelRange eppCell;
            int endColumn;

            endColumn = CellReference.ConvertColStringToIndex(strEndColumn) + 1;
            excelSheetData = new DataTable();
            eppSheet = workbook.Worksheets[tabName];

            for (int col = 1; col <= endColumn; col++)
            {
                excelSheetData.Columns.Add(new DataColumn(CellReference.ConvertNumToColString(col - 1)));
            }

            if (eppSheet == null || eppSheet.Hidden.Equals(eWorkSheetHidden.Hidden))
            {
                excelSheetData.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                {
                    if (propertyMapping.Keys.Contains(column.ColumnName))
                    {
                        column.ColumnName = propertyMapping[column.ColumnName];
                        // SqlDbType sqlType = (SqlDbType)Enum.Parse(typeof(SqlDbType), propertyTypeMapping[column.ColumnName], true);
                        // column.DataType = columnTypeMap[sqlType];
                    }
                    else
                    {
                        excelSheetData.Columns.Remove(column);
                    }
                });
                return excelSheetData;
            }

            if (endRow == 0)
            {
                var findRow = from cell in eppSheet.Cells[""]//ConfigManager.ExcelConfigEndKeyRow
                              where cell.Value is string
                              && cell.Value as string == ""//ConfigManager.ExcelConfigEndKey
                              select cell;
                if (findRow.Count<ExcelAddress>() != 0)
                {
                    endRow = findRow.Last().End.Row - 1;
                }
            }

            for (int i = 1; i <= endRow; i++)
            {
                eppRow = eppSheet.Cells[i, 1, i, endColumn];
                dataRow = excelSheetData.NewRow();
                excelSheetData.Rows.Add(dataRow);

                for (int j = 1; j <= endColumn; j++)
                {
                    eppCell = eppRow[i, j];
                    dataRow[j - 1] = eppCell.Value ?? string.Empty;
                }
            }

            rowsToIgnore.OrderByDescending(x => x).ForEach(r => { if (r != 0) excelSheetData.Rows[r - 1].Delete(); });
            excelSheetData.Columns.Cast<DataColumn>().ToList().ForEach(column =>
            {
                if (propertyMapping.Keys.Contains(column.ColumnName))
                {
                    column.ColumnName = propertyMapping[column.ColumnName];
                    // SqlDbType sqlType = (SqlDbType)Enum.Parse(typeof(SqlDbType), propertyTypeMapping[column.ColumnName], true);
                    // column.DataType = columnTypeMap[sqlType];
                }
                else
                {
                    excelSheetData.Columns.Remove(column);
                }
            });
            excelSheetData.AcceptChanges();

            return excelSheetData;
        }

        public IEnumerable<T> ReadExcelUsingEPPlus<T>(ExcelWorkbook eppWorkbook, string tabName) where T : ExcelModelBase
        {
            ExcelWorksheet eppSheet;
            ExcelRange eppRow;
            ExcelRange eppCell;

            int endRow;
            int endColumn;
            int headerRow = 1;
            int dataStartRow = 2;
            int dataStartColumn = 1;
            string colName;

            List<T> excelSheetData = new List<T>();

            eppSheet = eppWorkbook.Worksheets[tabName];
            endRow = eppSheet.Dimension.End.Row;
            endColumn = eppSheet.Dimension.End.Column;

            var findRow = from cell in eppSheet.Cells[AppSettings.ExcelConfigEndKeyRow]
                          where cell.Value is string
                          && cell.Value as string == AppSettings.ExcelConfigEndKey
                          select cell;
            if (findRow.Count<ExcelAddress>() != 0)
            {
                endRow = findRow.Last().End.Row - 1;
            }

            var findColumn = from cell in eppSheet.Cells[string.Format(AppSettings.ExcelConfigEndKeyColumn, headerRow)]
                             where cell.Value is string
                             && cell.Value as string == AppSettings.ExcelConfigEndKey
                             select cell;

            if (findColumn.Count<ExcelAddress>() != 0)
            {
                endColumn = findColumn.Last().End.Column - 1;
            }

            for (int i = dataStartRow; i <= endRow; i++)
            {
                T dataRow = Activator.CreateInstance<T>();
                eppRow = eppSheet.Cells[i, dataStartColumn, i, endColumn];

                for (int j = dataStartColumn; j <= endColumn; j++)
                {
                    colName = eppRow[headerRow, j].Value.ToString().Trim();
                    eppCell = eppRow[i, j];

                    if (eppCell.Value != null)
                    {
                        //dataRow[colName] = eppCell.Value;
                        switch (Type.GetTypeCode(typeof(T).GetProperty(colName).PropertyType))
                        {
                            case TypeCode.String:
                                dataRow[colName] = eppCell.Value.ToString();
                                break;
                            case TypeCode.Int32:
                                dataRow[colName] = int.Parse(eppCell.Value.ToString());
                                break;
                            case TypeCode.Int64:
                                dataRow[colName] = int.Parse(eppCell.Value.ToString());
                                break;
                            case TypeCode.Double:
                                dataRow[colName] = double.Parse(eppCell.Value.ToString());
                                break;
                            case TypeCode.DateTime:
                                dataRow[colName] = DateTime.FromOADate((double)eppCell.Value);
                                break;
                            case TypeCode.Boolean:
                                dataRow[colName] = eppCell.Value.ToString().Equals("Y") ? true : false;
                                break;
                            default:
                                dataRow[colName] = eppCell.Value;
                                break;
                        }
                    }
                }

                excelSheetData.Add(dataRow);
            }

            return excelSheetData;
        }

        public void ExportToExcel<T>(IList<T> dataSource, string fileName)
        {
            using (var excel = new ExcelPackage())
            {
                var sheet = excel.Workbook.Worksheets.Add("Test");
                sheet.Cells["A1"].LoadFromCollection<T>(dataSource, true);
                excel.SaveAs(new FileInfo(fileName));
            }
        }

        public void ProcessAccountRevLossWorksheet(DataTable dt, string vertical, string country, ExcelPackage pack)
        {
            if (country == "Canada")
            {
                var ws = pack.Workbook.Worksheets[vertical + " Rev Loss Summary"];
                ws.Cells["A20:W20"].Merge = true;
                ws.Cells["A20:W20"].Value = "Canada Region";
                ws.Cells["A20:W20"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["A20:W20"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E26B0A"));
                ws.Cells["A20:W20"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["A20:W20"].Style.Font.Bold = false;
                ws.Cells["A20:W20"].Style.Font.Name = "Calibri";
                ws.Cells["A20:W20"].Style.Font.Size = 9;
                ws.Cells["A20:W20"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                var headerRange = ws.Cells["A21:W22"];
                var range = ws.Cells["A23:W34"];
                GenerateAccountRevLossHeader(headerRange, country);
                ApplyHeaderStyle(ws.Cells["A21:W22"]);
                GenerateData(range, dt);
                ApplyHeaderStyle(ws.Cells["A23:A36"]);
                ws.Cells["B23:W36"].Style.Numberformat.Format = STR_ACCOUNTING_FORMAT;

                GenerateTotal(ws.Cells["B36:W36"], ws.Cells["B23:W34"], ws);
                ws.Cells["A36"].Value = "Grand Total";
                ws.Cells["A36"].Style.Font.Size = 11;

                ApplyFooterStyle(ws.Cells["B36:C36"]);
                ApplyHeaderStyle(ws.Cells["D36:F36"]);
                ws.Cells["D36:F36"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["G36:H36"]);
                ApplyHeaderStyle(ws.Cells["I36:K36"]);
                ws.Cells["I36:K36"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["L36:M36"]);
                ApplyHeaderStyle(ws.Cells["N36:P36"]);
                ws.Cells["N36:P36"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["Q36:R36"]);
                ApplyHeaderStyle(ws.Cells["S36:U36"]);
                ws.Cells["S36:U36"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["V36:W36"]);
                ws.Cells["B23:B35"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["B23:B35"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6E0B4"));
                ws.Cells["G23:H35"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["G23:H35"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["L23:M35"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["L23:M35"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["Q23:R35"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["Q23:R35"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["V23:W35"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["V23:W35"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["A20:W34"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["A20:W35"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["A20:W35"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["A20:W35"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["B23:W35"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["A23:A34"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["C23:W35"].Style.Font.Bold = false;
            }
            else if (country == "US")
            {
                var ws = pack.Workbook.Worksheets[vertical + " Rev Loss Summary"];
                ws.Cells["A38:R38"].Merge = true;
                ws.Cells["A38:R38"].Value = "North America Region";
                ws.Cells["A38:R38"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["A38:R38"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E26B0A"));
                ws.Cells["A38:R38"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["A38:R38"].Style.Font.Bold = false;
                ws.Cells["A38:R38"].Style.Font.Name = "Calibri";
                ws.Cells["A38:R38"].Style.Font.Size = 9;
                ws.Cells["A38:R38"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                var headerRange = ws.Cells["A39:R40"];
                var range = ws.Cells["A41:R52"];
                GenerateAccountRevLossHeader(headerRange, country);
                ApplyHeaderStyle(ws.Cells["A39:R40"]);
                GenerateData(range, dt);
                ApplyHeaderStyle(ws.Cells["A41:A54"]);
                ws.Cells["B41:R54"].Style.Numberformat.Format = STR_ACCOUNTING_FORMAT;

                GenerateTotal(ws.Cells["B54:R54"], ws.Cells["B41:R52"], ws);
                ws.Cells["A54"].Value = "Grand Total";
                ws.Cells["A54"].Style.Font.Size = 11;

                ApplyFooterStyle(ws.Cells["B54"]);
                ApplyHeaderStyle(ws.Cells["C54:E54"]);
                ws.Cells["C54:E54"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["F54"]);
                ApplyHeaderStyle(ws.Cells["G54:I54"]);
                ws.Cells["G54:I54"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["J54"]);
                ApplyHeaderStyle(ws.Cells["K54:M54"]);
                ws.Cells["K54:M54"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["N54"]);
                ApplyHeaderStyle(ws.Cells["O54:Q54"]);
                ws.Cells["O54:Q54"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["R54"]);
                ws.Cells["B41:B53"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["B41:B53"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6E0B4"));
                ws.Cells["F41:F53"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["F41:F53"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["J41:J53"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["J41:J53"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["N41:N53"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["N41:N53"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["N41:N53"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["R41:R53"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["R41:R53"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));

                ws.Cells["A38:R53"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["A38:R53"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["A38:R53"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["A38:R53"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["B41:R53"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["A41:A54"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["C41:R52"].Style.Font.Bold = false;

            }
            else if (country == "UK")
            {
                var ws = pack.Workbook.Worksheets[vertical + " Rev Loss Summary"];
                ws.Cells["A103:W103"].Merge = true;
                ws.Cells["A103:W103"].Value = "Europe";
                ws.Cells["A103:W103"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["A103:W103"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E26B0A"));
                ws.Cells["A103:W103"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["A103:W103"].Style.Font.Bold = false;
                ws.Cells["A103:W103"].Style.Font.Name = "Calibri";
                ws.Cells["A103:W103"].Style.Font.Size = 9;
                for (int i = 56; i <= 102; i++)
                {
                    ws.Row(i).Hidden = true;
                }
                var headerRange = ws.Cells["A104:W105"];
                var range = ws.Cells["A106:W117"];
                GenerateAccountRevLossHeader(headerRange, country);
                ApplyHeaderStyle(ws.Cells["A104:W105"]);
                GenerateData(range, dt);
                ApplyHeaderStyle(ws.Cells["A106:A119"]);
                ws.Cells["B106:W119"].Style.Numberformat.Format = STR_ACCOUNTING_FORMAT;

                GenerateTotal(ws.Cells["B119:W119"], ws.Cells["B106:W117"], ws);
                ws.Cells["A119"].Value = "Grand Total";
                ws.Cells["A119"].Style.Font.Size = 11;
                ApplyFooterStyle(ws.Cells["B119:C119"]);
                ApplyHeaderStyle(ws.Cells["D119:F119"]);
                ws.Cells["D119:F119"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["G119:H119"]);
                ApplyHeaderStyle(ws.Cells["I119:K119"]);
                ws.Cells["I119:K119"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["L119:M119"]);
                ApplyHeaderStyle(ws.Cells["N119:P119"]);
                ws.Cells["N119:P119"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["Q119:R119"]);
                ApplyHeaderStyle(ws.Cells["S119:U119"]);
                ws.Cells["S119:U119"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["V119:W119"]);
                ws.Cells["B106:C118"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["B106:C118"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6E0B4"));
                ws.Cells["G106:H118"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["G106:H118"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["L106:M118"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["L106:M118"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["Q106:R118"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["Q106:R118"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["V106:W118"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["V106:W118"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D6DCE4"));
                ws.Cells["A103:W118"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["A103:W118"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["A103:W118"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["A103:W118"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["B106:W118"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["A106:A117"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["C106:W117"].Style.Font.Bold = false;
            }
        }

        public void ProcessOverallRevLossWorksheet(DataTable dt, string vertical, ExcelPackage pack)
        {
            if (vertical == null)
            {
                var ws = pack.Workbook.Worksheets["Summary"];
                ws.Cells["B21:H21"].Merge = true;
                ws.Cells["B21:H21"].Value = "Revenue Loss Summary";
                ws.Cells["B21:H21"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B21:H22"].Style.Font.Bold = true;

                ws.Cells["B21:H22"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["B21:H22"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#BDD7EE"));
                ws.Cells["C22"].Value = "As On Date";
                ws.Cells["D22:H22"].Merge = true;
                ws.Cells["D22:H22"].Value = "Projected (inclusive of Revenue loss as of date numbers";
                ws.Cells["C22:H22"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var headerRange = ws.Cells["B23:H23"];
                var range = ws.Cells["B24:H35"];
                GenerateOverallRevLossHeader(headerRange, vertical);
                GenerateData(range, dt);
                ws.Cells["C24:H37"].Style.Numberformat.Format = STR_ACCOUNTING_FORMAT;

                GenerateTotal(ws.Cells["C37:H37"], ws.Cells["C24:H35"], ws);
                ws.Cells["B37"].Value = "Total";
                ApplyHeaderStyle(ws.Cells["B24:B37"]);
                ws.Cells["B24:B37"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["B24:B37"].Style.Font.Size = 10;
                ws.Cells["C37:H37"].Style.Font.Size = 10;
                ws.Cells["C37:H37"].Style.Font.Bold = true;
                ApplyFooterStyle(ws.Cells["C37:H37"]);

                ws.Cells["C24:C36"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["C24:C36"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6E0B4"));
                ws.Cells["H24:H36"].ConditionalFormatting.AddDatabar(Color.Red);
                ws.Cells["B21:H37"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["B21:H37"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["B21:H37"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["B21:H37"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["C24:H37"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["C24:H35"].Style.Font.Bold = false;
                for (int i = 2; i <= 12; i++)
                {
                    ws.Column(i).Width = 20;
                }
            }
            else
            {
                var ws = pack.Workbook.Worksheets[vertical + " Rev Loss Summary"];
                ws.TabColor = System.Drawing.ColorTranslator.FromHtml("#548235");
                ws.Cells["A1:B1"].Merge = true;
                ws.Cells["A1:B1"].Value = "Revenue Loss for Ultra Critical Requirements";
                ws.Cells["A1:B1"].Style.Font.Bold = true;
                ws.Cells["A2:G2"].Merge = true;
                ws.Cells["A2:G2"].Value = "Overall Revenue Loss (Canada+others)";
                ws.Cells["A2:G2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["A2:G2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E26B0A"));
                ws.Cells["A2:G2"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["A2:G2"].Style.Font.Bold = false;
                ws.Cells["A1:G2"].Style.Font.Name = "Calibri";
                ws.Cells["A1:G2"].Style.Font.Size = 9;
                ws.Cells["A1:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                var headerRange = ws.Cells["A3:G4"];
                var range = ws.Cells["A5:G16"];
                GenerateOverallRevLossHeader(headerRange, vertical);
                ApplyHeaderStyle(ws.Cells["A3:G4"]);
                GenerateData(range, dt);
                ApplyHeaderStyle(ws.Cells["A4:A18"]);
                ws.Cells["A4:A18"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["B5:G18"].Style.Numberformat.Format = STR_ACCOUNTING_FORMAT;

                GenerateTotal(ws.Cells["B18:G18"], ws.Cells["B5:G16"], ws);
                ws.Cells["A18"].Value = "Grand Total";
                ws.Cells["A18"].Style.Font.Size = 10;
                ApplyFooterStyle(ws.Cells["B18:G18"]);
                ws.Cells["B5:B17"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["B5:B17"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6E0B4"));
                ws.Cells["G5:G16"].ConditionalFormatting.AddDatabar(Color.Red);
                for (int i = 1; i <= 23; i++)
                {
                    ws.Column(i).Width = 20;
                }
                ws.Cells["B5:G16"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["B5:G17"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["B5:G17"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["B5:G17"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["B5:G17"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["C5:G16"].Style.Font.Bold = false;
            }
        }

        public void ProcessOverallPositionWorksheet(DataTable dt, string level, string vertical, ExcelPackage pack)
        {
            if ((level == "Horizontal") && (vertical == null))
            {
                var ws = pack.Workbook.Worksheets["Summary"];
                ws.Cells["A1:L2"].Merge = true;
                ws.Cells["A1:L2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:L2"].Value = "(Canada + North America+Europe) for TD, RBC, Scotia, CIBC & BMO";
                ws.Cells["A1:L2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A1:L2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["A1:L2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#BDD7EE"));
                ws.Cells["A1:L2"].Style.Font.Name = "Calibri";
                ws.Cells["A1:L2"].Style.Font.Bold = true;
                ws.Cells["A1:L2"].Style.Font.Size = 14;
                ws.Cells["A1:L2"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["A1:L2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["A1:L2"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["A1:L2"].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                ws.Cells["B4:F4"].Merge = true;
                ws.Cells["B4:F4"].Value = "Overall Open Requirements Summary for " + RevLossConstants.CurrentAssesmentYear + " (Includes future Pipeline requirements)";
                ws.Cells["B4:F4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                ws.Cells["B4:F4"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#BDD7EE"));
                ws.Cells["B4:F4"].Style.Font.Name = "Calibri";
                ws.Cells["B4:F4"].Style.Font.Size = 14;
                ws.Cells["B4:F4"].Style.Font.Bold = true;
                ws.Cells["B4:F4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:F4"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["B4:F4"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["B4:F4"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["B4:F4"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //sub header
                ws.Cells["B5"].Value = "Horizontals";
                ws.Cells["C5"].Value = "Onsite Positions";
                ws.Cells["D5"].Value = "Offshore Positions";
                ws.Cells["E5"].Value = "Total Positions (Includes Lost Positions)";
                ws.Cells["F5"].Value = "Comments***";
                ws.Cells["B5:F5"].Style.WrapText = true;
                ws.Cells["B5:F5"].Style.Font.Bold = true;
                ws.Cells["B5:F5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B5:F5"].Style.Font.Name = "Calibri";
                ws.Cells["B5:F5"].Style.Font.Size = 10;

                ws.Cells["B5:F5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["B5:F5"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#5B9BD5"));
                ws.Cells["B5:F5"].Style.Font.Color.SetColor(Color.White);

                var range = ws.Cells["B6:F18"];
                GenerateData(range, dt);
                var cf = ws.ConditionalFormatting.AddGreaterThan(ws.Cells["C6:D17"]);
                cf.Formula = "0";

                cf.Style.Fill.BackgroundColor.Color = Color.Gray;
                GenerateTotal(ws.Cells["C19:E19"], ws.Cells["C6:E17"], ws);
                ws.Cells["B19"].Value = "Total";
                ApplyFooterStyle(ws.Cells["C19:E19"]);
                ws.Cells["B19:E19"].Style.Font.Size = 10;
                ws.Cells["F6:F18"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["F6:F18"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F8CBAD"));
                ws.Cells["F6:F18"].Style.Font.Name = "Calibri";

                ws.Cells["B5:F18"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["B5:F18"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["B5:F18"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["B5:F18"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["C6:F18"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["E6:E17"].ConditionalFormatting.AddDatabar(Color.Red);
                ApplyHeaderStyle(ws.Cells["B6:B19"]);
                ws.Cells["B6:B19"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["B6:B19"].Style.Font.Size = 10;
            }
            else if ((level == "Vertical") && (vertical == null))
            {
                var ws = pack.Workbook.Worksheets["Summary"];
                //sub header
                ws.Cells["H9"].Value = "Account";
                ws.Cells["I9"].Value = "Onsite Positions";
                ws.Cells["J9"].Value = "Offshore Positions";
                ws.Cells["K9"].Value = "Total Positions (Includes Lost Positions)";
                ws.Cells["L9"].Value = "Comments***";
                ws.Cells["H9:L9"].Style.WrapText = true;
                ws.Cells["H9:L9"].Style.Font.Bold = true;
                ws.Cells["H9:L9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["H9:L9"].Style.Font.Name = "Calibri";
                ws.Cells["H9:L9"].Style.Font.Size = 10;

                ws.Cells["H9:L9"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["H9:L9"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#5B9BD5"));
                ws.Cells["H9:L9"].Style.Font.Color.SetColor(Color.White);
                ApplyHeaderStyle(ws.Cells["H10:H18"]);
                ws.Cells["H10:H18"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["H10:H18"].Style.Font.Size = 10;
                ws.Cells["L10:L17"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["L10:L17"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F8CBAD"));
                ws.Cells["L10:L17"].Style.Font.Name = "Calibri";
                //Data from Datatable
                var range = ws.Cells["H10:L17"];
                GenerateData(range, dt);

                ws.Cells["H9:L17"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["H9:L17"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["H9:L17"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["H9:L17"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["I10:L17"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["H18"].Value = "Total";
                ws.Cells["H18"].Style.Font.Size = 10;
                GenerateTotal(ws.Cells["I18:K18"], ws.Cells["I10:K17"], ws);
                ApplyFooterStyle(ws.Cells["I18:K18"]);

            }
            else if ((level == "Horizontal") && (vertical != null))
            {
                var ws = pack.Workbook.Worksheets[vertical + " Rev Loss Summary"];
                ws.Cells["I3"].Value = "Organization";
                ws.Cells["J3"].Value = "Offshore";
                ws.Cells["K3"].Value = "Onsite";
                ws.Cells["L3"].Value = "Grand Total";
                ws.Cells["I2:L2"].Merge = true;
                ws.Cells["I2:L2"].Value = "Resource Count (Canada+others)";
                ws.Cells["I2:L2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["I2:L2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#E26B0A"));
                ws.Cells["I2:L2"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["I2:L2"].Style.Font.Size = 9;
                ws.Cells["I2:L2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["I2:L2"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["I2:L2"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["I2:L2"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["I2:L2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ApplyHeaderStyle(ws.Cells["I3:L3"]);

                DataTable data = new DataTable();
                data.Columns.Add("Level");
                data.Columns.Add("OffshorePositions", typeof(int));
                data.Columns.Add("OnsitePositions", typeof(int));
                data.Columns.Add("TotalPositions", typeof(int));

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow row = data.NewRow();
                    row[0] = dr["Level"].ToString();
                    row[1] = Convert.ToInt32(dr["OffshorePositions"]);
                    row[2] = Convert.ToInt32(dr["OnsitePositions"]);
                    row[3] = Convert.ToInt32(dr["TotalPositions"]);

                    data.Rows.Add(row);

                }
                var range = ws.Cells["I4:L15"];
                GenerateData(range, data);
                GenerateTotal(ws.Cells["J16:L16"], ws.Cells["J4:L15"], ws);
                ApplyHeaderStyle(ws.Cells["I4:I16"]);
                ws.Cells["I4:I15"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["I16"].Value = "Grand Total";
                ws.Cells["I16"].Style.Font.Size = 11;
                ApplyFooterStyle(ws.Cells["J16:L16"]);
                ws.Cells["J4:L15"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["J4:L15"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["J4:L15"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["J4:L15"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["J4:L15"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells["J4:L15"].Style.Font.Name = "Calibri";
                ws.Cells["J4:L15"].Style.Font.Size = 11;
            }
        }

        public void ProcessAccountPositionsWorksheet(DataTable dt, string vertical, string country, ExcelPackage pack)
        {
            ExcelWorksheet ws;
            if (country != null)
            {
                ws = pack.Workbook.Worksheets[vertical + " " + country];
            }
            else
            {
                ws = pack.Workbook.Worksheets[vertical];
            }

            ws.Row(2).Height = 50;
            ws.TabColor = System.Drawing.ColorTranslator.FromHtml("#2F75B5");
            ws.Cells["A2:AG2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A2"].Value = "Horizontal";
            ws.Cells["B2"].Value = "Project Name";
            ws.Cells["C2"].Value = "Role";
            ws.Cells["D2"].Value = "Replacement? - Y/N";
            ws.Cells["E2"].Value = "Skill Set";
            ws.Cells["F2"].Value = "Number of Requirements";
            ws.Cells["G2"].Value = "No of Years of Experience";
            ws.Cells["H2"].Value = "Tentative Start Date of assignment";
            ws.Cells["I2"].Value = "Duration of assignment (in Months)";
            ws.Cells["J2"].Value = "Potential Bill Rate";
            ws.Cells["K2"].Value = "Fixed Price/T&M Assignment?";
            ws.Cells["L2"].Value = "Critical? – Y/N";
            ws.Cells["M2"].Value = "Client Interview? – Y/N";
            ws.Cells["N2"].Value = "Request Date";
            ws.Cells["O2"].Value = "Age";
            ws.Cells["P2"].Value = "SO number";
            ws.Cells["Q2"].Value = "Onsite/Offshore";
            ws.Cells["R2"].Value = "Location";
            ws.Cells["S2"].Value = "Marked for Hiring";
            ws.Cells["T2"].Value = "Comments";
            ws.Cells["U2"].Value = "Revenue Loss As on Date";
            ws.Cells["V2"].Value = "Jan";
            ws.Cells["W2"].Value = "Feb";
            ws.Cells["X2"].Value = "Mar";
            ws.Cells["Y2"].Value = "Apr";
            ws.Cells["Z2"].Value = "May";
            ws.Cells["AA2"].Value = "Jun";
            ws.Cells["AB2"].Value = "Jul";
            ws.Cells["AC2"].Value = "Aug";
            ws.Cells["AD2"].Value = "Sep";
            ws.Cells["AE2"].Value = "Oct";
            ws.Cells["AF2"].Value = "Nov";
            ws.Cells["AG2"].Value = "Dec";
            ws.Cells["A2:AG2"].AutoFitColumns();
            //data

            int rowindex = 3;
            int columnindex = 1;

            foreach (DataRow row in dt.Rows)
            {
                ws.Cells[rowindex, columnindex].Value = row["Horizontal"].ToString(); columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["ProjectName"].ToString(); columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["Role"].ToString(); columnindex++;
                if (Convert.ToBoolean(row["IsReplacement"].ToString()) == true)
                    ws.Cells[rowindex, columnindex].Value = "Y";
                else
                    ws.Cells[rowindex, columnindex].Value = "N";
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["SkillSet"].ToString(); columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToInt32(row["NumberOfRequirements"].ToString()); columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToInt32(row["YearsOfExperience"].ToString()); columnindex++;

                if (row["ProjectedAssignmentStartDate"].ToString() != string.Empty)
                {

                    ws.Cells[rowindex, columnindex].Value = Convert.ToDateTime(row["ProjectedAssignmentStartDate"]).ToString("dd-MM-yy");

                }
                else
                {
                    ws.Cells[rowindex, columnindex].Value = string.Empty;
                }
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToInt32(row["DurationOfAssignment"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToInt32(row["PotentialBillRate"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["TypeOfBilling"].ToString(); columnindex++;
                if (Convert.ToBoolean(row["IsCritical"].ToString()) == true)
                    ws.Cells[rowindex, columnindex].Value = "Y";
                else
                    ws.Cells[rowindex, columnindex].Value = "N";
                columnindex++;
                if (Convert.ToBoolean(row["HasClientInterview"].ToString()) == true)
                    ws.Cells[rowindex, columnindex].Value = "Y";
                else
                    ws.Cells[rowindex, columnindex].Value = "N";
                columnindex++;

                if (row["RequestDate"].ToString() != string.Empty)
                {

                    ws.Cells[rowindex, columnindex].Value = Convert.ToDateTime(row["RequestDate"]).ToString("dd-MM-yy");
                }
                else
                {
                    ws.Cells[rowindex, columnindex].Value = string.Empty;
                }
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["Age"].ToString(); columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["SONumber"].ToString(); columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["Location"].ToString();
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["City"].ToString(); columnindex++;
                if (Convert.ToBoolean(row["IsMarkedForHiring"].ToString()) == true)
                    ws.Cells[rowindex, columnindex].Value = "Y";
                else
                    ws.Cells[rowindex, columnindex].Value = "N";
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = row["Comments"].ToString(); columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["RevenueLossAsonDate"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Jan"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Feb"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Mar"].ToString());
                ws.Cells[rowindex, columnindex].Style.Numberformat.Format = "$    #,##0.00";
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Apr"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["May"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Jun"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Jul"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Aug"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Sep"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Oct"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Nov"].ToString());
                columnindex++;
                ws.Cells[rowindex, columnindex].Value = Convert.ToUInt32(row["Dec"].ToString());

                ws.Cells[rowindex, 21, rowindex, 33].Style.Numberformat.Format = STR_ACCOUNTING_FORMAT;
                ws.Cells[rowindex, 21, rowindex, 21].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[rowindex, 21, rowindex, 21].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6E0B4"));
                ws.Cells[rowindex, 21, rowindex, 21].Style.Font.Bold = true;
                ws.Cells[rowindex, 21, rowindex, 21].Style.Font.Size = 9;
                ws.Cells[rowindex, 21, rowindex, 21].Style.Font.Name = "Calibri";
                ws.Cells[rowindex, 21, rowindex, 33].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[rowindex, 22, rowindex, 33].Style.Font.Bold = false;
                ws.Cells[rowindex, 22, rowindex, 33].Style.Font.Size = 9;
                ws.Cells[rowindex, 22, rowindex, 33].Style.Font.Name = "Calibri";
                ws.Cells[rowindex, 1, rowindex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[rowindex, 4, rowindex, 19].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowindex, 20, rowindex, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[rowindex, 1, rowindex, 20].Style.Font.Name = "Calibri";
                ws.Cells[rowindex, 1, rowindex, 20].Style.Font.Size = 9;
                ws.Cells[rowindex, 1, rowindex, 20].Style.Font.Bold = false;
                rowindex++;
                columnindex = 1;
            }
            ws.Cells[2, 1, 2, 33].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[2, 1, 2, 33].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#757171"));
            ws.Cells[2, 1, 2, 33].Style.Font.Bold = true;
            ws.Cells[2, 1, 2, 33].Style.Font.Size = 9;
            ws.Cells[2, 1, 2, 33].Style.Font.Name = "Calibri";
            ws.Cells[2, 1, rowindex - 1, 33].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 1, rowindex - 1, 33].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 1, rowindex - 1, 33].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 1, rowindex - 1, 33].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Column(1).Width = 12.43;
            ws.Column(2).Width = 17.57;
            ws.Column(3).Width = 21.71;
            ws.Column(4).Width = 16;
            ws.Column(5).Width = 39.43;
            ws.Column(6).Width = 16;
            ws.Column(7).Width = 16;
            ws.Column(8).Width = 15;
            ws.Column(9).Width = 20.29;
            ws.Column(10).Width = 20;
            ws.Column(11).Width = 12.86;
            ws.Column(12).Width = 8.29;
            ws.Column(13).Width = 12.86;
            ws.Column(14).Width = 12.86;
            ws.Column(15).Width = 8.43;
            ws.Column(16).Width = 10.29;
            ws.Column(17).Width = 15;
            ws.Column(18).Width = 12;
            ws.Column(19).Width = 8.43;
            ws.Column(20).Width = 37.86;
            ws.Column(21).Width = 37.86;
            ws.Column(22).Width = 13.71;
            ws.Column(23).Width = 13.71;
            ws.Column(24).Width = 13.71;
            ws.Column(25).Width = 13.71;
            ws.Column(26).Width = 13.71;
            ws.Column(27).Width = 13.71;
            ws.Column(28).Width = 13.71;
            ws.Column(29).Width = 13.71;
            ws.Column(30).Width = 13.71;
            ws.Column(31).Width = 13.71;
            ws.Column(32).Width = 13.71;
            ws.Column(33).Width = 13.71;
        }

        private void GenerateTotal(ExcelRange destRange, ExcelRange sourceRange, ExcelWorksheet sheet)
        {
            int rowStartindex = sourceRange.Start.Row;
            int columnStartindex = sourceRange.Start.Column;
            int rowEndindex = sourceRange.End.Row;
            int columnEndindex = sourceRange.End.Column;

            int destrowStartindex = destRange.Start.Row;
            int destcolumnStartindex = destRange.Start.Column;
            int destcolumnEndindex = destRange.End.Column;

            for (int i = destcolumnStartindex; i <= destcolumnEndindex; i++)
            {
                string formula = string.Empty;
                var range1 = sourceRange[rowStartindex, i, rowStartindex, i];

                var range2 = sourceRange[rowEndindex, i, rowEndindex, i];

                destRange[destrowStartindex, i].Formula = String.Format("SUM({0}:{1})", sheet.Cells[rowStartindex, i], sheet.Cells[rowEndindex, i]);
                columnStartindex++;
            }
        }

        private void ApplyHeaderStyle(ExcelRange headerRange)
        {
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCFF"));
            headerRange.Style.Font.Name = "Calibri";
            headerRange.Style.Font.Size = 9;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.WrapText = true;
            headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private void ApplyFooterStyle(ExcelRange excelRange)
        {
            excelRange.Style.Font.Name = "Calibri";
            excelRange.Style.Font.Size = 10;
            excelRange.Style.Font.Bold = true;
            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#993300"));
            excelRange.Style.Font.Color.SetColor(Color.Yellow);
            excelRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            excelRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            excelRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private void GenerateData(ExcelRange range, DataTable dt)
        {
            range.LoadFromDataTable(dt, false);
        }

        private void GenerateOverallRevLossHeader(ExcelRange headerRange, string vertical)
        {
            if (vertical == null)
            {
                int rowindex = headerRange.Start.Row;
                int columnindex = headerRange.Start.Column;
                headerRange[rowindex, columnindex].Value = "Horizontals"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Revenue Loss As of Date"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q1"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q2"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q3"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q4"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Total Revenue Lost for " + RevLossConstants.CurrentAssesmentYear + " (USD)";
                headerRange[rowindex, 2, rowindex, columnindex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange[rowindex, 2, rowindex, columnindex].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#5B9BD5"));
                headerRange[rowindex, 2, rowindex, columnindex].Style.Font.Color.SetColor(Color.White);
                headerRange[rowindex, 2, rowindex, columnindex].Style.Font.Name = "Calibri";
                headerRange[rowindex, 2, rowindex, columnindex].Style.Font.Size = 10;
                headerRange[rowindex, 2, rowindex, columnindex].Style.Font.Bold = true;
                headerRange[rowindex, 2, rowindex, columnindex].Style.WrapText = true;
                headerRange[rowindex, 2, rowindex, columnindex].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                headerRange[rowindex, 2, rowindex, columnindex].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                headerRange[rowindex, 2, rowindex, columnindex].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                headerRange[rowindex, 2, rowindex, columnindex].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                headerRange[rowindex, 2, rowindex, columnindex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            else if (vertical != null)
            {
                int rowindex = headerRange.Start.Row;
                int columnindex = headerRange.Start.Column;
                headerRange[rowindex, columnindex].Value = "Organization"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Revenue Loss As of Date"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q1"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q2"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q3"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q4"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Total Projected Revenue Lost";
                rowindex++;

                headerRange[rowindex, 2, rowindex, columnindex].Merge = true;
                headerRange[rowindex, 2, rowindex, columnindex].Value = "USD";
            }
        }

        private void GenerateAccountRevLossHeader(ExcelRange headerRange, string country)
        {
            if ((country == "Canada") || (country == "UK"))
            {
                int rowindex = headerRange.Start.Row;
                int columnindex = headerRange.Start.Column;
                int endindex = headerRange.End.Column;
                headerRange[rowindex, columnindex].Value = "Organization"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Revenue Loss As of Date"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Revenue Loss As of Date"; columnindex++;
                headerRange[rowindex, columnindex].Value = "January"; columnindex++;
                headerRange[rowindex, columnindex].Value = "February"; columnindex++;
                headerRange[rowindex, columnindex].Value = "March"; columnindex++;
                headerRange[rowindex, columnindex, rowindex, columnindex + 1].Merge = true;
                headerRange[rowindex, columnindex, rowindex, columnindex + 1].Value = "Q1 Actual $ Lost"; columnindex += 2;
                headerRange[rowindex, columnindex].Value = "April"; columnindex++;
                headerRange[rowindex, columnindex].Value = "May"; columnindex++;
                headerRange[rowindex, columnindex].Value = "June"; columnindex++;
                headerRange[rowindex, columnindex, rowindex, columnindex + 1].Merge = true;
                headerRange[rowindex, columnindex, rowindex, columnindex + 1].Value = "Q2 Actual $ Lost"; columnindex += 2;
                headerRange[rowindex, columnindex].Value = "July"; columnindex++;
                headerRange[rowindex, columnindex].Value = "August"; columnindex++;
                headerRange[rowindex, columnindex].Value = "September"; columnindex++;
                headerRange[rowindex, columnindex, rowindex, columnindex + 1].Merge = true;
                headerRange[rowindex, columnindex, rowindex, columnindex + 1].Value = "Q3 Actual $ Lost"; columnindex += 2;
                headerRange[rowindex, columnindex].Value = "October"; columnindex++;
                headerRange[rowindex, columnindex].Value = "November"; columnindex++;
                headerRange[rowindex, columnindex].Value = "December"; columnindex++;
                headerRange[rowindex, columnindex, rowindex, columnindex + 1].Merge = true;
                headerRange[rowindex, columnindex, rowindex, columnindex + 1].Value = "Q4 Actual $ Lost";

                headerRange.Style.WrapText = true;
                rowindex++;
                columnindex = 2;
                for (int i = columnindex; i <= endindex; i += 4)
                {
                    if (country == "Canada")
                    {
                        headerRange[rowindex, i].Value = "CAD";
                    }
                    else
                    {
                        headerRange[rowindex, i].Value = "GBP";
                    }
                    i++;
                    headerRange[rowindex, i].Value = "USD";
                }

            }
            else if (country == "US")
            {
                int rowindex = headerRange.Start.Row;
                int columnindex = headerRange.Start.Column;
                int endindex = headerRange.End.Column;
                headerRange[rowindex, columnindex].Value = "Organization"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Revenue Loss As of Date"; columnindex++;
                headerRange[rowindex, columnindex].Value = "January"; columnindex++;
                headerRange[rowindex, columnindex].Value = "February"; columnindex++;
                headerRange[rowindex, columnindex].Value = "March"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q1 Actual $ Lost"; columnindex++;
                headerRange[rowindex, columnindex].Value = "April"; columnindex++;
                headerRange[rowindex, columnindex].Value = "May"; columnindex++;
                headerRange[rowindex, columnindex].Value = "June"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q2 Actual $ Lost"; columnindex++;
                headerRange[rowindex, columnindex].Value = "July"; columnindex++;
                headerRange[rowindex, columnindex].Value = "August"; columnindex++;
                headerRange[rowindex, columnindex].Value = "September"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q3 Actual $ Lost"; columnindex++;
                headerRange[rowindex, columnindex].Value = "October"; columnindex++;
                headerRange[rowindex, columnindex].Value = "November"; columnindex++;
                headerRange[rowindex, columnindex].Value = "December"; columnindex++;
                headerRange[rowindex, columnindex].Value = "Q4 Actual $ Lost";
                rowindex++;
                columnindex = 2;
                for (int i = columnindex; i <= endindex; i += 4)
                {
                    headerRange[rowindex, i].Value = "USD";
                }
            }
        }
    }
}
