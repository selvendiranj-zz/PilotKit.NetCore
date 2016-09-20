using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using PilotKit.Infrastructure.CrossCutting.Constants;
using PilotKit.Orchestration.Interfaces.RevenueLoss;
using PilotKit.Orchestration.Interfaces.Common;
using PilotKit.Domain.Model.RevenueLoss;
using System.IO;
using Microsoft.AspNetCore.Razor.CodeGenerators;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using PilotKit.Infrastructure.CrossCutting.Extensions;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PilotKit.Web.ApiControllers
{
    [Route("api/[controller]")]
    public class RevenueLossController : Controller
    {
        private AppSettings appSettings;
        private IHostingEnvironment hostingEnv;
        private IRevenueLossOrchestrator orchestrator;

        public RevenueLossController(IOptions<AppSettings> appSettings, IHostingEnvironment hostingEnv,
                                     IRevenueLossOrchestrator orchestrator)
        {
            this.appSettings = appSettings.Value;
            this.hostingEnv = hostingEnv;
            this.orchestrator = orchestrator;
        }

        // GET: /api/RevenueLoss/GetAssessmentYear
        [HttpGet("/api/revenueloss/GetAssessmentYear")]
        public IActionResult GetAssessmentYear()
        {
            return Json(RevLossConstants.CurrentAssesmentYear);
        }

        // POST: /api/RevenueLoss/SetAssessmentYear
        [HttpPost("/api/RevenueLoss/SetAssessmentYear")]
        public IActionResult SetAssessmentYear(int AsmtYear)
        {
            RevLossConstants.CurrentAssesmentYear = AsmtYear;
            return Json(RevLossConstants.CurrentAssesmentYear);
        }

        // POST: /api/RevenueLoss/FileUpload
        [HttpPost("/api/RevenueLoss/FileUpload")]
        public async Task<IActionResult> FileUpload()
        {
            long size = 0;
            IFormFileCollection files = Request.Form.Files;

            foreach (IFormFile file in files)
            {
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                filename = AppSettings.ExcelStageLocation + $@"\{filename}";
                size += file.Length;
                await file.SaveAsAsync(filename);

                orchestrator.UploadRevenueLoss(filename);
            }
            string message = $"{files.Count} file(s) / { size} bytes uploaded successfully!";

            string keyvalue = AppSettings.ExcelConfigEndKey;
            return Json(message);
        }

        // GET: /api/RevenueLoss/GetDetail
        [HttpGet("/api/RevenueLoss/GetDetail")]
        public IActionResult GetDetail(string vertical, string country)
        {
            if (country.Equals("null")) country = null;
            IList<Positions> positions = this.orchestrator.GetRevenueLossDetail(vertical, country);
            return Json(positions);
        }

        // GET: /api/RevenueLoss/GetSummary
        [HttpGet("/api/RevenueLoss/GetSummary")]
        public IActionResult GetSummary(string vertical)
        {
            if (vertical == null || vertical.Equals("null")) vertical = null;
            IList<Summary> summary = this.orchestrator.GetRevenueLossSummary(vertical);
            return Json(summary);
        }

        // GET: /api/RevenueLoss/GetSummaryDetail
        [HttpGet("/api/RevenueLoss/GetSummaryDetail")]
        public IActionResult GetSummaryDetail(string vertical, string country)
        {
            if (vertical == null || vertical.Equals("null")) vertical = null;
            if (country == null || country.Equals("null")) country = null;

            IList<SummaryDetail> summaryDetail = this.orchestrator.GetRevenueLossSummaryDetail(vertical, country);
            return Json(summaryDetail);
        }

        // GET: /api/RevenueLoss/GetPositionSummary
        [HttpGet("/api/RevenueLoss/GetPositionSummary")]
        public IActionResult GetPositionSummary(string level, string vertical)
        {
            if (level == null || level.Equals("null")) level = null;
            if (vertical == null || vertical.Equals("null")) vertical = null;

            IList<PositionSummary> openPositions = this.orchestrator.GetPositionSummary(level, vertical);
            return Json(openPositions);
        }

        // GET: /api/RevenueLoss/GetPositionMonthly
        [HttpGet("/api/RevenueLoss/GetPositionMonthly")]
        public IActionResult GetPositionMonthly(string level, string vertical)
        {
            if (level == null || level.Equals("null")) level = null;
            if (vertical == null || vertical.Equals("null")) vertical = null;

            IList<PositionMonthly> openPositions = this.orchestrator.GetPositionMonthly(level, vertical);
            return Json(openPositions);
        }

        // POST: /api/RevenueLoss/Export
        [HttpPost("/api/RevenueLoss/Export")]
        public IActionResult Export()
        {
            string fileName = this.orchestrator.ExportSummaryToExcel();
            return Json(fileName);
        }

        // GET: /api/RevenueLoss/DownloadReport
        [HttpGet("/api/RevenueLoss/DownloadReport")]
        public IActionResult DownloadReport(string fileName)
        {
            string fullPath = Path.Combine(AppSettings.RevenueLossReportPath, fileName);
            FileStreamResult fileStreamResult = null;

            FileStream fs = new FileStream(fullPath, FileMode.Open);
            fileStreamResult = new FileStreamResult(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };
            return fileStreamResult;
        }
    }
}
