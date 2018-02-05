using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Policy = "AdministratorOnly")]
	public class MaintenanceController : Controller
	{
		private readonly IMapper Mapper;
		private readonly ILogger<MaintenanceController> Log;
		private readonly MaintenanceService MaintenanceService;

		public MaintenanceController(IMapper mapper, ILogger<MaintenanceController> logger, MaintenanceService maintenance)
		{
			Mapper = mapper;
			Log = logger;
			MaintenanceService = maintenance;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult RecreateIndexes()
		{
			MaintenanceService.CreateIndexes();

			return RedirectToAction("Index");
		}
	}
}