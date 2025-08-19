using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhyGen.Application.Admin.Dtos;
using PhyGen.Application.Admin.Interfaces;
using PhyGen.Application.Admin.Response;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Users.Dtos;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Specs;
using PhyGen.Shared;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/admins")]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        private readonly IInvoiceExcelExporter _exporter;

        public StatisticController(IStatisticService statisticService, IInvoiceExcelExporter exporter)
        {
            _statisticService = statisticService;
            _exporter = exporter;
        }

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeekly()
        {
            var result = await _statisticService.GetWeeklyStatisticsAsync();
            return Ok(result);
        }

        [HttpGet("revenue/weekly-history")]
        public async Task<IActionResult> GetAllWeeklyRevenue()
        {
            var result = await _statisticService.GetAllWeeklyRevenueAsync();
            return Ok(result);
        }

        [HttpGet("statistics")]
        [ProducesResponseType(typeof(InvoiceResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetInvoiceStatistics(
        [FromQuery] int pageIndex = 1,
        [FromQuery] string? fullName = null,
        [FromQuery] string? status = null,
        [FromQuery] decimal? minAmount = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
        {
            var filter = new InvoiceFilter
            {
                PageIndex = pageIndex,
                FullName = fullName,
                Status = status,
                MinAmount = minAmount,
                FromDate = fromDate,
                ToDate = toDate
            };

            var result = await _statisticService.GetInvoiceStatistics(filter);
            return Ok(result);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportExcel(
        [FromQuery] string? fullName,
        [FromQuery] string? status,
        [FromQuery] decimal? minAmount,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate, CancellationToken ct)
        {
            var filter = new InvoiceExportFilter
            {
                FullName = fullName,
                Status = status,
                MinAmount = minAmount,
                FromDate = fromDate,
                ToDate = toDate
            };
            var bytes = await _exporter.ExportAsync(filter, ct);
            var fileName = $"BaoCaoHoaDon_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(bytes, contentType, fileName);
        }
    }
}
