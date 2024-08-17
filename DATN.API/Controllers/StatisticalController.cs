using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Statistical;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize(Roles = "Admin")]
    public class StatisticalController : ControllerBase
	{
		private readonly IStatisticalService _statistical;

		public StatisticalController(IStatisticalService statistical)
        {
			_statistical = statistical;
		}
        [HttpGet]
		public async Task<ResponseData<Statistical>> StatisticalIndex(DateTime? startDate, DateTime? endDate, int type = 1)
		{
			return await _statistical.StatisticalIndex(startDate,endDate, type);
		}
	}
}
