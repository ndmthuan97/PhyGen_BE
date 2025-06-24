using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeekly()
        {
            var result = await _statisticService.GetWeeklyStatisticsAsync();
            return Ok(result);
        }

        [HttpGet("statistics")]
        [ProducesResponseType(typeof(InvoiceResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetInvoiceStatistics()
        {
            var result = await _statisticService.GetInvoiceStatistics();
            return Ok(result);
        }
    }
}
