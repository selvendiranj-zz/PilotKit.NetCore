using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Infrastructure.CrossCutting.Constants
{
    public class AppSettings
    {
        public static string RepoRoot { get; set; }
        public static string PackageLocalPath { get; set; }
        public static string PackageSearchPattern { get; set; }
        public static string StaticViewPath { get; set; }
        public static string ExcelConfigEndKey { get; set; }
        public static string ExcelConfigEndKeyRow { get; set; }
        public static string ExcelConfigEndKeyColumn { get; set; }
        public static string ExcelStageLocation { get; set; }
        public static string RevenueLossReportPath { get; set; }
        public static string RevenueLossReportFileName { get; set; }
    }
}
